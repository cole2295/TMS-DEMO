using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.BLL.CloudBillService;
using System.IO;
using Vancl.TMS.Util.OfficeUtil;
using System.Net.Mail;
using System.Net;
using Vancl.TMS.Util.Net;
using System.Text.RegularExpressions;

namespace Vancl.TMS.BLL.Sorting.Outbound
{
    public partial class OutboundBLL
    {
        public IList<OutboundPrintModel> SearchOutboundPrint(int toStation, DateTime beginTime, DateTime endTime, int expressID, string batchNo, string waybillNo)
        {
            Enums.CompanyFlag companyFlag = Enums.CompanyFlag.Administration;

            if (toStation > 0)
            {
                var cmp = ExpressCompanyBLL.Get(toStation);
                if (cmp != null)
                {
                    //companyFlag: 部门类型（0行政部门、1分拣中心、2站点、3加盟商）
                    //companyFlag = ExpressCompanyBLL.Get(toStation).CompanyFlag;
                    companyFlag = Enums.CompanyFlag.SortingCenter;
                    expressID = 2;
                }
            }

            return outboundDAL.SearchOutboundPrint(toStation, beginTime, endTime, expressID, batchNo, waybillNo, companyFlag);
        }


        public IList<OutboundPrintExportModel> GetOutboundPrintExportModel(IList<string> batchNoList)
        {
            if (batchNoList == null) throw new ArgumentNullException();
            if (batchNoList.Count == 0) throw new ArgumentException("未传入有批次数据。");

            return outboundDAL.GetOutboundPrintExportModel(batchNoList);
        }


        public IList<OutboundOrderCountModel> GetOrderCount(IList<string> batchNoList)
        {
            if (batchNoList == null) throw new ArgumentNullException();
            if (batchNoList.Count == 0) throw new ArgumentException("未传入有批次数据。");

            return outboundDAL.GetOrderCount(batchNoList);
        }


        public IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo)
        {
            if (string.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException();
            return outboundDAL.GetPrintBatchDetail(batchNo);
        }


        public ResultModel OutBoundSendEmail(int expressCompanyId, IEnumerable<string> batchNoList, IEnumerable<string> emailList)
        {
            if (expressCompanyId <= 0) throw new ArgumentException("expressCompanyId");
            if (batchNoList == null || batchNoList.Count() == 0) throw new ArgumentNullException("batchNoList");              

            try
            {
                string emailBody = string.Format("({0}){1}批次{2}出库订单明细",
                    UserContext.CurrentUser.DeptName,
                    DateTime.Now.ToShortDateString(),
                    string.Join(",", batchNoList));
                string emailSubject = emailBody + "(系统邮件请勿回复)";
                //出库列表
                var outboundList = this.outboundDAL.GetOutboundEntityByBatchNoList(batchNoList.ToList());
                //运单号列表
                var formCodeList = outboundList.Select(x => x.FormCode).ToList();
                //批次运单信息
                var batchBillInfoList = this.outboundDAL.GetBatchBillInfoForOutBoundSendMail(formCodeList, outboundList[0].OutboundType);
                //从接口获取waybillSignInfo和waybillTakeSendInfo信息填充到batchBillInfo
                using (var billServiceClient = new BillServiceClient())
                {
                    var waybillList = formCodeList.Select(x => long.Parse(x)).ToArray();
                    var tackSendInfoList = billServiceClient.GetWaybillTakeSendInfoList(waybillList);
                    var signInfoList = billServiceClient.GetWaybillSignInfoList(waybillList);
                    foreach (var batchBillInfo in batchBillInfoList)
                    {
                        var waybillNo = long.Parse(batchBillInfo.FormCode);
                        if (tackSendInfoList != null)
                        {
                            var tackSendInfo = tackSendInfoList.FirstOrDefault(x => x.WaybillNo == waybillNo);
                            if (tackSendInfo != null)
                            {
                                batchBillInfo.ReceiveBy = tackSendInfo.ReceiveBy;
                                batchBillInfo.ReceiveAddress = tackSendInfo.ReceiveAddress;
                                batchBillInfo.ReceivePost = tackSendInfo.ReceivePost;
                                batchBillInfo.SendTimeType = tackSendInfo.SendTimeType;
                                batchBillInfo.ReceiveComment = tackSendInfo.ReceiveComment;
                                batchBillInfo.ReceiveProvince = tackSendInfo.ReceiveProvince;
                                batchBillInfo.ReceiveCity = tackSendInfo.ReceiveCity;
                                batchBillInfo.ReceiveArea = tackSendInfo.ReceiveArea;
                            }
                        }
                        if (signInfoList != null)
                        {
                            var signInfo = signInfoList.FirstOrDefault(x => x.WaybillNo == waybillNo);
                            if (signInfo != null)
                            {
                                batchBillInfo.SendTimeType = signInfo.AcceptType;
                                batchBillInfo.Amount = signInfo.Amount ?? 0;
                                batchBillInfo.PaidAmount = signInfo.PaidAmount ?? 0;
                                batchBillInfo.NeedAmount = signInfo.NeedAmount ?? 0;
                                batchBillInfo.NeedBackAmount = signInfo.NeedBackAmount ?? 0;
                                batchBillInfo.ProtectedPrice = signInfo.ProtectedPrice ?? 0;
                                batchBillInfo.Amount = signInfo.Amount ?? 0;
                            }
                        }
                    }
                }

                MemoryStream stream = new MemoryStream();
                using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
                {
                    helper.CreateNewWorksheet("出库单明细");
                    helper.WriteData(batchBillInfoList.To2Array());
                    helper.Save();
                }
                stream.Seek(0, 0);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Consts.SMTP_FROM);
                mail.Subject = emailSubject;
                mail.Body = emailBody;
                string.Join(";", emailList).Replace(",", ";").Split(';')
                    .Where(x => !string.IsNullOrWhiteSpace(x) && Regex.IsMatch(x, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled))
                    .ToList().ForEach(x => mail.To.Add(x));
                //for test
                //  mail.To.Clear();
                //   mail.To.Add("zhangbendong@vancl.cn");
                mail.Attachments.Add(new Attachment(stream, "出库单明细.xlsx"));

                MailHelper.Send(Consts.SMTP_HOST, new NetworkCredential(Consts.SMTP_ACCOUNT, Consts.SMTP_PASSWORD), mail);

                return ResultModel.Create(true);
            }
            catch (Exception ex)
            {
                return ResultModel.Create(false, ex.Message);
            }
        }

    }
}

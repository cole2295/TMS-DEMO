using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.BLL.Sorting.Common;
using System.Data;
using Vancl.TMS.Model.SMS;
using Vancl.TMS.IDAL.LMS;

namespace Vancl.TMS.BLL.Sorting.Return
{
    public class MerchantReturnBLL : SortCenterBLL, IMerchantReturnBLL
    {
        IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
        IWaybillDAL WaybillDAL = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_SQL");//LMSWaybillDAL_SQL|LMSWaybillDAL_Oracle
        IExpressCompanyDAL ExpressCompanyDAL = ServiceFactory.GetService<IExpressCompanyDAL>();
        /// <summary>
        /// 商家入库确认
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <param name="Message"></param>
        public void ReturnbillInBound(string FormCodes, out string Message)
        {
            Message = "";
            var codes = FormCodes.Split(',');
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                foreach (var item in codes)
                {
                    var bill = BillDAL.GetBillByFormCode(item);
                    //var waybill = WaybillDAL.GetModel(long.Parse(item));
                    CloudBillService.WaybillModel waybill;
                    using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
                    {
                        waybill = client.GetWaybillModel(long.Parse(item));
                    }

                    if (bill != null && waybill != null)
                    {
                        var express = ExpressCompanyDAL.GetModel(bill.CreateDept);
                        if (express != null && !express.DistributionCode.Equals(UserContext.CurrentUser.DistributionCode))
                        {
                            Message = "您没有权限操作单号为" + item.ToString() + "的运单！";
                            return;
                        }
                        var status = bill.Status;

                        if (bill.Source == Enums.BillSource.Others)
                        {
                            if (waybill.Status == (int)Enums.BillStatus.DeliverySuccess)
                            {
                                if (bill.BillType == Enums.BillType.SignReturn)
                                {
                                    status = Enums.BillStatus.ReturnSignedBounded;
                                    bill.ReturnStatus = Enums.ReturnStatus.ReturnSignedBounded;
                                }
                                else
                                {
                                    status = Enums.BillStatus.ReturnBounded;
                                    bill.ReturnStatus = Enums.ReturnStatus.ReturnExchangeInbounded;
                                }
                            }
                            else if (waybill.Status == (int)Enums.BillStatus.Rejected)
                            {
                                status = Enums.BillStatus.RefusedBounded;
                                bill.ReturnStatus = Enums.ReturnStatus.RejectedInbounded;
                            }
                        }
                        bill.UpdateBy = UserContext.CurrentUser.ID;
                        bill.UpdateDept = UserContext.CurrentUser.DeptID;
                        bill.UpdateTime = DateTime.Now;
                        bill.CurrentDistributionCode = UserContext.CurrentUser.DistributionCode;
                        //bill.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                        var log = new BillChangeLogDynamicModel
                        {
                            CreateBy = UserContext.CurrentUser.ID,
                            CreateDept = UserContext.CurrentUser.DeptID,
                            CurrentSatus = status,
                            CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                            DeliverStationID = bill.DeliverStationID,
                            FormCode = bill.FormCode,
                            OperateType = Enums.TmsOperateType.MerchantReturnBillInbound,
                            PreStatus = bill.Status,
                        };
                        if (BillDAL.UpdateBillModelMerchantInBound(bill) > 0)
                        {
                            log.ReturnStatus = (Enums.ReturnStatus)bill.ReturnStatus;
                        }
                        WriteBillChangeLog(log);
                        using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
                        {
                            var WaybillTakeSendInfo = client.GetWaybillTakeSendInfo(long.Parse(item));
                            var Template = client.GetphoneMessageTemplate(bill.MerchantID, (int)Enums.BillSource.Others, 4);
                            if (WaybillTakeSendInfo != null && CheckMobileValid(WaybillTakeSendInfo.ReceiveMobile) && !string.IsNullOrEmpty(Template))
                            {
                                var result = SMSSender.Send(new SMSMessage()
                                    {
                                        Content = Template,
                                        //Content = "退换货测试",
                                        FormCode = item,
                                        PhoneNumber = WaybillTakeSendInfo.ReceiveMobile,
                                        Title = @"退换货入库短信"
                                    });
                            }
                        }
                    }
                }
                scope.Complete();
            }
        }
        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        public List<MerchantReturnBillViewModel> GetMerchantReturnBillList(ReturnBillSeachModel SearchModel, string isHasPrint, string CreateDept, out string FormCodes)
        {
            FormCodes = "";
            List<MerchantReturnBillViewModel> Lists = new List<MerchantReturnBillViewModel>();
            IDictionary<string, string> Dict = new Dictionary<string, string>();
            if (SearchModel == null) return null;

            MerchantReturnSearchModel searchmodel = new MerchantReturnSearchModel()
            {
                FormCode = SearchModel.FormCode,
                Source = SearchModel.Source,
                CurrentDistributionCode = SearchModel.CurrentDistributionCode,
                DeliverCode = SearchModel.DeliverCode,
                BoxNo = SearchModel.BoxNo,
                DistributionCode = SearchModel.DistributionCode,
                ReturnStatus = SearchModel.ReturnStatus,
                CurrentDeptName = CreateDept,
                isHasPrint = isHasPrint == "1" ? true : false
            };
            var FormCodelists = BillDAL.GetFormCodeLists(searchmodel);
            if (!string.IsNullOrEmpty(FormCodelists))
            {
                using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
                {
                    CloudBillService.WaybillReturnSearchModel model = new CloudBillService.WaybillReturnSearchModel()
                    {
                        BeginTime = SearchModel.BeginTime,
                        EndTime = SearchModel.EndTime,
                        DistributionCode = SearchModel.DistributionCode,
                        Source = SearchModel.Source,
                        ReturnStatus = SearchModel.ReturnStatus,
                        PageSize = SearchModel.PageSize,
                        StationId = SearchModel.StationId,
                        MerchantId = SearchModel.MerchantId,
                        CityId = SearchModel.CityId,
                        CurrentDistributionCode = SearchModel.CurrentDistributionCode,
                        FormCodeLists = FormCodelists,
                        LabelNo = SearchModel.LabelNo,
                        CurrentDeptName = searchmodel.CurrentDeptName,

                        IsPageing = false
                    };
                    var billreturnlist = client.WaybillReturnInfoList(model);
                    if (billreturnlist != null)
                    {
                        foreach (var item in billreturnlist)
                        {
                            MerchantReturnBillViewModel billmodel = new MerchantReturnBillViewModel()
                            {

                                No = item.No,
                                BoxNo = item.BoxNo,
                                Source = item.Source,
                                BillFormType = item.BillFormType,
                                BillStatus = item.BillStatus,
                                Status = item.Status,
                                Weight = item.Weight,
                                CreateTime = item.CreateTime,
                                ReturnStatus = item.ReturnStatus,
                                ReturnReason = item.ReturnReason,
                                CustomerOrder = item.CustomerOrder,
                                DeliverCode = item.DeliverCode,
                                DistributionName = item.DistributionName,
                                EmployeeName = item.EmployeeName,
                                FormType = item.FormType,
                                LabelNo = item.LabelNo,
                                MerchantName = item.MerchantName,
                                NeedAmount = item.NeedAmount,
                                NeedBackAmount = item.NeedBackAmount
                            };
                            if (string.IsNullOrEmpty(FormCodes))
                            {
                                FormCodes += item.CustomerOrder;
                            }
                            else
                            {
                                FormCodes += "," + item.CustomerOrder;
                            }
                            Lists.Add(billmodel);
                        }
                    }
                }
            }
            return Lists;
        }

        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        public List<MerchantReturnBillStatisticModel> GetReturnbillStatistic(string FormCodelists)
        {
            if (string.IsNullOrEmpty(FormCodelists)) return null;
            List<MerchantReturnBillStatisticModel> statistic = new List<MerchantReturnBillStatisticModel>();
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                var lists = client.GetReturnWaybillStatistic(FormCodelists);
                if (lists != null)
                {
                    foreach (var item in lists)
                    {
                        MerchantReturnBillStatisticModel model = new MerchantReturnBillStatisticModel()
                        {
                            SourceName = item.SourceName,
                            ChangeCount = item.ChangeCount,
                            BackCount = item.BackCount,
                            RefuseCount = item.RefuseCount,
                            ChangeRefuseCount = item.ChangeRefuseCount,
                            ChangeWeight = item.ChangeWeight,
                            BackWeight = item.BackWeight,
                            RefuseWeight = item.RefuseWeight,
                            ChangeRefuseWeight = item.ChangeRefuseWeight
                        };
                        statistic.Add(model);
                    }
                    return statistic;
                }
                return null;
            }
        }
        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        public List<MerchantReturnBillViewModel> GetMerchantReturnBillListForPrint(string FormCodelists, out List<string> MerchantNames, out List<string> billStatus)
        {
            List<MerchantReturnBillViewModel> Lists = new List<MerchantReturnBillViewModel>();
            MerchantNames = new List<string>();
            billStatus = new List<string>();
            if (!string.IsNullOrEmpty(FormCodelists))
            {
                using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
                {
                    CloudBillService.WaybillReturnSearchModel model = new CloudBillService.WaybillReturnSearchModel()
                    {
                        DistributionCode = UserContext.CurrentUser.DistributionCode,
                        CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                        FormCodeLists = FormCodelists,
                        IsPageing = false
                    };
                    var billreturnlist = client.WaybillReturnInfoList(model);
                    if (billreturnlist != null)
                    {
                        foreach (var item in billreturnlist)
                        {
                            MerchantReturnBillViewModel billmodel = new MerchantReturnBillViewModel()
                            {

                                No = item.No,
                                BoxNo = item.BoxNo,
                                Source = item.Source,
                                Status = item.Status,
                                BillFormType = item.BillFormType,
                                BillStatus = item.BillStatus,
                                Weight = item.Weight,
                                CreateTime = item.CreateTime,
                                ReturnStatus = item.ReturnStatus,
                                ReturnReason = item.ReturnReason,
                                CustomerOrder = item.CustomerOrder,
                                DeliverCode = item.DeliverCode,
                                DistributionName = item.DistributionName,
                                EmployeeName = item.EmployeeName,
                                FormType = item.FormType,
                                LabelNo = item.LabelNo,
                                MerchantName = item.MerchantName,
                                NeedAmount = item.NeedAmount,
                                NeedBackAmount = item.NeedBackAmount
                            };
                            if (!MerchantNames.Contains(item.MerchantName))
                            {
                                MerchantNames.Add(item.MerchantName);
                            }
                            string statusname = item.BillStatus == "妥投" ? item.BillFormType : item.BillStatus;
                            if (!billStatus.Contains(statusname))
                            {
                                billStatus.Add(statusname);
                            }
                            if (Lists.FirstOrDefault(r => r.DeliverCode == item.DeliverCode) == null)
                            {
                                Lists.Add(billmodel);
                            }
                        }
                    }
                }
            }
            return Lists;
        }

        #region 私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveMobile"></param>
        /// <returns></returns>
        private static bool CheckMobileValid(string receiveMobile)
        {
            long phoneNumber = 0;
            return receiveMobile != string.Empty &&
                   receiveMobile.Length == 11 &&
                   Int64.TryParse(receiveMobile, out phoneNumber);
        }

        #endregion
    }
}

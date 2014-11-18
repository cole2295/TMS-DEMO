using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Util.Net;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.Sorting.Outbound
{
	public partial class OutboundBLLV2
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

			return outboundV2DAL.SearchOutboundPrint(toStation, beginTime, endTime, expressID, batchNo, waybillNo, companyFlag);
		}

		public PagedList<OutboundPrintModelV2> SearchOutboundPrintV2(OutboundPrintSearchModel searchModel)
		{

			return outboundV2DAL.SearchOutboundPrintV2(searchModel);
		}

		public IList<OutboundPrintModelV2> GetOutboundPrintReceipt(OutboundPrintSearchModel searchModel)
		{
			return outboundV2DAL.GetOutboundPrintReceipt(searchModel);
		}

		public IList<OutboundPrintModelV2> GetOutboundPrintReceiptByBatchNos(IList<string> batchNoList)
		{
			return outboundV2DAL.GetOutboundPrintReceiptByBatchNos(batchNoList);
		}

		public ResultModel CreateAndUpdateBatchNo(OutboundPrintSearchModel searchModel)
		{
			var Result = new ResultModel();
			
			if (!string.IsNullOrEmpty(searchModel.ArrivalIdList))
			{
				string[] arrArrivalIds = searchModel.ArrivalIdList.Split(',');
				for (int i = 0; i < arrArrivalIds.Length-1; i++)
				{
					//产生批次号
					String BatchNo = batchNoGenerator.Execute(new SerialNumberModel() { FillerCharacter = "0", NumberLength = 6 });
					if (String.IsNullOrWhiteSpace(BatchNo))
					{
						return Result.Failed("出库批次号产生失败") as ViewOutboundSimpleModel;
					}
					var batchModel = new OutboundBatchEntityModel()
					{
						BatchNo = BatchNo,
						DepartureID = searchModel.ExpressId,
						ArrivalID =  Convert.ToInt32(arrArrivalIds[i]),
						OutboundCount = 0,
						SyncFlag = Enums.SyncStatus.NotYet,
						CreateBy = UserContext.CurrentUser.ID,
						UpdateBy = UserContext.CurrentUser.ID
					};
					batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
					if (outboundbatchV2DAL.Add(batchModel) <= 0)
					{
						return Result.Failed("产生出库批次数据失败，请重试.");
					}
					searchModel.BatchNo = BatchNo;
					searchModel.ArrivalId = Convert.ToInt32(arrArrivalIds[i]);
					if (outboundV2DAL.UpdateBatchNo(searchModel) <= 0)
					{
						return Result.Failed("更新批次号失败，请重试.");
					}
				}
			}
			return Result.Succeed();
		}

		public IList<OutboundPrintExportModel> GetOutboundPrintExportModel(IList<string> batchNoList)
		{
			if (batchNoList == null) throw new ArgumentNullException();
			if (batchNoList.Count == 0) throw new ArgumentException("未传入有批次数据。");

			return outboundV2DAL.GetOutboundPrintExportModel(batchNoList);
		}


		public IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(IList<string> batchNoList)
		{
			return outboundV2DAL.GetOutboundPrintExportModelV2(batchNoList);
		}

		/// <summary>
		/// 获取出库打印导出数据V2
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		public IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(OutboundPrintSearchModel searchModel)
		{
			return outboundV2DAL.GetOutboundPrintExportModelV2(searchModel);
		}


		public IList<OutboundOrderCountModel> GetOrderCount(IList<string> batchNoList)
		{
			if (batchNoList == null) throw new ArgumentNullException();
			if (batchNoList.Count == 0) throw new ArgumentException("未传入有批次数据。");

			return outboundV2DAL.GetOrderCount(batchNoList);
		}


		public IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo)
		{
			if (string.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException();
			return outboundV2DAL.GetPrintBatchDetail(batchNo);
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
				var outboundList = this.outboundV2DAL.GetOutboundEntityByBatchNoList(batchNoList.ToList());
				//运单号列表
				var formCodeList = outboundList.Select(x => x.FormCode).ToList();
				//批次运单信息
				var batchBillInfoList = this.outboundV2DAL.GetBatchBillInfoForOutBoundSendMail(formCodeList, outboundList[0].OutboundType);
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

		public ResultModel OutBoundSendEmailV2(int expressCompanyId, IEnumerable<string> batchNoList,
		                                       IEnumerable<string> emailList, string selTimeType, string searchArg)
		{
			try
			{
				string companyName = expressCompanyDAL.GetModel(expressCompanyId).MnemonicName;
				string emailBody = string.Format("{0}{1}{2}出库明细",
				                                 DateTime.Now.ToShortDateString(),
				                                 UserContext.CurrentUser.DeptName,
				                                 companyName);
				string emailSubject = emailBody + "(系统邮件请勿回复)";

				//获取数据
				IList<OutboundPrintExportDetailsModelV2> batchBillInfoList = new List<OutboundPrintExportDetailsModelV2>();
				
				if (selTimeType == "0")
				{
					var searchModel = new OutboundPrintSearchModel();
					string[] aarSearchArg = searchArg.Split(',');
					searchModel.StartTime = string.IsNullOrEmpty(aarSearchArg[0])
							 ? DateTime.MinValue
							 : Convert.ToDateTime(aarSearchArg[0]);
					searchModel.EndTime = string.IsNullOrEmpty(aarSearchArg[1])
							 ? DateTime.MaxValue
							 : Convert.ToDateTime(aarSearchArg[1]);

					searchModel.BatchNo = aarSearchArg[2];
					searchModel.BoxNo = aarSearchArg[3];
					searchModel.FormCode = aarSearchArg[4];
					searchModel.ExpressId = UserContext.CurrentUser.DeptID;
					searchModel.ArrivalIdList = Convert.ToString(expressCompanyId);

					batchBillInfoList = outboundV2DAL.GetOutboundPrintExportModelV2(searchModel);
				}
				else
				{
					IList<string> batchNos = batchNoList.ToList();
					batchBillInfoList = outboundV2DAL.GetOutboundPrintExportModelV2(batchNos);
				}

				//发送邮件
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
				//mail.To.Clear();
				//mail.To.Add("syf8699@qq.com");
				mail.To.Add("sunyanfei@vancl.cn");
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

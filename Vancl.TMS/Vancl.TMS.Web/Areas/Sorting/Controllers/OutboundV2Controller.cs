using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.BLL.CustomizeFlow;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.IBLL.Sorting.Outbound;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Util.Extensions;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Util.Security;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
	public class OutboundV2Controller : Controller
	{
		private IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
		private IOutboundBLLV2 _outboundBLLV2 = ServiceFactory.GetService<IOutboundBLLV2>("SC_OutboundBLLV2");
		private IBillBLL _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
		private IUserContextBLL userContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");

		private FlowFunFacade TMSFlowFunFacade = new FlowFunFacade();

		/// <summary>
		/// 扫描出库
		/// </summary>
		/// <returns></returns>
		public ActionResult SimpleOutboundV2()
		{
			string distributionCode = UserContext.CurrentUser.DistributionCode;
			ViewBag.FunOutBoundStation = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunOutBoundStation);
			ViewBag.FunOutBoundDeliverCenter = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunOutBoundDeliverCenter);
			ViewBag.FunOutBoundCompany = TMSFlowFunFacade.IsExitsCurrFun(distributionCode,FunCode.FunOutBoundCompany);

			//分拣中心列表
			List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
			this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
			//配送商列表
			//List<ExpressCompanyModel> distributorList =
			//    _expressCompanyBLL.GetRelatedDistributor(distributionCode);
			List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetDistributionCooprelation(distributionCode);
			this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
			
			//出库验证
			SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
			OutboundPreConditionModel preCondition = null;
			if (userModel != null)
			{
				preCondition = _outboundBLLV2.GetPreCondition(distributionCode);
			}
			ViewOutboundValidateModel viewModel = new ViewOutboundValidateModel(userModel, preCondition);
			ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

			//获取当日出库总量
			var CurExpressId = Convert.ToInt32(userModel.ExpressId);
			var outboundcount = _outboundBLLV2.GetCurOutBoundCount(CurExpressId);
			ViewBag.CurOutBoundCount = outboundcount;
			return View();
		}

		/// <summary>
		/// 逐单出库（扫描出库）
		/// </summary>
		/// <param name="hidData"></param>
		/// <param name="selectedType"></param>
		/// <param name="selectStationValue"></param>
		/// <param name="code"></param>
		/// <param name="formType"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ScanFormCodeV2(string hidData, int selectedType, string selectStationValue, string code,
										   int formType, string BatchNo, int CurrentDisCount)
		{
			try
			{


				if (String.IsNullOrWhiteSpace(code))
				{
					return Json(new ViewOutboundSimpleModel() {IsSuccess = false, Message = "输入的单号为空"}, JsonRequestBehavior.AllowGet);
				}
				var OutboundData = JsonHelper.ConvertToObject<ViewOutboundValidateModel>(DES.Decrypt3DES(hidData));
				if (!OutboundData.IsValidate)
				{
					return Json(new ViewOutboundSimpleModel() {IsSuccess = false, Message = OutboundData.ValidateMsg},
					            JsonRequestBehavior.AllowGet);
				}
				var CorrectType = new int[] {0, 2, 3};
				if (!CorrectType.Contains(selectedType))
				{
					return Json(new ViewOutboundSimpleModel() {IsSuccess = false, Message = "请选择站点类型!"}, JsonRequestBehavior.AllowGet);
				}
				if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
				{
					return Json(new ViewOutboundSimpleModel() {IsSuccess = false, Message = "请选择站点!"}, JsonRequestBehavior.AllowGet);
				}
				var argument = new OutboundSimpleArgModel() {OpUser = OutboundData.OpUser, PreCondition = OutboundData.PreCondition};
				argument.ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue));
				argument.FormType = (Enums.SortCenterFormType) formType;
				argument.FormCode = code.Trim();
				argument.BatchNo = BatchNo;
				argument.CurrentDisCount = CurrentDisCount;

				//出库操作
				var viewOutboundSimpleModel = _outboundBLLV2.SimpleOutbound(argument,false);
				//获取最新的出库数量
				SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
				viewOutboundSimpleModel.CurOptCount = _outboundBLLV2.GetCurOutBoundCount(Convert.ToInt32(userModel.ExpressId));
				viewOutboundSimpleModel.CurArrivalCount = _outboundBLLV2.GetCurDisOutBoundCount(
					Convert.ToInt32(userModel.ExpressId), Convert.ToInt32(selectStationValue));

				return Json(viewOutboundSimpleModel, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new ViewOutboundSimpleModel() { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
			}
		}
		
		/// <summary>
		/// 按箱出库（单箱）
		/// </summary>
		/// <param name="selectedType"></param>
		/// <param name="selectStationValue"></param>
		/// <param name="boxNo"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult BoxOutBoundSimple(string selectedType, string selectStationValue, string boxNo)
		{
			try
			{

				ViewOutboundBatchModel result = new ViewOutboundBatchModel();
				if (String.IsNullOrWhiteSpace(selectedType))
				{
					result.IsSuccess = false;
					result.Message = "请选择分拣类型";
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				if (String.IsNullOrWhiteSpace(selectStationValue))
				{
					result.IsSuccess = false;
					result.Message = "请选择出库站点";
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				if (String.IsNullOrWhiteSpace(boxNo))
				{
					result.IsSuccess = false;
					result.Message = "请输入需要出库的箱号";
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				var argument = new OutboundByBoxArgModel()
					{
						CurrentBoxNo = boxNo,
						OpUser = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID),
						PreCondition = _outboundBLLV2.GetPreCondition(UserContext.CurrentUser.DistributionCode),
						ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue))
					};
				//逐箱出库
				result = _outboundBLLV2.BoxOutbound(argument);
				if (!result.IsSuccess)
				{
					result.BoxNo = boxNo;
					result.BoxOutboundStatus = "出库失败";
					result.OutboundDes = argument.ToStation.CompanyName;
					result.SucceedCount = 0;
				}
				else
				{
					//sCount++;
					result.BoxNo = boxNo;
					result.BoxOutboundStatus = "已出库";
					result.OutboundDes = argument.ToStation.CompanyName;
				}
				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new ViewOutboundBatchModel() {IsSuccess = false, Message = ex.Message}, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// 批量出库（原查询出库）
		/// Sorting/Outbound/SearchOutbound
		/// </summary>
		/// <returns></returns>
		public ActionResult BatchOutbound()
		{
			string distributionCode = UserContext.CurrentUser.DistributionCode;
			ViewBag.FunOutBoundStation = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunOutBoundStation);
			ViewBag.FunOutBoundDeliverCenter = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunOutBoundDeliverCenter);
			ViewBag.FunOutBoundCompany = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunOutBoundCompany);

			//分拣中心列表

			List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
			this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);

			//配送商列表

			//List<ExpressCompanyModel> distributorList =
			//    _expressCompanyBLL.GetRelatedDistributor(distributionCode);
			List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetDistributionCooprelation(distributionCode);
			this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);

			//出库验证
			SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
			OutboundPreConditionModel preCondition = null;
			if (userModel != null)
			{
				preCondition = _outboundBLLV2.GetPreCondition(distributionCode);
			}
			ViewOutboundValidateModel viewModel = new ViewOutboundValidateModel(userModel, preCondition);
			ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

			//获取当日出库总量
			var CurExpressId = Convert.ToInt32(userModel.ExpressId);
			var outboundcount = _outboundBLLV2.GetCurOutBoundCount(CurExpressId);
			ViewBag.CurOutBoundCount = outboundcount;
			return View();
		}

		/// <summary>
		/// 批量出库（按单号）
		/// </summary>
		/// <param name="selectedType"></param>
		/// <param name="selectStationValue"></param>
		/// <param name="Code"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult BatchOutboundByCode(String selectedType, String selectStationValue, String Code)
		{
			try
			{
				ViewOutboundSimpleModel result = null;
				if (String.IsNullOrWhiteSpace(selectedType))
				{
					result = new ViewOutboundSimpleModel() {IsSuccess = false, Message = "请选择分拣类型"};
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				if (String.IsNullOrWhiteSpace(selectStationValue))
				{
					result = new ViewOutboundSimpleModel() {IsSuccess = false, Message = "请选择出库站点"};
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				if (String.IsNullOrWhiteSpace(Code))
				{
					result = new ViewOutboundSimpleModel() {IsSuccess = false, Message = "请选择需要出库的运单"};
					return Json(result, JsonRequestBehavior.AllowGet);
				}
				//从arrCodes中取值逐单出库
				SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
				var argument = new OutboundSimpleArgModel();
				argument.OpUser = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
				argument.PreCondition = _outboundBLLV2.GetPreCondition(UserContext.CurrentUser.DistributionCode);
				argument.ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue));
				argument.FormType = Enums.SortCenterFormType.Waybill;
				argument.BatchNo = "0";

				argument.FormCode = Code;
				//逐单出库
				result = _outboundBLLV2.SimpleOutbound(argument,true);
				//获取最新的出库数量
				result.CurOptCount = _outboundBLLV2.GetCurOutBoundCount(Convert.ToInt32(userModel.ExpressId));
				result.CurArrivalCount = _outboundBLLV2.GetCurDisOutBoundCount(Convert.ToInt32(userModel.ExpressId),
				                                                               Convert.ToInt32(selectStationValue));
				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new ViewOutboundSimpleModel() {IsSuccess = false, Message = ex.Message}, JsonRequestBehavior.AllowGet);
			}
		}
		
		/// <summary>
		/// 获取出库到当前目的地数量和批次号
		/// </summary>
		/// <param name="selectStationValue"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult GetCountAndBatchNo(string selectStationValue)
		{
			if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
			{
				return Json(new ViewGetCountAndBatchNo() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
			}
			SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
			var CurExpressId = Convert.ToInt32(userModel.ExpressId);

			return Json(_outboundBLLV2.GetCountAndBatchNo(CurExpressId, Convert.ToInt32(selectStationValue)), JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult GetCount(string selectStationValue)
		{
			try
			{
				if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
				{
					return Json(new ViewGetCountAndBatchNo() {IsSuccess = false, Message = "请选择站点!"}, JsonRequestBehavior.AllowGet);
				}
				SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
				var CurExpressId = Convert.ToInt32(userModel.ExpressId);
				var viewGetCount = new ViewGetCountAndBatchNo();
				viewGetCount.Succeed();
				viewGetCount.CurOptCount = _outboundBLLV2.GetCurOutBoundCount(CurExpressId);
				viewGetCount.CurArrivalCount = _outboundBLLV2.GetCurDisOutBoundCount(CurExpressId,
				                                                                     Convert.ToInt32(selectStationValue));
				return Json(viewGetCount, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new ViewGetCountAndBatchNo() {IsSuccess = false, Message = ex.Message}, JsonRequestBehavior.AllowGet);
			}
		}

		#region 出库交接表相关
		/// <summary>
		/// 出库交接表
		/// </summary>
		/// <returns></returns>
		public ActionResult Print()
		{
			this.SetSearchListAjaxOptions();
			ViewBag.RedirectAction = "Print";
			if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
			{
				//分拣中心列表
				List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
				this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
				//配送商列表
				//List<ExpressCompanyModel> distributorList =
				//    _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
				List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetDistributionCooprelation(UserContext.CurrentUser.DistributionCode);
				this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
				return View();
			}
			//构建查询Model
			var searchModel = new OutboundPrintSearchModel();
			int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
			int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
			searchModel.PageIndex = pageIndex;
			searchModel.PageSize = pageSize;

			searchModel.TypeTime = Convert.ToInt32(Request["selTimeType"]);
			string BeginTime = Request["BeginTime"];
			string EndTime = Request["EndTime"];
			searchModel.StartTime = string.IsNullOrEmpty(BeginTime)
					 ? DateTime.MinValue
					 : Convert.ToDateTime(BeginTime);
			searchModel.EndTime = string.IsNullOrEmpty(EndTime)
					 ? DateTime.MaxValue
					 : Convert.ToDateTime(EndTime);
			//出库到得目的站点
			int station = -1;
			var selCityAndStation_Station = Request["selCityAndStation_Station"];
			if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
			{
				station = Convert.ToInt32(selCityAndStation_Station);
			}
			var SortingCenterList = Request["SortingCenterList"];
			if (SortingCenterList != null && SortingCenterList != "-1")
			{
				station = Convert.ToInt32(SortingCenterList);
			}
			var DistributorList = Request["DistributorList"];
			if (DistributorList != null && DistributorList != "-1")
			{
				station = Convert.ToInt32(DistributorList);
			}
			searchModel.ArrivalId = station;

			searchModel.ExpressId = UserContext.CurrentUser.DeptID;
			searchModel.BatchNo = Request["BatchNo"];
			searchModel.BoxNo = Request["BoxNo"];
			searchModel.FormCode = Request["FormCode"];

			//var list = _outboundBLLV2.SearchOutboundPrint(station, bt, et, UserContext.CurrentUser.DeptID, formColl["BatchNo"], formColl["WaybillNo"]);
			var list = _outboundBLLV2.SearchOutboundPrintV2(searchModel);
			ViewBag.HidData = BeginTime + "," + EndTime + "," + searchModel.BatchNo + "," + searchModel.BoxNo + "," +
							  searchModel.FormCode;
			return PartialView("_PartialPrintList", list);

		}

		//[HttpPost]
		//public ActionResult Print(FormCollection formColl)
		//{
		//    //构建查询Model
		//    var searchModel = new OutboundPrintSearchModel();

		//    int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
		//    int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
		//    searchModel.PageIndex = pageIndex;
		//    searchModel.PageSize = pageSize;

		//    searchModel.TypeTime = Convert.ToInt32(formColl["selTimeType"]);
		//    string BeginTime = formColl["BeginTime"];
		//    string EndTime = formColl["EndTime"];
		//    searchModel.StartTime = string.IsNullOrEmpty(BeginTime)
		//             ? DateTime.MinValue
		//             : Convert.ToDateTime(BeginTime);
		//    searchModel.EndTime = string.IsNullOrEmpty(EndTime)
		//             ? DateTime.MaxValue
		//             : Convert.ToDateTime(EndTime);
		//        //出库到得目的站点
		//    int station = -1;
		//    var selCityAndStation_Station = formColl["selCityAndStation_Station"];
		//    if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
		//    {
		//        station = Convert.ToInt32(selCityAndStation_Station);
		//    }
		//    var SortingCenterList = formColl["SortingCenterList"];
		//    if (SortingCenterList != null && SortingCenterList != "-1")
		//    {
		//        station = Convert.ToInt32(SortingCenterList);
		//    }
		//    var DistributorList = formColl["DistributorList"];
		//    if (DistributorList != null && DistributorList != "-1")
		//    {
		//        station = Convert.ToInt32(DistributorList);
		//    }
		//    searchModel.ArrivalId = station;

		//    searchModel.ExpressId = UserContext.CurrentUser.DeptID;
		//    searchModel.BatchNo = formColl["BatchNo"];
		//    searchModel.BoxNo = formColl["BoxNo"];
		//    searchModel.FormCode = formColl["FormCode"];

		//    //var list = _outboundBLLV2.SearchOutboundPrint(station, bt, et, UserContext.CurrentUser.DeptID, formColl["BatchNo"], formColl["WaybillNo"]);
		//    var list = _outboundBLLV2.SearchOutboundPrintV2(searchModel);
		//    ViewBag.HidData = BeginTime + "," + EndTime + "," + searchModel.BatchNo + "," + searchModel.BoxNo + "," +
		//                      searchModel.FormCode;
		//    return PartialView("_PartialPrintList", list);
		//}

		/// <summary>
		/// 打印交接表(已打印)
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		public ActionResult PrintReceipt(List<string> batchNo)
		{
			var ReceiptList = _outboundBLLV2.GetOutboundPrintReceiptByBatchNos(batchNo);

			ViewBag.curDistributionName = GetDistributionName(UserContext.CurrentUser.DistributionCode);
			return View(ReceiptList);
		}

		/// <summary>
		/// 打印交接表（未打印）
		/// </summary>
		/// <param name="searchArg"></param>
		/// <param name="arrivedList"></param>
		/// <returns></returns>
		public ActionResult PrintReceiptV2(string searchArg, string arrivedList)
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
			searchModel.ArrivalIdList = arrivedList;
			//生成批次号并更新SC_outbound中的batchNo
			var result = _outboundBLLV2.CreateAndUpdateBatchNo(searchModel);
			IList<OutboundPrintModelV2> ReceiptList = new List<OutboundPrintModelV2>();
			if (result.IsSuccess)
			{
				ReceiptList = _outboundBLLV2.GetOutboundPrintReceipt(searchModel);
			}
			else
			{
				ReceiptList = null;
			}
			ViewBag.curDistributionName = GetDistributionName(UserContext.CurrentUser.DistributionCode);
			return View("PrintReceipt", ReceiptList);
		}

		[HttpPost]
		public ActionResult Export(string batchNos)
		{
			var list = batchNos.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			var ReportList = _outboundBLLV2.GetOutboundPrintExportModel(list);
			var bs = ReportList.ExportToExcel("出库单明细");
			MemoryStream stream = new MemoryStream(bs);
			//using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
			//{
			//    helper.CreateNewWorksheet("出库单明细");
			//    helper.WriteData(ReportList.To2Array());
			//    helper.Save();
			//}
			stream.Seek(0, 0);
			return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = "出库单明细.xls" };
		}
		
		/// <summary>
		/// 导出出库明细
		/// </summary>
		/// <param name="batchNos"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ExportPrint(string batchNos)
		{
			var list = batchNos.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
		    var ReportList = _outboundBLLV2.GetOutboundPrintExportModelV2(list);

			MemoryStream stream = new MemoryStream();
			using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
			{
				helper.CreateNewWorksheet("出库单明细");
				helper.WriteData(ReportList.To2Array());
				helper.Save();
			}
			stream.Seek(0, 0);
			return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("出库单明细 " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx") };

			//var bs = ReportList.ExportToExcel("出库单明细");
			//MemoryStream stream = new MemoryStream(bs);
			//stream.Seek(0, 0);
			//return new FileStreamResult(stream, "application/vnd.ms-excel")
			//    {
			//        FileDownloadName = "出库单明细 " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
			//    };
		}

		/// <summary>
		/// 导出出库明细
		/// </summary>
		/// <param name="searchArg"></param>
		/// <param name="arrivedList"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ExportNoprint(string searchArg, string arrivedList)
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
			searchModel.ArrivalIdList = arrivedList;
			var ReportList = _outboundBLLV2.GetOutboundPrintExportModelV2(searchModel);

			MemoryStream stream = new MemoryStream();
			using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
			{
				helper.CreateNewWorksheet("出库单明细");
				helper.WriteData(ReportList.To2Array());
				helper.Save();
			}
			stream.Seek(0, 0);
			return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("出库单明细 " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx") };

			//var bs = ReportList.ExportToExcel("出库单明细");
			//MemoryStream stream = new MemoryStream(bs);
			//stream.Seek(0, 0);
			//return new FileStreamResult(stream, "application/vnd.ms-excel")
			//    {
			//        FileDownloadName = "出库单明细 " + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
			//    };
		}

		/// <summary>
		/// 打印批次明细
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		public ActionResult PrintBatchDetails(string batchNo)
		{
			var modelList = _outboundBLLV2.GetPrintBatchDetail(batchNo);
			return View(modelList);
		}

		/// <summary>
		/// 出库打印发送邮件
		/// </summary>
		/// <param name="batchNo"></param>
		/// <returns></returns>
		public JsonResult SendEmail(string batchInfo)
		{
			Request.BinaryRead(Request.ContentLength);
			//按ExpressCompanyId分组
			var groupInfo = batchInfo.Trim(',').Split(',')
				.Select(x => new
				{
					ExpressCompanyId = int.Parse(x.Split('|')[0]),
					BatchNo = x.Split('|')[1],
					Email = x.Split('|')[2],
				}).GroupBy(x => x.ExpressCompanyId);

			//加入当前站点邮件
			string currEmail = string.Empty;
			var ec = _expressCompanyBLL.Get(UserContext.CurrentUser.DeptID);
			if (ec != null && !string.IsNullOrWhiteSpace(ec.Email))
			{
				currEmail = ";" + ec.Email;
			}

			foreach (var group in groupInfo)
			{
				var expressCompanyId = group.Key;
				var batchNoList = group.Select(x => x.BatchNo);
				var emailList = group.Select(x => (x.Email + currEmail).ToLower()).FirstOrDefault().Split(';').Distinct();

				_outboundBLLV2.OutBoundSendEmail(expressCompanyId, batchNoList, emailList);
			}
			return Json(ResultModel.Create(true, batchInfo));
		}

		/// <summary>
		/// 出库交接表发送邮件
		/// </summary>
		/// <param name="batchInfo"></param>
		/// <returns></returns>
		public JsonResult SendEmailV2(string batchInfo, string selTimeType, string searchArg)
		{
			Request.BinaryRead(Request.ContentLength);
			//按ExpressCompanyId分组
			var groupInfo = batchInfo.Trim(',').Split(',')
			.Select(x => new
			{
				ExpressCompanyId = int.Parse(x.Split('|')[0]),
				BatchNo = x.Split('|')[1],
				Email = x.Split('|')[2],
			}).GroupBy(x => x.ExpressCompanyId);
			
			//加入当前站点邮件
			string currEmail = string.Empty;
			var ec = _expressCompanyBLL.Get(UserContext.CurrentUser.DeptID);
			if (ec != null && !string.IsNullOrWhiteSpace(ec.Email))
			{
				currEmail = ";" + ec.Email;
			}
			foreach (var group in groupInfo)
			{
				var expressCompanyId = group.Key;
				var batchNoList = group.Select(x => x.BatchNo);
				var emailList = group.Select(x => (x.Email + currEmail).ToLower()).FirstOrDefault().Split(';').Distinct();
				_outboundBLLV2.OutBoundSendEmailV2(expressCompanyId, batchNoList, emailList, selTimeType, searchArg);
			}
			return Json(ResultModel.Create(true, batchInfo));
			
		}

		#endregion
		/// <summary>
		/// 获得当前配送商名称
		/// </summary>
		/// <param name="distributionCode"></param>
		/// <returns></returns>
		public string GetDistributionName(string distributionCode)
		{
			var distributorList = UserContext.CurrentUser.DistributionCode;
			var exps = _expressCompanyBLL.GetAllDistributors();
			string curDistributionName = "";
			foreach (var exp in exps)
			{
				if (distributorList.Split(',').ToList().Contains(exp.DistributionCode))
				{
					curDistributionName = exp.CompanyAllName;
					return curDistributionName;
				}
			}
			return curDistributionName;
		}


		#region   批量操作（暂未使用）
		/// <summary>
		/// 按箱出库(出库)
		/// 单箱和多箱
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult BoxOutBoundV2(string selectedType, string selectStationValue, string arrBoxNos)
		{
			ViewOutboundBatchModel result = new ViewOutboundBatchModel();
			List<ViewOutboundBatchModel> resultList = new List<ViewOutboundBatchModel>();
			if (String.IsNullOrWhiteSpace(selectedType))
			{
				result.IsSuccess = false;
				result.Message = "请选择分拣类型";
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			if (String.IsNullOrWhiteSpace(selectStationValue))
			{
				result.IsSuccess = false;
				result.Message = "请选择出库站点";
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			if (String.IsNullOrWhiteSpace(arrBoxNos))
			{
				result.IsSuccess = false;
				result.Message = "请选择需要出库的箱号";
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			resultList = _outboundBLLV2.OutboundByBox(new OutboundByBoxArgModel()
			{
				BoxNos = arrBoxNos.Split(',').ToList(),
				OpUser = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID),
				PreCondition = _outboundBLLV2.GetPreCondition(UserContext.CurrentUser.DistributionCode),
				ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue))
			});
			return Json(resultList, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 批量出库（按单号s）
		/// </summary>
		/// <param name="selectedType"></param>
		/// <param name="selectStationValue"></param>
		/// <param name="arrCodes"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult BatchOutboundByCodes(String selectedType, String selectStationValue, String arrCodes)
		{
			//ViewOutboundBatchModel result = null;
			ViewOutboundSimpleModel result = null;
			List<ViewOutboundSimpleModel> resultList = new List<ViewOutboundSimpleModel>();
			if (String.IsNullOrWhiteSpace(selectedType))
			{
				result = new ViewOutboundSimpleModel() { IsSuccess = false, Message = "请选择分拣类型" };
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			if (String.IsNullOrWhiteSpace(selectStationValue))
			{
				result = new ViewOutboundSimpleModel() { IsSuccess = false, Message = "请选择出库站点" };
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			if (String.IsNullOrWhiteSpace(arrCodes))
			{
				result = new ViewOutboundSimpleModel() { IsSuccess = false, Message = "请选择需要出库的运单" };
				resultList.Add(result);
				return Json(resultList, JsonRequestBehavior.AllowGet);
			}
			//从arrCodes中取值逐单出库
			SortCenterUserModel userModel = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
			var argument = new OutboundSimpleArgModel();
			argument.OpUser = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID);
			argument.PreCondition = _outboundBLLV2.GetPreCondition(UserContext.CurrentUser.DistributionCode);
			argument.ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue));
			argument.FormType = Enums.SortCenterFormType.Waybill;
			argument.BatchNo = "0";
			string[] ArrFromCode = arrCodes.Split(',');
			foreach (var item in ArrFromCode)
			{
				argument.FormCode = item;
				//逐单出库
				result = _outboundBLLV2.SimpleOutbound(argument,true);
				//获取最新的出库数量
				result.CurOptCount = _outboundBLLV2.GetCurOutBoundCount(Convert.ToInt32(userModel.ExpressId));
				result.CurArrivalCount = _outboundBLLV2.GetCurDisOutBoundCount(Convert.ToInt32(userModel.ExpressId),
																			   Convert.ToInt32(selectStationValue));
				resultList.Add(result);
			}

			//result = _outboundBLLV2.SearchOutbound(new OutboundSearchArgModel()
			//    {
			//        ArrFormCode = arrCodes.Split(','),
			//        FormType = Enums.SortCenterFormType.Waybill,
			//        OpUser = _outboundBLLV2.GetUserModel(UserContext.CurrentUser.ID),
			//        PerBatchCount = 20,
			//        PreCondition = _outboundBLLV2.GetPreCondition(UserContext.CurrentUser.DistributionCode),
			//        ToStation = _outboundBLLV2.GetToStationModel(int.Parse(selectStationValue))
			//    });
			//return Json(result, JsonRequestBehavior.AllowGet);
			return Json(resultList, JsonRequestBehavior.AllowGet);
		}
		#endregion
	}
}

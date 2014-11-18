using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Sorting.Outbound;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using System.IO;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class OutboundController : Controller
    {
        IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        IOutboundBLL _outboundBLL = ServiceFactory.GetService<IOutboundBLL>("SC_OutboundBLL");
        IBillBLL _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        IUserContextBLL userContextService = ServiceFactory.GetService<IUserContextBLL>("UserContextBLL");
        //
        // GET: /Sorting/Inbound/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SimpleOutbound()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            SortCenterUserModel userModel = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID);
            OutboundPreConditionModel preCondition = null;
            if (userModel != null)
            {
                preCondition = _outboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode);
            }
            ViewOutboundValidateModel viewModel = new ViewOutboundValidateModel(userModel, preCondition);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

            return View();
        }

        public ActionResult OutBoundByBox()
        {
            this.SetSearchListAjaxOptions();
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            SortCenterUserModel userModel = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID);
            OutboundPreConditionModel preCondition = null;
            if (userModel != null)
            {
                preCondition = _outboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode);
            }
            ViewOutboundValidateModel viewModel = new ViewOutboundValidateModel(userModel, preCondition);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

            return View();
        }

        /// <summary>
        /// 扫描出库
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScanFormCode(string hidData, int selectedType, string selectStationValue, string code, int formType, string BatchNo)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                return Json(new ViewOutboundSimpleModel() { IsSuccess = false, Message = "输入的单号为空" }, JsonRequestBehavior.AllowGet);
            }
            var OutboundData = JsonHelper.ConvertToObject<ViewOutboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!OutboundData.IsValidate)
            {
                return Json(new ViewOutboundSimpleModel() { IsSuccess = false, Message = OutboundData.ValidateMsg }, JsonRequestBehavior.AllowGet);
            }
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new ViewOutboundSimpleModel() { IsSuccess = false, Message = "请选择站点类型!" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new ViewOutboundSimpleModel() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
            }
            var argument = new OutboundSimpleArgModel() { OpUser = OutboundData.OpUser, PreCondition = OutboundData.PreCondition };
            argument.ToStation = _outboundBLL.GetToStationModel(int.Parse(selectStationValue));
            argument.FormType = (Enums.SortCenterFormType)formType;
            argument.FormCode = code.Trim();
            argument.BatchNo = BatchNo;

            return Json(_outboundBLL.SimpleOutbound(argument), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 查询出库【出库】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SortCenter_Search_Outbound(String selectedType, String selectStationValue, String arrCodes)
        {
            ViewOutboundBatchModel result = null;
            if (String.IsNullOrWhiteSpace(selectedType))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择分拣类型" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrWhiteSpace(selectStationValue))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择出库站点" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrWhiteSpace(arrCodes))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择需要出库的运单" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = _outboundBLL.SearchOutbound(new OutboundSearchArgModel()
            {
                ArrFormCode = arrCodes.Split(','),
                FormType = Enums.SortCenterFormType.Waybill,
                OpUser = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID),
                PerBatchCount = 20,
                PreCondition = _outboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode),
                ToStation = _outboundBLL.GetToStationModel(int.Parse(selectStationValue))
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 按箱出库(出库)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BoxOutBound(string selectedType, string selectStationValue, string arrCodes)
        {
            ResultModel result = new ResultModel();
            if (String.IsNullOrWhiteSpace(selectedType))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择分拣类型" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrWhiteSpace(selectStationValue))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择出库站点" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrWhiteSpace(arrCodes))
            {
                result = new ViewOutboundBatchModel() { IsSuccess = false, Message = "请选择需要出库的运单" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result = _outboundBLL.OutboundByBox(new OutboundByBoxArgModel()
           {
               BoxNos = arrCodes.Split(',').ToList(),
               OpUser = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID),
               PreCondition = _outboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode),
               ToStation = _outboundBLL.GetToStationModel(int.Parse(selectStationValue))
           });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(string id)
        {
            ViewOutBoundBoxDetailModel model = _outboundBLL.GetBoxBillsByBoxNo(id);
            return View("OutBoundBoxDetail", model);
        }

        /// <summary>
        /// 按箱出库(查询)
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult SearchOutboundByBox()
        {
            if (Request.IsAjaxRequest())
            {
                int chooseType = Convert.ToInt32(Request["sortingCenterSelect"]);
                int ArrivalID = -1;
                if (chooseType == (int)Enums.SortCenterOperateType.SimpleSorting)
                {
                    var selCityAndStation_Station = Request["selCityAndStation_Station"];
                    if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
                    {
                        ArrivalID = Convert.ToInt32(selCityAndStation_Station);
                    }
                }
                else if (chooseType == (int)Enums.SortCenterOperateType.SecondSorting)
                {
                    var SortingCenterList = Request["SortingCenterList"];
                    if (SortingCenterList != null && SortingCenterList != "-1")
                    {
                        ArrivalID = Convert.ToInt32(SortingCenterList);
                    }
                }
                else
                {
                    var DistributorList = Request["DistributorList"];
                    if (DistributorList != null && DistributorList != "-1")
                    {
                        ArrivalID = Convert.ToInt32(DistributorList);
                    }
                }
                OutboundSearchModel searchModel = new OutboundSearchModel();
                searchModel.ArrivalID = ArrivalID;
                searchModel.InboundEndTime = Convert.ToDateTime(Request["InboundEndTime"]);
                searchModel.InboundStartTime = Convert.ToDateTime(Request["InboundStartTime"]);
                searchModel.BoxNo = Request["BoxNo"].Trim();
                searchModel.OpUser = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID);
                List<ViewOutBoundByBoxModel> list = _outboundBLL.SearchOutBoundByBox(searchModel);
                return PartialView("_PartialOutBoundByBox", list);
            }

            return View();
        }

        /// <summary>
        /// 查询出库【查询】
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchOutbound(OutboundSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();
            ViewBag.RedirectAction = "SearchOutbound";
            if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
            {
                InitOutboundSearchSelectList();
                //初始化页面时不查询
                return View();
            }
            if (String.IsNullOrWhiteSpace(Request["sortingCenterSelect"]))
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择[分拣类型]" });
            }
            var chooseType = Convert.ToInt32(Request["sortingCenterSelect"]);
            int ArrivalID = -1;
            if (chooseType == (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                var selCityAndStation_Station = Request["selCityAndStation_Station"];
                if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
                {
                    ArrivalID = Convert.ToInt32(selCityAndStation_Station);
                }
            }
            else if (chooseType == (int)Enums.SortCenterOperateType.SecondSorting)
            {
                var SortingCenterList = Request["SortingCenterList"];
                if (SortingCenterList != null && SortingCenterList != "-1")
                {
                    ArrivalID = Convert.ToInt32(SortingCenterList);
                }
            }
            else
            {
                var DistributorList = Request["DistributorList"];
                if (DistributorList != null && DistributorList != "-1")
                {
                    ArrivalID = Convert.ToInt32(DistributorList);
                }
            }
            if (ArrivalID <= 0)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择[目的地]" });
            }
            if (!searchModel.InboundStartTime.HasValue)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请输入[入库开始时间]" });
            }
            if (!searchModel.InboundEndTime.HasValue)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请输入[入库结束时间]" });
            }
            searchModel.ArrivalID = ArrivalID;
            OutboundSearchArgModel searchArg = new OutboundSearchArgModel()
            {
                InboundStartTime = searchModel.InboundStartTime,
                InboundEndTime = searchModel.InboundEndTime,
                OpUser = _outboundBLL.GetUserModel(UserContext.CurrentUser.ID),
                PreCondition = _outboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode),
                ToStation = _outboundBLL.GetToStationModel(searchModel.ArrivalID)
            };
            
            if (Request.IsAjaxRequest())
            {
                var ViewOutbound = _outboundBLL.GetNeededOutboundInfo(searchArg);
                return PartialView("_PartialSearchOutboundList", ViewOutbound);
            }

            InitOutboundSearchSelectList();
            return View();
        }

        /// <summary>
        /// 初始化查询出库检索条件
        /// </summary>
        private void InitOutboundSearchSelectList()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
        }

        #region 出库打印相关

        /// <summary>
        /// 出库打印
        /// </summary>
        /// <returns></returns>
        public ActionResult Print()
        {
            this.SetSearchListAjaxOptions();

            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            return View();
        }

        [HttpPost]
        public ActionResult Print(FormCollection formColl)
        {
            int station = -1;
            var selCityAndStation_Station = formColl["selCityAndStation_Station"];
            if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
            {
                station = Convert.ToInt32(selCityAndStation_Station);
            }
            var SortingCenterList = formColl["SortingCenterList"];
            if (SortingCenterList != null && SortingCenterList != "-1")
            {
                station = Convert.ToInt32(SortingCenterList);
            }
            var DistributorList = formColl["DistributorList"];
            if (DistributorList != null && DistributorList != "-1")
            {
                station = Convert.ToInt32(DistributorList);
            }

            DateTime bt, et;
            string BeginTime = formColl["BeginTime"];
            string EndTime = formColl["EndTime"];
            bt = string.IsNullOrEmpty(BeginTime)
                     ? DateTime.MinValue
                     : Convert.ToDateTime(BeginTime);
            et = string.IsNullOrEmpty(EndTime)
                     ? DateTime.MaxValue
                     : Convert.ToDateTime(EndTime);

            var list = _outboundBLL.SearchOutboundPrint(station, bt, et, UserContext.CurrentUser.DeptID, formColl["BatchNo"], formColl["WaybillNo"]);
            return PartialView("_PartialPrintList", list);
        }

        [HttpPost]
        public ActionResult Export(string batchNos)
        {
            var list = batchNos.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            var ReportList = _outboundBLL.GetOutboundPrintExportModel(list);
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
        /// 打印交接表
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public ActionResult PrintReceipt(List<string> batchNo)
        {
            var ReceiptList = _outboundBLL.GetOrderCount(batchNo);
            return View(ReceiptList);
        }

        /// <summary>
        /// 打印批次明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public ActionResult PrintBatchDetails(string batchNo)
        {
            var modelList = _outboundBLL.GetPrintBatchDetail(batchNo);
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


                _outboundBLL.OutBoundSendEmail(expressCompanyId, batchNoList, emailList);
            }
            return Json(ResultModel.Create(true, batchInfo));
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vancl.TMS.Web.Models;
using Vancl.TMS.Web.Areas.BaseInfo.Models;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using System.Web.Script.Serialization;
using System.Text;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.Web.Areas.BaseInfo.Controllers
{
    public class LinePlanController : Controller
    {
        ILinePlanBLL _linePlanService = ServiceFactory.GetService<ILinePlanBLL>();
        ICarrierBLL _carrierService = ServiceFactory.GetService<ICarrierBLL>("CarrierBLL");


        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="LPID"></param>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public ActionResult ShowOperateLog(String LPID, String LineID)
        {
            IOperateLogBLL oplog = _linePlanService as IOperateLogBLL;
            return View("OperateLog",
             oplog.Read(new LineOperateLogSearchModel()
             {
                 KeyValue = LPID,
                 LineID = LineID
             })
            );
        }

        public ActionResult Create()
        {
            ViewBag.Operate = "NewLine";
            InitPageData();
            return View();
        }

        [HttpPost]
        public ActionResult Create(LinePlanViewModel model)
        {
            ViewBag.Operate = "NewLine";
            if (ModelState.IsValid)
            {
                model.LineGoodsType = GetLineGoodsType();
                if (_linePlanService.IsExsitLinePlan(model))
                {
                    ViewBag.IsRepeat = true;
                    InitPageData(model);
                    return View(model);
                }

                ResultModel result = _linePlanService.Add(model, GetLinePrice(model));
                return View("SaveResult", result);
            }
            InitPageData(model);
            return View();
        }

        /// <summary>
        /// 填充线路计划修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {
            ViewBag.Operate = "Update";

            LinePlanModel model = _linePlanService.GetLinePlan(id);
            IList<ILinePrice> priceList = _linePlanService.GetLinePrice(id, model.ExpressionType);

            LinePlanViewModel lineViewModel = new LinePlanViewModel(model);

            lineViewModel.LineExpression = System.Web.Helpers.Json.Encode(priceList);
            InitPageData(lineViewModel);

            return View("Create", lineViewModel);
        }

        /// <summary>
        /// 修改线路计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(LinePlanViewModel model)
        {
            ViewBag.Operate = "Update";
            if (ModelState.IsValid)
            {
                model.LineGoodsType = GetLineGoodsType();
                ResultModel result = _linePlanService.Update(model, GetLinePrice(model));
                return View("SaveResult", result);
            }
            InitPageData(model);
            return View("Create", model);
        }

        /// <summary>
        /// 填充新增线路计划页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateLinePlan(int id)
        {
            ViewBag.Operate = "NewLinePlan";

            LinePlanModel model = _linePlanService.GetLinePlan(id);
            IList<ILinePrice> priceList = _linePlanService.GetLinePrice(id, model.ExpressionType);

            LinePlanViewModel lineViewModel = new LinePlanViewModel(model);

            lineViewModel.LineExpression = System.Web.Helpers.Json.Encode(priceList);

            InitPageData(lineViewModel);
            return View("Create", lineViewModel);
        }

        [HttpPost]
        public ActionResult CreateLinePlan(LinePlanViewModel model)
        {
            ViewBag.Operate = "NewLinePlan";
            if (ModelState.IsValid)
            {
                model.LineGoodsType = GetLineGoodsType();

                ResultModel result = _linePlanService.Add(model, GetLinePrice(model));
                return View("SaveResult", result);
            }
            InitPageData(model);
            return View("Create", model);
        }

        public ActionResult AuditLinePlan()
        {
            return View();
        }


        /// <summary>
        /// 审核线路计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AuditLinePlan(int id)
        {
            var approveStatus = (Enums.LineStatus)int.Parse(Request["ApproveStatus"]);
            var effectiveTime = DateTime.MinValue;
            if (Request["EffectiveTime"] != null && !string.IsNullOrEmpty(Request["EffectiveTime"]))
            {
                effectiveTime = DateTime.Parse(Request["EffectiveTime"]);
            }
            ResultModel result = _linePlanService.AuditLinePlan(id, approveStatus, effectiveTime);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BatchAudit()
        {
            var lpids = Request["lpids"];
            var status = (Enums.LineStatus)int.Parse(Request["ApproveStatus"]);
            List<int> lpidList = lpids.Split(',').Select(n => int.Parse(n)).ToList();
            ResultModel result = _linePlanService.BatchAuditLinePlan(lpidList, status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 线路计划列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            ViewBag.Carriers = _carrierService.GetAll().Select(m =>
                              new SelectListItem() { Text = m.CarrierName, Value = m.CarrierID.ToString() });
            this.SendEnumSelectListToView<Enums.TransportType>("TransportType");
            this.SendEnumSelectListToView<Enums.LineType>("LineType");
            this.SendEnumSelectListToView<Enums.LineStatus>("LineStatus");

            this.SetSearchListAjaxOptions();

            LinePlanSearchModel searchModel = GetSearchCondition();
            if(Request["LineStatus"] != null && !string.IsNullOrEmpty(Request["LineStatus"]))
            {
                searchModel.Status = (Enums.LineStatus)int.Parse(Request["LineStatus"]);
            }
            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }

            if (Request.IsAjaxRequest())
            {
                IList<ViewLinePlanModel> lines = _linePlanService.GetLinePlan(searchModel);
                return PartialView("_PartialLinePlanList", lines);
            }
            return View();
        }

        /// <summary>
        /// 线路计划未审核列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditList()
        {
            ViewBag.Carriers = _carrierService.GetAll().Select(m =>
                              new SelectListItem() { Text = m.CarrierName, Value = m.CarrierID.ToString() });
            this.SendEnumSelectListToView<Enums.TransportType>("TransportType");
            this.SendEnumSelectListToView<Enums.LineType>("LineType");

            this.SetSearchListAjaxOptions();

            LinePlanSearchModel searchModel = GetSearchCondition();
            searchModel.Status = Enums.LineStatus.NotApprove;
            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }
            IList<ViewLinePlanModel> lines = _linePlanService.GetLinePlan(searchModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialAuditLinePlanList", lines);
            }
            return View(lines);
        }

        /// <summary>
        /// 获取线路价格
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetExpressionDetail()
        {
            var lpid = 0;
            Enums.ExpressionType expressionType = Enums.ExpressionType.Fixed;
            if (Request["LPID"] != null && !string.IsNullOrEmpty(Request["LPID"]))
            {
                lpid = int.Parse(Request["LPID"]);
            }
            if (Request["ExpressionType"] != null && !string.IsNullOrEmpty(Request["ExpressionType"]))
            {
                expressionType = (Enums.ExpressionType)int.Parse(Request["ExpressionType"]);
            }
            IList<ILinePrice> priceList = _linePlanService.GetLinePrice(lpid, expressionType);
            if (priceList == null) priceList = new List<ILinePrice>();

            return Json(new { IsSuccess=true,Expression=System.Web.Helpers.Json.Encode(priceList) }
                ,JsonRequestBehavior.AllowGet);
        }


        public ActionResult SetFixedPrice()
        {
            return View();
        }

        public ActionResult SetLadderPrice()
        {
            return View();
        }

        public ActionResult SetFormulaPrice()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Delete()
        {
            string ids = Request["ids"];
            List<int> lpidList = ids.Split(',').Select(n => int.Parse(n)).ToList();

            ResultModel result = _linePlanService.Delete(lpidList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SetEnable()
        {
            string lineids = Request["lineids"];
            bool isEnabled = bool.Parse(Request["isEnabled"]);
            List<string> lineidList = lineids.Split(',').ToList<string>();
            ResultModel result = _linePlanService.SetIsEnabled(lineidList, isEnabled);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 私有方法

        /// <summary>
        /// 将线路运费字符串转换成对应费用模型列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private IList<ILinePrice> GetLinePrice(LinePlanViewModel model)
        {
            IList<ILinePrice> linePriceModelList = new List<ILinePrice>();
            //JavaScriptSerializer js = new JavaScriptSerializer();
            var lineExpress = model.LineExpression;
            if (string.IsNullOrEmpty(lineExpress))
            {
                return null;
            }
            if (model.ExpressionType == Enums.ExpressionType.Fixed)
            {
                IList<LineFixedPriceModel> fixedPriceList =
                    System.Web.Helpers.Json.Decode<IList<LineFixedPriceModel>>(lineExpress);
                linePriceModelList = fixedPriceList.ConvertToFatherListModel<ILinePrice, LineFixedPriceModel>();
            }
            else if (model.ExpressionType == Enums.ExpressionType.Ladder)
            {
                IList<LineLadderPriceModel> ladderPriceList =
                    System.Web.Helpers.Json.Decode<IList<LineLadderPriceModel>>(lineExpress);
                linePriceModelList = ladderPriceList.ConvertToFatherListModel<ILinePrice, LineLadderPriceModel>();
            }
            else
            {
                IList<LineFormulaPriceModel> formulaPriceList =
                    System.Web.Helpers.Json.Decode<IList<LineFormulaPriceModel>>(lineExpress);
                linePriceModelList = formulaPriceList.ConvertToFatherListModel<ILinePrice, LineFormulaPriceModel>();
            }
            return linePriceModelList;
        }

        private Enums.GoodsType GetLineGoodsType()
        {
            string lineGoodsType = Request["LineGoodsType"];
            string[] arr = lineGoodsType.Split(',');
            Enums.GoodsType gt = 0;
            int val = 0;
            foreach (string str in arr)
            {
                if (int.TryParse(str, out val))
                {
                    gt = gt | (Enums.GoodsType)val;
                }
            }
            return gt;
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private LinePlanSearchModel GetSearchCondition()
        {
            LinePlanSearchModel searchModel = new LinePlanSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            if (Request["S_ArrivalID"] != null && Request["S_ArrivalID"] != "")
            {
                searchModel.ArrivalID = Convert.ToInt32(Request["S_ArrivalID"]);
            }
            if (Request["S_DepartureID"] != null && Request["S_DepartureID"] != "")
            {
                searchModel.DepartureID = Convert.ToInt32(Request["S_DepartureID"]);
            }
            if (Request["S_CarrierID"] != null && Request["S_CarrierID"] != "")
            {
                searchModel.CarrierID = Convert.ToInt32(Request["S_CarrierID"]);
            }
            if (Request["TransportType"] != null && Request["TransportType"] != "")
            {
                searchModel.TransportType = (Enums.TransportType)int.Parse(Request["TransportType"]);
            }
            if (Request["LineType"] != null && Request["LineType"] != "")
            {
                searchModel.LineType = (Enums.LineType)int.Parse(Request["LineType"]);
            }

            return searchModel;
        }

        private void InitPageData(LinePlanViewModel model = null)
        {
            ViewBag.Carriers = _carrierService.GetAll().Select(m =>
                               new SelectListItem() { Text = m.CarrierName, Value = m.CarrierID.ToString() });
            if (model == null)
            {
                this.SendEnumSelectListToView<Enums.GoodsType>("LineGoodsType", Enums.GoodsType.Normal);
                this.SendEnumSelectListToView<Enums.ExpressionType>("ExpressionType");
                this.SendEnumSelectListToView("TransportType", Enums.TransportType.Railway);
                this.SendEnumSelectListToView("Priority", Enums.LinePriority.Priority);
                this.SendEnumSelectListToView("LineType", Enums.LineType.DeliveryLine);
                this.SendEnumSelectListToView("BusinessType", Enums.BusinessType.Outsourcing);
            }
            else
            {
                this.SendEnumSelectListToView("LineGoodsType", model.LineGoodsType);
                this.SendEnumSelectListToView("ExpressionType", model.ExpressionType);
                this.SendEnumSelectListToView("TransportType", model.TransportType);
                this.SendEnumSelectListToView("Priority", model.Priority);
                this.SendEnumSelectListToView("LineType", model.LineType);
                this.SendEnumSelectListToView("BusinessType", model.BusinessType);
            }
        }

        #endregion
    }
}

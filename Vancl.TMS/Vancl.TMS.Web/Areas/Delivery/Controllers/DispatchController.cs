using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Web.Areas.Delivery.Models;
using Vancl.TMS.Core.Logging;

namespace Vancl.TMS.Web.Areas.Delivery.Controllers
{
    /// <summary>
    /// 运输调度管理
    /// </summary>
    public class DispatchController : Controller
    {
        IDispatchBLL DispatchService = ServiceFactory.GetService<IDispatchBLL>();
        IPreDispatchBLL PreDispatchService = ServiceFactory.GetService<IPreDispatchBLL>();
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();
        ILinePlanBLL LinePlanService = ServiceFactory.GetService<ILinePlanBLL>();
        IFormula<string, SerialNumberModel> SerialNumberService = FormulasFactory.GetFormula<IFormula<string, SerialNumberModel>>("DeliveryNoGenerateFormula");

        /// <summary>
        /// 信息列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(DispatchSearchModel searchModel)
        {

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            this.SetSearchListAjaxOptions();
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (!string.IsNullOrWhiteSpace(Request["CarrierID"]))
            {
                searchModel.CarrierID = Convert.ToInt32(Request["CarrierID"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["StartTime"]))
            {
                searchModel.StartTime = Convert.ToDateTime(Request["StartTime"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["EndTime"]))
            {
                searchModel.EndTime = Convert.ToDateTime(Request["EndTime"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["DispatchingPageStatus"]))
            {
                searchModel.DispatchingPageStatus = (Enums.DispatchingPageStatus)Convert.ToInt32(Request["DispatchingPageStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["TransportType"]))
            {
                searchModel.TransportType = (Enums.TransportType)Convert.ToInt32(Request["TransportType"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["DepartureID"]))
            {
                searchModel.DepartureID = Convert.ToInt32(Request["DepartureID"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["ArrivalID"]))
            {
                searchModel.ArrivalID = Convert.ToInt32(Request["ArrivalID"]);
            }

            
            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }
            searchModel.Trim();
            ViewBag.ViewDispatchStatisticModel = new ViewDispatchStatisticModel();
            if (Request.IsAjaxRequest())
            {
                ViewBag.ViewDispatchStatisticModel = DispatchService.GetStatisticInfoEx(searchModel);
                var model = DispatchService.SearchEx(searchModel);
                return PartialView("_PartialDispatchList", model);
            }
            InitSelectList();
            return View();
        }

        /// <summary>
        /// 调度出库
        /// </summary>
        /// <param name="id">LPID</param>
        /// <returns></returns>
        public ActionResult Outbound(int lpid, int departureID, int arrivalID, int lineGoodsType, string preParams, string waybillNo, long did)
        {
            ViewBag.SerialNumber = SerialNumberService.Execute(new SerialNumberModel { FillerCharacter = "0", NumberLength = 4 });
            ViewBag.PreParams = preParams;
            ViewBag.WaybillNo = waybillNo;
            ViewBag.DID = did;
            //var model = PreDispatchService.GetPreDispatchBoxList(lpid);
            var model = PreDispatchService.GetPreDispatchBoxListV1(lpid);
            ViewBag.LinePlan = LinePlanService.GetViewLinePlan(lpid);
            //ViewBag.OP = "Update";
            ViewBag.Title = "运输调度出库";
            return View(model);
        }

        public ActionResult Update(int? id, string deliveryNo, int lpid, string preParams)
        {
            var model = DispatchService.GetDispatchedBoxList(deliveryNo);
            ViewBag.SerialNumber = deliveryNo;
            ViewBag.LinePlan = LinePlanService.GetViewLinePlan(lpid);
            ViewBag.Title = "运输调度修改";
            ViewBag.OP = "Update";
            ViewBag.PreParams = preParams;
            return View("Outbound", model);
        }

        public ActionResult OutboundLinePlan(int id)
        {
            var model = LinePlanService.GetViewLinePlan(id);
            return PartialView("_PartialOutboundLinePlan", model);
        }

        /// <summary>
        /// 添加箱子
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBox(int? id)
        {
            return View();
        }

        public ActionResult AddBoxList(int? id, int departureID, string dispatchedBoxes, string departureName, string arrivalName)
        {
            var boxes = dispatchedBoxes.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var model = DispatchService.GetValidBoxList(departureID, boxes, departureName, arrivalName);
            return View(model);
        }

        /// <summary>
        /// 撤回已调度单据
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdraw()
        {
            return View();
        }

        /// <summary>
        /// 输入物流单号码
        /// </summary>
        /// <returns></returns>
        public ActionResult InputWaybillNo(InputWaybillNoModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult InputWaybillNo(InputWaybillNoModel model, string deliveryNo)
        {
            if (ModelState.IsValid)
            {
                var result = DispatchService.UpdateWaybillNoByDeliveryNo(model.WaybillNo ?? " ", model.DeliveryNo);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ResultModel.Create(false, "请输入正确的物流单号！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 确认调度
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="waybillno"></param>
        /// <param name="lpid"></param>
        /// <param name="dispatchedBoxes"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <remarks>
        /// 该方法为异步调用
        /// </remarks>
        [HttpPost]
        public JsonResult ConfirmDispatch(string deliveryNo, string waybillno, int? lpid, string dispatchedBoxes, string op)
        {
            var boxes = dispatchedBoxes.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            ResultModel result = null;
            try
            {
                if (string.IsNullOrWhiteSpace(op))
                {
                    result = DispatchService.ConfirmDispatch(deliveryNo, waybillno, lpid.Value, boxes);
                }
                else
                {
                    result = DispatchService.Update(deliveryNo, waybillno, lpid.Value, boxes);
                }
            }
            catch (Exception ex)
            {
                Log.loggeremail.Error(ex.Message, ex);
                //异步调用出错返回JSON
                result = new ResultModel() { IsSuccess = false, Message = "调度失败！" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认调度
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="waybillno"></param>
        /// <param name="lpid"></param>
        /// <param name="dispatchedBoxes"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <remarks>
        /// 该方法为异步调用
        /// </remarks>
        [HttpPost]
        public JsonResult ConfirmDispatchEx(string deliveryNo, string waybillno, int? lpid, long did)
        {
            ResultModel result = null;
            try
            {
                result = DispatchService.ConfirmDispatchEx(deliveryNo, waybillno, lpid.Value, did);
            }
            catch (Exception ex)
            {
                Log.loggeremail.Error(ex.Message, ex);
                //异步调用出错返回JSON
                result = new ResultModel() { IsSuccess = false, Message = "调度失败！" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reject(string deliveryNo)
        {
            var result = DispatchService.RejectDispatchEx(deliveryNo.Split(',').ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(string dids)
        {
            string[] ss = dids.Split(',');
            List<long> ds = new List<long>();
            foreach (string item in ss)
            {
                ds.Add(long.Parse(item));
            }
            var result = DispatchService.Delete(ds);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 初始化搜索选择列表
        /// </summary>
        private void InitSelectList()
        {
            //承运商
            ViewBag.CarrierID = CarrierService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CarrierName,
                    Value = x.CarrierID.ToString(),
                    Selected = false,
                });
            //运输方式
            ViewBag.TransportType = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.TransportType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
            //调度状态
            ViewBag.DispatchingPageStatus = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.DispatchingPageStatus>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
        }

    }
}

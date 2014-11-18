using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.Delivery.InTransit;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Web.Areas.Delivery.Models;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.Web.Areas.Delivery.Controllers
{
    public class InTransitController : Controller
    {
        IInTransitBLL InTransitServie = ServiceFactory.GetService<IInTransitBLL>("InTransitBLL");
        ICarrierWaybillBLL CarrierWaybillServie = ServiceFactory.GetService<ICarrierWaybillBLL>("CarrierWaybillBLL");
        IDispatchBLL DispatchService = ServiceFactory.GetService<IDispatchBLL>();
        ILinePlanBLL _lineplanBll = ServiceFactory.GetService<ILinePlanBLL>("LinePlanBLL");
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();

        public ActionResult List(InTransitSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();
            ViewBag.ConfirmLimited = bool.Parse(ConfigurationHelper.GetAppSetting("ConfirmLimited"));       //是否限制到货确认
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            InitSelectList();
            searchModel.Trim();
            
            if (Request.IsAjaxRequest())
            {
                var pagelist = InTransitServie.Search(searchModel);
                return PartialView("_PartialInTransitList", pagelist);
            }
            return View();
        }

        //
        // GET: /Delivery/InTransit/Details/5

        public ActionResult Details(int id)
        {
            var model = InTransitServie.Get(id) ?? new ViewInTransitModel();
            return View(model);
        }

        public ActionResult SetWaybillInfo(long id)
        {
            var waybill = CarrierWaybillServie.GetByDispatchID(id);
            var delivery = DispatchService.Get(id);
            WaybillInfoViewModel model = new WaybillInfoViewModel
            {
                CWID = waybill.CWID,
                ArrivalTime = waybill.ArrivalTime,
                Boxcount = waybill.Boxcount,
                WaybillNo = waybill.WaybillNo,
                Weight = waybill.Weight,
                ProtectedPrice = delivery.ProtectedPrice,
                TotalAmount = delivery.TotalAmount
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult SetWaybillInfo(long id, WaybillInfoViewModel model)
        {
            ResultModel result = new ResultModel();

            //获取提货单信息
            DispatchModel dispModel = DispatchService.Get(id);
            //根据提货单线路计划信息获取线路计划
            LinePlanModel linePlan = _lineplanBll.GetLinePlan(dispModel.LPID);

            if (ModelState.IsValid)
            {
                var waybill = CarrierWaybillServie.GetByDispatchID(id);
                waybill.Weight = model.Weight;
                waybill.Boxcount = model.Boxcount;
                //#5466 将TMS→发货在途管理→录入基本信息→“到货时间[承运商反馈到货时间]”取消。
                //waybill.ArrivalTime = model.ArrivalTime;
                result = CarrierWaybillServie.Update(waybill);
                result = DispatchService.UpdateTotalAmountByID(id, model.TotalAmount, model.ProtectedPrice);
                //result = DispatchService.UpdateTotalAmountByID(id, model.TotalAmount, model.TotalAmount * linePlan.InsuranceRate);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "验证不通过";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetArrive(long id)
        {
            ViewBag.ConfirmLimited = bool.Parse(ConfigurationHelper.GetAppSetting("ConfirmLimited"));       //是否限制到货确认
            var viewModel = InTransitServie.GetSetArriveView(id);
            var model = new WaybillArriveViewModel
            {
                ExpectArrivalDate=viewModel.ExpectArrivalDate,
                ConfirmExpArrivalDate = viewModel.ConfirmExpArrivalDate,
                DeliveryNo = viewModel.DeliveryNo,
                WaybillNo = viewModel.WaybillNo,
                DispatchID = viewModel.DispatchID,
                DesReceiveDate = viewModel.DesReceiveDate ?? DateTime.Now,
                IsDelay = viewModel.ConfirmExpArrivalDate <
                    (viewModel.DesReceiveDate.HasValue ?
                        viewModel.DesReceiveDate.Value
                        : DateTime.Now),
                IsExpectDelay = viewModel.IsExpectDelay,
                ExpectDelayType = viewModel.ExpectDelayType,
                ExpectDelayDesc = viewModel.ExpectDelayDesc,
                ExpectDelayTime = viewModel.ExpectDelayTime
            };
            string msg = string.Empty;
            if (InTransitServie.IsNeedConfirm(viewModel.DeliveryNo, out msg))
            {
                ViewBag.Confirm = msg.Split(Environment.NewLine.ToArray());
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SetArrive(long id, WaybillArriveViewModel model)
        {
            ViewBag.ConfirmLimited = bool.Parse(ConfigurationHelper.GetAppSetting("ConfirmLimited"));       //是否限制到货确认
            var result = InTransitServie.SetArrive(id, model.DelayType, model.DelayReason, model.SignedUser, model.DesReceiveDate);

            ViewBag.Result = result;
            return View(model);
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
        }
    }
}

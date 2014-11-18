using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Web.Areas.Delivery.Models;
using Vancl.TMS.Core.Logging;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.Web.Areas.Delivery.Controllers
{
    public class PrintController : Controller
    {
        IDispatchBLL DispatchService = ServiceFactory.GetService<IDispatchBLL>();
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();
        IDispTransitionBLL DispTransitionService = ServiceFactory.GetService<IDispTransitionBLL>();

        public ActionResult List(DeliveryPrintSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            //if (!Request.IsAjaxRequest())
            //{
            //    searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            //}
            if (searchModel.StartTime.HasValue && searchModel.EndTime.HasValue)
                searchModel.EndTime = DateTime.Parse(searchModel.EndTime.Value.ToString("yyyy-MM-dd 23:59:59"));

            

            if (Request.IsAjaxRequest())
            {
                var model = DispatchService.SearchEx(searchModel);
                return PartialView("_PartialDispatchList", model);
            }
            InitSelectList();
            return View();
        }

        /// <summary>
        /// 补录调度交接信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Transition(string id)
        {
            var tranModel = DispTransitionService.Get(id);
            if (tranModel == null)
            {
                tranModel = new DispTransitionModel();
                tranModel.DeliveryNo = id;
            }
            var model = new DispTransitionViewModel(tranModel);
            return View(model);
        }

        [HttpPost]
        public ActionResult Transition(string id, DispTransitionViewModel model)
        {
            ResultModel result;
            try
            {
                result = DispTransitionService.Update(model);
            }
            catch (Exception ex)
            {
                Log.loggeremail.Error(ex.Message, ex);
                result = new ResultModel { IsSuccess = false, Message = "操作失败，发生异常，请联系管理员检查错误日志！" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print(List<string> DeliveryNo)
        {
            var model = DispatchService.GetPrintDeliveryInfo(DeliveryNo);
            //   model = model.Concat(model).Concat(model).ToList();
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
            //运输方式
            ViewBag.TransportType = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.TransportType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
            //调度状态
            ViewBag.DeliveryStatus = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.DeliveryStatus>()
                .Where(x => x.Key != (int)Enums.DeliveryStatus.WaitForDispatch)
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Web.Areas.Claim.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.Claim.Controllers
{
    public class DelayHandleController : Controller
    {
        IDelayHandleBLL _delayHandleService = ServiceFactory.GetService<IDelayHandleBLL>();

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private DelayHandleSearchModel GetSearchConditions(bool isApply)
        {
            DelayHandleSearchModel searchModel = new DelayHandleSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            if (Request["S_DeliveryNO"] != null && Request["S_DeliveryNO"] != "")
            {
                searchModel.DeliveryNO = Request["S_DeliveryNO"];
            }
            if (Request["S_CarrierWaybillNO"] != null && Request["S_CarrierWaybillNO"] != "")
            {
                searchModel.CarrierWaybillNO = Request["S_CarrierWaybillNO"];
            }
            if(!String.IsNullOrWhiteSpace(Request["CreateDateBegin"]))
            {
                searchModel.CreateDateBegin = Convert.ToDateTime(Request["CreateDateBegin"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["CreateDateEnd"]))
            {
                searchModel.CreateDateEnd = Convert.ToDateTime(Request["CreateDateEnd"]);
            }
            if (isApply)
            {
                if (string.IsNullOrWhiteSpace(Request["InputStatus"]))
                {
                    searchModel.IsInput = true;
                }
                else if (Request["InputStatus"] != "-1")
                {
                    searchModel.IsInput = Request["InputStatus"] == "0";
                }
            }
            searchModel.Trim();
            return searchModel;
        }

        /// <summary>
        /// 显示指定承运商的到货延误信息
        /// </summary>
        /// <returns></returns>
        public ActionResult DelayList()
        {
            

            this.SetSearchListAjaxOptions();

            if (Request.IsAjaxRequest())
            {
                IList<ViewDelayHandleModel> delayHandleModel = _delayHandleService.Search(GetSearchConditions(true));
                return PartialView("_PartialDelayList", delayHandleModel);
            }
            return View();
        }

        /// <summary>
        /// 显示到货延误复议申请信息
        /// </summary>
        /// <returns></returns>
        public ActionResult DelayHandleApplyList()
        {
            this.SetSearchListAjaxOptions();
            DelayHandleSearchModel searchModel = GetSearchConditions(false);
            searchModel.ApproveStatus = Enums.DelayHandleApproveStatus.NotApprove;

            

            if (Request.IsAjaxRequest())
            {
                IList<ViewDelayHandleModel> delayHandleModel = _delayHandleService.SearchDelayHandleApply(searchModel);
                return PartialView("_PartialDelayHandleApplyList", delayHandleModel);
            }
            return View();
        }

        /// <summary>
        /// 加载到货延误复议申请界面
        /// </summary>
        /// <returns></returns>
        public ActionResult DelayHandleApply()
        {
            return View();
        }

        /// <summary>
        /// 保存到货延误复议申请信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelayHandleApply(DelayHandleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _delayHandleService.Add(model);
                ViewBag.Data = MvcHtmlString.Create(System.Web.Helpers.Json.Encode(result));
            }
            return View();
        }

        /// <summary>
        /// 加载复议申请处理界面
        /// </summary>
        /// <returns></returns>
        public ActionResult DelayHandle()
        {
            return View();
        }

        /// <summary>
        /// 保存复议申请处理信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelayHandle(DelayHandleViewModel model)
        {
            //DelayHandleViewModel model = new DelayHandleViewModel();
            model.ApproveStatus = (Enums.ApproveStatus)int.Parse(Request["ApproveStatus"]);
            model.DHID = long.Parse(Request["DHID"]);
            if (!string.IsNullOrWhiteSpace(Request["DelayID"]))
            {
                model.DelayID = long.Parse(Request["DelayID"].ToString());
            }
            var result = _delayHandleService.Approve(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}

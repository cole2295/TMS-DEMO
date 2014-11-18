using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Web.Areas.Claim.Models;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.Web.Areas.Claim.Controllers
{
    public class ExpectDelayController : Controller
    {
        IExpectDelayBLL ExpectDelayService = ServiceFactory.GetService<IExpectDelayBLL>();
        /// <summary>
        /// 预计延迟交货申请列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyList()
        {
            this.SetSearchListAjaxOptions();
            ViewBag.Title = "预计延迟交货申请";

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            var searchModel = new ExpectDelaySearchModel();
            searchModel.IsApply = true;
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            DateTime tmp_data;
            if (DateTime.TryParse(Request["CreateDateFrom"], out tmp_data))
            {
                searchModel.CreateDateBegin = tmp_data;
            }
            if (DateTime.TryParse(Request["CreateDateTo"], out tmp_data))
            {
                searchModel.CreateDateEnd = tmp_data;
            }
            searchModel.DeliveryNO = Request["DeliveryNo"];
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                searchModel.DeliveryNO = searchModel.DeliveryNO.ToUpper();
            }
            searchModel.CarrierWaybillNO = Request["CarrierWaybillNo"];
            if (DateTime.TryParse(Request["ArrivalDateFrom"], out tmp_data))
            {
                searchModel.ArrivalTimeBegin = tmp_data;
            }
            if (DateTime.TryParse(Request["ArrivalDateTo"], out tmp_data))
            {
                searchModel.ArrivalTimeEnd = tmp_data;
            }
            if (!string.IsNullOrWhiteSpace(Request["DepartureID"]) && Request["DepartureID"] != "0")
            {
                searchModel.DepartureID = Convert.ToInt32(Request["DepartureID"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["ArrivalID"]) && Request["ArrivalID"] != "0")
            {
                searchModel.ArrivalID = Convert.ToInt32(Request["ArrivalID"]);
            }
            if (string.IsNullOrWhiteSpace(Request["InputStatus"]))
            {
                searchModel.IsInput = true;
            }
            else if (Request["InputStatus"] != "-1")
            {
                searchModel.IsInput = Request["InputStatus"] == "0";
            }

            //if (!Request.IsAjaxRequest())
            //{
            //    searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            //}
            searchModel.Trim();
            

            if (Request.IsAjaxRequest())
            {
                var pagelist = ExpectDelayService.Search(searchModel);
                return PartialView("_PartialApplyList", pagelist);
            }
            return View("List");
        }

        /// <summary>
        /// 预计延迟交货审核列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveList()
        {
            this.SetSearchListAjaxOptions();

            ViewBag.Title = "预计延迟交货审核列表";
            ViewBag.IsApprove = true;
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            var searchModel = new ExpectDelaySearchModel();
            searchModel.IsApply = false;
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            Enums.ApproveStatus ApproveStatus;
            if (Enum.TryParse(Request["ApproveStatus"], out ApproveStatus))
            {
                if (Enum.IsDefined(typeof(Enums.ApproveStatus), ApproveStatus))
                {
                    searchModel.ApproveStatus = ApproveStatus;
                }
            }
            DateTime tmp_data;
            if (DateTime.TryParse(Request["CreateDateFrom"], out tmp_data))
            {
                searchModel.CreateDateBegin = tmp_data;
            }
            if (DateTime.TryParse(Request["CreateDateTo"], out tmp_data))
            {
                searchModel.CreateDateEnd = tmp_data;
            }
            searchModel.DeliveryNO = Request["DeliveryNo"];
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                searchModel.DeliveryNO = searchModel.DeliveryNO.ToUpper();
            }
            searchModel.CarrierWaybillNO = Request["CarrierWaybillNo"];
            if (DateTime.TryParse(Request["ArrivalDateFrom"], out tmp_data))
            {
                searchModel.ArrivalTimeBegin = tmp_data;
            }
            if (DateTime.TryParse(Request["ArrivalDateTo"], out tmp_data))
            {
                searchModel.ArrivalTimeEnd = tmp_data;
            }
            if (!string.IsNullOrWhiteSpace(Request["DepartureID"]) && Request["DepartureID"] != "0")
            {
                searchModel.DepartureID = Convert.ToInt32(Request["DepartureID"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["ArrivalID"]) && Request["ArrivalID"] != "0")
            {
                searchModel.ArrivalID = Convert.ToInt32(Request["ArrivalID"]);
            }

            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }
            searchModel.Trim();
            

            if (Request.IsAjaxRequest())
            {
                var pagelist = ExpectDelayService.Search(searchModel);
                return PartialView("_PartialApproveList", pagelist);
            }
            return View("List");

        }

        /// <summary>
        /// 申请预期延误
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyFor(int id)
        {
            var expectDelay = ExpectDelayService.Get(id);
            var model = new ApplyForExpectDelayViewModel
            {
                DeliveryNo = expectDelay.DeliveryNo,
                DispatchID = expectDelay.DispatchID,
                DelayTime = 0,
                ExpectDelayType = Enums.ExpectDelayType.SystemUnusual,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyFor(int id, ApplyForExpectDelayViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ExpectDelayService.ApplyForExpectDelay(model.DispatchID, model.ExpectDelayType, model.DelayTime, model.DelayDesc);
                return Json(result);
            }
            return View();
        }

        public ActionResult Approve(int id)
        {
            var model = ExpectDelayService.Get(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Approve(int id, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var model = ExpectDelayService.Get(id);
                bool isPass = Convert.ToBoolean(form["ApproveResult"]);
                var result = ExpectDelayService.Audit(id, isPass ? Enums.ApproveStatus.Approved : Enums.ApproveStatus.Dismissed);
                return Json(result);
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            var model = ExpectDelayService.Get(id);
            return View(model);
        }
    }
}

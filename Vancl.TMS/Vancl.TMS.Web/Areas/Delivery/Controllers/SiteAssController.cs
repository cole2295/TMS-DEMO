using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IBLL.Delivery.Spot;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Delivery.Spot;
using Vancl.TMS.Web.Areas.Claim.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Web.Areas.Delivery.Models;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.Web.Areas.Delivery.Controllers
{
    public class SiteAssController : Controller
    {
        ISiteAssBLL _siteAssService = ServiceFactory.GetService<ISiteAssBLL>();
        ICarrierBLL _carrierService = ServiceFactory.GetService<ICarrierBLL>("CarrierBLL");

        /// <summary>
        /// 显示所有已调度并且未录入现场数据的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteAssList()
        {
            ViewBag.Carriers = _carrierService.GetAll().Select(m =>
                              new SelectListItem() { Text = m.CarrierName, Value = m.CarrierID.ToString() });
            this.SetSearchListAjaxOptions();

            

            if (Request.IsAjaxRequest())
            {
                IList<ViewSiteAssModel> siteAssModel = _siteAssService.Search(GetSearchConditions());
                return PartialView("_PartialSiteAssList", siteAssModel);
            }
            return View();
        }

        /// <summary>
        /// 加载现场数据录入页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 添加现场信息
        /// </summary>
        /// <returns></returns>
        public JsonResult Submit()
        {
            SiteAssessmentBatchModel model = new SiteAssessmentBatchModel();
            model.ListDeliveryNo = Request["DeliveryNOs"].ToString().Split(',').ToList();
            model.ArrivalTime = DateTime.Parse(Request["ArrivalTime"].ToString());
            model.LeaveTime = DateTime.Parse(Request["LeaveTime"].ToString());
            model.AssessmentStatus = bool.Parse(Request["AssessmentStatus"].ToString());
            model.Reason = Request["Reason"].ToString();
            var result = _siteAssService.AddBatch(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 私有方法

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private SiteAssSearchModel GetSearchConditions()
        {
            SiteAssSearchModel searchModel = new SiteAssSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (Request["S_DepartureID"] != null && Request["S_DepartureID"] != "")
            {
                searchModel.DepartureID = Convert.ToInt32(Request["S_DepartureID"]);
            }
            if (Request["S_ArrivalID"] != null && Request["S_ArrivalID"] != "")
            {
                searchModel.ArrivalID = Convert.ToInt32(Request["S_ArrivalID"]);
            }
            if (Request["S_CreateDateBegin"] != null && Request["S_CreateDateBegin"] != "")
            {
                searchModel.CreateDateBegin = Convert.ToDateTime(Request["S_CreateDateBegin"]);
            }
            if (Request["S_CreateDateEnd"] != null && Request["S_CreateDateEnd"] != "")
            {
                searchModel.CreateDateEnd = Convert.ToDateTime(Request["S_CreateDateEnd"]);
            }
            if (Request["S_CarrierID"] != null && Request["S_CarrierID"] != "")
            {
                searchModel.CarrierID = Request["S_CarrierID"];
            }
            //if (Request["S_ArrivalProvince"] != null && Request["S_ArrivalProvince"] != "")
            //{
            //    searchModel.ArrivalProvince = Convert.ToInt32(Request["S_ArrivalProvince"]);
            //}
            //if (Request["S_DeliveryStatus"] != null && Request["S_DeliveryStatus"] != "")
            //{
            //    searchModel.DeliveryStatus = (Enums.DeliveryStatus)Convert.ToInt32(Request["S_DeliveryStatus"]);
            //}
            //if (Request["S_GoodsType"] != null && Request["S_GoodsType"] != "")
            //{
            //    searchModel.LineType = (Enums.LineType)Convert.ToInt32(Request["S_GoodsType"]);
            //}
            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }
            searchModel.Trim();
            return searchModel;
        }

        #endregion

    }
}

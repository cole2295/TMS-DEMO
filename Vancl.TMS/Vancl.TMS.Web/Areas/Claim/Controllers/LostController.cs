using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Web.Areas.Claim.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Claim.Lost;
using System.Web.Script.Serialization;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Util.JsonUtil;

namespace Vancl.TMS.Web.Areas.Claim.Controllers
{
    public class LostController : Controller
    {
        //IDelayHandleBLL _delayHandleService = ServiceFactory.GetService<IDelayHandleBLL>();
        ILostBLL _lostService = ServiceFactory.GetService<ILostBLL>();

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        private LostSearchModel GetSearchConditions(bool isList)
        {
            LostSearchModel searchModel = new LostSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (!String.IsNullOrWhiteSpace(Request["S_CreateDateBegin"]))
            {
                searchModel.CreateDateBegin = Convert.ToDateTime(Request["S_CreateDateBegin"].Trim());
            }
            if (!String.IsNullOrWhiteSpace(Request["S_CreateDateEnd"]))
            {
                searchModel.CreateDateEnd = Convert.ToDateTime(Request["S_CreateDateEnd"].Trim());
            }
            if (!String.IsNullOrWhiteSpace(Request["S_DeliveryNO"]))
            {
                searchModel.DeliveryNO = Request["S_DeliveryNO"];
            }
            if (!String.IsNullOrWhiteSpace(Request["S_CarrierWaybillNO"]))
            {
                searchModel.CarrierWaybillNO = Request["S_CarrierWaybillNO"];
            }
            if (isList)
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

        public ActionResult LostList()
        {
            this.SetSearchListAjaxOptions();
            //IList<ViewDelayHandleModel> delayHandleModel = _delayHandleService.Search(GetSearchConditions());
            LostSearchModel searchModel = GetSearchConditions(true);
            searchModel.IsAddLost = true;
            

            if (Request.IsAjaxRequest())
            {
                IList<ViewLostModel> lostModel = _lostService.Search(searchModel);
                return PartialView("_PartialLostList", lostModel);
            }
            return View();
        }

        public ActionResult LostBoxList()
        {
            string deliveryNO = Request["DeliveryNO"];
            string op = Request["Operate"];
            ViewBag.Operate = op;

            ViewLostDetailModel detailModel = new ViewLostDetailModel();
            if (op.ToLower() == "addlost")
            {
                detailModel = _lostService.GetPreLostDetail(deliveryNO);
            }
            else
            {
                detailModel = _lostService.GetLostDetail(deliveryNO);
            }
            IList<ViewLostBoxModel> boxList = detailModel.BoxList;

            IList<LostOrderDetailModel> lostOrderList = detailModel.PreLostOrderList;

            string str = JsonHelper.ConvertToJosnString(
                lostOrderList.Select(x => new
                {
                    BoxNo = x.BoxNo,
                    FormCode = x.FormCode,
                }));

            ViewBag.PreLostOrderDetail = MvcHtmlString.Create(str);

            return View(boxList);
        }

        public ActionResult LostOrderList()
        {
            string boxNO = Request["BoxNo"];

            IList<ViewLostOrderModel> allOrderList = _lostService.GetOrderList(boxNO);
            return View(allOrderList);
        }

        [HttpPost]
        public JsonResult AddLost()
        {
            string deliveryNo = Request["DeliveryNo"];
            string boxNo = Request["BoxNo"];
            string formCode = Request["FormCode"];
            return Json(_lostService.AddLost(deliveryNo, boxNo.Split(',').ToList(), formCode.Split(',').ToList())
                , JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UpdateLost()
        {
            string deliveryNo = Request["DeliveryNo"];
            string boxNo = Request["BoxNo"];
            string formCode = Request["FormCode"];
            ResultModel result = new ResultModel();
            if (boxNo == "" && formCode == "")
            {
                result = _lostService.UpdateLost(deliveryNo, null, null, false);
            }
            else
            {
                result = _lostService.UpdateLost(deliveryNo, boxNo.Split(',').ToList(), formCode.Split(',').ToList(), false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LostApproveList()
        {
            this.SetSearchListAjaxOptions();
            LostSearchModel searchModel = GetSearchConditions(false);
            searchModel.IsAddLost = false;
            searchModel.ApproveStatus = Enums.ApproveStatus.NotApprove;
            

            if (Request.IsAjaxRequest())
            {
                IList<ViewLostModel> lostModel = _lostService.Search(searchModel);
                return PartialView("_PartialLostApproveList", lostModel);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Approve()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ApproveLost()
        {
            string deliveryNo = Request["DeliveryNo"];
            bool approvePass = bool.Parse(Request["ApprovePass"]);
            Enums.ApproveStatus approveStatus = Enums.ApproveStatus.NotApprove;
            if (approvePass)
            {
                approveStatus = Enums.ApproveStatus.Approved;
            }
            else
            {
                approveStatus = Enums.ApproveStatus.Dismissed;
            }
            return Json(_lostService.Approve(deliveryNo, approveStatus), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddAllLost()
        {
            var deliveryNo = Request["DeliveryNo"];
            ResultModel result = _lostService.AddAllLost(deliveryNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult UpdateAllLost()
        {
            var deliveryNo = Request["DeliveryNo"];
            ResultModel result = _lostService.UpdateLost(deliveryNo, null, null, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckLostStatus()
        {
            string deliveryNO = Request["DeliveryNO"];
            string op = Request["Operate"];
            var result = _lostService.CheckLostStatus(deliveryNO, op.ToLower() == "addlost" ? true : false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

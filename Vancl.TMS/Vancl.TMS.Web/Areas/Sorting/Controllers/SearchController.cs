using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using System.Collections;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Web.Areas.Sorting.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.IBLL.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.WeighReview;
using Vancl.TMS.IBLL.Sorting.Search;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class SearchController : Controller
    {
        IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>();
        IWeighReviewSearchService WeighReviewSearchService = ServiceFactory.GetService<IWeighReviewSearchService>();

        public ActionResult QueryDeliveryNo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QueryDeliveryNo(string id)
        {
            BillDeliveryModel billDeliveryModel;
            var rm = BillBLL.QueryBillDeliveryModel(id, out billDeliveryModel);
            if (!rm.IsSuccess) return Json(rm, JsonRequestBehavior.AllowGet);
            return Json(billDeliveryModel, JsonRequestBehavior.AllowGet);
        }

        #region 称重复核查询

        public ActionResult WeighReview()
        {
            this.SetSearchListAjaxOptions();
            WeighReviewSearchModel searchModel = new WeighReviewSearchModel();
            #region 构造查询条件
            //分页条件
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            //运单号
            string strcodeType = Request["selFormType"];
            if (strcodeType != null)
            {
                string strcode = Request["txtFormCode"];
                if (string.IsNullOrWhiteSpace(strcode) == false)
                {
                    if (strcodeType == "0")//运单号
                    {
                        searchModel.Code = strcode;
                    }
                    else if (strcodeType == "1")//订单号
                    {
                        searchModel.Code = BillBLL.GetMerchantFormCodeRelation(Enums.SortCenterFormType.Order, strcode)[0].FormCode;
                    }
                }
            }
            //复核状态
            string strWeighReviewStatus = Request["selReviewStatus"];
            if (strWeighReviewStatus != null)
            {
                if (strWeighReviewStatus == "1")//正常
                {
                    searchModel.WeighReviewStatus = WeighReviewStatus.Normal;

                }
                else if (strWeighReviewStatus == "2")//异常
                {
                    searchModel.WeighReviewStatus = WeighReviewStatus.Abnormal;
                }
            }
            else//默认为正常
            {
                searchModel.WeighReviewStatus = WeighReviewStatus.Normal;
            }
            //员工编码
            string strEmployeeCode = Request["txtEmployeeCode"];
            if (string.IsNullOrWhiteSpace(strEmployeeCode) == false)
            {
                searchModel.EmployeeCode = strEmployeeCode;
            }
            //操作时间范围
            string strBeginTime = Request["BeginTime"];
            string strEndTime = Request["EndTime"];
            searchModel.BeginTime = string.IsNullOrEmpty(strBeginTime)
                     ? DateTime.MinValue
                     : Convert.ToDateTime(strBeginTime);
            searchModel.EndTime = string.IsNullOrEmpty(strEndTime)
                     ? DateTime.MaxValue
                     : Convert.ToDateTime(strEndTime);
            #endregion 构造查询条件
            
            if (Request.IsAjaxRequest())
            {
                var pagelist = WeighReviewSearchService.WeighReviewSearch(searchModel);
                return PartialView("_PartialWeighReviewList", pagelist);
            }

            return View();
        }

        public ActionResult QueryReturnBill()
        {
            return View();
        }
        #endregion   称重复核查询
    }
}

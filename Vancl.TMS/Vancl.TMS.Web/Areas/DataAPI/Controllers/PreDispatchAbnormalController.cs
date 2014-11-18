using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.Transport.DeliveryAbnormal;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.DataAPI.Controllers
{
    public class PreDispatchAbnormalController : Controller
    {
        IPreDispatchBLL _preDispatchBll = ServiceFactory.GetService<IPreDispatchBLL>("PreDispatchBLL");

        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();
            PreDispatchAbnormalSearchModel searchModel = new PreDispatchAbnormalSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            if (string.IsNullOrWhiteSpace(Request["BoxTimeStart"]))
            {
                searchModel.BoxTimeStart = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            }
            else
            {
                searchModel.BoxTimeStart = DateTime.Parse(Request["BoxTimeStart"].ToString());
            }
            if (string.IsNullOrWhiteSpace(Request["BoxTimeEnd"]))
            {
                searchModel.BoxTimeEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
            }
            else
            {
                searchModel.BoxTimeEnd = DateTime.Parse(Request["BoxTimeEnd"].ToString());
            }
            searchModel.IsPreDispatch = Vancl.TMS.Model.Common.Enums.BatchPreDispatchedStatus.DispatchedError;
            var result=_preDispatchBll.GetPreDispatchAbnormalList(searchModel);
            if (Request.IsAjaxRequest())
            {
                return View("_PartialPreDispatchAbnormalList", result);
            }
            return View(result);
        }

        [HttpGet]
        public JsonResult RepairPreDispatch()
        {
            if (string.IsNullOrWhiteSpace(Request["bids"]))
                return Json(new ResultModel() { IsSuccess = false, Message = "没有找到需要处理的信息" }, JsonRequestBehavior.AllowGet);
            var bids = Request["bids"].Split(';');
            if (bids.Length<=0)
                return Json(new ResultModel() { IsSuccess = false, Message = "没有找到需要处理的信息" }, JsonRequestBehavior.AllowGet);

            List<Vancl.TMS.Model.BaseInfo.Order.BoxModel> boxList = new List<Model.BaseInfo.Order.BoxModel>();
            ResultModel resultModel=new ResultModel();
            foreach (string bid in bids)
            {
                Vancl.TMS.Model.BaseInfo.Order.BoxModel boxModel= _preDispatchBll.GetAbnormalPreDispatchByBID(Int64.Parse(bid));
                if (boxModel != null)
                    boxList.Add(boxModel);
                else
                {
                    resultModel.IsSuccess = false;
                    resultModel.Message += bid + "未找到异常，可能已被处理<br>";
                }
            }

            if (boxList.Count > 0)
            {
                List<ResultModel> resultList = _preDispatchBll.CommonPreDispatchV1(boxList);
                if (resultList.Count > 0)
                {
                    foreach (ResultModel r in resultList)
                    {
                        resultModel.Message += r.Message + "<br>";
                    }
                }
                else
                {
                    resultModel.IsSuccess = true;
                    resultModel.Message = "";
                }
            }

            return Json(resultModel,JsonRequestBehavior.AllowGet);
        }
    }
}

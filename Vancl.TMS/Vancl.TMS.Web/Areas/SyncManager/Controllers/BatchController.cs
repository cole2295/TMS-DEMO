using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using System.Text;

namespace Vancl.TMS.Web.Areas.SyncManager.Controllers
{
    public class BatchController : Controller
    {
        private IBatchBLL batchService = ServiceFactory.GetService<IBatchBLL>();

        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            BatchSearchModel searchModel = new BatchSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "100");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (!string.IsNullOrWhiteSpace(Request["BatchNo"]))
            {
                searchModel.BatchNo = Request["BatchNo"];
            }
            else
            {
                List<BatchModel> bm = new List<BatchModel>();
                PagedList<BatchModel> l = new PagedList<BatchModel>(bm, 0, 0);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_PartialBatchList", l);
                }
                return View(l);
            }

            searchModel.Trim();

            var pagelist = batchService.GetBatch(searchModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialBatchList", pagelist);
            }

            return View(pagelist);
        }

        [HttpGet]
        public JsonResult RepairBatchDetail()
        {
            string logIds = Request["ids"];
            string[] keys = logIds.Split(';');
            ResultModel result = new ResultModel();
            if (keys.Length > 0)
            {
                int j = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < keys.Length; i++)
                {
                    ResultModel rm = batchService.RepairBatchDetail(keys[i]);
                    if (rm.IsSuccess)
                    {
                        j++;
                    }else
                        sb.Append(rm.Message + "<br>");
                }
                if (j == keys.Length)
                {
                    result.IsSuccess = true;
                    result.Message = "操作成功,已重新同步" + j + "条记录";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "操作失败或部分操作成功,影响记录" + j + "条<br>" + sb.ToString();
                }
            }

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}

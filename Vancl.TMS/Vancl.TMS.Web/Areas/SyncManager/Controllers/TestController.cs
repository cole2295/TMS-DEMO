using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.Web.Areas.SyncManager.Controllers
{
    public class TestController : Controller
    {
        private IBatchBLL batchService = ServiceFactory.GetService<IBatchBLL>();

        public ActionResult Index()
        {
            
            return View();
        }

        public JsonResult Repair()
        {
            var txt = Request["txt"];
            if(string.IsNullOrWhiteSpace(txt))
                return Json(new ResultModel() { IsSuccess=false,Message="没有sql"}, JsonRequestBehavior.AllowGet);
            var r=new ResultModel();
            Int32 n=batchService.RepairTest(txt);
            r.IsSuccess = true;
            r.Message = n.ToString();
            return Json(r, JsonRequestBehavior.AllowGet);
        }
    }
}

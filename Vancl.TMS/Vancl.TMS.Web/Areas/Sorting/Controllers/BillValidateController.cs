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
using System.Threading;
using Vancl.TMS.IBLL.Sorting.BillPrint;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    /// <summary>
    /// 面单校验
    /// </summary>
    public class BillValidateController : Controller
    {
        IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Validate(string customerOrder, string formCode)
        {
            var rm = BillPrintBLL.ValidateBill(customerOrder, formCode);
            if (string.IsNullOrWhiteSpace(rm.Message))
            {
                if (rm.Result== Model.Sorting.BillPrint.BillValidateResult.Success) rm.Message = "校验通过";
                else
                {
                    if (rm.Exception != null) rm.Message = rm.Exception.Message;
                    else rm.Message = "校验未通过";
                }
            }
            return Json(rm);
        }
    }
}

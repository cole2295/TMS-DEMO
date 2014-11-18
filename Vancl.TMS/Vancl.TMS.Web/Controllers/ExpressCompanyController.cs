using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util;
using Vancl.TMS.Web.Models;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Core.Security;
using System.Threading;

namespace Vancl.TMS.Web.Controllers
{
    public class ExpressCompanyController : Controller
    {
        ITransPointBLL TransPointBLL = ServiceFactory.GetService<ITransPointBLL>("TransPointBLL");

        public ActionResult ShowTree(string id, string departureId)
        {
            ViewBag.DepartureId = departureId;
            return View();
        }

        // [HttpPost]
        public JsonResult GetChildData(string departureId)
        {
            var rootNode = new JsonTreeNode()
            {
                id = "",
                text = "所有站点",
                value = "ALL",
                showcheck = false,
                isexpand = true,
                hasChildren = true,
                complete = true,
            };
            List<ExpressCompanyModel> list = new List<ExpressCompanyModel>();
            if (string.IsNullOrWhiteSpace(departureId))
            {
                list = TransPointBLL.GetDepartures() ?? list;
            }
            else
            {
                list = TransPointBLL.GetArrivals(int.Parse(departureId)) ?? list;
            }
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            var children = list
                .Select(m => new JsonTreeNode
                {
                    id = m.CompanyCode,
                    text = m.CompanyAllName,
                    value = m.CompanyCode,
                    showcheck = true,
                    isexpand = true,
                    hasChildren = false,
                    ChildNodes = new List<JsonTreeNode>(),
                    complete = true,
                })
                .OrderBy(l => l.text);

            rootNode.ChildNodes = children.ToList();

            return Json(new List<JsonTreeNode> { rootNode }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLoginExpressCompany()
        {
            return Json(new { id = UserContext.CurrentUser.DeptID, name = UserContext.CurrentUser.DeptName }, JsonRequestBehavior.AllowGet);
        }
    }
}

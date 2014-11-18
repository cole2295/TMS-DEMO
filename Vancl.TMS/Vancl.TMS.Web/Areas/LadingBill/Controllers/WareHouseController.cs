using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Web.Models;

namespace Vancl.TMS.Web.Areas.LadingBill.Controllers
{
    public class WareHouseController : Controller
    {
        IMERCHANTWAREHOUSEBLL _merchantwarehousebll = ServiceFactory.GetService<IMERCHANTWAREHOUSEBLL>();
        //
        // GET: /LadingBill/WareHouse/

        public ActionResult Index(string merchantid = "")
        {
            ViewBag.merchantid = merchantid;
            return View();
        }

        public JsonResult GetmechantData(string merchantid)
        {
            Dictionary<string, Object> dictionary = new Dictionary<string, object>();
            dictionary.Add("merchantid", merchantid);

            var mechantData = _merchantwarehousebll.GetModelList(dictionary);
            if (mechantData != null)
            {
                var rootNode = new JsonTreeNode()
                {
                    id = "",
                    text = "商家仓库",
                    value = "ALL",
                    showcheck = false,
                    isexpand = true,
                    hasChildren = true,
                    complete = true,
                };
                var children = mechantData
                   .Select(m => new JsonTreeNode
                   {
                       id = m.WAREHOUSECODE.ToString(),
                       text = m.WAREHOUSENAME,
                       value = m.WAREHOUSEADDRESS.ToString(),
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
            else
            {
                var rootNode = new JsonTreeNode()
                {
                    id = "",
                    text = "商家仓库",
                    value = "ALL",
                    showcheck = false,
                    isexpand = true,
                    hasChildren = true,
                    complete = true,
                };
                return Json(new List<JsonTreeNode> { rootNode }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetWarehousebyMerchantid(string merchantid)
        {
            Dictionary<string, Object> dictionary = new Dictionary<string, object>();
            dictionary.Add("merchantid", merchantid);

            var mechantData = _merchantwarehousebll.GetModelList(dictionary);
            if (mechantData == null)
            {
                return Json("", JsonRequestBehavior.AllowGet); 
            }
            var query = from pro in mechantData
                        select new { pro.WAREHOUSECODE, pro.WAREHOUSENAME };

            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}

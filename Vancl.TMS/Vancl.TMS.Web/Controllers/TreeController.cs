using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Util;
using Vancl.TMS.Web.Models;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util;

namespace Vancl.TMS.Web.Controllers
{
    public class TreeController : Controller
    {
        private IExpressCompanyBLL bll = ServiceFactory.GetService<IExpressCompanyBLL>();

        private ExpressCompanyModel InitRoot()
        {
            return new ExpressCompanyModel()
            {
                ID = 0,
                Name = "所有站点",
                ParentID = 0,
                SimpleSpell = "ALL",
                HasChild = true
            };
        }

        public ActionResult ShowTree()
        {
            return View();
        }

        /// <summary>
        /// Gets the tree root.
        /// </summary>
        /// <returns></returns>
        private JsonResult GetTreeRoot()
        {
            var root = InitRoot();
            var list = bll.GetChildExpressCompany((int)root.ID);
            var childNodes = BuildTreeNodeFromExpressCompany(list);
            var rootNode = new JsonTreeNode()
            {
                id = root.ID.ToString(),
                text = root.Name,
                value = root.SimpleSpell,
                showcheck = true,
                isexpand = true,
                ChildNodes = childNodes,
                hasChildren = childNodes != null && childNodes.Count > 0
            };
            var nodes = new List<JsonTreeNode>();
            nodes.Add(rootNode);
            return Json(nodes);
        }

        [HttpPost]
        public JsonResult GetChildData(FormCollection form)
        {
            var stationName = Request["StationName"];
            if (form["id"].IsNullData())
            {
                if (!string.IsNullOrEmpty(stationName))
                {
                    var stationID = bll.GetExpressCompanyIDByName(stationName);
                    if (stationID != 0)
                    {
                        var stationList = bll.GetChildExpressCompany(stationID);
                        var childNodes = BuildTreeNodeFromExpressCompany(stationList, 0);
                        var rootNode = new JsonTreeNode()
                        {
                            id = stationID.ToString(),
                            text = stationName,
                            value = "",
                            showcheck = true,
                            isexpand = true,
                            ChildNodes = childNodes,
                            hasChildren = childNodes != null && childNodes.Count > 0
                        };
                        var nodes = new List<JsonTreeNode>();
                        nodes.Add(rootNode);
                        return Json(nodes);
                    }
                }
                return GetTreeRoot();
            }
            int parentID = Convert.ToInt32(form["id"]);
            byte state = Convert.ToByte(form["checkstate"]);
            var list = bll.GetChildExpressCompany(parentID);
            return Json(BuildTreeNodeFromExpressCompany(list, state));
        }

        [HttpPost]
        public JsonResult GetCompanyNames()
        {
            var list = bll.GetAllCompanyNames();
            return Json(list);
        }

        /// <summary>
        /// 建立部门(分拣中心、配送商、配送站)树节点
        /// </summary>
        /// <param name="list">部门列表</param>
        /// <returns></returns>
        private List<JsonTreeNode> BuildTreeNodeFromExpressCompany(IList<ExpressCompanyModel> list)
        {
            return BuildTreeNodeFromExpressCompany(list, 0);
        }

        /// <summary>
        /// 建立部门(分拣中心、配送商、配送站)树节点
        /// </summary>
        /// <param name="list">部门列表</param>
        /// <param name="state">当前状态</param>
        /// <returns></returns>
        private List<JsonTreeNode> BuildTreeNodeFromExpressCompany(IList<ExpressCompanyModel> list, byte state)
        {
            var models = list as List<ExpressCompanyModel>;
            return models.IsNullData() ? null :
                models.ConvertAll<JsonTreeNode>(d =>
            {
                return new JsonTreeNode()
                {
                    id = d.ID.ToString(),
                    text = d.Name,
                    value = d.SimpleSpell,
                    showcheck = true,
                    isexpand = false,
                    checkstate = state,
                    hasChildren = d.HasChild
                };
            });
        }
    }
}

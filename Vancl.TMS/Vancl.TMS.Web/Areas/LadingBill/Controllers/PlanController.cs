using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.LadingBill.Controllers
{
    public class PlanController : Controller
    {
        readonly ILB_PLANBLL _planService = ServiceFactory.GetService<ILB_PLANBLL>("LB_PLANBLL");
        //
        // GET: /LadingBill/Plan/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            LB_PLANDTO lbPlandto = GetSearchPanSearchModel();
            lbPlandto.OrderByString = " CREATETIME desc";

            if (Request.IsAjaxRequest())
            {
                var pagelist = _planService.GetPlanList(lbPlandto);
                return PartialView("_PartialSearchList", pagelist);
            }
            this.SendEnumSelectListToView<Enums.PlanStatus>("Status");
            return View();
        }


        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        public LB_PLANDTO GetSearchPanSearchModel()
        {
            LB_PLANDTO searchModel = new LB_PLANDTO();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            searchModel.FROMDISTRIBUTIONCODE = UserContext.CurrentUser.DistributionCode;
            if (Request["waybillSource_List_hide"] != null)
            {
                searchModel.MERCHANTID = Request["waybillSource_List_hide"].Replace("'", "") == "" ? -1 : decimal.Parse(Request["waybillSource_List_hide"].Replace("'", ""));
            }
            if ((Request["WAREHOUSEID"] ?? "0").ToString() != "0")
            {
                searchModel.WAREHOUSEID = Request["WAREHOUSEID"].Replace("'", "");
            }
            if (Request["distributionDiv_List_hide"] != null)
            {
                searchModel.TODISTRIBUTIONCODE = Request["distributionDiv_List_hide"].Replace("'", "");
            }
            if (Request["txtDept"] != null)
            {
                searchModel.DEPARTMENT = Request["txtDept"].Replace("'", "").Trim();
            }
            if (Request["Status"] != null)
            {
                searchModel.ISENABLED = Request["Status"].Replace("'", "") == ""
                                            ? -1
                                            : decimal.Parse(Request["Status"].Replace("'", ""));
            }
            return searchModel;
        }

        //
        // GET: /LadingBill/Plan/Details/5

        public ActionResult Details(int id)
        {
            LB_PLAN viewModel = _planService.GetPlanDetails(6);
            return View("Info", viewModel);
        }

        //
        // GET: /LadingBill/Plan/Create

        public ActionResult Create()
        {
            this.SendEnumSelectListToView("DateType", Enums.DateType.Day);
            this.SendEnumSelectListToView("PriceType", Enums.PriceType.Once);
            LB_PLAN viewModel = _planService.GetPlanDetails(12);
            return View("Create", viewModel);
        }

        //
        // POST: /LadingBill/Plan/SavePlan

        public ActionResult SavePlan()
        {
            //接受POST数据
            var xmlDoc = new XmlDataDocument();
            string result = "";
            try
            {
                LB_PLAN lbPlan = new LB_PLAN();
                //Request.InputStream为post的数据，xml格式
                xmlDoc.DataSet.ReadXml(Request.InputStream);
                DataRow row = xmlDoc.DataSet.Tables[0].Rows[0];
                lbPlan.MERCHANTID = decimal.Parse(row["MERCHANTID"].ToString());
                lbPlan.WAREHOUSEID = row["WAREHOUSEID"].ToString();
                lbPlan.FROMDISTRIBUTIONCODE = UserContext.CurrentUser.DistributionCode;
                lbPlan.DEPARTMENT = row["DEPARTMENT"].ToString();
                lbPlan.ORDERQUANTITY = decimal.Parse(row["ORDERQUANTITY"].ToString());
                lbPlan.PREDICTWEIGHT = decimal.Parse(row["PREDICTWEIGHT"].ToString());
                lbPlan.MILEAGE = decimal.Parse(row["MILEAGE"].ToString());
                lbPlan.PRICETYPE = decimal.Parse(row["PRICETYPE"].ToString());
                lbPlan.AMOUNT = double.Parse(row["AMOUNT"].ToString());
              
                lbPlan.TIMETYPE = decimal.Parse(row["TIMETYPE"].ToString());
                lbPlan.WEEK = row["WEEK"].ToString();
                lbPlan.SPECIFICTIME = row["SPECIFICTIME"].ToString();
                lbPlan.RECEIVEMAIL = row["RECEIVEMAIL"].ToString();
                lbPlan.TODISTRIBUTIONCODE = row["TODISTRIBUTIONCODE"].ToString();
                lbPlan.CreateBy = UserContext.CurrentUser.ID;
                lbPlan.UpdateBy = UserContext.CurrentUser.ID;
                lbPlan.UpdateTime = DateTime.Now;
                lbPlan.CreateTime = DateTime.Now;
                lbPlan.IsDeleted = false;
                lbPlan.ISENABLED = 0;
                lbPlan.ISCREATED = 0;
                result = _planService.Add(lbPlan) > 0 ? "保存成功！" : "保存失败！";
            }
            catch
            {
                result += "系统异常";
            }
            return Json(result);
        }

        //
        // GET: /LadingBill/Plan/Edit/5

        public ActionResult Edit(int id)
        {
            this.SendEnumSelectListToView("DateType", Enums.DateType.Day);
            this.SendEnumSelectListToView("PriceType", Enums.PriceType.Once);
            LB_PLAN viewModel = _planService.GetPlanDetails(id);
            return View("Edit", viewModel);
        }

        //
        // POST: /LadingBill/Plan/Edit/5
        public ActionResult EditPlan()
        {
            //接受POST数据
            var xmlDoc = new XmlDataDocument();
            string result = "";
            try
            {
                LB_PLAN lbPlan = new LB_PLAN();
                //Request.InputStream为post的数据，xml格式
                xmlDoc.DataSet.ReadXml(Request.InputStream);
                DataRow row = xmlDoc.DataSet.Tables[0].Rows[0];
                lbPlan.ID = decimal.Parse(row["ID"].ToString());
                lbPlan.MERCHANTID = decimal.Parse(row["MERCHANTID"].ToString());
                lbPlan.WAREHOUSEID = row["WAREHOUSEID"].ToString();
                lbPlan.FROMDISTRIBUTIONCODE = UserContext.CurrentUser.DistributionCode;
                lbPlan.DEPARTMENT = row["DEPARTMENT"].ToString();
                lbPlan.ORDERQUANTITY = decimal.Parse(row["ORDERQUANTITY"].ToString());
                lbPlan.PREDICTWEIGHT = decimal.Parse(row["PREDICTWEIGHT"].ToString());
                lbPlan.MILEAGE = decimal.Parse(row["MILEAGE"].ToString());
                lbPlan.PRICETYPE = decimal.Parse(row["PRICETYPE"].ToString());
                lbPlan.AMOUNT = double.Parse(row["AMOUNT"].ToString());
                lbPlan.TIMETYPE = decimal.Parse(row["TIMETYPE"].ToString());
                lbPlan.WEEK = row["WEEK"].ToString();
                lbPlan.SPECIFICTIME = row["SPECIFICTIME"].ToString();
                lbPlan.RECEIVEMAIL = row["RECEIVEMAIL"].ToString();
                lbPlan.TODISTRIBUTIONCODE = row["TODISTRIBUTIONCODE"].ToString();
                lbPlan.CreateBy = UserContext.CurrentUser.ID;
                lbPlan.UpdateBy = UserContext.CurrentUser.ID;
                lbPlan.UpdateTime = DateTime.Now;
                lbPlan.CreateTime = DateTime.Now;
                lbPlan.IsDeleted = false;
                result = _planService.Update(lbPlan) > 0 ? "修改成功！" : "修改失败！";
            }
            catch
            {
                result += "系统异常";
            }
            return Json(result);
        }

        //
        // GET: /LadingBill/Plan/Delete/5

        //
        // POST: /LadingBill/Plan/Delete/5

        [HttpGet]
        public JsonResult SetEnable()
        {
            string lineids = Request["planids"];
            bool isEnabled = bool.Parse(Request["isEnabled"]);
            List<string> lineidList = lineids.Split(',').ToList<string>();
            ResultModel result = _planService.SetIsEnabled(lineidList, isEnabled);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete()
        {
            string lineids = Request["planids"];
            List<string> lineidList = lineids.Split(',').ToList<string>();
            ResultModel result = _planService.Delete(lineidList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
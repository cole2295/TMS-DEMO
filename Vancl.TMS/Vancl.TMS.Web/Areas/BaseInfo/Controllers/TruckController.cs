using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Truck;
using Vancl.TMS.Web.Areas.BaseInfo.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.BaseInfo.Controllers
{
    public class TruckController : Controller
    {
        //
        // GET: /BaseInfo/Truck/
        ITruckBLL _truckService = ServiceFactory.GetService<ITruckBLL>("TruckBLL");
        IExpressCompanyBLL _expressCompanyService = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 加载查询列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            TruckSearchModel searchModel = new TruckSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            if (!string.IsNullOrEmpty(Request["S_TruckNO"]))
            {
                searchModel.TruckNO = Request["S_TruckNO"].Trim();
            }
            IList<ViewTruckModel> truckList = null;
            if (!Request.IsAjaxRequest())
            {
                if (!string.IsNullOrWhiteSpace(Request["isReturn"]) && Request["isReturn"] == "1")
                {
                    truckList = _truckService.GetTruckList(searchModel);
                }
            }
            if (Request.IsAjaxRequest())
            {
                truckList = _truckService.GetTruckList(searchModel);
                return PartialView("_PartialTruckList", truckList);
            }
            return View(truckList);
        }

        public ActionResult CreateTruck()
        {
            return View("Create");
        }

        [HttpPost]
        public JsonResult CreateTruck(TruckViewModel model)
        {
            ResultModel result = null;
            if (ModelState.IsValid)
            {
                if (Request["selProvinceAndCity_Province"].Equals("-1")
                    || Request["selProvinceAndCity_City"].Equals("-1"))
                {
                    return Json(ResultModel.Create(false, "请选择所属省市。"));
                }
                model.ProvinceID = Request["selProvinceAndCity_Province"];
                model.CityID = Request["selProvinceAndCity_City"];
                model.DistributionCode = "rfd";
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                result = _truckService.Add(model);
            }
            else
            {
                result = ResultModel.Create(false, "232323");
            }
            return Json(result);
        }

        public ActionResult Update(string id)
        {
            TruckBaseInfoModel model = _truckService.GetTruckBaseInfo(id);
            TruckViewModel truckViewModel = new TruckViewModel(model);
            ViewBag.Province = truckViewModel.ProvinceID;
            ViewBag.City = truckViewModel.CityID;
            return View("Create", truckViewModel);
        }

        [HttpPost]
        public JsonResult Update(TruckViewModel model)
        {
            ResultModel result = null;
            if (ModelState.IsValid)
            {
                if (Request["selProvinceAndCity_Province"].Equals("-1")
                   || Request["selProvinceAndCity_City"].Equals("-1"))
                {
                    return Json(ResultModel.Create(false, "请选择所属省市。"));
                }
                model.ProvinceID = Request["selProvinceAndCity_Province"];
                model.CityID = Request["selProvinceAndCity_City"];
                model.DistributionCode = "rfd";
                model.UpdateTime = DateTime.Now;
                result = _truckService.Update(model);
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult SetDisabled()
        {
            string ids = Request["ids"];
            List<string> tbidList = ids.Split(',').ToList<string>();

            ResultModel result = _truckService.SetDisabled(tbidList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

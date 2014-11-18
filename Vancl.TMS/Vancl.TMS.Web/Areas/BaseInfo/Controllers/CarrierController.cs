using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vancl.TMS.Web.Models;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using System.Diagnostics;
using Vancl.TMS.Web.Areas.BaseInfo.Models;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IBLL.AdministrationRegion;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Util.Converter;
using System.Web.Mvc.Ajax;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Util.JsonUtil;



namespace Vancl.TMS.Web.Areas.BaseInfo.Controllers
{
    /// <summary>
    /// 承运商管理
    /// </summary>
    public class CarrierController : Controller
    {
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>("CarrierBLL");
        IAdministrationBLL AdministrationService = ServiceFactory.GetService<IAdministrationBLL>("AdministrationBLL");

        public ActionResult Create()
        {
            this.SendEnumSelectListToView("Status", Enums.CarrierStatus.Valid);
            return View("Info");
        }

        [HttpPost]
        public ActionResult Create(CarrierViewModel carrier)
        {
            if (ModelState.IsValid)
            {
                IList<DelayCriteriaModel> delayCriteriaList = this.BuildDelayCriteria();
                IList<CoverageModel> coverageList = this.BuildCoverageModel();
                carrier.DistributionCode = UserContext.CurrentUser.DistributionCode;
                var result = CarrierService.Add(carrier, delayCriteriaList, coverageList);
                ViewBag.Title = "保存承运商";
                ViewBag.RedirectAction = "List";
                return View("SaveResult", result);
            }

            this.SendEnumSelectListToView("Status", carrier.Status);

            return View("Info", carrier);
        }

        public ActionResult Details(string id)
        {
            var model = CarrierService.Get(Convert.ToInt32(id));
            var viewModel = new CarrierUpdateViewModel(model);
            var allCityList = AdministrationService.GetCity();

            //延误考核列表
            ViewBag.DelayCriteriaList = CarrierService.GetDelayCriteriaByCarrierID(Convert.ToInt32(id))
                    .Select(x => new DelayCriteriaViewModel
                    {
                        DCID = x.DCID,
                        CarrierID = x.CarrierID,
                        StartRegion = x.StartRegion,
                        EndRegion = x.EndRegion,
                        Discount = x.Discount,
                    })
                    .ToList();
            var CoverageList = CarrierService.GetCoverageByCarrierID(Convert.ToInt32(id));
            ViewBag.CoverageCityIDs = String.Join(",", CoverageList.Select(x => x.CityID));

            ViewBag.CoverageCityNames = String.Join(",", CoverageList.Select(x =>
            {
                return allCityList.FirstOrDefault(city => city.CityID == x.CityID).CityName;
            }));

            this.SendEnumSelectListToView("Status", viewModel.Status);

            return View("Info", viewModel);
        }

        [HttpPost]
        public ActionResult Details(string id, CarrierViewModel carrier)
        {
            carrier.CarrierID = Convert.ToInt32(id);
            if (ModelState.IsValid)
            {
                IList<DelayCriteriaModel> delayCriteriaList = this.BuildDelayCriteria();
                IList<CoverageModel> coverageList = this.BuildCoverageModel();
                carrier.DistributionCode = UserContext.CurrentUser.DistributionCode;

                var result = CarrierService.Update(carrier, delayCriteriaList, coverageList);
                ViewBag.Title = "保存承运商";
                ViewBag.RedirectAction = "List";
                return View("SaveResult", result);
            }

            this.SendEnumSelectListToView("Status", carrier.Status);
            return View("Info", carrier);
        }

        [HttpGet]
        public ActionResult CheckCarrierName(string carrierName)
        {
            int carrierID = 0;
            var urlInfo = Request.UrlReferrer.Segments.ToList();
            if (urlInfo[urlInfo.Count - 2] == "Details/")
            {
                string strCarrierId = urlInfo[urlInfo.Count - 1];
                int.TryParse(strCarrierId, out carrierID);
            }
            return Json(!CarrierService.IsExistCarrier(carrierName.Trim(), carrierID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckCarrierAllName(string carrierAllName)
        {
            int carrierID = 0;
            var urlInfo = Request.UrlReferrer.Segments.ToList();
            if (urlInfo[urlInfo.Count - 2] == "Details/")
            {
                string strCarrierId = urlInfo[urlInfo.Count - 1];
                int.TryParse(strCarrierId, out carrierID);
            }
            return Json(!CarrierService.IsExistCarrier(carrierAllName.Trim(), carrierID, true), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckContractNumber(string contractNumber)
        {
            int carrierID = 0;
            var urlInfo = Request.UrlReferrer.Segments.ToList();
            if (urlInfo[urlInfo.Count - 2] == "Details/")
            {
                string strCarrierId = urlInfo[urlInfo.Count - 1];
                int.TryParse(strCarrierId, out carrierID);
            }
            return Json(!CarrierService.IsExistContractNumber(contractNumber.Trim(), carrierID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckCarrierEmail(string email)
        {
            int carrierID = 0;
            var urlInfo = Request.UrlReferrer.Segments.ToList();
            if (urlInfo[urlInfo.Count - 2] == "Details/")
            {
                string strCarrierId = urlInfo[urlInfo.Count - 1];
                int.TryParse(strCarrierId, out carrierID);
            }
            return Json(!CarrierService.IsExistEmail(email.Trim().ToLower(), carrierID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {

            this.SetSearchListAjaxOptions();

            var searchModel = new CarrierSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            searchModel.CarrierName = Request["CarrierName"];
            searchModel.CarrierAllName = Request["CarrierAllName"];
            searchModel.Contacter = Request["Contacter"];
            searchModel.ContractNumber = Request["ContractNumber"];
            if (!string.IsNullOrWhiteSpace(Request["Status"]))
            {
                searchModel.Status = (Enums.CarrierStatus)int.Parse(Request["Status"]);
            }
            ;
            if (Request.IsAjaxRequest())
            {
                var pagelist = CarrierService.Search(searchModel);
                return PartialView("_PartialCarrierList", pagelist);
            }

            this.SendEnumSelectListToView<Enums.CarrierStatus>("Status");
            return View();
        }

        [HttpGet]
        public JsonResult Delete()
        {
            string ids = Request["ids"];
            var model = CarrierService.Delete(ids.Split(',').ConvertArray<int>().ToList());
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="CarrierID"></param>
        /// <returns></returns>
        public ActionResult ShowOperateLog(String CarrierID)
        {
            IOperateLogBLL oplog = CarrierService as IOperateLogBLL;
            return View("OperateLog",
             oplog.Read(new Model.Log.BaseOperateLogSearchModel()
            {
                KeyValue = CarrierID
            })
            );
        }

        public ActionResult SetDelayCriteria()
        {
            return View();
        }

        /// <summary>
        /// 由Request的数据构造延误考核实体列表
        /// </summary>
        /// <returns>延误考核实体列表</returns>
        private IList<DelayCriteriaModel> BuildDelayCriteria()
        {
            IList<DelayCriteriaModel> delayCriteriaList = new List<DelayCriteriaModel>();
            string DelayCriteria = Request["DelayCriteria"];
            if (!string.IsNullOrWhiteSpace(DelayCriteria) && DelayCriteria != "null")
            {
                JsonHelper.ConvertToObject<List<DelayCriteriaJsonModel>>(DelayCriteria)
                    .ForEach(x =>
                    {
                        int StartRegion = Convert.ToInt32(x.StartRegion);
                        int? EndRegion = null;
                        if (x.EndRegion != null) EndRegion = Convert.ToInt32(x.EndRegion);
                        decimal Discount = Convert.ToDecimal(x.Discount);
                        delayCriteriaList.Add(new DelayCriteriaModel
                        {
                            StartRegion = StartRegion,
                            EndRegion = EndRegion,
                            Discount = Discount,
                        });
                    });
            }
            return delayCriteriaList;
        }

        /// <summary>
        /// 由Request的数据构造覆盖范围实体列表
        /// </summary>
        /// <returns>覆盖范围实体列表</returns>
        private IList<CoverageModel> BuildCoverageModel()
        {
            IList<CoverageModel> coverageList = new List<CoverageModel>();
            string CoverageCityIDs = Request["CoverageCityIDs"] ?? "";
            CoverageCityIDs.Split(',').ToList().ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x))
                {
                    coverageList.Add(new CoverageModel
                    {
                        CityID = x,
                    });
                }
            });
            return coverageList;
        }


        /// <summary>
        /// 获取行政区域json树的数据
        /// </summary>
        /// <returns></returns>
        public string GetAdministrationTreeJson()
        {
            var allDistrictList = AdministrationService.GetDistrict();
            var allProvinceList = AdministrationService.GetProvince();
            var allCityList = AdministrationService.GetCity();
            List<TreeNodeModel> TreeNodeList = new List<TreeNodeModel>();
            TreeNodeList.Add(new TreeNodeModel
            {
                id = "root",
                pId = "",
                name = "选择范围",
                nocheck = true,
                open = true,
            });
            foreach (var district in allDistrictList)
            {
                TreeNodeList.Add(new TreeNodeModel
                {
                    id = "district_" + district.DistrictID,
                    pId = "root",
                    name = district.DistrictName,
                    //  nocheck = true,
                    open = false,
                });
            }
            foreach (var province in allProvinceList)
            {
                if (province.DistrictID.Length == 1)
                {
                    province.DistrictID = "0" + province.DistrictID;
                }
                TreeNodeList.Add(new TreeNodeModel
                {
                    id = "province_" + province.ProvinceID,
                    pId = "district_" + province.DistrictID,
                    name = province.ProvinceName,
                    // nocheck = true,
                    open = false,
                });
            }
            foreach (var city in allCityList)
            {
                TreeNodeList.Add(new TreeNodeModel
                {
                    id = "city_" + city.CityID,
                    pId = "province_" + city.ProvinceID,
                    name = city.CityName,
                    open = false,
                });
            }
            return JsonHelper.ConvertToJosnString(TreeNodeList);
        }

        [HttpGet]
        public JsonResult GetDelayCriteriaList()
        {
            var carrierID = Request["CarrierID"];
            if (!string.IsNullOrEmpty(carrierID))
            {
                List<DelayCriteriaViewModel> delayCriteriaList = CarrierService.GetDelayCriteriaByCarrierID(Convert.ToInt32(carrierID))
                    .Select(x => new DelayCriteriaViewModel
                    {
                        DCID = x.DCID,
                        CarrierID = x.CarrierID,
                        StartRegion = x.StartRegion,
                        EndRegion = x.EndRegion,
                        Discount = x.Discount,
                    })
                    .ToList();
                return Json(new { IsSuccess = true, DelayCriteriaList = delayCriteriaList }
                , JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsSuccess = false }, JsonRequestBehavior.AllowGet);
        }

    }
}

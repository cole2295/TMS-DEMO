using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Sorting.Loading;
using Vancl.TMS.IBLL.Sorting.Loading;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class LoadingController : Controller
    {
        IBillTruckBLL _billTruckService = ServiceFactory.GetService<IBillTruckBLL>("BillTruckBLL");
        ITruckBLL _truckService = ServiceFactory.GetService<ITruckBLL>("TruckBLL");
        IEmployeeBLL _employeeService = ServiceFactory.GetService<IEmployeeBLL>("EmployeeBLL");
        IExpressCompanyBLL _expressCompanyService = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        IDistributionBLL _distributionService = ServiceFactory.GetService<IDistributionBLL>("DistributionBLL");

        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            BillTruckSearchModel searchModel = new BillTruckSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            searchModel.BatchNO = Request["BatchNO"];
            searchModel.FromCode = Request["FormCode"];
            searchModel.OutBoundBeginTime = Request["outBoundBeginTime"];
            searchModel.OutBoundEndTime = Request["outBoundEndTime"];
            searchModel.BillSource = Request["S_Source"];
            searchModel.StationId = Request["selCityAndStation_Station"];
            searchModel.ArrivalDistributionCode = Request["S_Distribution"]; 
            searchModel.DepartureDistributionCode = UserContext.CurrentUser.DistributionCode;
            searchModel.OnlyNotLoadingBill = Request["ckbShowLoadingBill"] != null;

            if (Request.IsAjaxRequest())
            {
                IList<BillTruckBatchModel> truckList = _billTruckService.GetBillTruckList(searchModel);
                return PartialView("_PartialBillTruckList", truckList);
            }

            ViewBag.Distribution = _expressCompanyService.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode).Select
                (
                    m => new SelectListItem() { Text = m.Name, Value = m.DistributionCode }
                );

            List<IDAndNameModel> citys = _expressCompanyService.GetCitiesHasAuthority();
            if (citys != null)
            {
                List<string> cityIDList = citys.Select(m => m.ID).ToList<string>();
                ViewBag.Drivers = _employeeService.GetDriverByCityList(cityIDList).Select(m =>
                            new SelectListItem() { Text = m.EmployeeName, Value = m.EmployeeID.ToString() });
            }
            else
            {
                var result = new List<SelectListItem>();
                ViewBag.Drivers = result;
            }

            ViewBag.TruckNos = _truckService.GetAll().Select(m =>
                             new SelectListItem() { Text = m.TruckNO, Value = m.TruckNO + " | " + m.GPSNO });


            return View();
        }

        [HttpGet]
        public JsonResult BeginLoading()
        {
            string batchNos = Request["BatchNos"];
            string truckNo = Request["TruckNo"];
            string driver = Request["Driver"];
            string gpsNo = Request["GPSNo"];
            List<string> batchNoList = batchNos.Split(',').ToList<string>();

            BillTruckModel model = new BillTruckModel();
            model.TruckNO = truckNo;
            model.DriverID = int.Parse(driver);
            model.CreateBy = UserContext.CurrentUser.ID;
            model.CreateDept = UserContext.CurrentUser.DeptID;
            model.CreateTime = DateTime.Now;
            model.UpdateBy = UserContext.CurrentUser.ID;
            model.UpdateTime = DateTime.Now;
            model.UpdateDept = UserContext.CurrentUser.DeptID;
            model.OpStationName = UserContext.CurrentUser.DeptName;
            model.GPSNO = gpsNo;

            ResultModel result = _billTruckService.LoadingByBatchNoList(batchNoList, model);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillTruckDetails(string id)
        {
            this.SetSearchListAjaxOptions();

            ViewBag.BatchNo = id;

            BillTruckSearchModel searchModel = new BillTruckSearchModel();
            searchModel.BatchNO = id;
            if (Request["ShowLoadingBill"] != null)
            {
                searchModel.OnlyNotLoadingBill = bool.Parse(Request["ShowLoadingBill"].ToString());
            }

            IList<ViewBillTruckModel> truckList = _billTruckService.GetOutbondByBatch(searchModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialBillTruckDetail", truckList);
            }

            List<IDAndNameModel> citys = _expressCompanyService.GetCitiesHasAuthority();
            if (citys != null)
            {
                List<string> cityIDList = citys.Select(m => m.ID).ToList<string>();
                ViewBag.Drivers = _employeeService.GetDriverByCityList(cityIDList).Select(m =>
                            new SelectListItem() { Text = m.EmployeeName, Value = m.EmployeeID.ToString() });
            }
            else
            {
                var result = new List<SelectListItem>();
                ViewBag.Drivers = result;
            }

            ViewBag.TruckNos = _truckService.GetAll().Select(m =>
                             new SelectListItem() { Text = m.TruckNO, Value = m.TruckNO + "|" + m.GPSNO });

            return View("BillTruckDetail", truckList);
        }

        [HttpGet]
        public JsonResult BeginLoadingByFormCodes()
        {
            string scanType = Request["ScanType"];

            string formCodes = Request["FormCodes"];
            string batchNo = Request["BatchNo"];

            List<string> formCodeList = formCodes.Split(',').ToList<string>();

            BillTruckModel model = new BillTruckModel();
            model.BatchNO = batchNo;
            model.CreateBy = UserContext.CurrentUser.ID;
            model.CreateTime = DateTime.Now;
            model.CreateDept = UserContext.CurrentUser.DeptID;
            model.UpdateBy = UserContext.CurrentUser.ID;
            model.UpdateTime = DateTime.Now;
            model.UpdateDept = UserContext.CurrentUser.DeptID;

            ResultModel result = new ResultModel();
            if (scanType.Equals("2"))
            {
                result = _billTruckService.RemovBillTruck(formCodeList, model);
            }
            else
            {
                string truckNo = Request["TruckNo"];
                string driver = Request["Driver"];
                string gpsNo = Request["GPSNo"];
                model.TruckNO = truckNo;
                model.DriverID = int.Parse(driver);
                model.OpStationName = UserContext.CurrentUser.DeptName;
                model.GPSNO = gpsNo;

                result = _billTruckService.LoadingByFormCodeList(formCodeList, model);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            this.SetSearchListAjaxOptions();
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyService.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            //     this.SendSelectListToView<ExpressCompanyModel>(distributorList, "Distribution", true);
            var list = distributorList.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.DistributionCode,
                }).ToList();
            list.Insert(0, new SelectListItem() { Selected = true, Text = "--请选择--", Value = "" });
            this.ViewBag.Distribution = list;

            return View();
        }

        [HttpPost]
        public ActionResult Search(BillTruckSearchModel searchModel)
        {
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            searchModel.BatchNO = Request["BatchNO"];
            searchModel.FromCode = Request["FormCode"];
            searchModel.OutBoundBeginTime = Request["BeginTime"];
            searchModel.OutBoundEndTime = Request["EndTime"];
            searchModel.StationId = Request["selCityAndStation_Station"];
            searchModel.DepartureDistributionCode = Request["Distribution"];
            var pageList = _billTruckService.GetBillTruckList(searchModel);

            return PartialView("_PartialSearchList", pageList);
        }

    }
}

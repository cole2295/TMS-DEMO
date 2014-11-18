using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IBLL.Sorting.CityScan;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.CityScan;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Util.Extensions;
using System.IO;
using Vancl.TMS.Util.Pager;


namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class CityScanController : Controller
    {
        ICityScanBLL cityScanBLL = ServiceFactory.GetService<ICityScanBLL>();
        IInboundBLL _inboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");

        public ActionResult Index()
        {
            UserModel curUser = UserContext.CurrentUser;
            SortCenterUserModel userModel = _inboundBLL.GetUserModel(UserContext.CurrentUser.ID);
            ViewData["CurrentUser"] = curUser;
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(userModel));
            return View("Index");
        }

        [HttpGet]
        public JsonResult ScanCode()
        {
            var formCode = Request["formCode"];
            var deptName = Request["DeptName"];
            var deptID = Request["DeptID"];
            var userID = Request["UserID"];
            var batchNo = Request["batchNo"];
            var hidData = JsonHelper.ConvertToObject<SortCenterUserModel>(DES.Decrypt3DES(Request["hidData"]));
            if (hidData.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Json(new ResultModel() { IsSuccess = false, Message = "请用分拣中心帐号操作" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(formCode))
            {
                return Json(new ResultModel() { IsSuccess = false, Message = "运单号不能为空" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(deptID))
            {
                return Json(new ResultModel() { IsSuccess = false, Message = "部门ID不能为空" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(userID))
            {
                return Json(new ResultModel() { IsSuccess = false, Message = "操作人不能为空" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(batchNo))
            {
                return Json(new ResultModel() { IsSuccess = false, Message = "批次号不能为空" }, JsonRequestBehavior.AllowGet);
            }

            CityScanModel cityScanModel = new CityScanModel()
            {
                FormCode = formCode,
                BatchNO = batchNo,
                CreateBy = int.Parse(userID),
                UpdateBy = int.Parse(userID),
                ScanSortCenter = deptID,
                ScanSortCenterName = deptName,
                IsDeleted=0,
            };

            ResultModel result = cityScanBLL.ScanCode(cityScanModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityScanStatistics(CityScanSearchModel searchModel)
        {
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            this.SetSearchListAjaxOptions();
            UserModel curUser = UserContext.CurrentUser;
            searchModel.ExpressCompanyID = curUser.DeptID.ToString();
            if (Request.IsAjaxRequest())
            {
                if(!string.IsNullOrWhiteSpace(Request["BatchNo"]))
                {
                    searchModel.BatchNo=Request["BatchNo"];
                }
                if (!string.IsNullOrWhiteSpace(Request["ScanStartTime"]))
                {
                    searchModel.ScanStartTime = Convert.ToDateTime(Request["ScanStartTime"]);
                }
                if (!string.IsNullOrWhiteSpace(Request["ScanEndTime"]))
                {
                    searchModel.ScanEndTime = Convert.ToDateTime(Request["ScanEndTime"]);
                }
                if (!string.IsNullOrWhiteSpace(Request["FormCode"]))
                {
                    searchModel.FormCode = Request["FormCode"];
                }
                
            }
            searchModel.Trim();
            

            
            ViewData["CurrentUser"] = curUser;
            if (Request.IsAjaxRequest())
            {
                var model = cityScanBLL.SearchCityScanStatistics(searchModel);
                return PartialView("_PartialCityScanStatisticsList", model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ExportScan()
        {
            var batchnoList = Request["batchNos"].Split(',').Where(row => !string.IsNullOrWhiteSpace(row)).ToList();
            var exportList = cityScanBLL.SearchExportScan(batchnoList);
            var b = exportList.ExportToExcel("同城单量统计导出明细");
            MemoryStream ms = new MemoryStream(b);
            return new FileStreamResult(ms, "application/vnd.ms-excel") { FileDownloadName = "同城单量统计导出明细.xls" };
        }

        public ActionResult CityScanPrint(List<String> batchNo)
        {
            IList<CityScanPrintModel> printList = new List<CityScanPrintModel>();
            UserModel curUser = UserContext.CurrentUser;
            ViewData["CurrentUser"] = curUser;
            foreach (string b in batchNo)
            {
                CityScanPrintModel print=new CityScanPrintModel();
                CityScanSearchModel searchModel = new CityScanSearchModel()
                {
                    BatchNo=b,
                    IsPaging=false,
                    ExpressCompanyID=curUser.DeptID.ToString()
                };
                PagedList<CityScanModel> cityScanModelList = cityScanBLL.SearchCityScanStatistics(searchModel);
                if (cityScanModelList != null && cityScanModelList.Count >= 1)
                {
                    print.BatchNo = cityScanModelList[0].BatchNO;
                    print.ScanSortCenterName = cityScanModelList[0].ScanSortCenterName;
                    var printDetail = cityScanBLL.SearchCityScanPrint(b);
                    print.Details = printDetail;
                    printList.Add(print);
                }
                else
                {
                    return View();
                }
            }
            return View(printList);
        }

        public ActionResult ScanBatchList()
        {
            var batchNo = Request["batchno"];
            if (!string.IsNullOrWhiteSpace(batchNo))
            {
                List<String> batchNoList = new List<string>();
                batchNoList.Add(batchNo);
                var exportList = cityScanBLL.SearchExportScan(batchNoList);
                return View(exportList);
            }
            return View();
        }
    }
}

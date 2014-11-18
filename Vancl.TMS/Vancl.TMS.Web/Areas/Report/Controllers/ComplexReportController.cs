using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.Report.ComplexReport;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Report.ComplexReport;
using Vancl.TMS.Util.Pager;
using System.IO;
using Vancl.TMS.Util.OfficeUtil;
using System.Reflection;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.Web.Areas.Report.Controllers
{
    public class ComplexReportController : Controller
    {
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();
        IComplexReportBLL ComplexReportService = ServiceFactory.GetService<IComplexReportBLL>("ComplexReportBLL");
        ILogBLL<DeliveryFlowLogModel> DeliveryFlowLogService = ServiceFactory.GetService<ILogBLL<DeliveryFlowLogModel>>();

        /// <summary>
        /// 初始化搜索选择列表
        /// </summary>
        private void InitSelectList()
        {
            //承运商
            ViewBag.CarrierID = CarrierService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CarrierName,
                    Value = x.CarrierID.ToString(),
                    Selected = false,
                });
            //调度状态
            ViewBag.ComplexReportDeliveryStatus = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.ComplexReportDeliveryStatus>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
            //是否延误
            var IsDelay = new List<SelectListItem>();
            IsDelay.Add(new SelectListItem() { Selected = false, Text = "是", Value = "true" });
            IsDelay.Add(new SelectListItem() { Selected = false, Text = "否", Value = "false" });
            ViewBag.IsDelay = IsDelay;
            //丢失类型
            ViewBag.LostType = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.LostType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
        }

        /// <summary>
        /// 构建条件检索对象
        /// </summary>
        /// <param name="searchModel"></param>
        private void BuildingSearchModel(ComplexReportSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(Request["ComplexReportDeliveryStatus"]))
            {
                searchModel.Status = (Enums.ComplexReportDeliveryStatus)Convert.ToInt32(Request["ComplexReportDeliveryStatus"]);
            }
            searchModel.Trim();
            if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
            {
                if (!searchModel.DeliveryNo.ToLower().StartsWith("ln"))
                {
                    searchModel.DeliveryNo = "LN" + searchModel.DeliveryNo;
                }
            }
        }

        [HttpGet]
        public ActionResult ShowLogs(string id)
        {
            return View("LogList", DeliveryFlowLogService.Read(new Model.Log.BaseLogSearchModel() { DeliveryNo = id }));
        }


        public ActionResult Export(ComplexReportSearchModel searchModel)
        {
            if (searchModel.DepartureID <= 0)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择一个出发地" });
            }
            BuildingSearchModel(searchModel);
            var ReportList = ComplexReportService.Export(searchModel);
            if (ReportList == null || ReportList.ReportData == null)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "木有数据" });
            }
            MemoryStream stream = new MemoryStream();
            using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
            {
                helper.CreateNewWorksheet("综合报表");
                List<String> filter = new List<string>();
                if (ReportList.Filter != null && ReportList.Filter.Count > 0)
                {
                    foreach (var item in ReportList.Filter)
                    {
                        filter.Add(item.ViewObjPropertyName);
                    }
                }
                helper.WriteData(ReportList.ReportData.To2Array(filter.ToArray()));
                helper.Save();
            }
            stream.Seek(0, 0);
            return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("综合报表.xlsx") };
        }
        //
        // GET: /Report/ComplexReport/List

        public ActionResult List(ComplexReportSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();
            if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
            {
                InitSelectList();
                //初始化页面时不查询
                return View(new ViewComplexReportPageModel()
                {
                    ReportData = null,
                    Filter = ComplexReportService.GetCurrentUserReportFilter()
                });
            }
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            BuildingSearchModel(searchModel);

            if (searchModel.DepartureID <= 0)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择一个出发地" });
            }

            

            if (Request.IsAjaxRequest())
            {
                var ReportList = ComplexReportService.Search(searchModel);
                return PartialView("_PartialComplexReportList", ReportList);
            }

            InitSelectList();
            return View();
        }


    }
}

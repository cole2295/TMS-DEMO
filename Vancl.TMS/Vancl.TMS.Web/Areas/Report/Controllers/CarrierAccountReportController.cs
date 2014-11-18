using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Report.CarrierAccountReport;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Report;
using System.IO;
using Vancl.TMS.Util.OfficeUtil;

namespace Vancl.TMS.Web.Areas.Report.Controllers
{
    public class CarrierAccountReportController : Controller
    {
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();
        ICarrierAccountReportBLL CarrierAccountReportBLL = ServiceFactory.GetService<ICarrierAccountReportBLL>("CarrierAccountReportBLL");
        //
        // GET: /Report/CarrierAccountReport/

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
            //运输方式
            ViewBag.TransportType = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.TransportType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });

        }

        public ActionResult Export(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel.DepartureID <= 0)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择一个出发地" });
            }
            searchModel.Trim();
            var ReportList = CarrierAccountReportBLL.Export(searchModel);
            if (ReportList == null || ReportList.ReportData == null)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "木有数据" });
            }
            MemoryStream stream = new MemoryStream();
            using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
            {
                helper.CreateNewWorksheet("承运商结算报表");
                helper.WriteData(ReportList.ReportData.To2Array());
                helper.Save();
            }
            stream.Seek(0, 0);
            return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("承运商结算报表.xlsx") };
        }

        public ActionResult List(CarrierAccountReportSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();
            if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
            {
                InitSelectList();
                //初始化页面时不查询
                return View(new ViewCarrierAccountReportPageModel()
                {
                    ReportData = null,
                    StatisticsData = null
                });
            }

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            searchModel.Trim();
            if (searchModel.DepartureID <= 0)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "请选择一个出发地" });
            }
            
            var ReportList = CarrierAccountReportBLL.Search(searchModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialCarrierAccountReportList", ReportList);
            }

            InitSelectList();
            return View(ReportList);
        }

    }
}

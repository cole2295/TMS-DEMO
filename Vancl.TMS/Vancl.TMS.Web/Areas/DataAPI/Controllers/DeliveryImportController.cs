using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.DeliveryImport;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.Model.Common;
using System.IO;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Web.Areas.DataAPI.Models;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.Web.Areas.DataAPI.Controllers
{
    public class DeliveryImportController : Controller
    {
        IDeliveryImportBLL deliveryImportService = ServiceFactory.GetService<IDeliveryImportBLL>();
        ILinePlanBLL _lineplanService = ServiceFactory.GetService<ILinePlanBLL>("LinePlanBLL");
        private static readonly string TEMPLATEFILEPATH = "~/files/DeliveryImport/DeliveryImportTemplate/提货单导入模版.xlsx";
        //
        // GET: /DataAPI/DeliveryImport/

        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            DeliveryInRecordSearchModel searchModel = new DeliveryInRecordSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (string.IsNullOrWhiteSpace(Request["CreateTime"]))
            {
                searchModel.CreateTime = DateTime.Now.AddDays(-3);
            }
            else
            {
                searchModel.CreateTime = DateTime.Parse(Request["CreateTime"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(Request["DeliverySource"]))
            {
                searchModel.DeliverySource = (Enums.DeliverySource)Convert.ToInt32(Request["DeliverySource"]);
            }

            searchModel.OrderByString = "CreateTime desc";
            var pagelist = deliveryImportService.GetRecordList(searchModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialDeliveryImportList", pagelist);
            }

            this.SendEnumSelectListToView<Enums.DeliverySource>("DeliverySource");
            return View(pagelist);
        }

        public ActionResult DownLoadErrorFile(string path)
        {
            ResultModel r = new ResultModel();
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    IFileTransfer transfer = FileIOToolFactory.GetFileIOTool(FtpAction.DownLoad);
                    transfer.LocalContext = new FtpTransferLocalContext();
                    transfer.ServerContext = new FtpTransferServerContext();
                    transfer.ServerContext.FileName = path;
                    transfer.DoAction();
                    return new FileStreamResult(transfer.ActionFileStream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("提货单导入回执文件.xlsx") };
                }
                catch (Exception e)
                {
                    r.IsSuccess = false;
                    r.Message = e.Message;
                }
            }
            else
            {
                r.IsSuccess = false;
                r.Message = "无可下载文件";
            }
            return View("DownLoadFileFaultDialog", r);
        }

        [HttpPost]
        public ActionResult CreateDelivery(ViewDispatchWithDetailsModel model)
        {
            if (model == null) throw new ArgumentNullException("model", new Exception("异步调用参数为空."));

            try
            {
                ResultModel r = deliveryImportService.AddToDispatch(model, model.Source);
                return Json(new { IsSuccess = r.IsSuccess, Message = r.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ImportDelivery()
        {
            return View("ImportDelivery");
        }

        public ActionResult CreateDeliveryByScan()
        {
            return View("CreateDeliveryByScan");
        }

        public ActionResult CreateDeliveryByQuery()
        {
            int DepartureID = Request["S_DepartureID"] != null ? Convert.ToInt32(Request["S_DepartureID"]) : 0;
            int ArrivalID = Request["S_ArrivalID"] != null ? Convert.ToInt32(Request["S_ArrivalID"]) : 0;
            DateTime beginDate = Request["OutBoundTimeBegin"] != null ? Convert.ToDateTime(Request["OutBoundTimeBegin"].ToString()) : DateTime.Now;
            DateTime endDate = Request["OutBoundTimeEnd"] != null ? Convert.ToDateTime(Request["OutBoundTimeEnd"].ToString()) : DateTime.Now;
            List<ViewPreDispatchModel> list = new List<ViewPreDispatchModel>();
            if (DepartureID != 0 && ArrivalID != 0)
            {
                PreDispatchSearchModel searchModel = new PreDispatchSearchModel();
                searchModel.ArrivalID = ArrivalID;
                searchModel.DepartureID = DepartureID;
                searchModel.BeginDate = beginDate;
                searchModel.EndDate = endDate;
                //list = deliveryImportService.SearchPreDispatchInfo(searchModel);
                list = deliveryImportService.SearchPreDispatchInfoV1(searchModel);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialPreDispathInfoList", list);
            }
            else
            {
                //   this.SetSearchListAjaxOptions();
                return View(list);
            }
        }

        [HttpPost, AjaxException]
        public ActionResult PostBatchNo(ScanBatchNoModel model)
        {
            int DepartureID = Request["S_DepartureID"] == null ? 0 : int.Parse(Request["S_DepartureID"]);
            int ArrivalID = Request["S_ArrivalID"] == null ? 0 : int.Parse(Request["S_ArrivalID"]);
            PreDispatchSearchModel searchModel = new PreDispatchSearchModel();
            searchModel.ArrivalID = ArrivalID;
            searchModel.CustomerBatchNo = model.BatchNo;
            searchModel.DepartureID = DepartureID;
            //List<ViewPreDispatchModel> list = deliveryImportService.GetBatchPreDispatchInfo(searchModel);
            List<ViewPreDispatchModel> list = deliveryImportService.GetBatchPreDispatchInfoV1(searchModel);
            if (list == null || list.Count == 0)
                return Json(new { HasError = true, Message = "该批次号/箱号不存在或已生成提货单或不存在相关运输计划或出发地目的地错误." }, JsonRequestBehavior.AllowGet);
            if (list.Count > 1)
                return Json(new { HasError = true, Message = "系统中存在重复的批次号/箱号:" + model.BatchNo + ".请联系管理员" }, JsonRequestBehavior.AllowGet);

            return Json(list[0], JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLinePlanInfo(int lpid)
        {
            LinePlanModel linePlan = _lineplanService.GetLinePlan(lpid);
            return Json(linePlan, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import(FormCollection collection)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 1)
            {
                HttpPostedFileBase file = files[0];
                string extName = Path.GetExtension(Path.GetFileName(file.FileName));
                if (extName != ".xlsx")
                {
                    ViewBag.IsOK = false;
                    ViewBag.AlertMsg = "仅支持.xlsx格式的文件.";
                }
                else
                {
                    string[,] data;
                    using (OpenXMLHelper helper = new OpenXMLHelper(file.InputStream, OpenExcelMode.OpenForRead))
                    {
                        data = helper.ReadUsedRangeToEndWithoutBlank();
                    }
                    ResultModel r = deliveryImportService.AddDelivery(data, Server.MapPath(TEMPLATEFILEPATH));
                    ViewBag.IsOK = r.IsSuccess;
                    ViewBag.AlertMsg = r.Message;
                }
            }
            else
            {
                ViewBag.IsOK = false;
                ViewBag.AlertMsg = "未找到任何上传的文件";
            }

            return View("ImportDelivery");
        }

    }
}

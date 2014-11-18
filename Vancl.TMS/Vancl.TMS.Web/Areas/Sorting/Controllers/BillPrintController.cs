using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.Web.Areas.Sorting.Models;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.BillPrint;
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using BarcodeLib;
using System.Drawing;
using Vancl.TMS.Util.BarCode;
using System.Drawing.Imaging;
using System.Threading;
using Vancl.TMS.Util.Caching;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Util.Converter;
using System.Data;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class BillPrintController : Controller, IDisposable
    {
        IMerchantBLL MerchantBLL = ServiceFactory.GetService<IMerchantBLL>();
        IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>();
        IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();
        IBillTemplatePrintBLL BillTemplatePrintBLL = ServiceFactory.GetService<IBillTemplatePrintBLL>();
        //   ISortCenterBLL  SortCenterBLL = ServiceFactory.GetService<ISortCenterBLL>();

        public ActionResult Index()
        {
            //获取配送商
            //    SortCenterUserModel UserModel =  SortCenterBLL.GetUserModel(UserContext.CurrentUser.ID);
            var DistributionCode = UserContext.CurrentUser.DistributionCode;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");

            //商家
            var merchantList = MerchantBLL.GetMerchantListByDistributionCode(DistributionCode).OrderBy(x => x.Name).ToList(); ;

            

            UserModel curUser = UserContext.CurrentUser;
            //根据配置设置登录账号数据权限
            merchantList = UserContext.GetPrivateMerchantList(merchantList, curUser.UserCode);

            //面单打印招行信用卡专用,其他商家配置编码也可专享
            if (Request.Params["Merchant"] != null && !string.IsNullOrWhiteSpace(Request.Params["Merchant"].ToString())
               )
            {
                merchantList = merchantList.Where(x => x.MerchantCode == Request.Params["Merchant"].ToString()).ToList();
            }

            ViewBag.MerchantList = merchantList;

            return View();
        }
        
        //批量面单打印
        public ActionResult BatchBillPrint()
        {
            //获取配送商
            //    SortCenterUserModel UserModel =  SortCenterBLL.GetUserModel(UserContext.CurrentUser.ID);
            var DistributionCode = UserContext.CurrentUser.DistributionCode;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
            UserModel curUser = UserContext.CurrentUser;
            //承运商
            //根据配置设置登录账号数据权限
            var merchantList= MerchantBLL.GetMerchantListByDistributionCode(DistributionCode).OrderBy(x => x.Name).ToList();
            merchantList = UserContext.GetPrivateMerchantList(merchantList, curUser.UserCode);
            ViewBag.MerchantList = merchantList;
            return View();
        }
        [HttpPost]
        public ActionResult BatchBillPrint(BatchBillPrintArgModel argModel)
        {
          //  Thread.Sleep(1000);
            IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();

            var lstMerchantFormCode = BillBLL.GetMerchantFormCodeRelation(argModel.FormType, argModel.FormCode, argModel.MerchantId);
            var rm = new BatchBillPrintResultModel
            {
                FormCode = argModel.FormCode,
            };
            if (lstMerchantFormCode == null)
            {
                rm.IsDealSuccess = false;
                rm.Message = "未找到该单记录！";
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

            var merchantFormCodeRelationModel = lstMerchantFormCode[0];
            rm.WaybillNo = Convert.ToInt64(lstMerchantFormCode[0].FormCode);

            var billScanWeightArgModel = new BillScanWeightArgModel
            {
                BillWeight = argModel.Weight,
                FormCode = lstMerchantFormCode[0].FormCode,
                //     IsNeedWeightReview = argModel.IsCheckWeight,
                //     MerchantCheckWeight = argModel.CheckWeight,
                MerchantID = argModel.MerchantId,
                IsSkipPrintBill = false,
            };

            var r = BillPrintBLL.ScanWeight(billScanWeightArgModel);
            //     rm.DataBag = lstMerchantFormCode[0];
            rm.IsDealSuccess = r.ScanResult != ScanResult.Failed;            
            rm.Message = r.ScanResult == ScanResult.Failed ? r.Message : "打印成功";
            return Json(rm);
        }

        /// <summary>
        /// 描述单号
        /// </summary>
        /// <returns></returns>
        // [HttpPost]
        public JsonResult ScanBill(BillWeightScanArgModel argModel)
        {
            IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();
            string FormCode;

            var lstMerchantFormCode = BillBLL.GetMerchantFormCodeRelation(argModel.ScanType, argModel.FormCode, argModel.MerchantId);
            var rm = new BillPageViewModel();
            if (lstMerchantFormCode == null)
            {
                rm = BillPageViewModel.Create(ScanResult.Failed, "该单不存在!");
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

            if (lstMerchantFormCode.Count > 1 && argModel.MerchantId == 0)
            {
                rm.Message = "该单号对应多个商家运单!";
                rm.ScanResult = ScanResult.Warming;
                rm.IsMultiMerchant = true;
                rm.DataBag = lstMerchantFormCode;
                return Json(rm, JsonRequestBehavior.AllowGet);
            }

            var merchantFormCodeRelationModel = lstMerchantFormCode[0];
            FormCode = lstMerchantFormCode[0].FormCode;

            //选择全部
            if (argModel.MerchantId == 0)
            {
                argModel.MerchantId = merchantFormCodeRelationModel.MerchantID;
                argModel.IsCheckWeight = merchantFormCodeRelationModel.IsCheckWeight;
                argModel.IsNeedWeight = merchantFormCodeRelationModel.IsNeedWeight;
                argModel.IsSkipPrintBill = merchantFormCodeRelationModel.IsSkipPrintBill;
                argModel.CheckWeight = merchantFormCodeRelationModel.CheckWeight;
            }

            var billScanWeightArgModel = new BillScanWeightArgModel
            {
                BillWeight = argModel.BillWeight,
                FormCode = FormCode,
                IsNeedWeightReview = argModel.IsCheckWeight,
                MerchantCheckWeight = argModel.CheckWeight,
                MerchantID = argModel.MerchantId,
                IsSkipPrintBill = argModel.IsSkipPrintBill,
            };

            rm = BillPrintBLL.ScanWeight(billScanWeightArgModel);
            rm.DataBag = lstMerchantFormCode[0];
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        private Stream BillPrintSteam = null;
        public FileStreamResult PrintData(string id)
        {
            string billno = id;
            
            using (LocalReport report = new LocalReport())
            using (Barcode barcode = new Barcode())
            {
                //设置需要打印的报表的文件名称。
                report.ReportPath = Server.MapPath("~/Content/ReportFiles/Bill.rdlc");
                report.DataSources.Clear();

                int PackageIndex = 0;
                if (id.IndexOf(',') >= 0)
                {
                    billno = id.Split(',')[0];
                    PackageIndex = Convert.ToInt32(id.Split(',')[1]);
                }
                var pm = BillPrintBLL.GetPrintData(billno, PackageIndex);

                //      Image img = Code128Rendering.GetCodeAorBImg(pm.FormCode, 70, 1, true);
                barcode.IncludeLabel = true;
                barcode.LabelFont = new Font("黑体", 10, FontStyle.Bold);
                Image img = barcode.Encode(TYPE.CODE128, pm.FormCode, 150, 60);
                pm.BarCode = barcode.GetImageData(SaveTypes.PNG);
                img.Dispose();
                //   pm.BarCode = GetByteImage(img);
                var list = new List<BillPrintViewModel>();
                list.Add(pm);
                ReportDataSource ds = new ReportDataSource("PrintModel", list);
                report.DataSources.Add(ds);
                report.Refresh();
                string deviceInfo =
                    "<DeviceInfo>" +
                    "  <OutputFormat>EMF</OutputFormat>" +
                     "  <PageWidth>8cm</PageWidth>" +
                     "  <PageHeight>15cm</PageHeight>" +
                    //   "  <DpiX>600</DpiX>" +
                    // "  <DpiY>600</DpiY>" +
                      "<PrintDpiX>300</PrintDpiX>" +
                      "<PrintDpiY>300</PrintDpiY>" +
                     "  <MarginTop>0pt</MarginTop>" +
                    "  <MarginLeft>0pt</MarginLeft>" +
                    "  <MarginRight>0pt</MarginRight>" +
                    "  <MarginBottom>0pt</MarginBottom>" +
                    "</DeviceInfo>";
                Warning[] warnings;
                //将报表的内容按照deviceInfo指定的格式输出到CreateStream函数提供的Stream中。
                report.Render("Image", deviceInfo, CreateStream, out warnings);
            }
            //BillPrintSteam.Position = 0;
            //Image pageImage = Image.FromStream(BillPrintSteam);
            //pageImage.Save(@"d:\1.emf", ImageFormat.Tiff);
            BillPrintSteam.Position = 0;
            return File(BillPrintSteam, "application/octet-stream");
        }

        //用来提供Stream对象的函数，用于LocalReport对象的Render方法的第三个参数。
        private Stream CreateStream(string name, string fileNameExtension,
          Encoding encoding, string mimeType, bool willSeek)
        {
            //如果需要将报表输出的数据保存为文件，请使用FileStream对象。
            Stream stream = new MemoryStream();
            //   m_streams.Add(stream);
            BillPrintSteam = stream;
            return stream;
        }


        public byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }

        [HttpGet]
        public ActionResult ReWeigh(string formCode, int packageIndex, decimal weight, int MerchantId, bool IsCheckWeight)
        {
            ReWeighResultModel rm = new ReWeighResultModel { Weight = weight, PackageIndex = packageIndex };
            if (string.IsNullOrWhiteSpace(formCode))
            {
                rm.Result = ReWeighResult.Failure;
                rm.Message = "单号不能为空";
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            if (packageIndex < 1)
            {
                rm.Result = ReWeighResult.Failure;
                rm.Message = "箱号不正确";
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            try
            {
                decimal totalWeight;
                var result = BillPrintBLL.ReWeigh(formCode, packageIndex, weight, MerchantId, out totalWeight);
                rm.Message = result.Message;
                if (result.IsSuccess)
                {
                    if (string.IsNullOrWhiteSpace(result.Message))
                    {
                        rm.Result = ReWeighResult.Success;
                        rm.Message = "重新称重成功!";
                        if (IsCheckWeight)
                        {
                            rm.Message += "此单重量称重复核正常";
                        }
                    }
                    else
                    {
                        rm.Result = ReWeighResult.Warning;
                    }
                }
                rm.TotalWeight = totalWeight;
            }
            catch (Exception)
            {
                rm.Result = ReWeighResult.Failure;
                rm.Message = "重新称重出错";
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            return Json(rm, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 面单套打设置
        /// </summary>
        /// <returns></returns>
        public ActionResult FormatPrintSetting(int id = 0)
        {
            BillPrintTemplateModel model = default(BillPrintTemplateModel);
            if (id > 0) model = BillTemplatePrintBLL.GetBillPrintTemplate(id);

            if (model != null) ViewBag.Template = CachingManager.Get<string>(model.Storage);

            ViewBag.PrintFields = BillTemplatePrintBLL.GetBillPrintFields();
            ViewBag.PrintTemplates = BillTemplatePrintBLL.GetBillPrintTemplates(UserContext.CurrentUser.DistributionCode);
            return View(model);
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FormatPrintSetting(PrintTemplateArgModel arg)
        {
            arg.Id = arg.TplId;
            if (arg.IsNew) arg.Id = 0;
            var rm = BillTemplatePrintBLL.SaveBillPrintTemplate(arg);
            return Json(rm, JsonRequestBehavior.AllowGet);
            //  return RedirectToAction("FormatPrintSetting", new { id = rm.DataBag.Id });
        }

        /// <summary>
        /// 面单套打预览
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintSettingPreview()
        {
            return View();
        }

        /// <summary>
        /// 面单套打
        /// </summary>
        /// <returns></returns>
        public ActionResult FormatPrintData()
        {
            ViewBag.PrintTemplates = BillTemplatePrintBLL.GetBillPrintTemplates(UserContext.CurrentUser.DistributionCode);
            string resourceName = ViewBag.FormatPrintDataName ?? Request["resource"];
            ViewBag.FormatPrintDataName = resourceName;
            if (string.IsNullOrWhiteSpace(resourceName)) return View();
            int pageIndex = 1;
            int pageSize = 20;
            if (!string.IsNullOrWhiteSpace(Request["page"]))
            {
                pageIndex = Convert.ToInt32(Request["page"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["pageSize"]))
            {
                pageSize = Convert.ToInt32(Request["pageSize"]);
            }
            var pagedList = GetPrintData(resourceName, pageIndex, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialPrintDataList", pagedList);
            }
            else
            {
                return View(pagedList);
            }
        }

        private PagedList<DataRow> GetPrintData(string resourceName, int pageIndex, int pageSize)
        {
            var dt = CachingManager.Get<DataTable>(resourceName);
            if (dt == null) return null;
            return PagedList.Create<DataRow>(dt.Rows.Cast<DataRow>().ToList(), pageIndex, pageSize);
        }

        [HttpPost]
        public ActionResult FormatPrintData(FormCollection fc)
        {
            if (Request.Files.Count > 0 && Request.Files[0].InputStream.Length > 0)
            {
                try
                {
                    OpenXMLHelper oph = new OpenXMLHelper(Request.Files[0].InputStream, OpenExcelMode.OpenForRead);
                    var a2 = oph.ReadUsedRangeToEndWithoutBlank();
                    var dt = VanclConverter.Array2ToDataTable(a2, true);
                    ViewBag.DataTable = dt;
                    var name = String.Format("FormatPrintData_{0}_{1}", UserContext.CurrentUser.DistributionCode, DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //缓存已解析的数据datatable，以免再次解析
                    CachingManager.Set(name, dt);
                    ViewBag.FormatPrintDataName = name;
                    return Redirect("FormatPrintData?resource=" + name + "&tpl=" + fc["TplId"]);
                }
                catch (Exception ex)
                {
                    ViewBag.ShowParseDataError = true;
                }
            }
            return FormatPrintData();
        }

        public ActionResult DeleteFormatPrint(long id)
        {
            var rm = BillTemplatePrintBLL.DeletePrintTemplate(id);
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormatPrint(long tpl, string data, string rows)
        {
            var template = BillTemplatePrintBLL.GetBillPrintTemplate(tpl);
            var dt = CachingManager.Get<DataTable>(data);
            var row = rows.Trim().Split(',').ToList()
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x))
                .ToList();

            var list = BillTemplatePrintBLL.RenderPrintData(tpl, data, row);
            ViewBag.Template = template;
            return View(list);
        }

        void IDisposable.Dispose()
        {
            if (BillPrintSteam != null)
            {
                BillPrintSteam.Dispose();
                BillPrintSteam = null;
            }
        }
    }
}

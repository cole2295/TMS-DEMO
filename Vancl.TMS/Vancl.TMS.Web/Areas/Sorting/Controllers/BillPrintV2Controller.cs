using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BarcodeLib;
using Microsoft.Reporting.WebForms;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.BillPrint;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Util.HtmlToImg;
using Vancl.TMS.Web.Areas.Sorting.Models;
using Vancl.TMS.Web.WebControls.Mvc.Authorization;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class BillPrintV2Controller : Controller
    {
        //
        // GET: /Sorting/BillPrintV2/
        IMerchantBLL MerchantBLL = ServiceFactory.GetService<IMerchantBLL>();
        IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>();
        IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();
        IBillTemplatePrintBLL BillTemplatePrintBLL = ServiceFactory.GetService<IBillTemplatePrintBLL>();
       
        private static string baseurl = System.AppDomain.CurrentDomain.BaseDirectory;
        private static string local = System.Configuration.ConfigurationManager.AppSettings["local"];
        private Stream BillPrintSteam = null;

        private static string scriptsbaseurl  = " <link href='" + baseurl + "Content\\page_frame.css" +
                                        "' rel=\"stylesheet\" type=\"text/css\" />" +
                                        " <link href='" + baseurl + "Scripts\\plugins\\textbox\\textbox.css" +
                                        "' rel=\"stylesheet\" type=\"text/css\" />" +
                                        " <script src='" + baseurl + "Scripts\\lib\\jquery-1.7.1.min.js" +
                                        "' type=\"text/javascript\"></script>" +
                                        " <link href='" + baseurl +
                                        "Content\\css\\Pages\\sorting\\BillFormatPrintSetting.css" +
                                        "' type='text/css' rel='Stylesheet'/>" +
                                        " <script src='" + baseurl + "Scripts\\references\\jquery-barcode-2.0.2.min.js" +
                                        "' type=\"text/javascript\"></script>";

        private static string scriptsnetwork = " <link href='"+local+"Content/page_frame.css' rel=\"stylesheet\" type=\"text/css\" />" +
                                               " <link href='" + local + "Scripts/plugins/textbox/textbox.css" +
                                               "' rel=\"stylesheet\" type=\"text/css\" />" +
                                               " <script src='" + local + "Scripts/lib/jquery-1.7.1.min.js" +
                                               "' type=\"text/javascript\"></script>" +
                                               " <link href='" + local +
                                               "Content/css/Pages/sorting/BillFormatPrintSetting.css" +
                                               "' type='text/css' rel='Stylesheet'/>" +
                                               " <script src='" + local +
                                               "Scripts/references/jquery-barcode-2.0.2.min.js' type=\"text/javascript\"></script>"
                                              
                                               ;
                                               
        private string Css = scriptsnetwork +
                              "<script type=\"text/javascript\">" +
            " window.onload = function () { $(document.body).focus() };" +
            " $(function () {" +
            "         $(\".BillElem[ItemType='barcode']\").each(function () {" +
            "                  var value = $.trim($(this).text());" +
            "                  var height = $(this).height();" +
            "                 $(\"td\", this).barcode(value, \"code128\", { barWidth: 1.4, barHeight: height });" +
            "         });" +
            " });" +
            " </script>" +
            "<style type=\"text/css\" media=\"print\">" +
            ".print" +
            "{" +
            "    display: inline-block !important;" +
            "}" +
            " .noprint" +
            "{" +
            "  display: none !import;" +
            "}" +
            "  .container" +
            "{" +
            "padding: 0px !important;" +
            " }" +
            " .itemSplit" +
            "{" +
            " height: 0px;" +
            "}" +
            " #BillArea{ border:0; box-shadow:none;}" +
            "</style>" +
            "<style type=\"text/css\">" +
            " html, body" +
            "{" +
            "   margin: 0;" +
            "   padding: 0;" +
            "}" +
            ".container" +
            "{" +
            "   min-width: auto;" +
            "   padding: 10px;" +
            "}" +
            " #topBar" +
            "{ " +
            "   height: 40px;" +
            "   width: 100%;" +
            "   border-bottom: 1px solid #ddd;" +
            "   background: #eeefff;" +
            "   position: fixed;" +
            "   top: 0;" +
            "   left: 0;" +
            "   z-index: 2000;" +
            "   line-height: 20px;" +
            "   padding: 0 10px;" +
            "}" +
            "#topBarAreaBack" +
            "{" +
            "  height: 40px;" +
            "}" +
            "#btnPrint" +
            " {" +
            " position: absolute;" +
            " z-index: 2;" +
            "right: 30px;" +
            "top: 10px;" +
            "}" +
            " .itemSplit" +
            "{" +
            " background: #ddd;" +
            "height: 8px;" +
            " overflow: hidden;" +
            "page-break-after: always;" +
            " }" +
            "#BillArea .CurrentElem.BillElem{ background:none;}" +
            "</style>";

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

        [AllowAnoumous]
        public ActionResult PrintDataNew(string id, string formCode)
        {
            string billno = formCode;

            using (LocalReport report = new LocalReport())
            using (Barcode barcode = new Barcode())
            {
                //设置需要打印的报表的文件名称。
                report.ReportPath = Server.MapPath("~/Content/ReportFiles/Bill.rdlc");
                report.DataSources.Clear();

                int PackageIndex = Convert.ToInt32(id);
 
                var pm = BillPrintBLL.GetPrintData(billno, 0);
                pm.PackageIndex = PackageIndex;

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

            #region 模板面单打印，未实现
            // var pm = BillPrintBLL.GetPrintData(formCode);

               // //string dir = Server.MapPath("~/file/HtmlFlie/");
               // //if (!Directory.Exists(dir))
               // //{
               // //    Directory.CreateDirectory(dir);
               // //}
               // //string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".html";
               // //string filePath = Path.Combine(dir, fileName);

               // var temp = BillTemplatePrintBLL.GetBillPrintTemplateData(Convert.ToInt64(id));
               // string begin = "<!DOCTYPE html public \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">"+
               //                "<html><head>" + Css +
               //                "</head><body> ";
               // string end = "</body></html>";
               // string Html = begin + temp.Trim() + end;   
               // string FilledHtml = BillTemplatePrintBLL.FillTemplateData(Html, pm);
               // //System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, System.Text.Encoding.GetEncoding("gb2312"));
               // //try
               // //{
               // //    sw.Write(FilledHtml);
               // //    sw.Flush();
               // // }
               // // finally
               // // {
               // //    sw.Close(); 
               // // }

               //// string FillHtml = "<html><head></head><body><h2>打印测试<h2></body></html>";
               //// PrintFormHtml printHtml = new PrintFormHtml();
               //// printHtml.HTML = FilledHtml;
               ////// printHtml.FilePath = filePath;
               //// printHtml.PrintHtml();
               //// return Json(new { IsSuccess = true, Html = FilledHtml }, JsonRequestBehavior.AllowGet);
               //Stream sm = new MemoryStream(Encoding.GetEncoding("gb2312").GetBytes(FilledHtml));
            //return File(sm, "application/octet-stream");
            #endregion
           

        }


        public ActionResult Index()
        {
            InitPage();
            return View();
        }

        public ActionResult ScanBill(BillWeightScanArgModel model)
        {
            var modelList = BillBLL.GetMerchantFormCodeRelation(model.ScanType, model.FormCode, model.MerchantId);
            bool ret = true;
            string message = "";
            if (modelList == null)
            {
                ret = false;
                message = "该单号不存在";
                return Json(new { done = true, result = ret, Message = message, Model = new MerchantFormCodeRelationModel() { FormCode = model.FormCode, DeliverCode = string.Empty } });
            }
            if (modelList.Count > 1)
            {
                ret = false;
                message = "存在多个商家";
                modelList.ForEach(a => { message += a.MerchantName; });
            }
            var billInfo = BillBLL.GetBillInfoModel(modelList[0].FormCode);
           
            return Json(new { done = true, result = ret, Message = message, Model = modelList[0],PackageCount = billInfo.PackageCount});
        }

        public ActionResult BatchBillPrint()
        {

            InitPage();
            return View();
        }

        private void InitPage()
        {
            var DistributionCode = UserContext.CurrentUser.DistributionCode;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");

            //商家
            var merchantList = MerchantBLL.GetMerchantListByDistributionCode(DistributionCode);



            UserModel curUser = UserContext.CurrentUser;
            //众诚e家账户只显示光大银行 后期可做配置
            if (curUser.UserCode == "zcyj")
            {
                merchantList = merchantList.Where(x => x.ID == 5799 || x.ID == 5809).ToList();
            }
            //面单打印招行信用卡专用,其他商家配置编码也可专享
            if (Request.Params["Merchant"] != null && !string.IsNullOrWhiteSpace(Request.Params["Merchant"].ToString())
               )
            {
                merchantList = merchantList.Where(x => x.MerchantCode == Request.Params["Merchant"].ToString()).ToList();
            }

            ViewBag.MerchantList = merchantList.OrderBy(x => x.Name).ToList();
            ViewBag.PrintTemplates = BillTemplatePrintBLL.GetBillPrintTemplates(UserContext.CurrentUser.DistributionCode);
        }

    }
}

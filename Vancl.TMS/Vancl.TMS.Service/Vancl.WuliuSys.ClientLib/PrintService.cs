using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Web;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using System.Drawing.Imaging;

namespace Vancl.WuliuSys.ClientLib
{
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class PrintService : IPrintService
    {

        private volatile byte[] PrintData = null;
        private volatile Image pageImage;
        private volatile string PrintUrl;
        private volatile Exception PrintException;
        private volatile bool DownloadIsSuccess;
        private volatile bool PrintIsSuccess;

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public PrintModel Print(string url)
        {
            try
            {
                PrintUrl = url;
                if (!Regex.IsMatch(url, @"[a-zA-z]+://[^\s]*"))
                {
                    return PrintModel.CreateFailedResult("Url格式不正确！");
                }

                string PrintTimeOut = ConfigurationManager.AppSettings["PrintTimeOut"];
                int timeOut;//= Convert.ToInt32(PrintTimeOut);
                if (!int.TryParse(PrintTimeOut, out timeOut))
                {
                    return PrintModel.CreateFailedResult("超时配置错误！");
                }
                //下载数据
                var result = DownData(timeOut);
                if (result.IsSuccess == false) return result;

                //开始打印
                result = PrintImageData();

                return result;
            }
            catch (Exception ex)
            {
                return PrintModel.CreateFailedResult("打印失败！", ex);
            }
        }

        private PrintModel DownData(int timeOut)
        {
            Thread thread = new Thread(delegate()
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        var data = wc.DownloadData(PrintUrl);
                        DownloadIsSuccess = true;
                        PrintData = data;
                        if (PrintData.Length == 0) return;
                        using (var stream = new MemoryStream(PrintData))
                        {
                            pageImage = Image.FromStream(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //  PrintException = ex;
                    DownloadIsSuccess = false;
                }
            });
            //     thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
            //等待超时后强制终止线程
            if (!thread.Join(timeOut))
            {
                try
                {
                    thread.Abort();
                    return PrintModel.CreateFailedResult("打印服务超时并终止！");
                }
                catch (Exception ex)
                {
                    PrintException = ex;
                    return PrintModel.CreateFailedResult("打印服务超时并终止！", PrintException);
                }
            }
            if (DownloadIsSuccess == false || PrintData == null || PrintData.Length == 0)
            {
                return PrintModel.CreateFailedResult("下载解析数据失败！", PrintException);
            }
            return PrintModel.CreateSuccessResult();
        }

        private PrintModel PrintImageData()
        {
            //声明PrintDocument对象用于数据的打印
            PrintDocument printDoc = new PrintDocument();
            printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            //指定需要使用的打印机的名称，使用空字符串""来指定默认打印机
            // printDoc.PrinterSettings.PrinterName = "";
            //判断指定的打印机是否可用
            if (!printDoc.PrinterSettings.IsValid)
            {
                return PrintModel.CreateFailedResult("未能找到默认的打印机！");
            }

            printDoc.PrintPage += new PrintPageEventHandler(delegate(object sender, PrintPageEventArgs ev)
            {
                try
                {
                    ev.Graphics.PageUnit = GraphicsUnit.Document;
                    ev.Graphics.PageScale = 1; 

                    if (pageImage == null) return;
                    // Adjust rectangular area with printer margins.
                    Rectangle adjustedRect = new Rectangle(
                        ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                        ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                        ev.PageBounds.Width,
                        ev.PageBounds.Height);

                    Rectangle srcRect = new Rectangle(0, 0, pageImage.Width, pageImage.Height);
                    // Draw a white background for the report
                    //  ev.Graphics.FillRectangle(Brushes.White, adjustedRect);
                    // Draw the report content
                    ev.Graphics.DrawImage(pageImage, srcRect);//, srcRect, GraphicsUnit.Pixel);
                    ev.HasMorePages = false;
                }
                catch (Exception ex)
                {
                    PrintException = ex;
                    PrintIsSuccess = false;
                }
            });

            try
            {
                if (pageImage == null)
                {
                    PrintIsSuccess = false;
                }
                else
                {
                    printDoc.Print();
                    PrintIsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                PrintException = ex;
                PrintIsSuccess = false;
            }

            return PrintModel.Create(PrintIsSuccess, PrintIsSuccess ? "打印成功！" : "打印失败！", PrintException);
        }
        
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public bool CanPrint()
        {
            return true;
        }
    }
}

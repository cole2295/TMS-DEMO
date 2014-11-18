using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.WuliuSys.ClientLib.AutoUpdate;
using System.ServiceModel.Web;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Net;

namespace Vancl.WuliuSys.ClientLib
{
    [JavascriptCallbackBehavior(UrlParameterName = "jsoncallback")]
    public class CfgService : ICfgService
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public string Version()
        {
            return AutoUpdateSettings.Default.CurrentVersion;
        }


        private volatile string Info;
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public string Test()
        {

            Thread thread = new Thread(delegate()
               {

                   try
                   {
                       using (WebClient wc = new WebClient())
                       {
                           var data = wc.DownloadData("http://www.baidu.com");

                           using (var stream = new MemoryStream(data))
                           {
                               var img = Image.FromStream(stream);
                               Info = "OK";
                           }
                       }
                   }
                   catch (Exception ex)
                   {
                       Info = "Error" + ex.Message;
                   }
               });
            //     thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
            //等待超时后强制终止线程
            if (!thread.Join(10000))
            {

            }

            return Info;
        }
    }
}

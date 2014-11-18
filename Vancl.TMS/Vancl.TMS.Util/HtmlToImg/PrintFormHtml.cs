using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Vancl.TMS.Util.HtmlToImg
{
    public class PrintFormHtml
    {
        public string HTML { get; set; }
        public string FilePath { get; set; }
        private bool flag = false;

        public bool PrintHtml()
        {
            try
            {
                Thread NewTh = new Thread(SC_IMG);
                flag = false;
                NewTh.SetApartmentState(ApartmentState.STA);
                NewTh.Start();
                while (NewTh.ThreadState == ThreadState.Running && !flag)
                {

                }
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception();
            }
           

        }

        private void SC_IMG()
        {
            const short PRINT_WAITFORCOMPLETION = 2;
            const int OLECMDID_PRINT = 6;
            const int OLECMDEXECOPT_DONTPROMPTUSER = 2;
            WebBrowser wb = new WebBrowser();
            //using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.GetEncoding("GB2312")))
            //{
            //    sw.Write(HTML);
            //    sw.Flush();
            //    sw.Close();
            //}
           
         
            wb.DocumentText=HTML;
            //因为没有窗体，所以必须如此
            while (wb.ReadyState != WebBrowserReadyState.Complete)
                System.Windows.Forms.Application.DoEvents();
            if (wb.ActiveXInstance == null)
                throw new Exception("实例不能为空");
            dynamic ie = wb.ActiveXInstance;
            ie.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER, PRINT_WAITFORCOMPLETION);
            flag = true;
            // File.Delete(FilePath);
        }
    }
}

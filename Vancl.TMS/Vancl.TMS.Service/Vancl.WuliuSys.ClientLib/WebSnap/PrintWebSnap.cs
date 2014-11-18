using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;

namespace Vancl.WuliuSys.ClientLib.WebSnap
{
    public class PrintWebSnap
    {
        Bitmap bmp = null;
        private string url;

        public PrintWebSnap(string printUrl)
        {
            this.url = printUrl;
        }

        public void Print()
        {
            using (var webCapture = new WebCapture(url))
            {
                try
                {
                    bmp = webCapture.Snap();
                }
                catch (Exception)
                {
                    return;
                }
            }
            if (bmp == null) return;
            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
                printDoc.EndPrint += new PrintEventHandler(printDoc_EndPrint);
                printDoc.Print();
            }
            catch (Exception)
            {
                return;
            }
        }

        void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        void printDoc_EndPrint(object sender, PrintEventArgs e)
        {
            if (bmp != null) bmp.Dispose();
        }

    }
}

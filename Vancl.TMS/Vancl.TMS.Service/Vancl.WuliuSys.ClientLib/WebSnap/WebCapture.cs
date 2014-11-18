using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Vancl.WuliuSys.ClientLib.WebSnap
{
    public class WebCapture:IDisposable
    {
        #region 字段、属性
        private int width = 0;
        private int height = 0;
        private string url;
        private WebBrowser browser;
        /// <summary>
        /// 属性：图片宽
        /// </summary>
        public int Width { get { return width; } }
        /// <summary>
        /// 属性：图片高
        /// </summary>
        public int Height { get { return height; } }
        /// <summary>
        /// 属性：Url
        /// </summary>
        public string Url { get { return url; } }
        #endregion
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public WebCapture()
        {
            browser = new WebBrowser();

            //System.Threading.Thread th = new System.Threading.Thread(
            //    new System.Threading.ThreadStart(
            //   () =>
            //   {
            //       browser = new WebBrowser();
            //   }));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">Url</param>
        public WebCapture(string url):this()
        {
            this.url = url;
        }
        #endregion

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="width">图片宽</param>
        /// <param name="height">图片高</param>
        /// <returns></returns>
        /// <remarks>width,height为0时获取完整大小</remarks>
        public Bitmap Snap(string url="", int width = 0, int height = 0)
        {
            if (string.IsNullOrWhiteSpace(url)) url = Url;
            if (string.IsNullOrWhiteSpace(url)) return null;

            //抓取图片
            InitComobject(url);//初始化窗体
            int scrollWidth = this.browser.Document.Body.ScrollRectangle.Width;
            int scrollHeight = this.browser.Document.Body.ScrollRectangle.Height;
            this.browser.Width = scrollWidth;
            this.browser.Height = scrollHeight;
            if (width == 0) width = scrollWidth;
            if (height == 0) height = scrollHeight;
            //核心语句
            Snapshot snap = new Snapshot();
            Bitmap bitmap = snap.TakeSnapshot(this.browser.ActiveXInstance, new Rectangle(0, 0, width, height));
            browser.Dispose();
            return bitmap;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="url"></param>
        protected void InitComobject(string url)
        {
            this.browser.ScriptErrorsSuppressed = false;
            this.browser.ScrollBarsEnabled = false;
            this.browser.Navigate(url);
            //因为没有窗体，所以必须如此
            while (this.browser.ReadyState != WebBrowserReadyState.Complete)
                System.Windows.Forms.Application.DoEvents();
            this.browser.Stop();
            if (this.browser.ActiveXInstance == null)
                throw new Exception("实例不能为空");
        }

        public void Dispose()
        {
            if (!browser.IsDisposed)
            {
                browser.Dispose();
            }
        }
    }
}

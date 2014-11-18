using System;
using System.Net;
using log4net;

namespace Vancl.TMS.Util
{
    public class MessageCollector
    {
        private string _ip = string.Empty;
        private static MessageCollector _instance = null;

        public static MessageCollector Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new MessageCollector();
                }
                return _instance;
            }
        }

        private MessageCollector()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Collect(Type type, string msg)
        {
            HandleMessage("", msg, false);
        }

        public void Collect(string key, string msg)
        {
            HandleMessage(key, msg, false);
        }

        public void Collect(Type type, string msg, bool needNotice)
        {
            HandleMessage("", msg, needNotice);
        }

        public void Collect(Type type, Exception ex, bool needNotice)
        {
            var msg = string.Format("信息{0},堆栈{1},{2}", ex.Message, ex.StackTrace, (ex.InnerException == null ? "" : ex.InnerException.Message));
            HandleMessage("", msg, needNotice);
        }

        public void Collect(string key, string msg, bool needNotice)
        {
            HandleMessage(key, msg, needNotice);
        }

        private void HandleMessage(string key, string msg, bool needNotice)
        {
            LogManager.GetLogger(key).Error(msg);
            if (needNotice)
            {
                LogManager.GetLogger("defaultEmail").Error(string.Format("IP:[{0}]  {1}", GetIP, msg));
            }
        }

        public string GetIP
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_ip))
                    {
                        string strHostName = Dns.GetHostName(); //得到本机的主机名
                        IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
                        _ip = ipEntry.AddressList[0].ToString(); //假设本地主机为单网卡
                    }
                }
                catch
                {
                    _ip = "未取得IP地址";
                }

                return _ip;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Common.Logging.Log4Net;


namespace Vancl.TMS.Core.Base
{
    /// <summary>
    /// TMS 系统日志管理
    /// </summary>
    public class TMSysLogger
    {
        /// <summary>
        /// Email通知异常日志
        /// </summary>
        public static readonly ILog logEmail = LogManager.GetLogger("EmailSend");
    }

}

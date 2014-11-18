using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.SMS;

namespace Vancl.TMS.IBLL.SMS
{
    /// <summary>
    /// 短信发送接口
    /// </summary>
    public interface ISMSSender
    {
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        ResultModel Send(SMSMessage msg);

    }
}

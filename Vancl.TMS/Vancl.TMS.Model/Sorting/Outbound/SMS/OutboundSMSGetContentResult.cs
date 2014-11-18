using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound.SMS
{
    /// <summary>
    /// 出库短信内容对象
    /// </summary>
    public class OutboundSMSGetContentResult : ResultModel
    {
        /// <summary>
        /// 短信发送内容
        /// </summary>
        public String Content { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 取得需要发送的入库短信内容结果对象
    /// </summary>
    public class InboundSMSGetContentResult : ResultModel
    {
        /// <summary>
        /// 短信发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 短信发送内容
        /// </summary>
        public String Content { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 检查是否发送入库短信上下文对象
    /// </summary>
    public class InboundSMSContext
    {
        /// <summary>
        /// 入库短信运单对象
        /// </summary>
        public InboundSMSBillModel BillModel { get; set; }

        /// <summary>
        /// 入库短信配置对象
        /// </summary>
        public InboundSMSConfigbaseModel SMSConfig { get; set; }
    }
}

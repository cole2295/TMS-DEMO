using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound.SMS
{
    /// <summary>
    /// 出库短信上下文
    /// </summary>
    public class OutboundSMSContext
    {
        /// <summary>
        /// 出库短信运单对象
        /// </summary>
        public OutboundSMSBillModel BillModel { get; set; }

        /// <summary>
        /// 短信相关配置
        /// </summary>
        public OutboundSMSConfigModel SMSConfig { get; set; }
    }
}

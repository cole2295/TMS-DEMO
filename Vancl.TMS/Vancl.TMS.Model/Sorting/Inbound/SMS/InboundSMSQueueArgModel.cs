using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea;
using Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 短信队列处理参数对象
    /// </summary>
    public class InboundSMSQueueArgModel
    {

        /// <summary>
        /// 入库短信队列子项对象
        /// </summary>
        public InboundSMSQueueEntityModel QueueItem { get; set; }

    }
}

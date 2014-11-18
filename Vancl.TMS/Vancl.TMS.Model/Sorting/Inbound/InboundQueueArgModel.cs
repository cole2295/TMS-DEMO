using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea;
using Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库队列JOB参数对象
    /// </summary>
    public class InboundQueueArgModel
    {
        /// <summary>
        /// 入库队列子项对象
        /// </summary>
        public InboundQueueEntityModel QueueItem { get; set; }


        /// <summary>
        /// 入库线路区域短信配置
        /// </summary>
        public LineAreaSMSConfigModel LineAreaSMSConfig { get; set; }

        /// <summary>
        /// 入库商家短信配置
        /// </summary>
        public MerchantSMSConfigModel MerchantSMSConfig { get; set; }

    }
}

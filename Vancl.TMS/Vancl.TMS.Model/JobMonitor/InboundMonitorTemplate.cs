using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.JobMonitor
{
    /// <summary>
    /// 入库监控通知模板
    /// </summary>
    public class InboundMonitorTemplate
    {
        /// <summary>
        /// 箱表统计数量信息
        /// </summary>
        public SyncStatisticInfo BoxStatisticInfio { get; set; }

        /// <summary>
        /// 订单表统计数量信息
        /// </summary>
        public SyncStatisticInfo OrderStatisticInfio { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.JobMonitor
{
    /// <summary>
    /// 同步统计数量信息
    /// </summary>
    public class SyncStatisticInfo
    {
        /// <summary>
        /// 监控时间
        /// </summary>
        public DateTime MonitorTime { get; set; }

        /// <summary>
        /// 未同步的数量
        /// </summary>
        public int NoSync { get; set; }

        /// <summary>
        /// 同步中的数量
        /// </summary>
        public int Syncing { get; set; }

        /// <summary>
        /// 已经同步的数量
        /// </summary>
        public int Synced { get; set; }

    }
}

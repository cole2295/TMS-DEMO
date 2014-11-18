using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度服务参数对象
    /// </summary>
    public class PreDispatchJobArgModel
    {
        /// <summary>
        /// 服务每次处理的运单数量
        /// </summary>
        public int PerBatchCount { get; set; }

        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public int IntervalDay { get; set; }

        private DateTime? _SyncTime;

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime
        {
            get
            {
                if (!_SyncTime.HasValue)
                {
                    _SyncTime = DateTime.Now.AddDays(-IntervalDay);
                }
                return _SyncTime.Value;
            }
        }

    }
}

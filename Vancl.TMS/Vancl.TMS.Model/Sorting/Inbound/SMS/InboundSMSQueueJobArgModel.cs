using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 入库短信队列参数对象
    /// </summary>
    public class InboundSMSQueueJobArgModel
    {
        /// <summary>
        /// 服务每次处理的运单数量
        /// </summary>
        public int PerBatchCount { get; set; }

        /// <summary>
        /// 余数
        /// </summary>
        public int Remaider { get; set; }

        /// <summary>
        /// 取模的值
        /// </summary>
        public int ModValue { get; set; }

        /// <summary>
        /// 处理次数
        /// </summary>
        public int OpCount { get; set; }

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

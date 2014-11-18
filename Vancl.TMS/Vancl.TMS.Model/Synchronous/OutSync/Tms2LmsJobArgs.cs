using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Synchronous.OutSync
{
    /// <summary>
    /// TMS同步到LMS job参数对象
    /// </summary>
    public class Tms2LmsJobArgs
    {
        /// <summary>
        /// Top Count
        /// </summary>
        public int TopCount { get; set; }

        /// <summary>
        /// 取模数
        /// </summary>
        public int Mod { get; set; }

        /// <summary>
        /// 余数
        /// </summary>
        public int Remainder { get; set; }

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
                    DateTime tmpTime = DateTime.Now.AddDays(-IntervalDay);
                    _SyncTime = tmpTime;
                }
                return _SyncTime.Value;
            }
        }

    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Core.ServiceCache
{
    public class BillSyncCacheModel
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int TryTimes { get; set; }

        private List<Exception> _exceptions = null;
        /// <summary>
        /// 错误列表
        /// </summary>
        public List<Exception> Exceptions
        {
            get
            {
                if (_exceptions == null)
                {
                    _exceptions = new List<Exception>();
                }
                return _exceptions;
            }
            set
            {
                _exceptions = value;
            }
        }
    }
}

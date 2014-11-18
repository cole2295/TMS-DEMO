using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.CacheService
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    [Serializable]
    public class Cache: IDisposable 
    {
        /// <summary>
        /// 缓存唯一标识
        /// </summary>
        public string CacheKey { get { return Guid.NewGuid().ToString(); } }

        /// <summary>
        /// 缓存名称
        /// </summary>
        public string CacheName { get; set; }

        /// <summary>
        /// 缓存值
        /// </summary>
        public object CacheValue { get; set; }

        /// <summary>
        /// 缓存过期日期
        /// </summary>
        public DateTime? CachePeriod { get; set; }

        public override string ToString()
        {
            return this.CacheName;
        }

        public void Dispose()
        {
            this.CacheValue = null;
        }
    }
}

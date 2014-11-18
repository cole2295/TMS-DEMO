using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

namespace Vancl.TMS.Util.CacheService
{
    public class CachePool
    {
        private static readonly CachePool _pool = new CachePool();
        private static object _lockObj = new object();
        private HashSet<Cache> _caches = null;

        private CachePool()
        {
            if (_caches == null)
                _caches = new HashSet<Cache>();
        }

        private bool ContainsName(string name)
        {
            return _caches.FirstOrDefault(c => c.CacheName == name) != null;
        }

        /// <summary>
        /// 根据唯一标识获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Cache this[string name]
        {
            get
            {
                if (_caches != null && _caches.Count > 0)
                {
                    return _caches.FirstOrDefault(c => c.CacheName == name);
                }
                return null;
            }
            set
            {
                var cache = _caches.FirstOrDefault(c => c.CacheName == name);
                cache.CacheValue = value;
            }
        }

        /// <summary>
        /// 新增缓存对象
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public string Add(Cache cache)
        {
            lock (_lockObj)
            {
                if (!this.ContainsName(cache.CacheName))
                {
                    _caches.Add(cache);
                }
                else
                {
                    this[cache.CacheName] = cache;
                }

                return cache.CacheKey;
            }
        }

        /// <summary>
        /// 从缓存池中移除缓存对象
        /// </summary>
        /// <param name="key">缓存对象唯一标识</param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            lock (_lockObj)
            {
                return _caches.Remove(this[name]);
            }
        }

        /// <summary>
        /// 清空缓存池
        /// </summary>
        public void ClearAll()
        {
            _caches.Clear();
        }

        /// <summary>
        /// 获取当前服务端缓存数量
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return _caches.Count;
        }

        /// <summary>
        /// 移除过期缓存
        /// </summary>
        public void ClearExpiredCaches()
        {
            lock (_lockObj)
            {
                var caches = _caches.Where(x => x.CachePeriod.HasValue && x.CachePeriod.Value <= DateTime.Now);
                foreach (var item in caches)
                {
                    this.Remove(item.CacheName);
                }
            }
        }

        /// <summary>
        /// 获取缓存池实例
        /// </summary>
        /// <returns></returns>
        public static CachePool GetPool()
        {
            return _pool;
        }
    }
}

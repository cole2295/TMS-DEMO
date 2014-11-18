using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Vancl.TMS.Util.Caching
{
    public class HttpRuntimeCacheStategy : ICacheStrategy
    {
        private static readonly HttpRuntimeCacheStategy instance = new HttpRuntimeCacheStategy();
        protected static volatile Cache webCache = HttpRuntime.Cache;

        #region ICacheStrategy 成员

        public TimeSpan _DefaultExpires = TimeSpan.Zero;
        public TimeSpan DefaultExpires
        {
            get
            {
                return _DefaultExpires;
            }
            set
            {
                _DefaultExpires = value;
            }
        }

        public void Set(string key, object value)
        {
            this.Set(key, value, _DefaultExpires);
        }

        public void Set(string key, object value, TimeSpan timeSpan)
        {
            if (key == null || key.Length == 0 || value == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
            webCache.Insert(key, value, null, DateTime.Now + timeSpan, TimeSpan.Zero);
        }

        public void Set(string key, object value, DateTime dateTime)
        {
            if (key == null || key.Length == 0 || value == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);
            webCache.Insert(key, value, null, dateTime, TimeSpan.Zero
                );
        }

        public object Get(string key)
        {
            return webCache.Get(key);
        }

        public T Get<T>(string key)
        {
            return (T)webCache.Get(key);
        }

        public object Remove(string key)
        {
            return webCache.Remove(key);
        }

        public T Remove<T>(string key)
        {
            return (T)webCache.Remove(key);
        }

        #endregion

        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:
                    break;
                case CacheItemRemovedReason.Expired:
                    {
                        //CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(this.onRemove);

                        //webCache.Insert(key, val, null, System.DateTime.Now.AddMinutes(TimeOut),
                        // System.Web.Caching.Cache.NoSlidingExpiration,
                        // System.Web.Caching.CacheItemPriority.High,
                        // callBack);
                        break;
                    }
                case CacheItemRemovedReason.Removed:
                    {
                        break;
                    }
                case CacheItemRemovedReason.Underused:
                    {
                        break;
                    }
                default: break;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Vancl.TMS.Util.CacheService
{
    public class Caching : ICaching
    {
        private CachePool _pool = CachePool.GetPool();

        #region ICaching 成员

        public void Store(Cache cache)
        {
            _pool.Add(cache);
        }

        public Cache Get(string name)
        {
            return _pool[name];
        }

        public bool Remove(string name)
        {
            return _pool.Remove(name);
        }

        public int Count()
        {
            return _pool.GetCount();
        }

        #endregion
    }
}

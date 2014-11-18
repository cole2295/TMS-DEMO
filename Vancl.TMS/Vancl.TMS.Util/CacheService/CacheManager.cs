using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.ServiceModel;

namespace Vancl.TMS.Util.CacheService
{
    public class CacheManager
    {
        private CachePool _pool;
        private Timer _timer;
        private ServiceHost _host;

        public CacheManager()
        {
            _pool = CachePool.GetPool();
            _timer = new Timer(2000);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Start();
        }

        public void Add(Cache cache)
        {
            _pool.Add(cache);
        }

        public void StartCacheHost()
        {
            _host = new ServiceHost(typeof(Caching));
            try
            {
                _host.Open();
            }
            catch
            {
                throw;
            }
        }

        public void StopCacheHost()
        {
            try
            {
                _host.Close();
            }
            catch
            {
                throw;
            }
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _pool.ClearExpiredCaches();
        }
    }
}

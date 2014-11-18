using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.CacheService;
using System.Threading;

namespace CacheTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CacheManager m = new CacheManager();
            for (int i = 0; i < 10; i++)
            {
                m.Add(new Cache() { CacheName = i.ToString(), CacheValue = i.ToString() });
            }
            m.StartCacheHost();

            Console.ReadLine();
        }
    }
}

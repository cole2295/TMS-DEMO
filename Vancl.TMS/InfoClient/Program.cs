using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util;
using Vancl.TMS.Util.Transfer.Endpoint;
using System.Net;
using Vancl.TMS.Util.CacheService;

namespace InfoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceReference1.CachingClient client = new ServiceReference1.CachingClient())
            {
                Cache c = new Cache() { CacheName = "Name", CacheValue="Value" };
                client.Store(c);
            }

            Console.ReadLine();
        }
    }
}

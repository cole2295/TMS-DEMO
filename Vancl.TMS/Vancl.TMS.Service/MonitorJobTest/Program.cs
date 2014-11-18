using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Schedule.MonitorImpl;

namespace MonitorJobTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseQuartzManager mm = new QuartzManager();
            Console.WriteLine("TMS Monitor服务运行中...");
            mm.Start();
            string CMD = String.Empty;
            Console.WriteLine("请输入命令");
            while (true)
            {
                CMD = Console.ReadLine();
                if (CMD.ToLower().Equals("stop"))
                {
                    Console.WriteLine("TMS Monitor服务停止...");
                    break;
                }
                Console.WriteLine("请输入命令");
            }
            mm.Stop();
            Console.ReadLine();
            Console.WriteLine("END....");
        }
    }
}

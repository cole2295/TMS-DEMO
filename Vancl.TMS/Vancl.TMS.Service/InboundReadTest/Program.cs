using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Schedule.InboundReadImpl;

namespace InboundReadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            QuartzManager manager = new QuartzManager();
            manager.Start();
        }
    }
}

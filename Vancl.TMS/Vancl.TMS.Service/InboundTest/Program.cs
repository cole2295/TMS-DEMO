using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Schedule.InboundWriteImpl;

namespace InboundWriteTest
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Schedule.LinePlanServiceImpl;

namespace LinePlanServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LinePlanDeadLineJob job = new LinePlanDeadLineJob();
            job.DoJob(null);
        }
    }
}

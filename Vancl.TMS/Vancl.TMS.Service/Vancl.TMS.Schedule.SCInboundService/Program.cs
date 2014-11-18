using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Schedule.SCInboundImpl;

namespace Vancl.TMS.Schedule.SCInboundService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {


            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SCInboundService() 
            };
            ServiceBase.Run(ServicesToRun);

            //// Test
            //SCInboundJob job = new SCInboundJob();
            //job.DoJob(0);
        }
    }
}

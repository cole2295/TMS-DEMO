﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Vancl.TMS.Schedule.Tms2ThridPartyService
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
				new Tms2ThridPartyService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

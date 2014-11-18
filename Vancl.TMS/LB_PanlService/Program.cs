using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using Vancl.TMS.Util.ConfigUtil;
using System.Configuration;

namespace LB_PanlService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //Application.Run(new Form1());

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new  CreateTask_Service(),  
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Reflection;
using System.Management;
using Microsoft.Win32;


namespace Vancl.WuliuSys.ClientService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

       // private const string CurrentServiceName = "WuliuSysService";

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            this.SetServiceDesktopInsteract(this.serviceInstaller1.ServiceName);
          //  SetServiceDesktopInsteract(CurrentServiceName);
            ServiceController controller = new ServiceController(this.serviceInstaller1.ServiceName);
            if (controller != null)
            {
                controller.Start();
            }
        }

        private ServiceController GetService(string serviceName)
        {
            return ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
        }
        private void StopService(string serviceName)
        {
            var controller = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
            if (controller != null && controller.CanStop)
            {
                controller.Stop();
                controller.Dispose();
            }
        }
        /// <summary>
        /// 允许服务使用界面交互
        /// </summary>
        /// <param name="serviceName"></param>
        private void SetServiceDesktopInsteract(string serviceName)
        {
            ConnectionOptions coOptions = new ConnectionOptions();
            coOptions.Impersonation = ImpersonationLevel.Impersonate;
            ManagementScope mgmtScope = new System.Management.ManagementScope(@"root/CIMV2", coOptions);
            mgmtScope.Connect(); 

            ManagementObject wmiService = new ManagementObject(string.Format("Win32_Service.Name='{0}'", serviceName));
            ManagementBaseObject changeMethod = wmiService.GetMethodParameters("Change");
            changeMethod["DesktopInteract"] = true;
            ManagementBaseObject OutParam = wmiService.InvokeMethod("Change", changeMethod, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using Vancl.WuliuSys.ClientLib;
using Vancl.WuliuSys.ClientLib.AutoUpdate;

namespace Vancl.WuliuSys.ClientService
{
    partial class WuliuSysService : ServiceBase
    {
        public WuliuSysService()
        {
            InitializeComponent();
        }

        private ServiceHost CfgServiceHost { get; set; }
        private ServiceHost PrintServiceHost { get; set; }
        private ServiceHost WeightServiceHost { get; set; }

        protected override void OnStart(string[] args)
        {
            this.OpenCfgServiceHost();
            this.OpenPrintServiceHost();
            this.OpenWeightServiceHost();
            ServiceUpdate.StartCheckUpdate();
        }

        protected override void OnStop()
        {
            this.CloseServiceHost();
            ServiceUpdate.StopCheckUpdate();
        }

        #region private method
        private bool OpenCfgServiceHost()
        {
            if (CfgServiceHost != null)
            {
                CfgServiceHost.Close();
                CfgServiceHost = null;
            }
            CfgServiceHost = new ServiceHost(typeof(CfgService));
            CfgServiceHost.Open();

            return true;
        }
        private bool OpenPrintServiceHost()
        {
            if (PrintServiceHost != null)
            {
                PrintServiceHost.Close();
                PrintServiceHost = null;
            }
            PrintServiceHost = new ServiceHost(typeof(PrintService));
            PrintServiceHost.Open();

            return true;
        }

        private bool OpenWeightServiceHost()
        {
            if (WeightServiceHost != null)
            {
                WeightServiceHost.Close();
                WeightServiceHost = null;
            }
            WeightServiceHost = new ServiceHost(typeof(WeightService));
            WeightServiceHost.Open();

            return true;
        }

        private void CloseServiceHost()
        {
            if (CfgServiceHost != null)
            {
                CfgServiceHost.Close();
                CfgServiceHost = null;
            }
            if (PrintServiceHost != null)
            {
                PrintServiceHost.Close();
                PrintServiceHost = null;
            }
            if (WeightServiceHost != null)
            {
                WeightServiceHost.Close();
                WeightServiceHost = null;
            }
        }
        #endregion
    }
}

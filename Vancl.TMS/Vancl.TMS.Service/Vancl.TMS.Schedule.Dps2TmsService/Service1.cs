using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Vancl.TMS.Schedule.Dps2TmsService
{
    public partial class Service1 : ServiceBase
    {
        private ServicesManager _smgr = null;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (null == _smgr)
            {
                _smgr = new ServicesManager();
            }
            _smgr.Start();
        }

        protected override void OnStop()
        {
            if (null != _smgr)
            {
                _smgr.Stop();
            }
        }
    }
}

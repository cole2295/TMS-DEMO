using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;

namespace LB_PanlService
{
    partial class CreateTask_Service : ServiceBase
    {
        private ServicesManager _smgr = null;

        public CreateTask_Service()
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

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Schedule.TransportPlanImpl;

namespace Vancl.TMS.Schedule.TransportPlanService
{
    public partial class TransportPlanService : ServiceBase
    {
        BaseQuartzManager mm = new QuartzManager();
        public TransportPlanService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            mm.Start();
            string mailsubject = CommonConst.ServiceEnabledEmailSubject + "被启用";
            string mailcontent = mailsubject + "， 请您关注！";
            MailHelper mailer = new MailHelper(mailsubject, mailcontent, false);
            mailer.SendMailAsync();
        }

        protected override void OnStop()
        {
            mm.Stop();
            string mailsubject = CommonConst.ServiceEnabledEmailSubject + "被停用";
            string mailcontent = mailsubject + "， 请您关注！";
            MailHelper mailer = new MailHelper(mailsubject, mailcontent, false);
            mailer.SendMailAsync();
        }
    }
}

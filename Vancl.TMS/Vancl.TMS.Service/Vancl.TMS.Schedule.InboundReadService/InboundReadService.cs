﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Vancl.TMS.Schedule.InboundReadImpl;
using Vancl.TMS.Core.Schedule;

namespace Vancl.TMS.Schedule.InboundReadService
{
    public partial class InboundReadService : ServiceBase
    {
        BaseQuartzManager manager = new QuartzManager();
        public InboundReadService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            manager.Start();
            string mailsubject = CommonConst.ServiceEnabledEmailSubject + "被启用";
            string mailcontent = mailsubject + "， 请您关注！";
            MailHelper mailer = new MailHelper(mailsubject, mailcontent, false);
            mailer.SendMailAsync();
        }

        protected override void OnStop()
        {
            manager.Stop();
            string mailsubject = CommonConst.ServiceEnabledEmailSubject + "被停用";
            string mailcontent = mailsubject + "， 请您关注！";
            MailHelper mailer = new MailHelper(mailsubject, mailcontent, false);
            mailer.SendMailAsync();
        }
    }
}

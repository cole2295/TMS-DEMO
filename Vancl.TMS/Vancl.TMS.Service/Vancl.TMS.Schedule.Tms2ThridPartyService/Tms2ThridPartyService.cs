﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Schedule.Tms2ThridPartyImpl;

namespace Vancl.TMS.Schedule.Tms2ThridPartyService
{
    public partial class Tms2ThridPartyService : ServiceBase
    {
        BaseQuartzManager manager = new QuartzManager();

        public Tms2ThridPartyService()
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Vancl.TMS.Schedule.SCInboundImpl;
using Vancl.TMS.Core.Schedule;

namespace Vancl.TMS.Schedule.SCInboundService
{
    public partial class SCInboundService : ServiceBase
    {
        BaseQuartzManager mm = new QuartzManager();

        public SCInboundService()
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysMailerHelper;

namespace Vancl.TMS.Core.Schedule
{
    /// <summary>
    /// 
    /// </summary>
    public class MailHelper
    {
        private MailerSender _sender = null;
        private SysMailMessage _message = null;

        public SysMailMessage Message
        {
            get
            {
                return _message;
            }
        }

        public MailHelper(string body, bool bHTML)
        {
            _sender = new MailerSender(CommonConst.SmtpServer
                  , CommonConst.SmtpUID
                  , CommonConst.SmtpPWD
                  , CommonConst.SmtpUseDefaultCredentials
                  , CommonConst.SmtpMailPort
                  , CommonConst.SmtpEnableSsl);
            _message = new SysMailMessage(CommonConst.MailFrom
                , CommonConst.To
                , CommonConst.Subject
                , body
                , bHTML);
            _message.CC = CommonConst.CC;
            _message.FromDisplayName = CommonConst.FromDisplayName;
            _message.Priority = SysMailPriority.High;
        }

        public MailHelper(string subjectName , string body, bool bHTML)
        {
            _sender = new MailerSender(CommonConst.SmtpServer
                  , CommonConst.SmtpUID
                  , CommonConst.SmtpPWD
                  , CommonConst.SmtpUseDefaultCredentials
                  , CommonConst.SmtpMailPort
                  , CommonConst.SmtpEnableSsl);
            _message = new SysMailMessage(CommonConst.MailFrom
                , CommonConst.To
                , subjectName
                , body
                , bHTML);
            _message.CC = CommonConst.CC;
            _message.FromDisplayName = CommonConst.FromDisplayName;
            _message.Priority = SysMailPriority.High;
        }

        public void SendMailAsync()
        {
            _sender.SendMailAsync(_message);
        }

    }
}

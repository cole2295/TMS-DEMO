using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Vancl.TMS.Util.Net
{
    public static class MailHelper
    {
        //public static void Send(string from, string to, string subject, string body, string smtpHost, NetworkCredential cred = null, AttachmentCollection attachments = null)
        //{
        //    MailMessage mail = new MailMessage(from, to, subject, body);
        //    SmtpClient smtpclient = new SmtpClient();
        //    smtpclient.UseDefaultCredentials = false;
        //    smtpclient.Host = smtpHost;
        //    smtpclient.Credentials = cred;//用户名和密码
        //    smtpclient.Send(mail);
        //}

        public static void Send(string smtpHost, NetworkCredential cred, MailMessage mail)
        {
            SmtpClient smtpclient = new SmtpClient();
            smtpclient.UseDefaultCredentials = false;
            smtpclient.Host = smtpHost;
            smtpclient.Credentials = cred;//用户名和密码
            smtpclient.Send(mail);
        }
    }
}

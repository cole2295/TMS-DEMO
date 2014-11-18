using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.IO;
using System.Data.SqlClient;
using Vancl.TMS.Core.Base;
using Vancl.TMS.Model.Except;
using System.Data.Common;

namespace Vancl.TMS.Core.Schedule
{
    public abstract class QuartzExecute : IStatefulJob
    {
        #region IJob 成员

        public string GetExceptionDesc(Exception e)
        {
            if (null == e)
            {
                return "";
            }
            else
            {
                return String.Format("Message:\r\n{0}\r\nStackTree:\r\n{1}\r\n{2}", e.Message, e.StackTrace, GetExceptionDesc(e.InnerException));
            }
        }

        public void Execute(JobExecutionContext context)
        {
            try
            {
                DoJob(context);
            }
            catch (SyncFileException e)
            {
                MailHelper sender = new MailHelper(
                GetExceptionDesc(e)    //string.Format("{0}\r\n{1}\r\n{2}\r\n{3}", e.Message, e.StackTrace, e.InnerException == null ? "" : e.StackTrace, e.InnerException == null ? "" : e.Message)
                , false);
                sender.Message.Attachments = new String[] { e.AbnormalFileFullName };
                sender.SendMailAsync();
            }
            catch (IOException e)
            {
                TMSysLogger.logEmail.ErrorFormat("TMS数据同步IO异常:{0}", GetExceptionDesc(e));
            }
            catch (DbException e)
            {
                TMSysLogger.logEmail.ErrorFormat("TMS数据同步SQL异常:{0}", GetExceptionDesc(e));
            }
            catch (Exception e)
            {
                TMSysLogger.logEmail.ErrorFormat("TMS数据同步异常:{0}", GetExceptionDesc(e));
            }
        }

        #endregion

        public abstract void DoJob(JobExecutionContext context);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.JobMonitor;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Model.JobMonitor;

namespace Vancl.TMS.Schedule.MonitorImpl
{
    public class OutBoundMonitorJob : QuartzExecute
    {
        IOutBoundMonitorBLL outboundmonitorervice = ServiceFactory.GetService<IOutBoundMonitorBLL>("OutBoundMonitorBLL");

        /// <summary>
        /// 构建监控通知信息
        /// </summary>
        /// <param name="templatefilename">通知email模板信息</param>
        /// <returns></returns>
        private string BuilderNoticeContent(string templatefilename,OutboundMonitorTemplate templateModle)
        {
            string templateContent = IOHelper.GetFileContent(templatefilename);
            templateContent = templateContent.Replace("[NoSync]", templateModle.StatisticInfio.NoSync.ToString());
            templateContent = templateContent.Replace("[Syncing]", templateModle.StatisticInfio.Syncing.ToString());
            templateContent = templateContent.Replace("[Synced]", templateModle.StatisticInfio.Synced.ToString());
            templateContent = templateContent.Replace("[MonitorTime]", templateModle.StatisticInfio.MonitorTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return templateContent;
        }

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            string JobConfigDir = Consts.JobConfigDir;
            if (!Consts.JobConfigDir.EndsWith(@"\"))
            {
                JobConfigDir += @"\";
            }
            string templatefilename = JobConfigDir + context.JobDetail.JobDataMap["TemplateFileName"].ToString();
            MailHelper mailer = new MailHelper(context.JobDetail.JobDataMap["EmailSubjectName"].ToString(), BuilderNoticeContent(templatefilename, outboundmonitorervice.GetNoticeTemplate()), true);
            mailer.SendMailAsync();            
        }
    }
}

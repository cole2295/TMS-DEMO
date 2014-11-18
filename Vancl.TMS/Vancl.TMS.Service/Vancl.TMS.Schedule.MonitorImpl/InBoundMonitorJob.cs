using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.JobMonitor;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.IO;

namespace Vancl.TMS.Schedule.MonitorImpl
{
    public class InBoundMonitorJob : QuartzExecute
    {
        IInBoundMonitorBLL inboundmonitorervice = ServiceFactory.GetService<IInBoundMonitorBLL>("InBoundMonitorBLL");

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            string JobConfigDir = Consts.JobConfigDir;
            if (!Consts.JobConfigDir.EndsWith(@"\"))
            {
                JobConfigDir += @"\";
            }
            string templatefilename = JobConfigDir + context.JobDetail.JobDataMap["TemplateFileName"].ToString();
            MailHelper mailer = new MailHelper(context.JobDetail.JobDataMap["EmailSubjectName"].ToString(), BuilderNoticeContent(templatefilename, inboundmonitorervice.GetNoticeTemplate()), true);
            mailer.SendMailAsync();        
        }

        /// <summary>
        /// 构建监控通知信息
        /// </summary>
        /// <param name="templatefilename">通知email模板信息</param>
        /// <returns></returns>
        private string BuilderNoticeContent(string templatefilename, Model.JobMonitor.InboundMonitorTemplate templateModle)
        {
            string templateContent = IOHelper.GetFileContent(templatefilename);
            //Box Statistic Info
            templateContent = templateContent.Replace("[BoxNoSync]", templateModle.BoxStatisticInfio.NoSync.ToString());
            templateContent = templateContent.Replace("[BoxSyncing]", templateModle.BoxStatisticInfio.Syncing.ToString());
            templateContent = templateContent.Replace("[BoxSynced]", templateModle.BoxStatisticInfio.Synced.ToString());
            templateContent = templateContent.Replace("[BoxMonitorTime]", templateModle.BoxStatisticInfio.MonitorTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //Order Statistic Info
            templateContent = templateContent.Replace("[OrderNoSync]", templateModle.OrderStatisticInfio.NoSync.ToString());
            templateContent = templateContent.Replace("[OrderSyncing]", templateModle.OrderStatisticInfio.Syncing.ToString());
            templateContent = templateContent.Replace("[OrderSynced]", templateModle.OrderStatisticInfio.Synced.ToString());
            templateContent = templateContent.Replace("[OrderMonitorTime]", templateModle.OrderStatisticInfio.MonitorTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return templateContent;
        }

    }
}

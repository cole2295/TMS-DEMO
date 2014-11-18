using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.Inbound.SMS;

namespace Vancl.TMS.Schedule.SCInboundImpl
{
    public class SCInboundSMSJob : QuartzExecute
    {
        IInboundBLL InboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
        IInboundSMSQueueBLL smsqueue = ServiceFactory.GetService<IInboundSMSQueueBLL>();

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            var list = smsqueue.GetInboundSMSQueue(new InboundSMSQueueJobArgModel()
            {
                OpCount = Consts.OpCount,
                IntervalDay = Consts.IntervalDay,
                PerBatchCount = Consts.PerBatchCount
            });
            if (list == null || list.Count <= 0)
            {
                return;
            }
            var listErrorInfo = new List<string>();
            foreach (var item in list)
            {
                try
                {
                    InboundBLL.HandleInboundSMSQueue(new InboundSMSQueueArgModel()
                    {
                        QueueItem = item
                    });
                }
                catch (Exception ex)
                {
                    var errorInfo = GetExceptionDesc(ex);
                    smsqueue.UpdateOpCount(item.QUID, errorInfo);
                    listErrorInfo.Add(String.Format(@"运单:{0}入库短信发送服务执行失败,错误:{1}", item.FormCode, errorInfo));
                }
            }
            if (listErrorInfo.Count > 0)
            {
                throw new Exception(String.Join(@"\r\n", listErrorInfo.ToArray()));
            }
        }
    }
}

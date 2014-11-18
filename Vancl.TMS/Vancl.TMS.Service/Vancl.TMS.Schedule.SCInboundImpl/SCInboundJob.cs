using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util;


namespace Vancl.TMS.Schedule.SCInboundImpl
{
    public class SCInboundJob : QuartzExecute
    {
        IInboundBLL InboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
        IInboundQueueBLL queue = ServiceFactory.GetService<IInboundQueueBLL>();

        public override void DoJob(Quartz.JobExecutionContext context)
        {
           int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);

            DoJob(remainder);
           
        }

        public void DoJob(int remainder)
        {

            var list = queue.GetInboundQueueList(new InboundQueueJobArgModel()
            {
                OpCount = Consts.OpCount,
                IntervalDay = Consts.IntervalDay,
                PerBatchCount = Consts.PerBatchCount,
                ModValue = Consts.ModValue,
                Remaider = remainder
            });
            if (list == null || list.Count <= 0)
            {
                return;
            }
            var listErrorInfo = new List<string>();
            var LineAreaSMSConfig = InboundBLL.GetLineAreaSMSConfig();
            var MerchantSMSConfig = InboundBLL.GetMerchantSMSConfig();
            foreach (var item in list)
            {
                try
                {
                    InboundQueueArgModel argument = new InboundQueueArgModel()
                    {
                        LineAreaSMSConfig = LineAreaSMSConfig,
                        MerchantSMSConfig = MerchantSMSConfig,
                        QueueItem = item
                    };
                    if (argument.QueueItem.Version == 2)
                        InboundBLL.HandleInboundQueueV2(argument);
                    else
                        InboundBLL.HandleInboundQueue(argument);
                }
                catch (Exception ex)
                {
                    queue.UpdateOpCount(item.IBSID);
                    listErrorInfo.Add(String.Format(@"运单:{0}入库服务执行失败,错误:{1}", item.FormCode, GetExceptionDesc(ex)));
                }
            }
            if (listErrorInfo.Count > 0)
            {
                throw new Exception(String.Join(@"\r\n", listErrorInfo.ToArray()));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Quartz;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Schedule.OutBoundReadImpl
{
    /// <summary>
    /// 出库同步
    /// </summary>
    public class OutboundReadDBjob : QuartzExecute
    {
        private IOutboundBLL _outboundBLL = ServiceFactory.GetService<IOutboundBLL>("outboundBLL");

        public override void DoJob(JobExecutionContext context)
        {
            Enums.SyncNoType NoType = (Enums.SyncNoType)int.Parse(context.JobDetail.JobDataMap["NoType"].ToString());
            string CurFileDir = Consts.FILE_DIR;
            if (!Consts.FILE_DIR.EndsWith(@"\"))
            {
                CurFileDir += @"\";
            }
            CurFileDir += Consts.THREAD_CATEGORY_DIR;
            if (NoType == Enums.SyncNoType.Batch)
            {
                CurFileDir += String.Format(@"_{0}", Consts.BatchModelDirName);
            }
            CurFileDir += context.JobDetail.JobDataMap["Remaider"].ToString();
            _outboundBLL.LMSOutbondDataToFile(
                new OutboundReadParam(
                CurFileDir
                , Consts.WriteFileHeader
                , Consts.ReadFileHeader) 
                { 
                    NoType = NoType, 
                    NormalModValue = Consts.NormalModValue, 
                    BatchModValue = Consts.BatchModValue, 
                    Remaider = int.Parse(context.JobDetail.JobDataMap["Remaider"].ToString()),
                    OnlineTime = Consts.OnlineTime,
                    IntervalDay = Consts.IntervalDay
                }
                );
        }
    }

}

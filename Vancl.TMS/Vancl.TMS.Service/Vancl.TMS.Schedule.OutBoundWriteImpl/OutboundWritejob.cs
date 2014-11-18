using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Schedule.OutBoundWriteImpl
{
    public class OutboundWritejob : QuartzExecute
    {
        private IOutboundBLL _outboundBLL = ServiceFactory.GetService<IOutboundBLL>("outboundBLL");

        public override void DoJob(Quartz.JobExecutionContext context)
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
            OutboundWriteParam argument = new OutboundWriteParam(
                CurFileDir
                , Consts.SEARCH_PATTERN
                , Consts.BACKUP_DIR
                , Consts.ABNORMAL_DIR) { NoType = NoType, Remaider = int.Parse(context.JobDetail.JobDataMap["Remaider"].ToString()) };

            _outboundBLL.FileToTMSOrder(argument);

        }
    }
}

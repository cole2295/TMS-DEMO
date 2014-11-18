using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Quartz;
using Vancl.TMS.Model.Synchronous.InSync;

namespace Vancl.TMS.Schedule.Lms2TmsImpl
{
    public class Lms2TmsJob : QuartzExecute
    {
        ILms2TmsSyncBLL _bll = ServiceFactory.GetService<ILms2TmsSyncBLL>("Lms2TmsSyncBLL");
        public override void DoJob(JobExecutionContext context)
        {
            int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);
            List<LmsWaybillStatusChangeLogModel> list = _bll.ReadLmsChangeLogs(
                new Lms2TmsJobArgs()
                {
                    IntervalDay = Consts.IntervalDay,
                    Mod = Consts.THREAD_COUNT,
                    OnlineTime = Consts.OnlineTime,
                    Remainder = remainder,
                    TopCount = Consts.BATCH_MAX_COUNT
                });
            if (list == null || list.Count == 0)
            {
                return;
            }
            var OrderedLogModelList = list.OrderBy(p => p.WaybillNo);
            foreach (LmsWaybillStatusChangeLogModel model in OrderedLogModelList)
            {
                _bll.DoSync(model);
            }
        }
    }
}

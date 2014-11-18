using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Quartz;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.Schedule.Tms2LmsImpl
{
    public class Tms2LmsJob : QuartzExecute
    {
        ITms2LmsSyncBLL _bll = ServiceFactory.GetService<ITms2LmsSyncBLL>("Tms2LmsSyncBLL");
        public override void DoJob(JobExecutionContext context)
        {
            int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);
            Do(remainder);
        }

        public void Do(int remainder)
        {
            List<BillChangeLogModel> list = _bll.ReadTmsChangeLogs
                (
                    new Tms2LmsJobArgs()
                    {
                        IntervalDay = Consts.IntervalDay,
                        TopCount = Consts.BATCH_MAX_COUNT,
                        Mod = Consts.THREAD_COUNT,
                        Remainder = remainder
                    }
                );
            if (list == null || list.Count == 0)
            {
                return;
            }
            foreach (BillChangeLogModel model in list)
            {
                _bll.DoSync(model);
            }
        }
    }
}

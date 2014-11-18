using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.Schedule.Tms2ThridPartyImpl
{
    public class TMS2LMS4AgingMonitoringJob : QuartzExecute
    {
        ITms2ThridPartyBLL target = ServiceFactory.GetService<ITms2ThridPartyBLL>("Tms2ThridPartyBLL");

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            Tms2ThridPartyJobArgs args = new Tms2ThridPartyJobArgs()
            {
                IntervalDay = Consts.IntervalDay,
                OnlineTime = Consts.OnlineTime,
                TopCount = Consts.BATCH_MAX_COUNT
            }; 
            target.TMS2LMS4AgingMonitoring(args);
        }

    }
}

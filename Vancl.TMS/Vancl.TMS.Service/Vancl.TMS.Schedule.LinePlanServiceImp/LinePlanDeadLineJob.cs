using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.Schedule.LinePlanServiceImpl
{
    public class LinePlanDeadLineJob : QuartzExecute
    {
        ILinePlanBLL linePlanService = ServiceFactory.GetService<ILinePlanBLL>();

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            linePlanService.UpdateDeadLineStatus();
        }
    }
}

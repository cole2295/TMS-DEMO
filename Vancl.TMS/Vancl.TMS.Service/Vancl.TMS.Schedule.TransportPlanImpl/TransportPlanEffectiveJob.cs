using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Transport.Plan;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.Schedule.TransportPlanImpl
{
    public class TransportPlanEffectiveJob : QuartzExecute
    {
        ITransportPlanBLL _transportplanBLL = ServiceFactory.GetService<ITransportPlanBLL>("TransportPlanBLL");

        public override void DoJob(Quartz.JobExecutionContext context)
        {
            _transportplanBLL.UpdateNeedEffectived();
        }
    }
}

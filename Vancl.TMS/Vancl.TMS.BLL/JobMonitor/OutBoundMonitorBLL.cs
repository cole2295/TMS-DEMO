using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.JobMonitor;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.JobMonitor;

namespace Vancl.TMS.BLL.JobMonitor
{
    public class OutBoundMonitorBLL : IOutBoundMonitorBLL
    {
        #region IOutBoundMonitor 成员

        private IOutboundLMSDAL lmsDAL = ServiceFactory.GetService<IOutboundLMSDAL>("lmsOutboundDAL");

        public Model.JobMonitor.OutboundMonitorTemplate GetNoticeTemplate()
        {
            return new OutboundMonitorTemplate()
            {
                StatisticInfio = lmsDAL.GetStatisticInfo()
            };
        }

        #endregion
    }
}

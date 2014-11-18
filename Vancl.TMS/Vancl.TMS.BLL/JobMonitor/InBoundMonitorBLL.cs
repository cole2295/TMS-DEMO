using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.JobMonitor;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.JobMonitor
{
    /// <summary>
    /// 入库同步监控
    /// </summary>
    public class InBoundMonitorBLL : IInBoundMonitorBLL
    {
        IInboundLMSDAL _lmsDAL = ServiceFactory.GetService<IInboundLMSDAL>("InboundLMSDAL");

        #region IInBoundMonitorBLL 成员

        public Model.JobMonitor.InboundMonitorTemplate GetNoticeTemplate()
        {
            return new Model.JobMonitor.InboundMonitorTemplate()
            {
                OrderStatisticInfio = _lmsDAL.GetOrderStatisticInfo(),
                BoxStatisticInfio = _lmsDAL.GetBoxStatisticInfo()
            };
        }

        #endregion
    }
}

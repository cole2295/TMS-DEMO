using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.JobMonitor;

namespace Vancl.TMS.IBLL.JobMonitor
{
    /// <summary>
    /// 入库同步监听通知接口
    /// </summary>
    public interface IInBoundMonitorBLL
    {
        /// <summary>
        /// 监控通知模板数据信息
        /// </summary>
        /// <returns></returns>
        InboundMonitorTemplate GetNoticeTemplate();
    }
}

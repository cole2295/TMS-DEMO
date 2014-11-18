using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.OutServiceProxy;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.OutServiceProxy
{
    /// <summary>
    /// 云平台服务代理接口
    /// </summary>
    public interface IProxy4Cloud
    {

        /// <summary>
        /// 添加时效监控日志
        /// </summary>
        /// <param name="proxyModel"></param>
        ResultModel AddAgingMonitoringLog(AgingMonitoringLogProxyModel proxyModel);

    }
}

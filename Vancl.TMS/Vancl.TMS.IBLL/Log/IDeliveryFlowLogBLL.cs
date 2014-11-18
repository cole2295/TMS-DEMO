using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IBLL.Log
{
    /// <summary>
    /// 运输状态日志
    /// </summary>
    public interface IDeliveryFlowLogBLL
    {
        /// <summary>
        /// 读取运输状态日志
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<DeliveryFlowLogModel> Read(DeliveryFlowLogSearchModel searchmodel);
    }
}

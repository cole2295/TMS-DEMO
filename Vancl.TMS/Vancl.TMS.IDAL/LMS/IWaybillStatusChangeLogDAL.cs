using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 状态变更日志数据层接口
    /// </summary>
    public interface IWaybillStatusChangeLogDAL
    {
        /// <summary>
        /// 添加状态变更日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(WaybillStatusChangeLogEntityModel model);
    }

}

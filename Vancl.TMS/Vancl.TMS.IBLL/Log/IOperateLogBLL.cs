using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IBLL.Log
{
    /// <summary>
    /// 操作日志相关
    /// </summary>
    public interface IOperateLogBLL
    {
        /// <summary>
        /// 读取操作日志
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<OperateLogModel> Read(BaseOperateLogSearchModel searchmodel);
    }

}

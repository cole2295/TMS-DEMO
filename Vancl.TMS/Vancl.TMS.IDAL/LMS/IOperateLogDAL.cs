using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 操作日志数据层接口
    /// </summary>
    public interface IOperateLogDAL
    {
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行数</returns>
        int Add(OperateLogEntityModel model);

    }
}

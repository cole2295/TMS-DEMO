using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IDAL.Log
{
    /// <summary>
    /// 外包日志数据层接口
    /// </summary>
    public interface IOutsourcingLogDAL
    {
        /// <summary>
        /// 增加外包日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(OutsourcingLogModel model);

        /// <summary>
        /// 批量增加外包日志
        /// </summary>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int BatchAdd(List<OutsourcingLogModel> listModel);
    }
}

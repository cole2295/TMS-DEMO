using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 入库数据层接口
    /// </summary>
    public interface IInboundDAL
    {
        /// <summary>
        /// 新增入库数据
        /// </summary>
        /// <param name="model">入库实体对象</param>
        /// <returns>成功返回主键ID，否者返回0</returns>
        long Add(InboundEntityModel model);

    }
}

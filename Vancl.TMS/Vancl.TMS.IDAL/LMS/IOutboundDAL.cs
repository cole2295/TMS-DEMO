using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 出库数据层接口
    /// </summary>
    public  interface IOutboundDAL
    {
        /// <summary>
        /// 添加出库数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>出库主键ID</returns>
        long Add(OutboundEntityModel model);


    }
}

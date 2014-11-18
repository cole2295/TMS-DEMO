using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.IDAL.Sorting.Outbound
{
    /// <summary>
    /// 分拣同步到TMS城际运输【出库】数据层接口
    /// </summary>
    public interface ISC_SYN_TMS_OutboxDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(SC_SYN_TMS_OutboxEntityModel model);

    }


}


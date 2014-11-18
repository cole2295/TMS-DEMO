using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound;

namespace Vancl.TMS.IDAL.Sorting.Inbound
{
    /// <summary>
    /// 分拣同步到TMS城际运输
    /// </summary>
    public interface ISC_SYN_TMS_InorderDAL
    {
        /// <summary>
        /// 新增分拣同步到
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(SC_SYN_TMS_InorderEntityModel model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// LMS到FMS财务中间表数据
    /// </summary>
    public interface ILMS_SYN_FMS_CODDAL
    {
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(LMS_SYN_FMS_CODEntityModel model);
    }

}

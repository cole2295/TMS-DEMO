using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣批量操作错误显示对象
    /// </summary>
    public class SortCenterBatchErrorModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public String WaybillNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public String CustomerOrder { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public String ErrorMsg { get; set; }
    }
}

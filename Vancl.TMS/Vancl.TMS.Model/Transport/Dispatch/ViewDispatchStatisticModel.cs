using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 运输调度统计信息显示对象
    /// </summary>
    public class ViewDispatchStatisticModel : BaseModel
    {
        /// <summary>
        /// 总单量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 调度完成单量
        /// </summary>
        public int DispatchedCount { get; set; }

        /// <summary>
        /// 待调度单量
        /// </summary>
        public int PreDispatchCount { get; set; }

        /// <summary>
        /// 无法识别单量
        /// </summary>
        public int UnrecognizedCount { get; set; }

        /// <summary>
        /// 超期滞留单量
        /// </summary>
        public int ExtendedStayCount { get; set; }
    }
}

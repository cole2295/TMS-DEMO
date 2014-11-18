using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.DeliveryAbnormal
{
    /// <summary>
    /// 预调度异常处理查询
    /// </summary>
    public class PreDispatchAbnormalSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime? BoxTimeStart { get; set; }

        /// <summary>
        /// 查询结束
        /// </summary>
        public DateTime? BoxTimeEnd { get; set; }

        /// <summary>
        /// 预调度状态
        /// </summary>
        public Enums.BatchPreDispatchedStatus IsPreDispatch { get; set; } 
    }
}

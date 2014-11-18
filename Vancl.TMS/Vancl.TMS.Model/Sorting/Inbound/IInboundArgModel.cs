using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库操作参数接口
    /// </summary>
    public interface IInboundArgModel : ISortCenterArgModel
    {

        /// <summary>
        /// 分拣入库前置条件
        /// </summary>
        InboundPreConditionModel PreCondition { get; set; }

        /// <summary>
        /// 入库限制数量
        /// </summary>
        int LimitedInboundCount { get; set; }

        /// <summary>
        /// 是否入库限制数量
        /// </summary>
        bool IsLimitedQuantity { get; set; }

    }
}

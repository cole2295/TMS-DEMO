using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库公用参数接口对象
    /// </summary>
    public interface IOutboundArgModel : ISortCenterArgModel
    {
        /// <summary>
        /// 出库前置条件
        /// </summary>
        OutboundPreConditionModel PreCondition { get; set; }
    }


}

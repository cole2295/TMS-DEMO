using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库前置条件对象
    /// </summary>
    public class OutboundPreConditionModel
    {
        /// <summary>
        /// 出库运单前一状态[已分拣状态]
        /// </summary>
        public Enums.BillStatus PreStatus { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库后置条件对象
    /// </summary>
    public class OutboundAfterConditionModel
    {
        /// <summary>
        /// 出库后的运单状态
        /// </summary>
        public Enums.BillStatus AfterStatus { get; set; }

        /// <summary>
        /// 当前配送商Code
        /// </summary>
        public String CurrentDistributionCode { get; set; }

    }
}

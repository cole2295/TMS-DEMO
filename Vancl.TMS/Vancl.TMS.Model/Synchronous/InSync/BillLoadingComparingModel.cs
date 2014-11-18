using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous.InSync
{
    /// <summary>
    /// LMS同步到TMS时装车功能用来比较的model
    /// </summary>
    public class BillLoadingComparingModel : IBillLms2TmsForComparing
    {
        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 当前配送商编号
        /// </summary>
        public string CurrentDistributionCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound.SMS
{
    /// <summary>
    /// 出库短信运单对象
    /// </summary>
    public class OutboundSMSBillModel
    {
        /// <summary>
        /// 系统运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 运单来源
        /// </summary>
        public Enums.BillSource Source { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 分拣操作类型
        /// </summary>
        public Enums.SortCenterOperateType OpType { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

    }
}

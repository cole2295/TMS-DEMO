using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库运单对象
    /// </summary>
    public class OutboundBillModel : BillModel
    {
        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

		public int ToDeliverstationid { get; set; }

	    public string ToDistributionCode { get; set; }

	    /// <summary>
		/// 目的分拣中心
		/// </summary>
		public int ToSortingCenterId { get; set; }

	    /// <summary>
        /// 入库类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }

        /// <summary>
        /// 客户移动电话
        /// </summary>
        public String ReceiveMobile { get; set; }
    }
}

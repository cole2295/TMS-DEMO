using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	public class OutboundBoxModel : BoxModel
	{
		/// <summary>
		/// 箱号
		/// </summary>
		public string BoxNo { get; set; }

		/// <summary>
		/// 数量
		/// </summary>
		public int BillCount { get; set; }


		/// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }

		/// <summary>
		/// 是否出库
		/// </summary>
		public int IsOutbounded { get; set; }

		/// <summary>
        /// 客户移动电话
        /// </summary>
        public String ReceiveMobile { get; set; }
	}
}

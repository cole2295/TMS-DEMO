using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	/// <summary>
	/// 出库交接表打印查询Model
	/// </summary>
	public class OutboundPrintSearchModel : BaseSearchModel
	{
		/// <summary>
		/// 时间类型（出库时间、批次打印时间）
		/// </summary>
		public int TypeTime { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		/// <summary>
		/// 当前操作部门
		/// </summary>
		public int ExpressId { get; set; }

		/// <summary>
		/// 目的地ID
		/// </summary>
		public int ArrivalId { get; set; }

		public string  BatchNo { get; set; }

		public string BoxNo { get; set; }

		public string FormCode { get; set; }

		//已选目的地ID列表
		public string ArrivalIdList { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.RangeDefined
{
	/// <summary>
	/// 查询分拣范围列表
	/// </summary>
	public class ViewRangeDefinedSearchListModel : BaseSearchModel
	{
		public string RangeDefinedId { get; set; }

		/// <summary>
		/// 分拣中心
		/// </summary>
		public string SortingCenter { get; set; }

		/// <summary>
		/// 分拣范围
		/// </summary>
		public string RangeDefined { get; set; }

	}
}

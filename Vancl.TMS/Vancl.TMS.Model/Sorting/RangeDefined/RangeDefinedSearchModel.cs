using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.RangeDefined
{
	/// <summary>
	/// 分拣范围定义查询条件对象
	/// </summary>
	public class RangeDefinedSearchModel : BaseSearchModel
	{
		/// <summary>
		/// 范围定义基础配送公司ID
		/// </summary>
		public int BaseExpressCompanyId { get; set; }

		/// <summary>
		/// 范围定义后的目的配送公司类型
		/// </summary>
		public int CompanyFlag { get; set; }

		/// <summary>
		/// 范围定义后的目的配送公司ID
		/// </summary>
		public string RangedExpressCompanyIds { get; set; }

		/// <summary>
		/// 分拣当前操作人员对象
		/// </summary>
		public SortCenterUserModel OpUser { get; set; }
	}
}

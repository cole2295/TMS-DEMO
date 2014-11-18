using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.RangeDefined
{
	/// <summary>
	/// 分拣范围定义
	/// </summary>
	[Serializable]
	 public class RangeDefinedModel : BaseModel,ISequenceable
	{
		 public System.Decimal RangeDefinedId { get; set; }

		 public int BaseExpressCompanyId { get; set; }

		 public string BaseDistributionCode { get; set; }

		 public int RangedExpressCompanyId { get; set; }

		 public int RangedCompanyFlag { get; set; }

		 #region ISequenceable 成员

		 public string SequenceName
		 {
			 get { return "SEQ_TMS_RANGEDEFINED_ID"; }
		 }

		 #endregion

	}
}

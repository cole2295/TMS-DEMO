using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Model.Sorting.RangeDefined
{
	/// <summary>
	/// 分拣范围定义打印导出模型
	/// </summary>
	public class RangeDefinedPrintExportModel : IExportXlsable
	{
		[ExportXls("序号")]
		public int No { get; set; }

		[ExportXls("分拣中心")]
		public string SortingCenter { get; set; }

		[ExportXls("分拣范围")]
		public string RangeDefined { get; set; }
	}
}

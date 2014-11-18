using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	/// <summary>
	/// 出库打印导出模型V2
	/// </summary>
	public class OutboundPrintExportModelV2 : IExportXlsable
	{
		[ExportXls("序号")]
		public int No { get; set; }

		[ExportXls("出库时间")]
		public DateTime OutboundTime { get; set; }

		[ExportXls("出库目的地")]
		public string OutboundDesName { get; set; }

		[ExportXls("批次号")]
		public string BatchNo { get; set; }

		[ExportXls("箱数")]
		public int BoxsCount { get; set; }

		[ExportXls("运单数")]
		public int FormCodesCount { get; set; }

		[ExportXls("接收邮件")]
		public string Email { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Extensions;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	public class OutboundPrintExportDetailsModelV2 : IExportXlsable
	{
		[ExportXls("序号")]
		[Description("序号")]
		public string No { get; set; }

		[ExportXls("运单号")]
		[Description("运单号")]
		public string FormCode { get; set; }

		[ExportXls("配送单号")]
		[Description("配送单号")]
		public string CustomerCode { get; set; }

		[ExportXls("商家")]
		[Description("商家")]
		public string MerchantName { get; set; }

		[ExportXls("应收金额")]
		[Description("应收金额")]
		public Decimal NeedFund { get; set; }

		//[ExportXls("应退金额")]
		//public Decimal BackFund { get; set; }

		[ExportXls("出库时间")]
		[Description("出库时间")]
		public DateTime? OutboundTime { get; set; }

		[ExportXls("出库目的地")]
		[Description("出库目的地")]
		public string ArrivedDes { get; set; }
	
	}
}

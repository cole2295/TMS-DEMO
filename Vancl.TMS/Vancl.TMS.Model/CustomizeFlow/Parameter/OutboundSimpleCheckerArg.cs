using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.Model.CustomizeFlow.Parameter
{
	public class OutboundSimpleCheckerArg : CheckerParameter
	{
		public IOutboundArgModel OutboundSimpleArg { get; set; }

		public OutboundBillModel OutboundBill { get; set; }

		public OutboundBoxModel OutboundBox { get; set; }

		public string BoxNo { get; set; }
	}
}

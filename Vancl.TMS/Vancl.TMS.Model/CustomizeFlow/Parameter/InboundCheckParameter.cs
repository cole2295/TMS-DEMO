using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.CustomizeFlow.Parameter
{
	public class InboundCheckParameter : CheckerParameter
	{
		public bool IsFirstSorting { get; set; }

		public string CurDistributionCode { get; set; }
	}
}

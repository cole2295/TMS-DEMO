using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	/// <summary>
	/// 获取出库到当前目的地的数量和批次号
	/// </summary>
	public class ViewGetCountAndBatchNo : ResultModel
	{
		//今日出库总数量
		public int CurOptCount { get; set; }

		//出库到当前目的地的数量
		public int CurArrivalCount { get; set; }

		//出库到当前目的地的批次号
		public String CurArrivalBatchNo { get; set; }
	}
}

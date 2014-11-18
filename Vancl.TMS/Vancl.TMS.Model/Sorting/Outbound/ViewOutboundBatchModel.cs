using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 批量出库View Model
    /// </summary>
    public class ViewOutboundBatchModel : ViewSortCenterBatchModel
    {
		/// <summary>
		/// 箱号
		/// </summary>
		public string BoxNo { get; set; }

		/// <summary>
		/// 出库到的目的地
		/// </summary>
		public string OutboundDes { get; set; }

		/// <summary>
		/// 出库状态（成功失败）
		/// </summary>
		public string BoxOutboundStatus { get; set; }
    }


}

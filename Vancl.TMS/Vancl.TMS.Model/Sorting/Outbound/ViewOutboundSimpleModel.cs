using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 逐单入库View Model
    /// </summary>
    public class ViewOutboundSimpleModel : ResultModel
    {
        /// <summary>
        /// 出库批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public String CustomerOrder { get; set; }

		/// <summary>
		/// 出库到的目的地
		/// </summary>
		public string OutboundDes { get; set; }

		/// <summary>
		/// 出库状态
		/// </summary>
		public string OutboundStatus { get; set; }

	    /// <summary>
	    /// 出库回执信息
	    /// </summary>
	    //public string OutboundMessage { get; set; }

		/// <summary>
		/// 今日出库总数量
		/// </summary>
		public int CurOptCount { get; set; }

		/// <summary>
		/// 今日已出库到当前目的地的数量
		/// </summary>
		public int CurArrivalCount { get; set; }

    }
}

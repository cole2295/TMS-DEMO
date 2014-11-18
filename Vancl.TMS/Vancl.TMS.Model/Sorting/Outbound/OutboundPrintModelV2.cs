using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
	/// <summary>
	/// 出库打印模型V2
	/// </summary>
	public class OutboundPrintModelV2
	{
		public int Id { get; set; }

		/// <summary>
		/// 出库时间
		/// </summary>
		public DateTime OutboundTime { get; set; }

		/// <summary>
		/// 批次打印时间
		/// </summary>
		public DateTime BatchPrintTime { get; set; }

		/// <summary>
		/// 出库目的地Id
		/// </summary>
		public int OutboundArrivedId{ get; set; }
		/// <summary>
		/// 出库目的地
		/// </summary>
		public string OutboundDesName { get; set; }

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchNo { get; set; }

		/// <summary>
		/// 箱数
		/// </summary>
		public int BoxsCount { get; set; }

		/// <summary>
		/// 运单数
		/// </summary>
		public int FormCodesCount { get; set; }

		/// <summary>
		/// 接收邮件
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// 重量
		/// </summary>
		public Decimal Weight { get; set; }

		/// <summary>
		/// 发货地
		/// </summary>
		public string DepartureName { get; set; }
	}
}

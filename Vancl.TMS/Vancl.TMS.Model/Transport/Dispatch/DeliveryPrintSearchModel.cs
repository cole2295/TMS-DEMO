using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 提货单打印搜索列表
    /// </summary>
    public class DeliveryPrintSearchModel : BaseSearchModel
    {      
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int? DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int? CarrierID { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 调度状态
        /// </summary>
        public Enums.DeliveryStatus? DeliveryStatus { get; set; }
    }
}

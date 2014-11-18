using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Report.CarrierAccountReport
{
    /// <summary>
    /// 承运商结算报表检索对象
    /// </summary>
    public class CarrierAccountReportSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int? CarrierID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int? DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        /// <summary>
        /// 发货开始时间
        /// </summary>
        public DateTime? DepartureStartTime { get; set; }

        /// <summary>
        /// 发货结束时间
        /// </summary>
        public DateTime? DepartureEndTime { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus? DeliveryStatus { get; set; }

    }

}

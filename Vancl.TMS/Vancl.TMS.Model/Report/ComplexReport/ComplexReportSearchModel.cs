using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Report.ComplexReport
{
    /// <summary>
    /// 综合报表检索对象
    /// </summary>
    public class ComplexReportSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 出发开始时间
        /// </summary>
        public DateTime? DepartureStartTime { get; set; }

        /// <summary>
        /// 出发结束时间
        /// </summary>
        public DateTime? DepartureEndTime { get; set; }

        /// <summary>
        /// 预计到货开始时间
        /// </summary>
        public DateTime? ExparrivalStartTime { get; set; }

        /// <summary>
        /// 预计到货结束时间
        /// </summary>
        public DateTime? ExparrivalEndTime { get; set; }

        /// <summary>
        /// 实际到货开始时间
        /// </summary>
        public DateTime? DesreceiveStartTime { get; set; }

        /// <summary>
        /// 实际到货结束时间
        /// </summary>
        public DateTime? DesreceiveEndTime { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public String CarrierID { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.ComplexReportDeliveryStatus? Status { get; set; }

        /// <summary>
        /// 是否延误
        /// </summary>
        public bool? IsDelay { get; set; }

        /// <summary>
        /// 延误时长开始点
        /// </summary>
        public decimal? DelaytimeStart { get; set; }

        /// <summary>
        /// 延误时长结束
        /// </summary>
        public decimal? DelaytimeEnd { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public String DeliveryNo { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public String CustWaybillNo { get; set; }

        /// <summary>
        /// 丢失类型
        /// </summary>
        public Enums.LostType? LostType { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public String FormCode { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    public class DeliveryAssessmentSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 发货开始日期
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 发货结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 出发地id
        /// </summary>
        public int? DepartureID { get; set; }

        /// <summary>
        /// 承运商主键id
        /// </summary>
        public int? CarrierID { get; set; }

        /// <summary>
        /// 延误时长开始区间
        /// </summary>
        public decimal? DelaySpanBegin { get; set; }

        /// <summary>
        /// 延误时长结束区间
        /// </summary>
        public decimal? DelaySpanEnd { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus? DeliveryStatus { get; set; }
    }
}

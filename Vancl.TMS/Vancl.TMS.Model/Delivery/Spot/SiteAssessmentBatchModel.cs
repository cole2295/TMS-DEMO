using System;
using Vancl.TMS.Model.Common;
using System.Collections.Generic;

namespace Vancl.TMS.Model.Delivery.Spot
{
    /// <summary>
    /// 现场考核信息
    /// </summary>
    [Serializable]
    public class SiteAssessmentBatchModel
    {
        /// <summary>
        /// 提货单号(主键)
        /// </summary>
        public List<string> ListDeliveryNo { get; set; }

        /// <summary>
        /// 实际到货时间
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// 实际离库时间
        /// </summary>
        public DateTime LeaveTime { get; set; }

        /// <summary>
        /// 考核状态
        /// </summary>
        public bool AssessmentStatus { get; set; }

        /// <summary>
        /// 异常原因
        /// </summary>
        public string Reason { get; set; }
    }
}

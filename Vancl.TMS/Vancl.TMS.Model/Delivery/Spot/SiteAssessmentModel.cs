using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.Spot
{
    /// <summary>
    /// 现场考核信息
    /// </summary>
    [Serializable]
    public class SiteAssessmentModel : BaseModel, IOperateLogable
    {
        #region ILogable 成员

        public string PrimaryKey
        {
            get { return DeliveryNO; }
            set { DeliveryNO = value; }
        }

        #endregion

        /// <summary>
        /// 提货单号(主键)
        /// </summary>
        public virtual string DeliveryNO { get; set; }

        /// <summary>
        /// 实际到货时间
        /// </summary>
        public virtual DateTime ArrivalTime { get; set; }

        /// <summary>
        /// 实际离库时间
        /// </summary>
        public virtual DateTime LeaveTime { get; set; }

        /// <summary>
        /// 考核状态
        /// </summary>
        public virtual bool AssessmentStatus { get; set; }

        /// <summary>
        /// 异常原因
        /// </summary>
        public virtual string Reason { get; set; }
    }
}

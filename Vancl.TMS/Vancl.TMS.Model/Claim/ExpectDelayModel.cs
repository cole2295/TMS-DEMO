using System;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim
{
    /// <summary>
    /// 预计延误处理信息
    /// </summary>
    [Serializable]
    public class ExpectDelayModel : BaseModel, ISequenceable, IDeliveryLogable
    {
        public string PrimaryKey
        {
            get { return EDID.ToString(); }
            set { EDID = long.Parse(value); }
        }

        /// <summary>
        /// 主键标识
        /// </summary>
        public long EDID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        [LogName("提货单号")]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 承运商物流单号
        /// </summary>
        [LogName("承运商物流单号")]
        public string CarrierWaybillNo { get; set; }

        /// <summary>
        /// 预计延误类型
        /// </summary>
        [LogName("预计延误类型")]
        public Enums.ExpectDelayType ExpectDelayType { get; set; }

        /// <summary>
        /// 预计延误时间(小时)
        /// </summary>
        [LogName("预计延误时间")]
        public int DelayTime { get; set; }

        /// <summary>
        /// 预计延误说明
        /// </summary>
        [LogName("预计延误说明")]
        public string DelayDesc { get; set; }

        /// <summary>
        /// 预计延误后的到货日期
        /// </summary>
        [LogName("预计延误后的到货日期")]
        public DateTime ExpectTime { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [LogName("审核状态")]
        public Enums.ApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [LogName("审核人")]
        public int? ApproveBy { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [LogName("审核时间")]
        public DateTime? ApproveTime { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_ExpectDelay_EDID"; }
        }

        #endregion
    }
}

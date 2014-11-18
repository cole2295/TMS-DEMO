using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.InTransit
{
    /// <summary>
    /// 到货延误
    /// </summary>
    [Serializable]
    public class DelayModel : BaseModel, IOperateLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return DID.ToString(); }
            set { DID = long.Parse(value); }
        }

        /// <summary>
        /// 主键标识
        /// </summary>
        public long DID { get; set; }

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
        /// 延误类型
        /// </summary>
        [LogName("延误类型")]
        public Enums.DelayType DelayType { get; set; }

        /// <summary>
        /// 延误原因
        /// </summary>
        [LogName("延误原因")]
        public string DelayReason { get; set; }

        /// <summary>
        /// 延误时长
        /// </summary>
        [LogName("延误时长")]
        public decimal DelayTimeSpan { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_Delay_DID"; }
        }

        #endregion

    }
}

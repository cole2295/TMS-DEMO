using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Claim.Lost
{
    /// <summary>
    /// 丢失信息
    /// </summary>
    [Serializable]
    public class LostModel : BaseModel, IDeliveryLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return LID.ToString(); }
            set { LID = long.Parse(value); }
        }

        /// <summary>
        /// 主键标识
        /// </summary>
        public long LID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        [LogName("提货单号")]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [LogName("出发地ID")]
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [LogName("目的地ID")]
        public int ArrivalID { get; set; }

        /// <summary>
        /// 是否全部丢失
        /// </summary>
        [LogName("是否全部丢失")]
        public bool IsAllLost { get; set; }

        /// <summary>
        /// 丢失订单总价
        /// </summary>
        [LogName("丢失订单总价")]
        public decimal LostAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [LogName("保价金额")]
        public decimal ProtectedPrice { get; set; }

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

        /// <summary>
        /// 审核状态
        /// </summary>
        [LogName("审核状态")]
        public Enums.ApproveStatus ApproveStatus { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_Lost_LID"; }
        }

        #endregion
    }
}

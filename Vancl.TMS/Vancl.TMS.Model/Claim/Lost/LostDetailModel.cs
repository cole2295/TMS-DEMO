using System;

namespace Vancl.TMS.Model.Claim.Lost
{
    /// <summary>
    /// 丢失信息明细
    /// </summary>
    [Serializable]
    public class LostDetailModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public long LDID { get; set; }

        /// <summary>
        /// 丢失主表主键
        /// </summary>
        public long LID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_LostDetail_LDID"; }
        }

        #endregion
    }
}

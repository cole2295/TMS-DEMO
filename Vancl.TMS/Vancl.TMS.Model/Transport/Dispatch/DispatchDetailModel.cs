
namespace Vancl.TMS.Model.Transport.Dispatch
{
    public class DispatchDetailModel : BaseModel, ISequenceable
    {
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DispatchDetail_DDID"; }
        }

        #endregion

        /// <summary>
        /// 主键ID
        /// </summary>
        public long DDID { get; set; }

        /// <summary>
        /// 调度主表ID
        /// </summary>
        public long DID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 预调度主键ID
        /// </summary>
        public long? PDID { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 订单总价格
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 是否采用预调度计划
        /// </summary>
        public bool IsPlan { get; set; }
    }
}

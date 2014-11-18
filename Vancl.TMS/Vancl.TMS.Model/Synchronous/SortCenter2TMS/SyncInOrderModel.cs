using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous
{
    /// <summary>
    /// 入库订单同步表
    /// </summary>
    [Serializable]
    public class SyncInOrderModel : ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 入库分拣中心ID
        /// </summary>
        public int InBoundID { get; set; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SC2TMSSyncFlag SyncFlag { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime SyncTime { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_LMS_SYN_TMS_INOrder_ID"; }
        }

        #endregion
    }
}

using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.InTransit
{
    /// <summary>
    /// 承运商运单
    /// </summary>
    [Serializable]
    public class CarrierWaybillModel : BaseModel, ISequenceable, IOperateLogable
    {
        /// <summary>
        /// TMS_Dispatch 主键
        /// </summary>
        public long DID { get; set; }

        /// <summary>
        /// 主键标识
        /// </summary>
        public long CWID { get; set; }

        /// <summary>
        /// 承运商物流单号
        /// </summary>
        [LogName("承运商物流单号")]
        public string WaybillNo { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [LogName("数量")]
        public long TotalCount { get; set; }

        /// <summary>
        /// 批次数/箱数
        /// </summary>
        [LogName("批次数/箱数")]
        public Int32 Boxcount { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [LogName("重量")]
        public decimal Weight { get; set; }

        /// <summary>
        /// 承运商反馈到货时间
        /// </summary>
        [LogName("承运商反馈到货时间")]
        public DateTime? ArrivalTime { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_CarrierWaybill_CWID"; }
        }

        #endregion

        #region ILogable 成员

        public string PrimaryKey
        {
            get
            {
                return CWID.ToString();
            }
            set
            {
                CWID = long.Parse(value);
            }
        }

        #endregion
    }
}

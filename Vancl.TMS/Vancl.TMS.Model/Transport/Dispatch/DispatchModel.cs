using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 调度表
    /// </summary>
    [Serializable]
    public class DispatchModel : BaseModel, IDeliveryLogable, ISequenceable
    {

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_Dispatch_DID"; }
        }

        #endregion

        #region ILogable 成员

        public string PrimaryKey
        {
            get { return DID.ToString(); }
            set { DID = long.Parse(value); }
        }

        #endregion

        /// <summary>
        /// 主键ID
        /// </summary>
        public long DID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 箱子数量
        /// </summary>
        public int BoxCount { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 线路计划ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 到货时效
        /// </summary>
        public decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 预计到货日期
        /// </summary>
        public DateTime? ExpectArrivalDate { get; set; }

        /// <summary>
        /// 确认修改后的预计到货日期（初始化同预计到货日期相同）
        /// </summary>
        public DateTime? ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 目的地接收日期
        /// </summary>
        public DateTime? DesReceiveDate { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 承运商运单主键ID
        /// </summary>
        public long CarrierWaybillID { get; set; }

        /// <summary>
        /// 结算单号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        public string SignedUser { get; set; }

        /// <summary>
        /// 是否到货延误
        /// </summary>
        public bool? IsDelay { get; set; }

        /// <summary>
        /// 是否存在丢失
        /// </summary>
        public bool IsExistsLost { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public Enums.DeliverySource DeliverySource { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 调度时间
        /// </summary>
        public DateTime? DispatchTime { get; set; }

    }
}

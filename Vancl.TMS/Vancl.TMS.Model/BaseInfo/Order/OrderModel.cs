using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Order
{
    /// <summary>
    /// 订单表
    /// </summary>
    [Serializable]
    public class OrderModel : BaseModel, IOperateLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return OID.ToString(); }
            set { OID = long.Parse(value); }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public long OID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        [LogName("单号")]
        public string FormCode { get; set; }

        /// <summary>
        /// LMS运单号
        /// </summary>
        [LogName("LMS运单号")]
        public long? LMSwaybillNo { get; set; }

        /// <summary>
        /// LMS运单类型
        /// </summary>
        [LogName("LMS运单类型")]
        public int? LMSwaybillType { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [LogName("保价金额")]
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [LogName("价格")]
        public decimal Price { get; set; }

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
        /// 订单状态
        /// </summary>
        [LogName("订单状态")]
        public Enums.OrderTMSStatus OrderTMSStatus { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        [LogName("货物类型")]
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 初始批次号箱号
        /// </summary>
        [LogName("初始批次号")]
        public string BoxNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        [LogName("单号")]
        public String CustomerOrder { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_Order_OID"; }
        }

        #endregion
    }
}

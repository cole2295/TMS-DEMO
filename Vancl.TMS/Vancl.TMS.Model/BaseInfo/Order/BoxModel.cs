using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Order
{
    /// <summary>
    /// 箱对象、批次对象
    /// </summary>
    [Serializable]
    public class BoxModel : BaseModel, IOperateLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return BID.ToString(); }
            set { BID = long.Parse(value); }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public long BID { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [LogName("来源")]
        public Enums.TMSEntranceSource Source { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        [LogName("系统批次号")]
        public string BoxNo { get; set; }

        /// <summary>
        /// 客户批次号,箱号
        /// </summary>
        [LogName("客户批次号")]
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [LogName("出发地ID")]
        public int DepartureID { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [LogName("保价金额")]
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [LogName("目的地ID")]
        public int ArrivalID { get; set; }

        /// <summary>
        /// 订单总数量
        /// </summary>
        [LogName("订单总数量")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        [LogName("总价格")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [LogName("重量")]
        public decimal Weight { get; set; }

        /// <summary>
        /// 箱子类型
        /// </summary>
        [LogName("箱子类型")]
        public Enums.BoxType BoxType { get; set; }

        /// <summary>
        /// 预调度状态
        /// </summary>
        [LogName("预调度状态")]
        public Enums.BatchPreDispatchedStatus IsPreDispatch { get; set; }

        /// <summary>
        /// 包含货物类型
        /// </summary>
        [LogName("包含货物类型")]
        public Enums.GoodsType ContentType { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_Box_BID"; }
        }

        #endregion
    }
}

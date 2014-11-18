using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Order;
using System.Collections.Generic;

namespace Vancl.TMS.Model.Synchronous
{
    /// <summary>
    /// TMS同步出库箱号中间表
    /// </summary>
    [Serializable]
    public class OutBoxModel : BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String SSTOID { get; set; }

        /// <summary>
        /// 箱号、批次号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 总计数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        public Enums.GoodsType ContentType { get; set; }

        /// <summary>
        /// 分拣到TMS城际运输同步标志
        /// </summary>
        public Enums.SC2TMSSyncFlag SC2TMSFlag { get; set; }

        /// <summary>
        /// 分拣到TMS城际运输同步时间
        /// </summary>
        public DateTime SC2TMSSyncTime { get; set; }

        /// <summary>
        /// 编号类型
        /// </summary>
        public Enums.SyncNoType NoType { get; set; }

        /// <summary>
        /// 箱子所包含订单
        /// </summary>
        public List<OrderModel> Order
        {
            get;
            set;
        }

        /// <summary>
        /// 箱子所包含的订单货物明细
        /// </summary>
        [Obsolete]
        public List<OrderDetailModel> OrderDetail
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库打印批次详情模型
    /// </summary>
    public class PrintBatchDetailModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 配送站点
        /// </summary>
        public string DeliveryStation { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutboundTime { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal ReceivableAmount { get; set; }
    }
}

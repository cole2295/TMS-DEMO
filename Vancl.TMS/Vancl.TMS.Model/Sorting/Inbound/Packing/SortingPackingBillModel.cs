using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 分拣装箱订单对象
    /// </summary>
    [Serializable]
    public class SortingPackingBillModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 目的地id
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public string ArrivalName { get; set; }

        /// <summary>
        /// 目的地配送商编号
        /// </summary>
        public string ArrivalDistributionCode { get; set; }

        /// <summary>
        /// 目的地配送商编号
        /// </summary>
        public string ArrivalDistributionName { get; set; }

        /// <summary>
        /// 入库分拣中心id
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 是否是入库中
        /// </summary>
        public bool IsInbounding { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }

        /// <summary>
        /// 验证信息
        /// </summary>
        public string ValidateMsg { get; set; }
    }
}

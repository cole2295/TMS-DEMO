using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Loading
{
    /// <summary>
    /// 批次装车数据模型
    /// </summary>
    public class BillTruckBatchModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 发货分拣中心
        /// </summary>
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public string ArrivalName { get; set; }

        /// <summary>
        /// 总单量
        /// </summary>
        public int TotalBillCount { get; set; }

        /// <summary>
        /// 已装车数量
        /// </summary>
        public int LoadingCount { get; set; }

        /// <summary>
        /// 未装车数量
        /// </summary>
        public int NotLoadingCount { get; set; }

        /// <summary>
        /// 配送站
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 配送站名称
        /// </summary>
        public string DeliverStationName { get; set; }

        /// <summary>
        /// 车牌叼
        /// </summary>
        public string TruckNo { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// 装车时间
        /// </summary>
        public DateTime? LoadingTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Loading
{
    public class BillTruckSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNO { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string FromCode { get; set; }

        /// <summary>
        /// 出库起始时间
        /// </summary>
        public string OutBoundBeginTime { get; set; }

        /// <summary>
        /// 出库结束时间
        /// </summary>
        public string OutBoundEndTime { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string BillSource { get; set; }

        /// <summary>
        /// 只显示未装车订单
        /// </summary>
        public bool OnlyNotLoadingBill { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 目的地配送商编码
        /// </summary>
        public string ArrivalDistributionCode { get; set; }

        /// <summary>
        /// 出发地配送商编码
        /// </summary>
        public string DepartureDistributionCode { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Loading
{
    public class OutBoundLoadingModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 出库类型
        /// </summary>
        public Enums.SortCenterOperateType OutboundType { get; set; }

        /// <summary>
        /// 配送商ID
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus BillStatus { get; set; }

        /// <summary>
        /// 运单当前状态
        /// </summary>
        public Enums.BillStatus CurBillStatus { get; set; }

        /// <summary>
        /// 配送商
        /// </summary>
        public string CurrentDistributionCode { get; set; }
    }
}

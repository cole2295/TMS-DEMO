using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Loading
{
    public class ViewBillTruckModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNO { get; set; }


        /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { set; get; }


        /// <summary>
        /// 仓库
        /// </summary>
        public string WarehouseName { get; set; }


        /// <summary>
        /// 运单配送站ID
        /// </summary>
        public int DeliverStationID { set; get; }

        /// <summary>
        /// 运单配送站名称
        /// </summary>
        public string DeliverStationName { set; get; }

        /// <summary>
        /// 当前配送商
        /// </summary>
        public string CurrentDistributionCode { set; get; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public Enums.BillStatus BillStatus { set; get; }

        /// <summary>
        /// 订单当前状态
        /// </summary>
        public Enums.BillStatus CurBillStatus { set; get; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string BillTypeName { set; get; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public string SourceName { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string TruckNo { set; get; }

    }
}

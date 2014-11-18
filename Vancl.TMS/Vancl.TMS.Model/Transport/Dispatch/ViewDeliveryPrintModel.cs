﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 运输调度主数据行视图对象
    /// </summary>
    public class ViewDeliveryPrintModel : BaseModel
    {
        /// <summary>
        /// 调度时间
        /// </summary>
        public DateTime? DispatchTime { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的站
        /// </summary>
        public int ArrivalID { get; set; }
        public string ArrivalName { get; set; }

        /// <summary>
        /// 箱数/批次数
        /// </summary>
        public int BoxCount { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string CarrierID { get; set; }
        public string CarrierName { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 时效H
        /// </summary>
        public decimal? ArrivalTiming { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 物流运单号
        /// </summary>
        public string WaybillNo { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }
                
        /// <summary>
        /// 修正预计到货时间
        /// </summary>
        public DateTime? ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 预计到货时间
        /// </summary>
        public DateTime? ExpectArrivalDate { get; set; }

        /// <summary>
        /// 调度状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 线路计划ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNo { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public string Consignor { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhone { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public string ReceiveAddress { get; set; }
    }
}

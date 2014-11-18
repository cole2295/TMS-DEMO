using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.InTransit
{
    public class ViewInTransitModel : BaseModel
    {
        /// <summary>
        /// 调度ID
        /// </summary>
        public long DispatchID { get; set; }

        /// <summary>
        /// 调度时间
        /// </summary>
        public DateTime CreateTime { get; set; }

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

        //[Display(Name = "线路计划编号")]
        //public  string BoxCount { get; set; } 

        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int BoxCount { get; set; }

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
        public decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 物流运单号
        /// </summary>
        public string WaybillNo { get; set; }


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
        /// 预计到货时间
        /// </summary>
        public DateTime ExpectArrivalDate { get; set; }

        /// <summary>
        /// 预计到货日期
        /// </summary>
        public DateTime ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 目的地接收日期
        /// </summary>
        public DateTime? DesReceiveDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 是否丢失
        /// </summary>
        public bool IsLost { get; set; }

        /// <summary>
        /// waybill重量
        /// </summary>
        public long Weight { get; set; }

        /// <summary>
        /// waybill总数量
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime DeliveryTime { get; set; }
    }
}

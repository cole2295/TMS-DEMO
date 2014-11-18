using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim
{
    public class ViewExpectDelayModel : BaseModel
    {
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// 调度ID
        /// </summary>
        public long DispatchID { get; set; }
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }
        /// <summary>
        /// 物流运单号
        /// </summary>
        public string WaybillNo { get; set; }
        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }
        public string ArrivalName { get; set; }
        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { set; get; }

        ///// <summary>
        ///// 中转站
        ///// </summary>
        //public int TransferStation { get; set; }
        //public string TransferStationName { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int TotalCount { get; set; }

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
        /// 到货时效
        /// </summary>
        public decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 预计延误表ID
        /// </summary>
        public long ExpectDelayID { get; set; }

        /// <summary>
        /// 预计到货时间
        /// </summary>
        public DateTime? ExpectArrivalDate { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }
        /// <summary>
        /// 预计延误时间(小时)
        /// </summary>
        public int DelayTime { get; set; }
        /// <summary>
        /// 预计延误说明
        /// </summary>
        public string DelayDesc { get; set; }
        /// <summary>
        /// 延误类型
        /// </summary>
        public Enums.ExpectDelayType ExpectDelayType { get; set; }
        /// <summary>
        /// 系统计算修正到货日期
        /// </summary>
        public DateTime? ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public Enums.ApproveStatus ApproveStatus { get; set; }
    }
}

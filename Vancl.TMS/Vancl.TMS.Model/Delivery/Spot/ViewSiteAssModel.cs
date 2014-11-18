using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.Spot
{
    public class ViewSiteAssModel:BaseModel
    {
        /// <summary>
        /// 调度时间
        /// </summary>
        public DateTime? DispatchCreateTime { get; set; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的站
        /// </summary>
        public string ArrivalName { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int BoxCount { get; set; }

        //[Display(Name = "线路计划编号")]
        //public  string BoxCount { get; set; } 

        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 城际运输商
        /// </summary>
        public string CarrierName { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType TransPortType { get; set; }

        /// <summary>
        /// 时效H
        /// </summary>
        public decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNO { get; set; }

        /// <summary>
        /// 物流运单号
        /// </summary>
        public string CarrierWaybillNO { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 预计到货时间
        /// </summary>
        public string ExpectArrivalDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 到库考核点
        /// </summary>
        public DateTime ArrivalAssessmentTime { get; set; }

        /// <summary>
        /// 离库考核点
        /// </summary>
        public DateTime LeaveAssessmentTime { get; set; }
    }
}

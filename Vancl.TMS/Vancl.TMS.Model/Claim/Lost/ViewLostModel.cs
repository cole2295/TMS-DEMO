using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class ViewLostModel : BaseModel
    {
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的站
        /// </summary>
        public string ArrivalName { get; set; }

        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int TotalCount { get; set; }

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
        public string ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 是否已添加丢失信息
        /// </summary>
        public bool IsAddedLost { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public Enums.ApproveStatus? ApproveStatus { get; set; }

        /// <summary>
        /// 是否全部丢失
        /// </summary>
        public bool IsAllLost { get; set; }

        /// <summary>
        /// 丢失总价
        /// </summary>
        public decimal LostAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{

    /// <summary>
    /// KPI考核主列表信息
    /// </summary>
    public class ViewDeliveryAssessmentModel : BaseModel
    {
        /// <summary>
        /// 发货时间
        /// </summary>
        public new DateTime DepartureTime { get; set; }

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
        /// 目的地城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 承运商名
        /// </summary>
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
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string WaybillNo { get; set; }

        /// <summary>
        /// 货物类型
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
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 修正预计到货时间
        /// </summary>
        public DateTime ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 预计到货时间
        /// </summary>
        public DateTime ExpectArrivalDate { get; set; }

        /// <summary>
        /// 目的地接收时间（实际到货时间）
        /// </summary>
        public DateTime DesReceiveDate { get; set; }

        /// <summary>
        /// 延误时长
        /// </summary>
        public decimal? DelayTimeSpan { get; set; }

        /// <summary>
        /// 延误类型
        /// </summary>
        public Enums.DelayType? DelayType { get; set; }

        /// <summary>
        /// 延误原因
        /// </summary>
        public string DelayReason { get; set; }

        /// <summary>
        /// 复议审核状态
        /// </summary>
        public Enums.ApproveStatus DelayHandleStatus { get; set; }
    }
}

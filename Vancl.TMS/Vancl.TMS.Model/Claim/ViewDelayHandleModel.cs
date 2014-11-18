using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.Model.Claim
{
    public class ViewDelayHandleModel :BaseModel
    {
        /// <summary>
        /// 调度ID
        /// </summary>
        public string DispatchID { get; set; }
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
        /// 录入预计延误时长
        /// </summary>
        public decimal ExpectDelayTime { get; set; }

        /// <summary>
        /// 录入预计延误原因
        /// </summary>
        public string ExpectDelayDesc { get; set; }

        /// <summary>
        /// 系统计算修正到货日期
        /// </summary>
        public DateTime  ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 延误类型
        /// </summary>
        public string DelayType { get; set; }

        /// <summary>
        /// 延迟交货申请是否成立
        /// </summary>
        public int ExpectDelayApproveStatus { get; set; }

        /// <summary>
        /// 实际延误主键ID
        /// </summary>
        public int DelayID { get; set; }
        /// <summary>
        /// 实际延误时长
        /// </summary>
        public decimal DelayTimeSpan { get; set; }

        /// <summary>
        /// 延误处理主键ID
        /// </summary>
        public int DHID { get; set; }

        /// <summary>
        /// 申请复议理由
        /// </summary>
        public string DelayHandleNote { get; set; }

        /// <summary>
        /// 复议处理标识
        /// </summary>
        public int DelayHandleApproveStatus { get; set; }      
    }
}

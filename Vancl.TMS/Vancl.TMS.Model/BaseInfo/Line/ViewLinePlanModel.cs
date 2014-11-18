using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    public class ViewLinePlanModel : BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public string DepartureName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string ArrivalName { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string CarrierName { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 始发站所在城市
        /// </summary>
        public string DepartureCityName { get; set; }

        /// <summary>
        /// 目的站所在城市
        /// </summary>
        public string ArrivalCityName { get; set; }

        /// <summary>
        /// 提货到库考核点
        /// </summary>
        public DateTime ArrivalAssessmentTime { get; set; }

        /// <summary>
        /// 离库考核点
        /// </summary>
        public DateTime LeaveAssessmentTime { get; set; }

        /// <summary>
        /// 到货时效
        /// </summary>
        public decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 保险费率
        /// </summary>
        public decimal InsuranceRate { get; set; }

        /// <summary>
        /// 最低收费
        /// </summary>
        public decimal LowestPrice { get; set; }

        /// <summary>
        /// 超远送货费用
        /// </summary>
        public virtual decimal LongDeliveryPrice { get; set; }

        /// <summary>
        /// 超远提货费用
        /// </summary>
        public decimal LongPickPrice { get; set; }

        /// <summary>
        /// 超远运转费率
        /// </summary>
        public decimal LongTransferRate { get; set; }

        /// <summary>
        /// 优先级别
        /// </summary>
        public Enums.LinePriority Priority { get; set; }

        /// <summary>
        /// 线路状态
        /// </summary>
        public Enums.LineStatus Status { get; set; }

        /// <summary>
        /// 线路类型
        /// </summary>
        public Enums.LineType LineType { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 运费类型
        /// </summary>
        public Enums.ExpressionType ExpressionType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 线路审核状态
        /// </summary>
        public bool ApproveStatus { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}

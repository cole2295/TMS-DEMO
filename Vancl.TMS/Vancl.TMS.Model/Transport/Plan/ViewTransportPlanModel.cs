using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Plan
{
    public class ViewTransportPlanModel : BaseModel
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public int TPID { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public string Arrival { get; set; }

        /// <summary>
        /// 目的地所在城市名称
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 是否中转
        /// </summary>
        public bool IsTransit { get; set; }

        /// <summary>
        /// 中转站点
        /// </summary>
        public string TransferStation { get; set; }


        /// <summary>
        /// 多次中转站点
        /// </summary>
        public string TransferStationMulti { get; set; }


        /// <summary>
        /// 运输计划生效时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 运输计划状态
        /// </summary>
        public Enums.TransportStatus Status { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}

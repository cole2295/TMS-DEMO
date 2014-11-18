using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Plan
{
    public class ViewTansportEditorModel : BaseModel
    {
        public int TPID { get; set; }

        public int DepartureID { get; set; }

        public string DepartureName { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        public string ArrivalName { get; set; }

        /// <summary>
        /// 中转站
        /// </summary>
        public string TransitStationName { get; set; }

        /// <summary>
        /// 中转站ID
        /// </summary>
        public int TransitStationID { get; set; }
        /// <summary>
        /// 多次中转ID
        /// </summary>
        public string TransitStationMulti { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        public DateTime DeadLine { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 是否中转
        /// </summary>
        public bool IsTransifer { get; set; }

        /// <summary>
        /// 默认线路(第一条)
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// 第二条线路
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// 多条线路
        /// </summary>
        public string Lines { get; set; }

    }
}

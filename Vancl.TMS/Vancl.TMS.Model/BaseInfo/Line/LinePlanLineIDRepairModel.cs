using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    public class LinePlanLineIDRepairModel : BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 营业类型
        /// </summary>
        public Enums.BusinessType BusinessType { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }
    }
}

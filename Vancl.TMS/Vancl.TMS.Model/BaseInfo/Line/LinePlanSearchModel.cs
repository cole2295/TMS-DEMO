using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    [Serializable]
    public class LinePlanSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 承运商ID
        /// </summary>
        public virtual int? CarrierID { get; set; }

        /// <summary>
        /// 承运商编号
        /// </summary>
        public virtual string CarrierNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public virtual int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public virtual int ArrivalID { get; set; }

        /// <summary>
        /// 线路状态
        /// </summary>
        public virtual Enums.LineStatus? Status { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        public Enums.GoodsType? LineGoodsType { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 线路类型
        /// </summary>
        public Enums.LineType? LineType { get; set; }
    }
}

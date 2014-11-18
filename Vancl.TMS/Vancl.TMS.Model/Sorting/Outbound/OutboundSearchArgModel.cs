using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 查询出库参数对象
    /// </summary>
    public class OutboundSearchArgModel : SortCenterBatchArgModel, IOutboundArgModel
    {
        /// <summary>
        /// 入库开始时间
        /// </summary>
        public DateTime? InboundStartTime { get; set; }

        /// <summary>
        /// 入库结束时间
        /// </summary>
        public DateTime? InboundEndTime { get; set; }

        #region ISortCenterArgModel 成员

        public OutboundPreConditionModel PreCondition { get; set; }

        public SortCenterToStationModel ToStation { get; set; }

        public SortCenterUserModel OpUser { get; set; }

        /// <summary>
        /// 装箱开始时间
        /// </summary>
        public DateTime? PackStartTime { get; set; }

        /// <summary>
        /// 装箱结束时间
        /// </summary>
        public DateTime? PackEndTime { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 查询出库查询条件对象
    /// </summary>
    public class OutboundSearchModel: BaseSearchModel
    {
        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 入库开始时间
        /// </summary>
        public DateTime? InboundStartTime { get; set; }

        /// <summary>
        /// 入库结束时间
        /// </summary>
        public DateTime? InboundEndTime { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>
        /// 分拣当前操作人员对象
        /// </summary>
        public SortCenterUserModel OpUser { get; set; }

    }
}

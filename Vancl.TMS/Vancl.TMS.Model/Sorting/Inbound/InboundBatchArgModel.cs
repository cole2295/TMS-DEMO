using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库批量参数
    /// </summary>
    [Serializable]
    public class InboundBatchArgModel : SortCenterBatchArgModel, IInboundArgModel
    {

        #region IInboundArgModel 成员

        /// <summary>
        /// 入库前置条件
        /// </summary>
        public InboundPreConditionModel PreCondition { get; set; }

        /// <summary>
        /// 入库限制数量
        /// </summary>
        public int LimitedInboundCount { get; set; }

        /// <summary>
        /// 是否入库限制数量
        /// </summary>
        public bool IsLimitedQuantity { get; set; }

        #endregion

        #region ISortCenterArgModel 成员

        /// <summary>
        /// 目的地对象
        /// </summary>
        public SortCenterToStationModel ToStation { get; set; }

        /// <summary>
        /// 当前操作人
        /// </summary>
        public SortCenterUserModel OpUser { get; set; }

        #endregion
    }
}

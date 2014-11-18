using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 批量出库参数对象
    /// </summary>
    public class OutboundBatchArgModel : SortCenterBatchArgModel, IOutboundArgModel
    {

        #region ISortCenterArgModel 成员

        public OutboundPreConditionModel PreCondition { get; set; }

        public SortCenterToStationModel ToStation
        {
            get;
            set;
        }

        public SortCenterUserModel OpUser
        {
            get;
            set;
        }

        #endregion
    }

}

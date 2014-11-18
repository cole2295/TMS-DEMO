using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 逐单出库参数对象
    /// </summary>
    public class OutboundSimpleArgModel : SortCenterSimpleArgModel, IOutboundArgModel
    {
        /// <summary>
        /// 出库批次号
        /// </summary>
        public String BatchNo { get; set; }

		/// <summary>
		/// 出库到当前目的地的数量
		/// </summary>
		public int CurrentDisCount { get; set; }

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

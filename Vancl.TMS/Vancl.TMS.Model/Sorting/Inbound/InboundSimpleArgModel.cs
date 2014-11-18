using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库逐单参数
    /// </summary>
    [Serializable]
    [DataContract]
    public class InboundSimpleArgModel : SortCenterSimpleArgModel, IInboundArgModel
    {

        #region IInboundArgModel 成员

        /// <summary>
        /// 入库前置条件
        /// </summary>
        [DataMember]
        public InboundPreConditionModel PreCondition { get; set; }

        /// <summary>
        /// 入库限制数量
        /// </summary>
        [DataMember]
        public int LimitedInboundCount { get; set; }

        /// <summary>
        /// 是否入库限制数量
        /// </summary>
        [DataMember]
        public bool IsLimitedQuantity { get; set; }

        #endregion

        #region ISortCenterArgModel 成员

        /// <summary>
        /// 目的地对象
        /// </summary>
        [DataMember]
        public SortCenterToStationModel ToStation { get; set; }

        /// <summary>
        /// 当前操作人
        /// </summary>
        [DataMember]
        public SortCenterUserModel OpUser { get; set; }

        #endregion
    }



}

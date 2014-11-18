using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库前置条件
    /// </summary>
    [Serializable]
    [DataContract]
    public class InboundPreConditionModel
    {
        /// <summary>
        /// 入库运单前一状态(PMS根据配送商配置取得)
        /// </summary>
        [DataMember]
        public Enums.BillStatus PreStatus { get; set; }

    }


}

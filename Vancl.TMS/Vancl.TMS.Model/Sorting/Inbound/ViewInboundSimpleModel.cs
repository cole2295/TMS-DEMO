using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库UI处理对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class ViewInboundSimpleModel : ResultModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        [DataMember]
        public String FormCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public String CustomerOrder { get; set; }

        /// <summary>
        /// 当前入库数量
        /// </summary>
        [DataMember]
        public int InboundCount { get; set; }
        /// <summary>
        /// 该运单件数
        /// </summary>
        [DataMember]
        public int PackegeCount { get; set; }
        /// <summary>
        /// 当前第几件
        /// </summary>
        [DataMember]
        public int CurrentCount { get; set; }
        /// <summary>
        /// 客户重量
        /// </summary>
        [DataMember]
        public decimal CustomerWeight { get; set; }
    }
}

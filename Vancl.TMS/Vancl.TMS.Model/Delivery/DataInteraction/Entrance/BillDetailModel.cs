using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.DataInteraction.Entrance
{
    /// <summary>
    /// 订单明细对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class BillDetailModel
    {
        /// <summary>
        /// 系统运单号
        /// </summary>
        [DataMember]
        public String FormCode { get; set; }

        /// <summary>
        /// 运单类型
        /// </summary>
        [DataMember]
        public Enums.BillType BillType { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        [DataMember]
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public String CustomerOrder { get; set; }

    }


}

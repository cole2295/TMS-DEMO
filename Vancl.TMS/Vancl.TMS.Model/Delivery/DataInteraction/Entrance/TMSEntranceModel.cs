using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.DataInteraction.Entrance
{
    /// <summary>
    /// TMS数据统一入口对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class TMSEntranceModel
    {
        /// <summary>
        /// 数据来源
        /// </summary>
        [DataMember]
        public Enums.TMSEntranceSource Source { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        [DataMember]
        public String BatchNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [DataMember]
        public String Departure { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [DataMember]
        public String Arrival { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        [DataMember]
        public Enums.GoodsType ContentType { get; set; }

        /// <summary>
        /// 运单明细
        /// </summary>
        [DataMember]
        public List<BillDetailModel> Detail { get; set; }

    }
}

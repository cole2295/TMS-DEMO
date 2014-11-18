using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Synchronous
{
    [Serializable]
    public class InboundModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 入库分拣中心ID
        /// </summary>
        public int InboundID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 是否是整箱
        /// </summary>
        public bool IsFCL { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

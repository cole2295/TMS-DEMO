using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.DeliveryAbnormal
{
    /// <summary>
    /// 预调度异常处理
    /// </summary>
    public class PreDispatchAbnormalModel
    {
        /// <summary>
        /// 唯一键
        /// </summary>
        public String BID { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public String SerialNumber { get; set; }

        /// <summary>
        /// 箱号、批次号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 出发地ID
        /// </summary>
        public String DepartureID { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 目的ID
        /// </summary>
        public String ArrivalID { get; set; }

        /// <summary>
        /// 目的名称
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 货品属性
        /// </summary>
        public Enums.GoodsType ContentType { get; set; }

        /// <summary>
        /// 批次类型
        /// </summary>
        public Enums.BoxType BoxType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

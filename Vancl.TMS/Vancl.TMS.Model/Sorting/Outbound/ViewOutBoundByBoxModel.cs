using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    public class ViewOutBoundByBoxModel : ResultModel
    {
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 重量(kg)
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 装箱时间
        /// </summary>
        public DateTime PackTime { get; set; }

        /// <summary>
        /// 运单数量
        /// </summary>
        public int BillCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    public class ViewOutBoundBoxDetailModel
    {
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 订单数
        /// </summary>
        public int BillCount { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 单号明细
        /// </summary>
        public List<string> FormCodes { get; set; }
    }
}

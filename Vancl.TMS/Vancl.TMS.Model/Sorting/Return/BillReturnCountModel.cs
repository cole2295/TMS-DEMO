using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class BillReturnCountModel
    {
        /// <summary>
        /// 一箱中的运单对应的不同商家的个数
        /// </summary>
        public int MerchantCount { get; set; }

        /// <summary>
        /// 一箱中的运单对应的不同接货配送商的个数
        /// </summary>
        public int DistributionCount { get; set; }

    }
}

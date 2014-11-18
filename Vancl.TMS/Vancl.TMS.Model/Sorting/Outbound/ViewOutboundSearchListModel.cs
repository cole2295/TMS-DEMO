using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 查询出库列表
    /// </summary>
    public class ViewOutboundSearchListModel
    {
        /// <summary>
        /// 内部运单对象
        /// </summary>
        public class InnerFormCodeList
        {
            /// <summary>
            /// 系统运单号
            /// </summary>
            public String FormCode { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public String CustomerOrder { get; set; }

        }

        /// <summary>
        /// 运单列表
        /// </summary>
        public List<InnerFormCodeList> FormCodeList { get; set; }

        /// <summary>
        /// 入库中数量
        /// </summary>
        public int InboundingCount { get; set; }

    }


}

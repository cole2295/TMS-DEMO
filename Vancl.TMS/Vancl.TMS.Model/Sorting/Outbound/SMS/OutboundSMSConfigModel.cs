using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound.SMS
{
    /// <summary>
    /// 出库短信配置对象
    /// </summary>
    public class OutboundSMSConfigModel
    {
        /// <summary>
        /// 出库短信配置明细[如需扩展，后续在抽取单独类]
        /// </summary>
        public class SortCenterOuboundDetailConfig
        {
            /// <summary>
            /// 运单来源
            /// </summary>
            public Enums.BillSource Source { get; set; }

            /// <summary>
            /// 商家ID
            /// </summary>
            public int MerchantID { get; set; }

            /// <summary>
            /// 分拣操作类型
            /// </summary>
            public Enums.SortCenterOperateType OpType { get; set; }

            /// <summary>
            /// 短信内容模板
            /// </summary>
            public String Template { get; set; }
        }

        /// <summary>
        /// 短信配置明细
        /// </summary>
        public List<SortCenterOuboundDetailConfig> Detail { get; set; }
    }


}

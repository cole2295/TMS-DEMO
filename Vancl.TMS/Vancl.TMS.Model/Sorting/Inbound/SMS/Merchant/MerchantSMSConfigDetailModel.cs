using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant
{
    /// <summary>
    /// 商家入库短信配置对象明细子项对象
    /// </summary>
    public class MerchantSMSConfigDetailModel
    {
        /// <summary>
        /// 发送时间范围配置
        /// </summary>
        public class TimeRangeConfig
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public DateTime StartTime { get; set; }

            /// <summary>
            /// 结束时间
            /// </summary>
            public DateTime EndTime { get; set; }

            /// <summary>
            /// 发送时间
            /// </summary>
            public DateTime SendTime { get; set; }

            /// <summary>
            /// 是否发送短信
            /// </summary>
            public bool IsSend { get; set; }

            /// <summary>
            /// 是否立即发送
            /// </summary>
            public bool ImmSend { get; set; }

            /// <summary>
            /// 延迟天数
            /// </summary>
            public int? DelayDay { get; set; }
        }

        /// <summary>
        /// 运单来源
        /// </summary>
        public Enums.BillSource Source { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 短信模板
        /// </summary>
        public String SMSTemplate { get; set; }

        /// <summary>
        /// 是否需要验证第一次入库时发生短信
        /// </summary>
        public bool IsValidateFirstInbound { get; set; }

        /// <summary>
        /// 时间范围配置
        /// </summary>
        public List<TimeRangeConfig> ListTimeConfig { get; set; }
    }
}

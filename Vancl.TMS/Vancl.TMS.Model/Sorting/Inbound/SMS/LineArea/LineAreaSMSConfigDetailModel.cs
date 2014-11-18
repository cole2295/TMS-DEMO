using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea
{
    /// <summary>
    /// 线路区域入库短信配置明细子项对象
    /// </summary>
    public class LineAreaSMSConfigDetailModel
    {
        /// <summary>
        /// 区域短信模板配置
        /// </summary>
        public class RangeTemplateConfig
        {
            /// <summary>
            /// 接收区域
            /// </summary>
            public String[] ReceiveRange { get; set; }

            /// <summary>
            /// 短信模板
            /// </summary>
            public String SMSTemplate { get; set; }
        }

        /// <summary>
        /// 发送时间范围配置
        /// </summary>
        public class TimeRangeTemplateConfig
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
            /// 及时发送
            /// </summary>
            public bool ImmSend { get; set; }

            /// <summary>
            /// 是否验证区域,是则使用区域模板
            /// </summary>
            public bool IsValidateRange { get; set; }

            /// <summary>
            /// 区域短信模板配置
            /// </summary>
            public List<RangeTemplateConfig> RangeCfg { get; set; }

            /// <summary>
            /// 短信模板
            /// </summary>
            public String SMSTemplate { get; set; }
        }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地列表
        /// </summary>
        public List<int> ListArrivalID { get; set; }

        /// <summary>
        /// 运单来源列表
        /// </summary>
        public List<Enums.BillSource> ListSource { get; set; }

        /// <summary>
        /// 时间区域配置列表
        /// </summary>
        public List<TimeRangeTemplateConfig> TimeRangeTemplateCfg { get; set; }
    }
}

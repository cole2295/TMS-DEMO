using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 入库短信配置基对象
    /// </summary>
    public abstract class InboundSMSConfigbaseModel : IEffectiveEnabled
    {
        #region IEffectiveEnabled 成员

        /// <summary>
        /// 启用停用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EffectiveTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? DeadLine { get; set; }

        #endregion
    }
}

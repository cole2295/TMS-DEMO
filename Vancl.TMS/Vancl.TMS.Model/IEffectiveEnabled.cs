using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model
{
    /// <summary>
    /// 启用,停用,生效,截止标志
    /// </summary>
    public interface IEffectiveEnabled
    {
        /// <summary>
        /// 启用停用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        DateTime? EffectiveTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        DateTime? DeadLine { get; set; }
    }
}

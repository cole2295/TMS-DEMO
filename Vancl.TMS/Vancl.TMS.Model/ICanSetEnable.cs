using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model
{
    /// <summary>
    /// 启用停用日志接口
    /// </summary>
    public interface ICanSetEnable
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 记录id
        /// </summary>
        string LogKeyValue { get; set; }
    }
}

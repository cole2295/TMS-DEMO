using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model
{
    /// <summary>
    /// 可自定义日志接口
    /// </summary>
    public interface ICustomizeLogable
    {
        /// <summary>
        /// 自定义的日志信息
        /// </summary>
        String CustomizeLog { get; set; }
    }
}

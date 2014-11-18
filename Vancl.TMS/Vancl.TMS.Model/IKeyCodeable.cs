using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model
{
    /// <summary>
    /// 可产生KEY编码
    /// </summary>
    public interface IKeyCodeable
    {
        /// <summary>
        /// 序列ID名
        /// </summary>
        String SequenceName { get; }
        /// <summary>
        /// Table Code
        /// </summary>
        String TableCode { get; }
    }

}

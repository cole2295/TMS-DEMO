using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// 编号
    /// </summary>
    public class SerialNumberModel
    {
        /// <summary>
        /// 编号数字长度
        /// </summary>
        public int NumberLength { get; set; }

        /// <summary>
        /// 填充字符
        /// </summary>
        public string FillerCharacter { get; set; }

    }
}

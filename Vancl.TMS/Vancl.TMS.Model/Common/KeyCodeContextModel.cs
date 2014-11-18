using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    /// <summary>
    /// KeyCode上下文对象
    /// </summary>
    public class KeyCodeContextModel
    {

        /// <summary>
        /// 数据库编码
        /// </summary>
        public String DBCode { get; set; }

        /// <summary>
        /// 表编码
        /// </summary>
        public String TableCode { get; set; }

        /// <summary>
        /// 序列名称
        /// </summary>
        public String SequenceName { get; set; }

    }


}

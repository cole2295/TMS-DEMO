using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Tools.VCMRecords.Tool.Return
{
    /// <summary>
    /// 错误类
    /// </summary>
    public class Fault
    {
        /// <summary>
        /// 错误类
        /// </summary>
        /// <param name="detail">错误描述</param>
        /// <param name="obj">错误对象，通常为exception</param>
        public Fault(String detail, Object obj = null)
        {
            this.Detail = detail;
            this.Obj = obj;
        }

        /// <summary>
        /// 错误的描述信息
        /// </summary>
        public string Detail;

        /// <summary>
        /// 错误发生的对象,通常是Exeception
        /// </summary>
        public Object Obj;
    }
}

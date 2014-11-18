using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Tools.VCMRecords.Tool.Return
{
    /// <summary>
    /// 返回结果对象
    /// </summary>
    public class ReturnResult
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool Result = true;

        /// <summary>
        /// 错误对象
        /// </summary>
        public Fault Fault;

        public ReturnResult(bool result=true, Fault fault = null)
        {
            this.Result = result;
            this.Fault = fault;
        }
    }
}

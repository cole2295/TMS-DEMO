using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    public class BillValidateResultModel
    {
        /// <summary>
        /// 验证结果
        /// </summary>
        public BillValidateResult Result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [ScriptIgnore]
        public Exception Exception { get; set; }
    }
    public enum BillValidateResult
    {
        Failure = 0,
        Success = 1,
        Warning = 2,
    }
}

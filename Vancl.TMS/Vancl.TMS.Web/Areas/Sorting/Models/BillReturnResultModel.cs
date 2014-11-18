using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public enum ResultAlertType
    {
        Alert=0,
        Prompt,
        Confirm,
    }

    [Serializable]
    public class BillReturnResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        public ResultAlertType ResultAlertType { get; set; } 

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public dynamic ExtendedObj { get; set; }
    }
}
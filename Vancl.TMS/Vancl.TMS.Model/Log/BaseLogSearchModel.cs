using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 日志检索对象
    /// </summary>
    public class BaseLogSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

    }
}

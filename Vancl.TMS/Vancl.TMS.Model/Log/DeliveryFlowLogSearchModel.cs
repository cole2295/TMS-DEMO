using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 运输流程日志检索
    /// </summary>
    [Serializable]
    public class DeliveryFlowLogSearchModel : BaseLogSearchModel
    {
        /// <summary>
        /// 提货单号
        /// </summary>
        public String DeliveryNo { get; set; }
    }
}

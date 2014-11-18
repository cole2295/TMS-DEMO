using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Inbound
{    
    /// <summary>
    /// 入库批量View对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class ViewInboundBatchModel : ViewSortCenterBatchModel
    {
        /// <summary>
        /// 当前入库总数量
        /// </summary>
        public int InboundCount { get; set; }
    }

}

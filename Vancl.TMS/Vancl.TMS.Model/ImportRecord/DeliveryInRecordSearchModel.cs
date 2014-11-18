using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.ImportRecord
{
    public class DeliveryInRecordSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 来源
        /// </summary>
        public Enums.DeliverySource? DeliverySource { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}

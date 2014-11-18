using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea
{
    /// <summary>
    /// 线路区域入库短信配置对象
    /// </summary>
    public class LineAreaSMSConfigModel : InboundSMSConfigbaseModel
    {

        /// <summary>
        /// 短信配置对象
        /// </summary>
        public List<LineAreaSMSConfigDetailModel> Detail { get; set; }

    }
}

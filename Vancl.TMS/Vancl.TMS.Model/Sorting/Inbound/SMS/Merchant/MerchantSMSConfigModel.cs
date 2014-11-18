using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant
{

    /// <summary>
    /// 商家入库短信配置对象
    /// </summary>
    public class MerchantSMSConfigModel : InboundSMSConfigbaseModel
    {
        /// <summary>
        /// 短信配置对象
        /// </summary>
        public List<MerchantSMSConfigDetailModel> Detail { get; set; }
    }

}

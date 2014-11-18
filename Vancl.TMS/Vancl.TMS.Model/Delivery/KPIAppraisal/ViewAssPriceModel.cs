using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// KPI考核运费UI Model
    /// </summary>
    public class ViewAssPriceModel : DeliveryAssessmentModel
    {
        /// <summary>
        /// 运费
        /// </summary>
        public List<IAssPriceModel> AssPriceList { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.Plan;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    public class PreDispatchGetTransportResultModel : ResultModel
    {
        /// <summary>
        /// 运输计划
        /// </summary>
        public TransportPlanModel TransportPlan { get; set; }

        /// <summary>
        /// 运输计划明细
        /// </summary>
        public List<TransportPlanDetailModel> TransportPlanDetail { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    public class PreDispatchGetLineResultModel : ResultModel
    {

        /// <summary>
        /// 线路计划
        /// </summary>
        public LinePlanModel LinePlan { get; set; }

    }
}

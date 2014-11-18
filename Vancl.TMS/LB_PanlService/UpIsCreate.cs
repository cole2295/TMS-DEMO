using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Util;

namespace LB_PanlService
{
    public class UpIsCreate : IStatefulJob
    {
        public void Execute(JobExecutionContext context)
        {
            RunUpIsCreate();
        }

        /// <summary>
        /// 更新计划 IScreate=0
        /// </summary>
        public static void RunUpIsCreate()
        {
            ILB_PLANBLL _planService = ServiceFactory.GetService<ILB_PLANBLL>("LB_PLANBLL");
            var resultCount = _planService.UpPlanIsCreate();

            MessageCollector.Instance.Collect("default", "提货计划生成状态改为0（未生成）成功：" + resultCount + "条", true);
        }
    }
}

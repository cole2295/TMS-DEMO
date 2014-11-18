using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Formula.PreDispatch
{
    /// <summary>
    /// 预调度取得线路默认算法
    /// </summary>
    public class PreDispatchGetLineFormula : IFormula<PreDispatchGetLineResultModel, PreDispatchGetLineContext>
    {
        ILinePlanDAL _lineplandal = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");

        #region IFormula<PreDispatchGetLineResultModel,PreDispatchGetLineContext> 成员

        public PreDispatchGetLineResultModel Execute(PreDispatchGetLineContext context)
        {
            if (context == null) throw new ArgumentNullException("PreDispatchGetLineContext is null");
            if (String.IsNullOrWhiteSpace(context.LineID)) throw new ArgumentNullException("PreDispatchGetLineContext.LineID is null");
            var result = new PreDispatchGetLineResultModel();
            var listUsful = _lineplandal.GetEnabledUsefulLinePlan(context.LineID);
            if (listUsful == null || listUsful.Count <= 0)
            {
                return result.Failed("当前未取得生效的并且可用的线路计划") as PreDispatchGetLineResultModel;
            }
            var finedLine = listUsful.FirstOrDefault();
            result.LinePlan = finedLine;
            return result.Succeed() as PreDispatchGetLineResultModel;
        }

        #endregion
    }
}

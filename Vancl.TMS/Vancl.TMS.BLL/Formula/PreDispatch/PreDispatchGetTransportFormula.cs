using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IDAL.Transport.Plan;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Formula.PreDispatch
{
    /// <summary>
    /// 预调度取得运输计划默认算法
    /// </summary>
    public class PreDispatchGetTransportFormula : IFormula<PreDispatchGetTransportResultModel, PreDispatchGetTransportContext>
    {
        ITransportPlanDAL _transportPlanDAL = ServiceFactory.GetService<ITransportPlanDAL>("TransportPlanDAL");
        ITransportPlanDetailDAL _transportPlanDetailDAL = ServiceFactory.GetService<ITransportPlanDetailDAL>("TransportPlanDetailDAL");

        #region IFormula<PreDispatchGetTransportResultModel,PreDispatchGetTransportContext> 成员

        public PreDispatchGetTransportResultModel Execute(PreDispatchGetTransportContext context)
        {
            if (context == null) throw new ArgumentNullException("PreDispatchGetTransportContext is null");
            var result = new PreDispatchGetTransportResultModel();
            var listUsefulTransport = _transportPlanDAL.GetEnabledUsefulTransportPlan(context.DepartureID, context.ArrivalID);
            if (listUsefulTransport == null || listUsefulTransport.Count <= 0)
            {
                return result.Failed("当前未取到生效并且可用的运输计划") as PreDispatchGetTransportResultModel;
            }
            //默认货物属性包含的第一条运输计划
            var findedTransport = listUsefulTransport.FirstOrDefault(p => p.LineGoodsType == (p.LineGoodsType | context.ContentType));
            if (findedTransport == null)
            {
                return result.Failed("系统中设置运输计划货物属性不包含当前批次的货物属性，无法匹配") as PreDispatchGetTransportResultModel;
            }
            result.TransportPlan = findedTransport;
            result.TransportPlanDetail = _transportPlanDetailDAL.GetByTransportPlanID(findedTransport.TPID).ToList();
            return result.Succeed() as PreDispatchGetTransportResultModel;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IDAL.Formula.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.IBLL.BaseInfo;

namespace Vancl.TMS.BLL.Formula.Common
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：运输计划
    * 说明：基础算法获取两点之间所有路径
    * 作者：任 钰
    * 创建日期：2012-02-14 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    internal class LinePathSearchFormula : IFormula<PointPathModel, PointPathSearchModel>
    {
        ILinePlanDAL _LinePlandal = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");
        IExpressCompanyBLL _expresscompanyBll = ServiceFactory.GetService<IExpressCompanyBLL>(); 
        #region IFormula<PointPathModel,TransportPlanModel> 成员

        public PointPathModel Execute(PointPathSearchModel context)
        {
            if (null == context) throw new ArgumentNullException("PointPathSearchModel");
            if (!context.IsTransit) throw new Exception("需要选择中转选项");
            IList<int> listTransfer = _LinePlandal.GetEffectiveNextStation(context.DepartureID);
            if (listTransfer != null)
            {
                PointPathModel result = new PointPathModel() { DepartureID = context.DepartureID, ArrivalID = context.ArrivalID};
                List<int> listNeededTransfer = new List<int>();
                for (int i = 0; i < listTransfer.Count; i++)
                {
                    IList<int> tmplistTransfer = _LinePlandal.GetEffectiveNextStation(listTransfer[i]);
                    //中转站的下一站包含目的站
                    if (tmplistTransfer != null
                         && (tmplistTransfer.Contains(context.ArrivalID) || context.ArrivalID == -2))
                    {
                        listNeededTransfer.Add(listTransfer[i]);
                    }
                }
                if (listNeededTransfer.Count > 0)
                {
                    result.TransferStation = _expresscompanyBll.Search(listNeededTransfer) as List<ExpressCompanyModel>;
                }
                //
                return result;
            }
            return null;
        }

        #endregion
    }
}

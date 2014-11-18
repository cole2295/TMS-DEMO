using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IDAL.Transport.Plan
{
    public interface ITransportPlanDAL : ISequenceDAL
    {
        /// <summary>
        /// 新增运输计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(TransportPlanModel model);

        /// <summary>
        /// 修改运输计划
        /// </summary>
        /// <param name="model"></param>
        /// <param name="plandetail"></param>
        /// <returns></returns>
        int Update(TransportPlanModel model);

        /// <summary>
        /// 批量更新为停用状态
        /// </summary>
        /// <param name="listTPID">TPID列表</param>
        /// <returns></returns>
        int BatchUpdateToDisabled(List<int> listTPID);

        /// <summary>
        /// 更新为停用状态
        /// </summary>
        /// <param name="TPID">TPID</param>
        /// <returns></returns>
        int UpdateToDisabled(int TPID);

        /// <summary>
        /// 更新为启用状态
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        int UpdateToEnabled(int TPID);

        /// <summary>
        /// 运输计划是否存在(始发地，目的地只能有唯一的运输计划)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool IsExistsTransportPlan(TransportPlanModel model);

        /// <summary>
        /// 批量删除运输计划
        /// </summary>
        /// <param name="lstTpid">运输计划id列表</param>
        /// <returns></returns>
        int Delete(List<int> lstTpid);

        /// <summary>
        /// 运输计划列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewTransportPlanModel> Search(TransportPlanSearchModel searchModel);

        /// <summary>
        /// 根据出发地目的地取得生效并且可用的运输计划
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        List<TransportPlanModel> GetEnabledUsefulTransportPlan(int DepartureID, int ArrivalID);

        /// <summary>
        /// 根据主键ID取得一条数据
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        TransportPlanModel Get(int tpid);

        /// <summary>
        /// 根据出发地目的地和货物类型取得所有运输计划主键tpid
        /// </summary>
        /// <param name="DepartureID">出发地</param>
        /// <param name="ArrivalID">目的地</param>
        /// <param name="lineGoodsType">货物类型</param>
        /// <returns>tpid</returns>
        List<int> GetTpids(int departureID, int arrivalID, Enums.GoodsType lineGoodsType);

        /// <summary>
        ///  根据出发地目的地和货物类型取得可以使用的运输计划主键TPID
        /// </summary>
        /// <param name="departureID">出发地</param>
        /// <param name="arrivalID">目的地</param>
        /// <param name="lineGoodsType">货物类型</param>
        /// <returns></returns>
        List<int> GetUsefullyTPIDs(int departureID, int arrivalID, Enums.GoodsType lineGoodsType);

        /// <summary>
        /// 根据主键ID获取界面显示数据
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        ViewTansportEditorModel GetViewData(int tpid);

        /// <summary>
        /// 根据运输计划id取得线路明细
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        IList<ViewLinePlanModel> GetLinePlanByTpid(int tpid);


        /// <summary>
        /// 设置为生效
        /// </summary>
        /// <param name="TPID">运输计划TPID</param>
        /// <returns></returns>
        int UpdateToEffective(int TPID);

        /// <summary>
        /// 取得需要做生效处理的运输计划
        /// </summary>
        /// <returns></returns>
        TransportPlanModel GetNeedEffectivedTPID();

        /// <summary>
        /// 根据条件获取线路计划(存在多个取优先)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        LinePlanModel GetLinePlan(TransportPlanSearchModel condition);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Line;


namespace Vancl.TMS.IBLL.Transport.Plan
{
    public interface ITransportPlanBLL
    {
        /// <summary>
        /// 新增运输计划
        /// </summary>
        /// <param name="model">运输计划主信息</param>
        /// <param name="plandetail">运输计划明细线路路径信息</param>
        /// <returns></returns>
        ResultModel Add(TransportPlanModel model, IList<TransportPlanDetailModel> plandetail);

        /// <summary>
        /// 修改运输计划
        /// </summary>
        /// <param name="model">运输计划主信息</param>
        /// <param name="plandetail">运输计划明细线路路径信息</param>
        /// <returns></returns>
        ResultModel Update(TransportPlanModel model, IList<TransportPlanDetailModel> plandetail);

        /// <summary>
        /// 运输计划是否存在(始发地，目的地只能有唯一的运输计划)
        /// </summary>
        /// <param name="model">运输计划对象</param>
        /// <returns></returns>
        bool IsExistsTransportPlan(TransportPlanModel model);

        /// <summary>
        /// 批量停用运输计划
        /// </summary>
        /// <param name="listTPID">运输计划ID列表</param>
        /// <returns></returns>
        ResultModel BatchSetToDisabled(List<int> listTPID);

        /// <summary>
        /// 设置为停用状态
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        ResultModel SetToDisabled(int tpid);

        /// <summary>
        /// 设置为启用状态
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        ResultModel SetToEnabled(int tpid);

        /// <summary>
        /// 批量启用运输计划
        /// </summary>
        /// <param name="listTPID">运输计划ID列表</param>
        /// <returns></returns>
        ResultModel BatchSetToEnabled(List<int> listTPID);

        /// <summary>
        /// 批量删除运输计划
        /// </summary>
        /// <param name="lstTpid"></param>
        /// <returns></returns>
        ResultModel Delete(List<int> lstTpid);

        /// <summary>
        /// 运输计划列表
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewTransportPlanModel> Search(TransportPlanSearchModel searchModel);

        /// <summary>
        /// 搜索运输线路路径明细信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        IList<TransportPlanDetailModel> SearchTransportPlanPath(TransportPlanSearchModel searchModel);

        /// <summary>
        /// 搜索两点之间所有可用的线路路径
        /// </summary>
        /// <param name="searchModel">路径检索对象</param>
        /// <returns></returns>
        PointPathModel SearchAllPath(PointPathSearchModel searchModel);

        /// <summary>
        /// 取得运输计划对象
        /// </summary>
        /// <param name="tpid">运输计划主键ID</param>
        /// <returns></returns>
        TransportPlanModel Get(int tpid);

        /// <summary>
        /// 根据主键ID获取界面显示数据
        /// </summary>
        /// <param name="tpid">运输计划主键ID</param>
        /// <returns></returns>
        ViewTansportEditorModel GetViewData(int tpid);

        /// <summary>
        /// 根据运输计划id取得线路明细
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        IList<ViewLinePlanModel> GetLinePlanByTpid(int tpid);

        /// <summary>
        /// 设置需要做生效处理的TPID
        /// </summary>
        /// <returns></returns>
        void UpdateNeedEffectived();

        /// <summary>
        /// 取得所有运输计划详细
        /// </summary>
        /// <returns></returns>
        List<TransportPlanDetailLineIDRepairModel> GetValidTransportPlanDetail();

        /// <summary>
        /// 修复运输计划详细中的线路编号
        /// </summary>
        /// <param name="lstTPDModel">运输计划model</param>
        /// <param name="lstLinePlanModel">线路计划model</param>
        /// <returns></returns>
        ResultModel RepairTransportPlanDetailLineID(List<TransportPlanDetailLineIDRepairModel> lstTPDModel, List<LinePlanLineIDRepairModel> lstLinePlanModel);
    
        /// <summary>
        /// 恢复运输计划详细中的线路编号
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        ResultModel RestoreLineID(List<TransportPlanDetailLineIDRepairModel> lstModel);

        /// <summary>
        /// 根据条件获取线路计划(存在多个取优先)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        LinePlanModel GetLinePlan(TransportPlanSearchModel condition);
    }
}

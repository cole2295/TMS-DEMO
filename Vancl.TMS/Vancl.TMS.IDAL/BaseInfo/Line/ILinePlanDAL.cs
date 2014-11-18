using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;
using System;

namespace Vancl.TMS.IDAL.BaseInfo.Line
{
    public interface ILinePlanDAL : ISequenceDAL
    {
        /// <summary>
        /// 新增线路计划
        /// </summary>
        /// <param name="model">线路计划</param>
        /// <returns></returns>
        int Add(LinePlanModel model);
        /// <summary>
        /// 审核线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <param name="lineStatus">线路计划状态</param>
        /// <param name="isBatch">是否是批量审核</param>
        /// <returns></returns>
        int AuditLinePlan(int lpid, Enums.LineStatus lineStatus, DateTime? effectiveTime, bool isBatch);
        /// <summary>
        /// 根据线路计划id取得线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        LinePlanModel GetLinePlan(int lpid);

        /// <summary>
        /// 取得该线路生效并且可用的线路计划列表
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        List<LinePlanModel> GetEnabledUsefulLinePlan(String LineID);

        /// <summary>
        /// 取得需要生效的线路计划
        /// </summary>
        /// <returns></returns>
        int? GetNeedEffectivedLinePlan();

        /// <summary>
        /// 取得处于生效状态的线路计划列表
        /// </summary>
        /// <param name="LineID">线路ID</param>
        /// <returns></returns>
        List<int> GetEffectivedLinePlan(String LineID);

        /// <summary>
        /// 设置为生效
        /// </summary>
        /// <param name="TPID">线路计划LPID</param>
        /// <returns></returns>
        int UpdateToEffective(int LPID);

        /// <summary>
        /// 设置为作废
        /// </summary>
        /// <param name="LPID"></param>
        /// <returns></returns>
        int UpdateToExpired(int LPID);


        /// <summary>
        /// 根据线路计划id取得页面显示线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        ViewLinePlanModel GetViewLinePlan(int lpid);
        /// <summary>
        /// 查询线路计划
        /// </summary>
        /// <param name="model">查询条件</param>
        /// <returns></returns>
        PagedList<ViewLinePlanModel> GetLinePlan(LinePlanSearchModel model);
        /// <summary>
        /// 删除线路计划
        /// </summary>
        /// <param name="lpidList">线路ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);
        /// <summary>
        /// 更新线路计划
        /// </summary>
        /// <param name="model">线路计划</param>
        /// <returns></returns>
        int Update(LinePlanModel model);
        /// <summary>
        /// 取得起点所能到达的所有有效的点集合
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <returns></returns>
        IList<int> GetEffectiveNextStation(int DepartureID);

        /// <summary>
        /// 判断线路计划是否已存在
        /// </summary>
        /// <param name="linePlanModel"></param>
        /// <returns></returns>
        bool IsExsitLinePlan(LinePlanModel linePlanModel);

        /// <summary>
        /// 更新线路状态
        /// </summary>
        /// <returns>影响数据条数</returns>
        int UpdateDeadLineStatus();

        /// <summary>
        /// 设置线路启用停用状态
        /// </summary>
        /// <param name="lineID">线路id列表</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        int SetIsEnabled(List<string> lineID, bool isEnabled);

        /// <summary>
        /// 获取线路停用启用状态
        /// </summary>
        /// <param name="lineID"></param>
        /// <returns></returns>
        bool GetLineIsEnabled(string lineID);

        /// <summary>
        /// 取得所有线路计划
        /// </summary>
        /// <returns></returns>
        List<LinePlanLineIDRepairModel> GetAllValidLinePlan();

        /// <summary>
        /// 修复线路编号
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        int RepairLineID(List<LinePlanLineIDRepairModel> lstModel);
    }
}

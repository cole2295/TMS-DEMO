using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface ILinePlanBLL
    {
        /// <summary>
        /// 新增线路计划
        /// </summary>
        /// <param name="model">线路计划</param>
        /// <param name="linePriceModel">线路价格</param>
        /// <returns></returns>
        ResultModel Add(LinePlanModel model, IList<ILinePrice> linePriceModel);
        /// <summary>
        /// 审核线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <param name="lineStatus">线路计划状态</param>
        /// <returns></returns>
        ResultModel AuditLinePlan(int lpid, Enums.LineStatus lineStatus, DateTime? effectiveTime);
        /// <summary>
        /// 批量审核线路计划
        /// </summary>
        /// <param name="lstLpid">线路计划id</param>
        /// <param name="lineStatus">线路计划状态</param>
        /// <returns></returns>
        ResultModel BatchAuditLinePlan(List<int> lstLpid, Enums.LineStatus lineStatus);
        /// <summary>
        /// 根据线路计划id取得线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        LinePlanModel GetLinePlan(int lpid);
        /// <summary>
        /// 根据线路计划id取得页面显示线路计划
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        ViewLinePlanModel GetViewLinePlan(int lpid);
        /// <summary>
        /// 查询线路计划
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagedList<ViewLinePlanModel> GetLinePlan(LinePlanSearchModel searchModel);
        /// <summary>
        /// 更新线路计划
        /// </summary>
        /// <param name="model">线路计划</param>
        /// <param name="linePriceModel">线路价格</param>
        /// <returns></returns>
        ResultModel Update(LinePlanModel model, IList<ILinePrice> linePriceModel);
        /// <summary>
        /// 取得线路价格
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <param name="expressionType">线路价格类型</param>
        /// <returns></returns>
        IList<ILinePrice> GetLinePrice(int lpid, Enums.ExpressionType expressionType);
        /// <summary>
        /// 批量删除线路
        /// </summary>
        /// <param name="lpidList">线路ID列表</param>
        /// <returns></returns>
        ResultModel Delete(List<int> lpidList);
        /// <summary>
        /// 判断线路计划是否已存在
        /// </summary>
        /// <param name="linePlanModel"></param>
        /// <returns></returns>
        bool IsExsitLinePlan(LinePlanModel linePlanModel);
        /// <summary>
        /// 更新线路状态
        /// </summary>
        /// <returns>更新条数</returns>
        int UpdateDeadLineStatus();

        /// <summary>
        /// 设置线路启用停用状态
        /// </summary>
        /// <param name="lineID">线路id列表</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        ResultModel SetIsEnabled(List<string> lineID, bool isEnabled);

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
        ResultModel RepairLineID(List<LinePlanLineIDRepairModel> lstModel);

        /// <summary>
        /// 恢复线路编号
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        ResultModel RestoreLineID(List<LinePlanLineIDRepairModel> lstModel);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Claim;

namespace Vancl.TMS.IDAL.Claim
{
    /// <summary>
    /// 预期延误数据层操作接口
    /// </summary>
    public interface IExpectDelayDAL
    {
        /// <summary>
        /// 根据条件搜索出预期延误的分页列表
        /// </summary>
        /// <param name="searchModel">搜索条件</param>
        /// <returns>预期延误的分页列表</returns>
        PagedList<ViewExpectDelayModel> Search(ExpectDelaySearchModel searchModel);

        /// <summary>
        /// 根据调度ID获取预期延误
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <returns>预期延误信息</returns>
        ViewExpectDelayModel GetViewExpectDelayModel(long dispatchID);

        /// <summary>
        /// 根据ID获取预期延误模型
        /// </summary>
        /// <param name="id">预期延误模型ID</param>
        /// <returns>预期延误模型</returns>
        ExpectDelayModel Get(long id);

        /// <summary>
        /// 根据提货单号获取预期延误模型
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns>预期延误模型</returns>
        ExpectDelayModel GetByDeliveryNo(string deliveryNo);

        /// <summary>
        /// 添加预期延误模型
        /// </summary>
        /// <param name="model">预期延误模型</param>
        /// <returns>数据库所影响数</returns>
        int Add(ExpectDelayModel model);

        /// <summary>
        /// 更新预期延误模型
        /// </summary>
        /// <param name="model">预期延误模型</param>
        /// <returns>数据库所影响数</returns>
        int Update(ExpectDelayModel model);

        /// <summary>
        /// 审批预期延误模型
        /// </summary>
        /// <param name="model">预期延误模型</param>
        /// <returns>数据库所影响数</returns>
        int Approve(ExpectDelayModel model);

        /// <summary>
        /// 根据预期延误模型ID删除模型
        /// </summary>
        /// <param name="id">预期延误模型ID</param>
        /// <returns>数据库所影响数</returns>
        int Delete(long id);

        /// <summary>
        /// 是否存在未审批或已驳回状态的预计延误信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns>存在:true,不存在:false</returns>
        bool IsExistExpectDelayInfo(string deliveryNo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Claim
{
    public interface IExpectDelayBLL
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
        ViewExpectDelayModel Get(long dispatchID);

        /// <summary>
        /// 申请预期延误
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <param name="expectDelayType">预期延误类型</param>
        /// <param name="delayTime">延误时间</param>
        /// <param name="delayDesc">延误描述</param>
        ResultModel ApplyForExpectDelay(long dispatchID, Enums.ExpectDelayType expectDelayType, int delayTime, string delayDesc);

        /// <summary>
        /// 预期延误审核
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <param name="approveStatus">审核状态</param>
        ResultModel Audit(long dispatchID, Enums.ApproveStatus approveStatus);
    }
}

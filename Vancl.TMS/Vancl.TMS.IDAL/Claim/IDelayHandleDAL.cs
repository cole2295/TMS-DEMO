using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Claim
{
    public interface IDelayHandleDAL
    {
        /// <summary>
        ///  查询到货延误提货单信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewDelayHandleModel> Search(DelayHandleSearchModel searchModel);

        /// <summary>
        /// 新增到货延误复议申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(DelayHandleModel model);

        /// <summary>
        /// 查询到货延误复议申请
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewDelayHandleModel> SearchDelayHandleApply(DelayHandleSearchModel searchModel);

        /// <summary>
        /// 到货延误复议申请审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Approve(DelayHandleModel model);

        /// <summary>
        /// 获取到货延误复议处理
        /// </summary>
        /// <param name="delayID">延误主键id</param>
        /// <param name="approveStatus">审核状态</param>
        /// <returns></returns>
        DelayHandleModel GetDelayHandle(long delayID, Enums.ApproveStatus approveStatus);

        /// <summary>
        /// 更新到货延误复议申请原因
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateDelayHandleApplyReason(DelayHandleModel model);
    }
}

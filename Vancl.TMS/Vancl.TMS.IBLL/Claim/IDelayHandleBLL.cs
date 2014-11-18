using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Claim
{
    public interface IDelayHandleBLL
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
        ResultModel Add(DelayHandleModel model);

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
        ResultModel Approve(DelayHandleModel model);
    }
}

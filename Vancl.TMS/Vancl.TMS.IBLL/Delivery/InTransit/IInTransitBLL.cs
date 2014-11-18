using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Delivery.InTransit
{
    public interface IInTransitBLL
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagedList<ViewInTransitModel> Search(InTransitSearchModel searchModel);

        /// <summary>
        /// 根据提货单主键id取得界面用提货单
        /// </summary>
        /// <param name="dispatchID">提货单主键id</param>
        /// <returns></returns>
        ViewInTransitModel Get(long dispatchID);

        /// <summary>
        /// 确认到货
        /// </summary>
        /// <param name="dispatchID">提货单主键id</param>
        /// <param name="delayType">延误类型（可空）</param>
        /// <param name="delayReason">延误原因</param>
        /// <param name="signedUser">签收人</param>
        /// <param name="desReceiveDate">到货时间</param>
        /// <returns></returns>
        ResultModel SetArrive(long dispatchID, Enums.DelayType? delayType, string delayReason, string signedUser, DateTime? desReceiveDate = null);

        /// <summary>
        /// 到货确认是否需要提醒
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="message">提醒信息</param>
        /// <returns></returns>
        bool IsNeedConfirm(string deliveryNo, out string message);

        /// <summary>
        /// 获得确认到货的view
        /// </summary>
        /// <param name="dispatchID"></param>
        /// <returns></returns>
        ViewSetArriveModel GetSetArriveView(long dispatchID);
    }
}

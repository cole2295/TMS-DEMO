using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.InTransit;

namespace Vancl.TMS.IDAL.Delivery.InTransit
{
    public interface IInTransitDAL : ISequenceDAL
    {
        Util.Pager.PagedList<ViewInTransitModel> Search(InTransitSearchModel searchModel);

        ViewInTransitModel Get(long dispatchID);

        /// <summary>
        /// 确认到库时更新order表状态为已完成
        /// </summary>
        /// <param name="dispatchID">调度主键id</param>
        /// <returns></returns>
        int UpdateOrderTMSStatusToFinished(long dispatchID);

        /// <summary>
        /// 获得确认到货的view
        /// </summary>
        /// <param name="dispatchID"></param>
        /// <returns></returns>
        ViewSetArriveModel GetSetArriveView(long dispatchID);
    }
}

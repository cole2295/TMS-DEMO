using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.IDAL.Sorting.Common
{
    /// <summary>
    /// 分拣通用数据层接口
    /// </summary>
    public interface ISortCenterDAL
    {

        /// <summary>
        /// 取得分拣用户对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        SortCenterUserModel GetUserModel(int UserID);

        /// <summary>
        /// 取得目的地对象
        /// </summary>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        SortCenterToStationModel GetToStationModel(int ArrivalID);

        /// <summary>
        /// 运单是否属于所选分拣中心
        /// </summary>
        /// <param name="SortCenterID">分拣中心ID</param>
        /// <param name="model">运单对象</param>
        /// <returns></returns>
        bool IsBillBelongSortCenter(int SortCenterID, BillModel model);

    }
}

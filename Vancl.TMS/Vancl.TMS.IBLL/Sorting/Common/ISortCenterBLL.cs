using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.IBLL.Sorting.Common
{
    /// <summary>
    /// 分拣通用业务接口
    /// </summary>
    public interface ISortCenterBLL
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

    }
}

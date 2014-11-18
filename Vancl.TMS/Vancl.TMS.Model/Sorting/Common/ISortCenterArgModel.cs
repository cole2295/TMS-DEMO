using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣操作参数接口
    /// </summary>
    public interface ISortCenterArgModel
    {
        /// <summary>
        /// 目的地对象
        /// </summary>
        SortCenterToStationModel ToStation { get; set; }

        /// <summary>
        /// 操作人员对象
        /// </summary>
        SortCenterUserModel OpUser { get; set; }
    }
}

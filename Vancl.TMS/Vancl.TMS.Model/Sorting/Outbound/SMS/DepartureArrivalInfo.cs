using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound.SMS
{
    /// <summary>
    /// 出发地目的地对象
    /// </summary>
    public class DepartureArrivalInfo
    {
        /// <summary>
        /// 出发地城市名称
        /// </summary>
        public String DepartureCityName { get; set; }

        /// <summary>
        /// 当前操作者出发地部门名称
        /// </summary>
        public String DepartureDeptName { get; set; }

        /// <summary>
        /// 目的地城市名称
        /// </summary>
        public String ArrivalCityName { get; set; }

        /// <summary>
        /// 目的地部门名称
        /// </summary>
        public String ArrivalDeptName { get; set; }
    }
}

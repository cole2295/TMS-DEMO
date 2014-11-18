using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Sorting
{
    public class BillDeliveryModel
    {     /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
    //    public Enums.BillStatus BillStatus { get; set; }

        /// <summary>
        /// 配送商编号
        /// </summary>
        public string StationNum { get; set; }

        /// <summary>
        /// 配送商名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 部门类型（2、站点 3、加盟商）
        /// </summary>
        public int CompanyFlag { get; set; }
    }
}

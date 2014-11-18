using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 分拣装箱箱子对象
    /// </summary>
    [Serializable]
    public class SortingPackingBoxModel
    {
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 箱子重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 目的地站点id
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 目的地配送商编号
        /// </summary>
        public string ArrivalDistributionCode { get; set; }

        /// <summary>
        /// 目的地配送商编号
        /// </summary>
        public string ArrivalDistributionName { get; set; }

        /// <summary>
        /// 目的地站点类型
        /// </summary>
        public Enums.CompanyFlag ArrivalCompanyFlag { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public string ArrivalCompanyName { get; set; }

        /// <summary>
        /// 加密后的值
        /// </summary>
        public string EncryptValue { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }
    }
}

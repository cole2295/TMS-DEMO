using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度检索对象
    /// </summary>
    public class PreDispatchSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 批次来源
        /// </summary>
        public Enums.TMSEntranceSource? Source { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        

    }
}

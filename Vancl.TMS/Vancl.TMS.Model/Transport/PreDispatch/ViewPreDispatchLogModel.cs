using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度日志View对象
    /// </summary>
    public class ViewPreDispatchLogModel
    {

        /// <summary>
        /// 批次主键ID
        /// </summary>
        public long BID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 批次来源
        /// </summary>
        public Enums.TMSEntranceSource Source { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Note { get; set; }

    }
}

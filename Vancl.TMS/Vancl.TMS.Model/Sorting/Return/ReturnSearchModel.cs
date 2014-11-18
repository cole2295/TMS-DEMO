using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class ReturnSearchModel : BaseSearchModel
    {
        #region enums
        public enum TimeSource
        {
            /// <summary>
            /// 站点归班时间
            /// </summary>
            [Description("站点归班时间")]
            StationBack = 0,
            /// <summary>
            /// 站点打印时间
            /// </summary>
            [Description("站点打印时间")]
            StationPrint = 1
        }
        #endregion

        /// <summary>
        /// 时间
        /// </summary>
        public TimeSource TimeType { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 商家
        /// </summary>
        public int? MerchantID { get; set; }

        /// <summary>
        /// 运单类型
        /// </summary>
        public Enums.BillType? BillType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public List<Enums.BillStatus> BillStatus { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long? WaybillNo { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public int? ProvinceID { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public int? CityID { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public int? StationID { get; set; }
    }
}

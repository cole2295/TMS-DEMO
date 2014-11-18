using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.OutServiceProxy
{
    /// <summary>
    /// 时效监控代理对象
    /// </summary>
    public class AgingMonitoringLogProxyModel
    {

        /// <summary>
        /// TMS提货单订单明细keyID
        /// </summary>
        public long DODID { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int Operator { get; set; }

        /// <summary>
        /// 操作部门
        /// </summary>
        public int OperateDept { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 运单类型
        /// </summary>
        public Enums.BillType BillType { get; set; }

        /// <summary>
        /// 接货配送商
        /// </summary>
        public String DistributionCode { get; set; }

        /// <summary>
        /// 当前操作人所属配送商
        /// </summary>
        public String CurrentDistributionCode { get; set; }

        /// <summary>
        /// 操作省
        /// </summary>
        public String OperateProvince { get; set; }

        /// <summary>
        /// 操作城市
        /// </summary>
        public String OperateCity { get; set; }

        /// <summary>
        /// 操作地区
        /// </summary>
        public String OperateArea { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 运输出发地或者目的地
        /// </summary>
        public int TransportStationID { get; set; }

    }
}

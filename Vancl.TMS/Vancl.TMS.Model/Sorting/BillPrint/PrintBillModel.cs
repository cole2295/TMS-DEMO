using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
   /// <summary>
   /// 面单打印用到的运单模型
   /// （因为主要用户展示信息，所以就在这里独立建一个）
   /// </summary>
    public class PrintBillModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long BID { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 运单类型
        /// </summary>
        public Enums.BillType BillType { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 运单分配的配送站点
        /// </summary>
        public int DeliverStationID { get; set; }
        /// <summary>
        /// 运单分配的配送站点名称
        /// </summary>
        public string DeliverStationName { get; set; }

        /// <summary>
        /// 配送商
        /// </summary>
        public string DistributionName { get; set; }
        /// <summary>
        /// 运单当前所属配送商
        /// </summary>
        public string CurrentDistributionCode { get; set; }

        /// <summary>
        /// 商家编号    
        /// </summary>
        public int MerchantID { get; set; }
        /// <summary>
        /// 商家名
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 接货配送商
        /// </summary>
        public string DistributionCode { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public Enums.BillSource Source { get; set; }
        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }

        /// <summary>
        /// 配送商编号
        /// </summary>
        public string SiteNo { get; set; }
        /// <summary>
        /// 公司类型 3：配送商
        /// </summary>
        public int CompanyFlag { get; set; }
        /// <summary>
        /// 配送商名称
        /// </summary>
        public string CompanyName { get; set; }
    }
}

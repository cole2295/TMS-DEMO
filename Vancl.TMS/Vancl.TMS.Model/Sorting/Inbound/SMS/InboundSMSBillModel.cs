using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 入库短信运单对象
    /// </summary>
    public class InboundSMSBillModel
    {
        /// <summary>
        /// 系统运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime InboundTime { get; set; }

        /// <summary>
        /// 当前操作分拣中心ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 运单当前操作分检中心名称
        /// </summary>
        public String SortCenterName { get; set; }

        /// <summary>
        /// 入库目的地ExpressCompanyID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 客户收货地区
        /// </summary>
        public String ReceiveArea { get; set; }

        /// <summary>
        /// 配送站点
        /// </summary>
        public int DeliverStationID { get; set; }
        
        /// <summary>
        /// 配送站点联系电话
        /// </summary>
        public String DeliverStationContacterPhone { get; set; }

        /// <summary>
        /// 运单来源
        /// </summary>
        public Enums.BillSource Source { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 是否第一次分拣入库
        /// </summary>
        public bool IsFirstInbound { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Sorting
{
    /// <summary>
    /// 分拣运单主表
    /// </summary>
    [Serializable]
    public class BillModel : BaseModel, ISequenceable
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
        /// 出库key
        /// </summary>
        public string OutboundKey { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 运单类型
        /// </summary>
        public Enums.BillType BillType { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public string WarehouseID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 运单分配的配送站点
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 转站key
        /// </summary>
        public string TurnstationKey { get; set; }

        /// <summary>
        /// 入库key
        /// </summary>
        public string InboundKey { get; set; }

        /// <summary>
        /// 运单当前所属配送商
        /// </summary>
        public string CurrentDistributionCode { get; set; }

        /// <summary>
        /// 商家编号    
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 接货配送商
        /// </summary>
        public string DistributionCode { get; set; }

        /// <summary>
        /// 更新部门
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public Enums.BillSource Source { get; set; }

        /// <summary>
        /// 接货分检中心ID
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }

        /// <summary>
        /// 逆向状态
        /// </summary>
        public Enums.ReturnStatus? ReturnStatus { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_Bill";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_BILL_BID"; }
        }

        #endregion
    }
}

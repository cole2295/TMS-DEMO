using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Dps
{
    public class LifeCycleModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 运单编号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }

        /// <summary>
        /// 状态变化环节
        /// </summary>
        public int CurNode { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 当前子状态
        /// </summary>
        public int SubStatus { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public int? MerchantID { get; set; }

        /// <summary>
        /// 配送商编号
        /// </summary>
        public string DistributionCode { get; set; }

        /// <summary>
        /// 配送站点
        /// </summary>
        public int? DeliverStationID { get; set; }

        /// <summary>
        /// 目的配送商编号
        /// </summary>
        public string ToDistributionCode { get; set; }

        /// <summary>
        /// 目的部门
        /// </summary>
        public int? ToExpressCompanyID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateStation { get; set; }

        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSyn { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }
    }
}

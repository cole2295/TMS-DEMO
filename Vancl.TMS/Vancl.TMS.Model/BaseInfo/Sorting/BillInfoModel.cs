using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Sorting
{
    /// <summary>
    /// 分拣运单信息表
    /// </summary>
    [Serializable]
    public class BillInfoModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long BIID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 客户重量
        /// </summary>
        public decimal CustomerWeight { get; set; }

        /// <summary>
        /// 客户箱码
        /// </summary>
        public string CustomerBoxNo { get; set; }

        /// <summary>
        /// 货品属性
        /// </summary>
        public Enums.BillGoodsType BillGoodsType { get; set; }

        /// <summary>
        /// 称重重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public Enums.PayType PayType { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal ReceivableAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal InsuredAmount { get; set; }

        /// <summary>
        /// 客户备注
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 总件数
        /// </summary>
        public int PackageCount { get; set; }

        /// <summary>
        /// 包装方式
        /// </summary>
        public string PackageMode { get; set; }

        /// <summary>
        /// 总计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 是否通过面单校验
        /// </summary>
        public bool IsValidateBill { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_BillInfo";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_BILLINFO_BIID"; }
        }

        #endregion
    }
}

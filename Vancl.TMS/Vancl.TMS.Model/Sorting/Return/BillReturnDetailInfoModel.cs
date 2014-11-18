using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 退货分拣称重装箱用到的模型
    /// </summary>
    [Serializable]
    public class BillReturnDetailInfoModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string RBID { get; set; }
        /// <summary>
        /// 查询的行号
        /// </summary>
        public int rNow { get; set; }
        /// <summary>
        /// 系统运单号
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 返货目的地
        /// </summary>
        public string ReturnTo { get; set; }
        /// <summary>
        /// 创建部门
        /// </summary>
        public string CreateDept { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }
        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_BILLRETURNDETAILINFO";
            }
        }
        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "016"; }
        }

        #endregion
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_BillReturnDetail_RBID"; }
        }

        #endregion
    }
}

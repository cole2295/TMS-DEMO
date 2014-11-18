using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 退货分拣装箱箱号模型
    /// </summary>
    public class BillReturnBoxInfoModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string RBoxID { get; set; }
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
        /// 退货商家
        /// </summary>
        public string ReturnMerchant { get; set; }
        /// <summary>
        /// 创建部门
        /// </summary>
        public string CreateDept { get; set; }
        /// <summary>
        /// 创建部门ID
        /// </summary>
        public int CreateDeptID { get; set; }
        /// <summary>
        /// 当前操作部门名称
        /// </summary>
        public string CurrentDeptName { get; set; }
        /// <summary>
        /// 当前配送上Code
        /// </summary>
        public string CurrentDistributionCode { get; set; }
        /// <summary>
        /// 箱号中含有订单数量
        /// </summary>
        public int BillNum { get; set; }
        /// <summary>
        /// 是否已经装箱打印
        /// </summary>
        public bool IsPrintBackPacking { get; set; }
        /// <summary>
        /// 是否已打印退货交接表
        /// </summary>
        public bool IsPrintBackForm { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_BILLRETURNBOXINFO";
            }
        }
        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "015"; }
        }

        #endregion
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_BillReturnBox_RBoxID"; }
        }

        #endregion
    }
}

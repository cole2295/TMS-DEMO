using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 退货分拣称重箱号模型
    /// </summary>
    public class WaybillReturnBoxInfoEntityModel:BaseModel,ISequenceable
    {
        public long WaybillReturnBoxInfoID { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 返货商家
        /// </summary>
        public string ReturnMerchant { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public string ReturnTo { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public string CreateDep { get; set; }
        /// <summary>
        /// 称重重量
        /// </summary>
        public decimal Weight { get; set; }

        public bool IsPrintBackPacking { get; set; }
        public bool IsPrintBackForm { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "WAYBILLRETURNBOXINFO";
            }
        }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_WAYBILLRETURNBOX_RID"; }
        }

        #endregion
    }
}

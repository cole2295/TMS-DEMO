using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    public class WaybillReturnDetailInfoEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long WaybillReturnDetailInfoID { get; set; }
         /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int CreateBy { get; set; }

        public string ReturnTo { get; set; }

        public string CreateDep { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 运单称重重量
        /// </summary>
        public decimal Weight { get; set; }
        public override string ModelTableName
        {
            get
            {
                return "WaybillReturnDetailInfo";
            }
        }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_WaybillReturnDetail_RID"; }
        }
        #endregion
    }
}

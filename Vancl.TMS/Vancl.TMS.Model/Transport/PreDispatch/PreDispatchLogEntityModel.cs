using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    public class PreDispatchLogEntityModel : BaseModel, ISequenceable
    {
        public override string ModelTableName
        {
            get
            {
                return "TMS_PreDispatchLog";
            }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public long PDLID { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public String Note { get; set; }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_tms_predispatchlog_pdlid"; }
        }

        #endregion
    }
}

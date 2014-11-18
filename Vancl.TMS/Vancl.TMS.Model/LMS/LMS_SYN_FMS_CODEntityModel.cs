using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// LMS同步到FMS财务中间表实体对象
    /// </summary>
    public class LMS_SYN_FMS_CODEntityModel : BaseModel, ISequenceable, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long LMS_SYN_FMS_CODIDID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int OperateType { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 配送站点ID
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// COD之间数据创建人
        /// </summary>
        public String CODCreateBy { get; set; }

        /// <summary>
        /// KEY
        /// </summary>
        public String LmsSynFMSCodKid { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "LMS_SYN_FMS_COD";
            }
        }

        #region ISequenceable 成员

        string ISequenceable.SequenceName
        {
            get { return "seq_lms_syn_fms_cod"; }
        }

        #endregion

        #region IKeyCodeable 成员

        string IKeyCodeable.SequenceName
        {
            get { return "SEQ_WL_LMS_SYN_FMS_COD_KeyID"; }
        }

        string IKeyCodeable.TableCode
        {
            get { return "012"; }
        }

        #endregion
    }
}

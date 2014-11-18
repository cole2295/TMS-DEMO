using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 操作日志实体对象
    /// </summary>
    public class OperateLogEntityModel : BaseModel, ISequenceable, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long OperateLogID { get; set; }

        /// <summary>
        /// 运单编码
        /// </summary>
        public long WaybillNO { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        public int? LogType { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public String Operation { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public String LogOperator { get; set; }

        /// <summary>
        /// 操作站点
        /// </summary>
        public int OperatorStation { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public String Result { get; set; }

        /// <summary>
        /// 操作运单当前状态 
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 是否同步
        /// </summary>
        public int? IsSyn { get; set; }

        /// <summary>
        /// 配送员编号
        /// </summary>
        public String OldDeliverMan { get; set; }

        /// <summary>
        /// 日志表KEY
        /// </summary>
        public String OperateLogKid { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "OperateLog";
            }
        }


        #region ISequenceable 成员

        string ISequenceable.SequenceName
        {
            get { return "seq_operatelog"; }
        }

        #endregion

        #region IKeyCodeable 成员

        string IKeyCodeable.SequenceName
        {
            get { return "SEQ_WL_OperateLog_KeyID"; }
        }

        string IKeyCodeable.TableCode
        {
            get { return "007"; }
        }

        #endregion
    }



}

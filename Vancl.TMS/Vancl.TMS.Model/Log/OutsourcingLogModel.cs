using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 外包操作日志表
    /// </summary>
    [Serializable]
    public class OutsourcingLogModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string OLID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 业主方ID
        /// </summary>
        public int PrincipalUserID { get; set; }

        /// <summary>
        /// 业主方部门ID
        /// </summary>
        public int PrincipalDeptID { get; set; }

        /// <summary>
        /// 业主方配送商编码
        /// </summary>
        public string PrincipalDistributionCode { get; set; }

        /// <summary>
        /// 代理方ID
        /// </summary>
        public int AgentUserID { get; set; }

        /// <summary>
        /// 代理方ID
        /// </summary>
        public int AgentDeptID { get; set; }

        /// <summary>
        /// 代理方配送商编码
        /// </summary>
        public string AgentDistributionCode { get; set; }

        /// <summary>
        /// 外包操作类型
        /// </summary>
        public Enums.TmsOperateType OperateType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "SC_OutsourcingLog";
            }
        }

        #region IKeyCodeable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_OUTSOURCINGLOG_OLID"; }
        }

        public string TableCode
        {
            get { return "014"; }
        }

        #endregion
    }
}

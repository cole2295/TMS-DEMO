using System;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 日志实体对象基类
    /// </summary>
    [Serializable]
    public class BaseLogModel
    {
        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int OperateBy { get; set; }

        /// <summary>
        /// 当前操作人姓名
        /// </summary>
        public String Operator { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 操作部门
        /// </summary>
        public int OperateDeptID { get; set; }

        /// <summary>
        /// 操作部门名称
        /// </summary>
        public string OperateDeptName { get; set; }

        /// <summary>
        /// 操作公司
        /// </summary>
        public int OperateCompanyID { get; set; }

        /// <summary>
        /// 操作公司名称
        /// </summary>
        public string OperateCompanyName { get; set; }
    }
}

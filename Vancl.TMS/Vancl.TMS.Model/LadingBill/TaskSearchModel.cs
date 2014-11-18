using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LadingBill
{
    [Serializable]
    public class TaskSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 时间类型（预计提货时间（0），实际提货时间（1））
        /// </summary>
        public virtual int? timeType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime starTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime endTime { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public virtual int taskStatus { get; set; }

        /// <summary>
        /// 商家
        /// </summary>
        public virtual string merchantid { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public virtual string warehouseid { get; set; }

        /// <summary>
        /// 任务编号
        /// </summary>
        public virtual string taskCode { get; set; }
        /// <summary>
        /// 提货公司
        /// </summary>
        public virtual string todisribution { get; set; }

        /// <summary>
        /// 指派公司
        /// </summary>
        public virtual string FROMDISTRIBUTIONCODE { get; set; }

        /// <summary>
        /// 提货部门
        /// </summary>
        public virtual string DEPARTMENT { get; set; }
    }
}

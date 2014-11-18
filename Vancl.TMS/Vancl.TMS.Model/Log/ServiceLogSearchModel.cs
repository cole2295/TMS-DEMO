using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    public class ServiceLogSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 系统单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool? IsSuccessed { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public Enums.ServiceLogType? LogType { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int? OpFunction { get; set; }

        /// <summary>
        /// 是否已重新处理
        /// </summary>
        public bool? IsHandeld { get; set; }
    }
}

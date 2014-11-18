using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 操作日志检索基类
    /// </summary>
    public class BaseOperateLogSearchModel : BaseLogSearchModel
    {
        /// <summary>
        /// 操作相关主键值
        /// </summary>
        public String KeyValue { get; set; }

        /// <summary>
        /// 系统模块
        /// </summary>
        public Enums.SysModule Module { get; set; }
    }
}

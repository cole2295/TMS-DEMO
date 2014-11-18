using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路操作日志检索
    /// </summary>
    public class LineOperateLogSearchModel : BaseOperateLogSearchModel
    {
        /// <summary>
        /// 线路编码
        /// </summary>
        public String LineID { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Report
{
    /// <summary>
    /// 报表筛选检索对象
    /// </summary>
    public class ReportFilterSearchModel: BaseSearchModel
    {
        /// <summary>
        /// 报表种类
        /// </summary>
        public Enums.ReportCategory ReportType { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool? IsShow { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }

    }
}

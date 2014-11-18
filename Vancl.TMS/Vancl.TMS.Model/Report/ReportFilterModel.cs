using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Report.ComplexReport;
using System.ComponentModel;

namespace Vancl.TMS.Model.Report
{
    /// <summary>
    /// 报表筛选对象
    /// </summary>
    public class ReportFilterModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键RFID
        /// </summary>
        public long RFDI { get; set; }

        /// <summary>
        /// 报表种类
        /// </summary>
        public Enums.ReportCategory ReportType { get; set; }

        /// <summary>
        /// View对象属性名称
        /// </summary>
        public String ViewObjPropertyName { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public String ShowName
        {
            get
            {
                if (ViewObjPropertyName != null)
                {
                    if (ReportType == Enums.ReportCategory.ComplexReport)
                    {
                        Type type = typeof(ViewComplexReport);
                        var property = type.GetProperty(ViewObjPropertyName);
                        if (property != null)
                        {
                            var descAttr = property.GetCustomAttributes(typeof(DescriptionAttribute), false);
                            if (descAttr != null && descAttr.Length > 0)
                            {
                                return (descAttr[0] as DescriptionAttribute).Description;
                            }
                            return ViewObjPropertyName;
                        }
                    }
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 客户自定义标题
        /// </summary>
        public String CustShowName { get; set; }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_ReportFilter_RFDI"; }
        }

        #endregion
    }
}

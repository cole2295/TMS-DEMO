using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.DbAttributes;

namespace Vancl.TMS.Model.Sorting.BillPrint
{

    [Table(Name = "SC_BillPrintField")]
    public class BillPrintFieldModel : BaseDbModel<long>, ISequenceable
    {
        public override string ModelTableName
        {
            get
            {
                return "SC_BillPrintField";
            }
        }
        public string SequenceName
        {
            get { return "Seq_SC_BillPrintField_ID"; }
        }
        /// <summary>
        /// 配送商编码
        /// </summary>
        [Column]
        public string DistributionCode { get; set; }
        /// <summary>
        /// 字段显示名称
        /// </summary>
        [Column]
        public string ShowName { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        [Column]
        public string FieldName { get; set; }
        /// <summary>
        /// 显示模板
        /// </summary>
        [Column]
        public string FieldFormat { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [Column]
        public string DefaultValue { get; set; }
        /// <summary>
        /// 默认样式
        /// </summary>
        [Column]
        public string DefaultStyle { get; set; }
        /// <summary>
        /// 标注
        /// </summary>
        [Column]
        public string Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Column]
        public int Sort { get; set; }
    }
}

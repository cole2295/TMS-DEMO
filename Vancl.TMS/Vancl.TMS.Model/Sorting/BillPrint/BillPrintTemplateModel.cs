using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.DbAttributes;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    [Table(Name = "sc_billprinttemplate")]
    public class BillPrintTemplateModel : BaseDbModel<long>, ISequenceable
    {
        public override string ModelTableName
        {
            get
            {
                return "SC_BillPrintTemplate";
            }
        }
        public string SequenceName
        {
            get { return "Seq_SC_BillPrintTemplate_ID"; }
        }
        /// <summary>
        /// 配送商编码
        /// </summary>
        [Column]
        public string DistributionCode { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        [Column]
        public string Name { get; set; }
        /// <summary>
        /// 模板内容存储
        /// </summary>
        [Column]
        public string Storage { get; set; }
        /// <summary>
        /// 模板宽度（单位：厘米）
        /// </summary>
        [Column]
        public float Width { get; set; }
        /// <summary>
        /// 模板高度（单位：厘米）
        /// </summary>
        [Column]
        public float Height { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        [Column]
        public string Background { get; set; }
        /// <summary>
        /// 模板备注
        /// </summary>
        [Column]
        public string Remark { get; set; }

        /// <summary>
        /// 是否是默认模板
        /// </summary>
        [Column]
        public bool IsDefault { get; set; }
    }
}

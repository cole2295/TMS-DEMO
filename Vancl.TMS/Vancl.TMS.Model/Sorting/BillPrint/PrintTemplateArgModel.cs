using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    public class PrintTemplateArgModel
    {
        /// <summary>
        /// 模板id
        /// </summary>
        public long Id { get; set; }
        public long TplId { get; set; }
        public string Name { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public string Background { get; set; }
        public string Content { get; set; }

        public bool IsNew { get; set; }
        public bool IsDefault { get; set; }
    }
}

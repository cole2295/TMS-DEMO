using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// lms表WaybillThirdPartyInfo实体
    /// </summary>
    public class WaybillThirdPartyInfoEntityModel
    {
        public long WaybillThirdPartyInfoId { get; set; }
        public long WaybillNo { get; set; }
        public string BoxNo { get; set; }
        public string BoxType { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}

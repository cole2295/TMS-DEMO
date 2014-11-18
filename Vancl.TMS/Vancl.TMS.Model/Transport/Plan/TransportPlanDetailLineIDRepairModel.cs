using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.Plan
{
    public class TransportPlanDetailLineIDRepairModel : BaseModel
    {
        /// <summary>
        /// 运输计划明细主键ID
        /// </summary>
        public long TPDID { get; set; }

        /// <summary>
        /// 旧线路编号
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// 新线路编号
        /// </summary>
        public string NewLineID { get; set; }

        /// <summary>
        /// 线路主键ID
        /// </summary>
        public int LPID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    [Serializable]
    public class BatchModel : BaseModel
    {
        public override string ModelTableName
        {
            get
            {
                return "TMS_BoxDetail";
            }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public Int32 SerialNumber { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 出库批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 生成箱号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 出发站
        /// </summary>
        public String DepartureId { get; set; }

        /// <summary>
        /// 出发站名称
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 到达站
        /// </summary>
        public String ArrivalId { get; set; }

        /// <summary>
        /// 到达站名称
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 是否一致
        /// </summary>
        public Boolean IsConsistency { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class LostOrderDetailModel : BaseModel
    {
        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        public string FormCode { get; set; }

    }
}

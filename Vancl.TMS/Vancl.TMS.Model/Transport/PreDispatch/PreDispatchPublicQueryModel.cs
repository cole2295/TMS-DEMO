using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    public class PreDispatchPublicQueryModel
    {
        /// <summary>
        /// 调度ID
        /// </summary>
        public long DID { get; set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public Enums.DispatchStatus Status { get; set; }
    }
}

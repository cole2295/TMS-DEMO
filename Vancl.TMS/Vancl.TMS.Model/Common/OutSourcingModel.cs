using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    [Serializable]
    public class OutSourcingModel
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 业主方ID
        /// </summary>
        public int PrincipalUserID { get; set; }
    }
}

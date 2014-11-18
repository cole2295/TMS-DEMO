using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.PDA.Core.Model
{
    public class PDAUserModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string UserDisplayName { get; set; }

        public int? UserDeptID { get; set; }

        public string UserDistributeCode { get; set; }

        public DateTime UserLoginTime { get; set; }
    }
}

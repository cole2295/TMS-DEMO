using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Security
{
    public class TMSUser : IUser
    {
        #region IUser 成员

        public int ID
        {
            get;
            set;
        }

        public int DeptID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        #endregion
    }
}

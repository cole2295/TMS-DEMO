using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Security
{
    public interface IUser
    {
        int ID { get; set; }
        int DeptID { get; set; }
        string Name { get; set; }
    }
}

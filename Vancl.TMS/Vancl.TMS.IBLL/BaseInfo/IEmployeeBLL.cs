using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface IEmployeeBLL
    {
        IList<EmployeeModel> GetDriverByCityList(List<string> cityList); 
    }
}

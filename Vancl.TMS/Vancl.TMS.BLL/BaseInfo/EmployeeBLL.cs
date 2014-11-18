using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class EmployeeBLL : IEmployeeBLL
    {
        IEmployeeDAL _dal = ServiceFactory.GetService<IEmployeeDAL>("EmployeeDAL");

        public IList<EmployeeModel> GetDriverByCityList(List<string> cityList)
        {
            var list = _dal.GetDriverByCityList(cityList);
            if (list != null) return list;
            return new List<EmployeeModel>();
        }
    }
}

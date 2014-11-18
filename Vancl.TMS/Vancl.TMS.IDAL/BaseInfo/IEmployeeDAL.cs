using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.OutServiceProxy;

namespace Vancl.TMS.IDAL.BaseInfo
{
    public interface IEmployeeDAL
    {
        /// <summary>
        /// 取得用户
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        String GetEmployeeName(int employeeID);

        /// <summary>
        /// 根据城市获取司机
        /// </summary>
        /// <param name="cityList"></param>
        /// <returns></returns>
        IList<EmployeeModel> GetDriverByCityList(List<string> cityList);

        /// <summary>
        /// 取得到货时效操作人对象
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        AgingMonitoringLogProxyModel GetAgingMonitoringLogEmployee(int employeeID);
    }
}

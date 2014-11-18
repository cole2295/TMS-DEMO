using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.OutServiceProxy;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    public class EmployeeDAL : BaseDAL, IEmployeeDAL
    {
        #region IEmployeeDAL 成员
        /// <summary>
        /// 取得用户
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public string GetEmployeeName(int employeeID)
        {
            String sql = @"
SELECT EmployeeName 
FROM Employee
WHERE EmployeeID = :EmployeeID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "EmployeeID" , DbType = DbType.Int32, Value = employeeID}
            };
            var objResult = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objResult != null && objResult != DBNull.Value)
            {
                return objResult.ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// 根据城市获取司机
        /// </summary>
        /// <param name="cityList"></param>
        /// <returns></returns>
        public IList<EmployeeModel> GetDriverByCityList(List<string> cityList)
        {
            string sql =  string.Format(
                                    @"SELECT   e.EmployeeID,e.EmployeeName
                                        FROM   employee e 
                                        JOIN   EmployeeRole er 
                                          ON   er.EmployeeID = e.EmployeeID
                                        JOIN   ExpressCompany ec 
                                          ON   ec.ExpressCompanyID = e.StationID
                                        JOIN   Role r 
                                          ON   r.RoleID = er.RoleID
                                        JOIN   City c 
                                          ON   c.CityID = ec.CityID
                                       WHERE   r.RoleName = '司机'
                                               And e.IsDeleted=0
                                               AND c.CityID IN ({0})
                                        Order by e.EmployeeName",
                               "'"+string.Join("','", cityList)+"'");

            return ExecuteSql_ByDataTableReflect<EmployeeModel>(TMSReadOnlyConnection, sql);
        }

        #endregion

        #region IEmployeeDAL 成员


        public Model.OutServiceProxy.AgingMonitoringLogProxyModel GetAgingMonitoringLogEmployee(int employeeID)
        {
            String sql = @"
SELECT E.EMPLOYEEID as Operator 
,EC.EXPRESSCOMPANYID as OperateDept 
,EC.PROVINCEID as OperateProvince 
,EC.CITYID as OperateCity
, ' ' as OperateArea
, ec.distributioncode AS CurrentDistributionCode
FROM Employee E
     JOIN EXPRESSCOMPANY EC ON E.STATIONID = EC.EXPRESSCOMPANYID
WHERE E.EMPLOYEEID = :EMPLOYEEID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "EmployeeID" , DbType = DbType.Int32, Value = employeeID}
            };
            return ExecuteSqlSingle_ByReaderReflect<AgingMonitoringLogProxyModel>(TMSReadOnlyConnection, sql, arguments);
        }

        #endregion
    }
}

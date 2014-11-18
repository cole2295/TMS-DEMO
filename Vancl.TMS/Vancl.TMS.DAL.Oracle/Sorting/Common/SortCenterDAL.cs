using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Common;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.DAL.Oracle.Sorting.Common
{
    /// <summary>
    /// 分拣通用数据层
    /// </summary>
    public class SortCenterDAL : BaseDAL, ISortCenterDAL
    {
        #region ISortCenterDAL 成员

        /// <summary>
        /// 取得分拣用户对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public SortCenterUserModel GetUserModel(int UserID)
        {
            String sql = @"
SELECT  
        EMP.EmployeeID AS UserId ,
        EMP.EmployeeName  AS UserName ,
        EMP.StationID AS ExpressId ,
        EC.CompanyFlag ,
        EC.CompanyName ,
        EC.MnemonicCode ,
        EC.MnemonicName ,
        DIS.DistributionCode ,
        DIS.DistributionName ,
        DIS.MnemonicName AS DistributionMnemonicName
FROM    Employee EMP 
        JOIN   ExpressCompany EC ON  EMP.StationID = EC.ExpressCompanyID
        JOIN   Distribution DIS ON EMP.DistributionCode = DIS.DistributionCode
WHERE  EMP.EmployeeID = :EmployeeID
            AND EMP.IsDeleted = 0 
            AND DIS.Isdelete = 0
";
            OracleParameter[] parameters = new OracleParameter[]
            { 
                new OracleParameter() { ParameterName = "EmployeeID", DbType = System.Data.DbType.Int32 , Value = UserID }
            };
            return ExecuteSqlSingle_ByReaderReflect<SortCenterUserModel>(TMSReadOnlyConnection, sql, parameters);
        }

        /// <summary>
        /// 取得目的地对象
        /// </summary>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        public SortCenterToStationModel GetToStationModel(int ArrivalID)
        {
            String sql = @"
SELECT ExpressCompanyID, CompanyFlag, DistributionCode, MnemonicName,CompanyName
FROM ExpressCompany
WHERE ExpressCompanyID = :ExpressCompanyID
";
            OracleParameter[] parameters = new OracleParameter[]
            { 
                new OracleParameter() { ParameterName = "ExpressCompanyID", OracleDbType = OracleDbType.Int32 , Value = ArrivalID }
            };
            return ExecuteSqlSingle_ByReaderReflect<SortCenterToStationModel>(TMSReadOnlyConnection, sql, parameters);
        }


        /// <summary>
        /// 运单是否属于所选分拣中心
        /// </summary>
        /// <param name="SortCenterID">分拣中心ID</param>
        /// <param name="model">运单对象</param>
        /// <returns></returns>
        public bool IsBillBelongSortCenter(int SortCenterID, BillModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null.");
            String sql = @"
SELECT 1
FROM ExpressCompany
WHERE ExpressCompanyID = :ExpressCompanyID
AND ParentID = :ParentID
AND IsDeleted = 0
AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "ExpressCompanyID" ,  OracleDbType = OracleDbType.Int32 , Value = model.DeliverStationID },
                new OracleParameter() { ParameterName = "ParentID" ,  OracleDbType = OracleDbType.Int32 , Value = SortCenterID }
            };
            var objValue = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

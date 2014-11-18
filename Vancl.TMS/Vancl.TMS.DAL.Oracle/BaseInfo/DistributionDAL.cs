using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    public class DistributionDAL : BaseDAL, IDistributionDAL
    {
        #region IDistributionDAL 成员

        public Model.BaseInfo.Distribution GetModel(string DistributionCode)
        {
            throw new NotImplementedException();
        }


        public string GetDistributionNameByCode(string distributionCode)
        {
            var sql = @"
SELECT DistributionName 
FROM  distribution
WHERE rownum = 1 
    AND DistributionCode=:DistributionCode";
            var parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "DistributionCode" , DbType = DbType.String ,Value = distributionCode }
            };
            var name = ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters);
            if (name == null) return null;
            return Convert.ToString(name);
        }

        #endregion

        #region IDistributionDAL 成员

        /// <summary>
        /// 取得配送商前一个状态
        /// </summary>
        /// <param name="DistributionCode">配送商Code</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        public Enums.BillStatus? GetDistributionPreBillStatus(string DistributionCode, Enums.BillStatus Status)
        {
            if (String.IsNullOrWhiteSpace(DistributionCode)) throw new ArgumentNullException("DistributionCode is null or empty.");
            String sql = @"
SELECT BillStatus
FROM
(
  SELECT BusinessStatus AS BillStatus
  FROM   DistributionBusinessRelation dbr
      JOIN Distribution d  ON d.DistributionID= dbr.DistributionId
  WHERE  d.DistributionCode = :DistributionCode
                  AND Sorting < (
                                   SELECT  Sorting
                                   FROM   DistributionBusinessRelation
                                   WHERE  BusinessStatus = :BillStatus  AND DistributionId = d.DistributionID AND ROWNUM = 1
                             )
  ORDER BY Sorting DESC
) 
WHERE ROWNUM = 1
";
            var parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "DistributionCode" , DbType = DbType.String ,Value = DistributionCode },
                new OracleParameter(){ParameterName = "BillStatus" , DbType = DbType.Int32 ,Value = (int)Status }
            };
            var result = ExecuteSqlDataTable(TMSReadOnlyConnection, sql, parameters);
            if (result != null && result.Rows.Count > 0)
            {
                return (Enums.BillStatus)Convert.ToInt32(result.Rows[0]["BillStatus"]);
            }
            return null;
        }

        /// <summary>
        /// 判断配送商是否支持分拣
        /// </summary>
        /// <param name="distributionCode">配送商代码</param>
        /// <returns></returns>
        public bool IsSupportSorting(string distributionCode)
        {
            string sql = string.Format(@"
                                SELECT    dbr.BusinessStatus
                                  FROM    DistributionBusinessRelation dbr 
                                  JOIN    Distribution d 
                                    ON    d.DistributionID = dbr.DistributionId
                                 WHERE    dbr.BusinessStatus IN ({0}, {1})
                                   AND    d.DistributionCode = :DistributionCode",
                                (int)Enums.BillStatus.HaveBeenSorting,
                                (int)Enums.BillStatus.Outbounded);
            OracleParameter[] arguments =
            {
                new OracleParameter(){ ParameterName=":DistributionCode" , DbType= DbType.String, Value = distributionCode}
            };

            var status = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (status == null)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region IDistributionDAL 成员

        /// <summary>
        /// 是否存在运输中心
        /// </summary>
        /// <param name="distributionCode">配送商</param>
        /// <returns></returns>
        public bool ExistsTrafficCenter(string distributionCode)
        {
            String sql = @"
SELECT 1
FROM  DistributionBusinessRelation dbr
    JOIN Distribution d ON  d.DistributionID = dbr.DistributionID
WHERE  dbr.BusinessStatus = :BusinessStatus AND d.DistributionCode = :DistributionCode
AND dbr.IsDelete = 0 AND d.IsDelete = 0 AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DistributionCode", DbType = DbType.String , Value = distributionCode },
                new OracleParameter() { ParameterName="BusinessStatus", DbType = DbType.Int32 , Value = (int)Enums.BillStatus.InTransit }
            };
            object objValue = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否存在分拣中心
        /// </summary>
        /// <param name="distributionCode">配送商</param>
        /// <returns></returns>
        public bool ExistsSortCenter(string distributionCode)
        {
            String sql = String.Format(@"
SELECT COUNT(1)
FROM  DistributionBusinessRelation dbr
    JOIN Distribution d ON  d.DistributionID = dbr.DistributionID
WHERE  dbr.BusinessStatus IN ({0},{1})  AND d.DistributionCode = :DistributionCode
AND dbr.IsDelete = 0 AND d.IsDelete = 0
",
    (int)Enums.BillStatus.HaveBeenSorting,
    (int)Enums.BillStatus.Outbounded
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DistributionCode", DbType = DbType.String , Value = distributionCode }
            };
            object objValue = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return Convert.ToInt32(objValue) > 1;
            }
            return false;
        }

        #endregion

        #region IDistributionDAL 成员

        public IList<Distribution> GetDistributionList()
        {
            string sql = @" 
                       SELECT   DistributionID,
                                DistributionCode,
                                DistributionName
                         FROM   Distribution
                        WHERE   IsDelete = 0 ";
            return ExecuteSql_ByDataTableReflect<Distribution>(TMSReadOnlyConnection,sql);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.OutSync;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.Synchronous.OutSync
{
    public class Tms2LmsSyncTMSDAL : BaseDAL, ITms2LmsSyncTMSDAL
    {
        #region ITms2LmsSyncTMSDAL 成员

        public List<BillChangeLogModel> ReadTmsChangeLogs(Tms2LmsJobArgs argument)
        {
            if (argument == null) throw new ArgumentNullException("Tms2LmsJobArgs is null.");
            String sql = String.Format(@"
SELECT *
FROM
(
    SELECT BCID , FormCode , CurrentStatus, PreStatus, ReturnStatus , OperateType, CurrentDistributionCode, DeliverStationID, Note, CreateBy, CreateDept, CreateTime ,Todistributioncode ,Toexpresscompanyid
    FROM SC_BillChangeLog t1
    WHERE CreateTime > :SyncTime
        AND SyncFlag = {0}
    ORDER BY CreateTime ASC
)
WHERE ROWNUM <= :TopCount
",
 (int)Enums.SyncStatus.NotYet
 );
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="SyncTime" , OracleDbType = OracleDbType.Date , Value = argument.SyncTime },
                new OracleParameter() { ParameterName="TopCount" , OracleDbType = OracleDbType.Int32 , Value = argument.TopCount  }
            };
            var result = ExecuteSql_ByReaderReflect<BillChangeLogModel>(TMSWriteConnection, sql, arguments);
            if (result != null)
            {
                var lookupResult = 
                                   from p in result
                                   where long.Parse(p.FormCode) % argument.Mod == argument.Remainder
                                   select p;
                if (lookupResult != null)
                {
                    return lookupResult.ToList();
                }
            }
            return null;
        }
        /// <summary>
        /// 更新tms中间表同步状态
        /// </summary>
        /// <param name="bcid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateSyncStatus(String bcid, Enums.SyncStatus status)
        {
            if (String.IsNullOrWhiteSpace(bcid)) throw new ArgumentNullException("bcid is null or empty.");
            String sql = @"
UPDATE SC_BillChangeLog
SET SyncFlag = :SyncFlag, SyncTime = SYSDATE
WHERE BCID = :BCID
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="BCID" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = bcid },
                new OracleParameter() { ParameterName="SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)status }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }
        #endregion
    }
}

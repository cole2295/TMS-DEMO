using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.DAL.Oracle.Log
{
    public class ServiceLogDAL : BaseDAL, IServiceLogDAL
    {
        #region IServiceLogDAL 成员

        /// <summary>
        /// 批量写日志
        /// </summary>
        /// <param name="listLog"></param>
        /// <returns></returns>
        public int BatchWriteLog(List<ServiceLogModel> listLog)
        {
            if (listLog != null && listLog.Count > 0)
            {
                string sql = string.Format(@"
INSERT INTO SC_ServiceLog (  LogID, FormCode, IsSuccessed,  Note,  LogType, OpFunction,  SyncKey, IsHandled ) 
VALUES (  {0}, :FormCode,  :IsSuccessed, :Note,  :LogType, :OpFunction, :SyncKey, 0 )
", listLog[0].SequenceNextValue());
                String[] arrFormCode = new String[listLog.Count];
                int[] arrIsSuccessed = new int[listLog.Count];
                String[] arrNote = new String[listLog.Count];
                int[] arrLogType = new int[listLog.Count];
                int[] arrOpFunction = new int[listLog.Count];
                String[] arrSyncKey = new String[listLog.Count];
                for (int i = 0; i < listLog.Count; i++)
                {
                    arrFormCode[i] = listLog[i].FormCode;
                    arrIsSuccessed[i] = Convert.ToInt32(listLog[i].IsSuccessed);
                    arrNote[i] = listLog[i].Note;
                    arrLogType[i] = (int)listLog[i].LogType;
                    arrOpFunction[i] = listLog[i].OpFunction;
                    arrSyncKey[i] = listLog[i].SyncKey;
                }
                OracleParameter[] arguments = new OracleParameter[] 
                {
                    new OracleParameter() { ParameterName="FormCode",  OracleDbType = OracleDbType.Varchar2  , Size = 50 , Value = arrFormCode },
                    new OracleParameter() { ParameterName="IsSuccessed", OracleDbType = OracleDbType.Int16 , Value = arrIsSuccessed },
                    new OracleParameter() { ParameterName="Note", OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = arrNote },
                    new OracleParameter() { ParameterName="LogType", OracleDbType = OracleDbType.Int16 ,Value = arrLogType },
                    new OracleParameter() { ParameterName="OpFunction", OracleDbType = OracleDbType.Int32 ,Value = arrOpFunction    },
                    new OracleParameter() { ParameterName="SyncKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 ,Value = arrSyncKey }
                };
                return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listLog.Count, arguments);
            }
            return 0;
        }

        public int WriteLog(ServiceLogModel log)
        {
            if (log != null)
            {
                string sql = string.Format(@"
INSERT INTO SC_ServiceLog (  LogID, FormCode, IsSuccessed,  Note,  LogType, OpFunction,  SyncKey, IsHandled ) 
VALUES (  {0}, :FormCode,  :IsSuccessed, :Note,  :LogType, :OpFunction, :SyncKey, 0 )
", log.SequenceNextValue());
                OracleParameter[] arguments = new OracleParameter[] 
                {
                    new OracleParameter() { ParameterName="FormCode",  OracleDbType = OracleDbType.Varchar2  , Size = 50 , Value = log.FormCode},
                    new OracleParameter() { ParameterName="IsSuccessed", OracleDbType = OracleDbType.Int16 , Value = Convert.ToInt16(log.IsSuccessed) },
                    new OracleParameter() { ParameterName="Note", OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = log.Note},
                    new OracleParameter() { ParameterName="LogType", OracleDbType = OracleDbType.Int16 ,Value = (int)log.LogType },
                    new OracleParameter() { ParameterName="OpFunction", OracleDbType = OracleDbType.Int32 ,Value = log.OpFunction},
                    new OracleParameter() { ParameterName="SyncKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 ,Value = log.SyncKey}
                };
                return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
            }
            return 0;
        }

        public PagedList<ServiceLogModel> ReadLogs(ServiceLogSearchModel conditions)
        {
            if (conditions != null)
            {
                StringBuilder sb = new StringBuilder(@"
SELECT LogID,FormCode,IsSuccessed,Note,CreateTime,LogType,OpFunction,SyncKey,IsHandled,ProcessingTime 
FROM SC_ServiceLog 
WHERE CreateTime > :CreateTime
");
                List<OracleParameter> arguments = new List<OracleParameter>();
                arguments.Add(new OracleParameter() { ParameterName = "CreateTime", OracleDbType = OracleDbType.Date, Value = conditions.CreateTime });
                if (!string.IsNullOrWhiteSpace(conditions.FormCode))
                {
                    sb.Append(" AND FormCode=:FormCode");
                    arguments.Add(new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2, Size = 50 , Value = conditions.FormCode });
                }
                if (conditions.IsSuccessed.HasValue)
                {
                    sb.Append(" AND IsSuccessed=:IsSuccessed");
                    arguments.Add(new OracleParameter() { ParameterName = "IsSuccessed", OracleDbType = OracleDbType.Int16 , Value = Convert.ToInt16(conditions.IsSuccessed.Value) });
                }
                if (conditions.OpFunction.HasValue)
                {
                    sb.Append(" AND OpFunction=:OpFunction");
                    arguments.Add(new OracleParameter() { ParameterName = "OpFunction", OracleDbType = OracleDbType.Int32, Value = conditions.OpFunction.Value });
                }
                if (conditions.LogType.HasValue)
                {
                    sb.Append(" AND LogType=:LogType");
                    arguments.Add(new OracleParameter() { ParameterName = "LogType", OracleDbType = OracleDbType.Int32, Value = (int)conditions.LogType.Value });
                }
                if (conditions.IsHandeld.HasValue)
                {
                    sb.Append(" AND IsHandled=:IsHandled");
                    arguments.Add(new OracleParameter() { ParameterName = "IsHandled", OracleDbType = OracleDbType.Int16 , Value = Convert.ToInt16(conditions.IsHandeld.Value) });
                }
                sb.Append(" ORDER BY CreateTime desc");

                return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ServiceLogModel>(TMSReadOnlyConnection, sb.ToString(), conditions, arguments.ToArray());
            }
            else
                return null;
        }

        public int SetLogStatus(long logID, Enums.ServiceLogProcessingStatus status)
        {
            string sql = @"
UPDATE SC_ServiceLog
SET IsHandled = :IsHandled,
        ProcessingTime = SYSDATE
WHERE LogID = :LogID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IsHandled", OracleDbType = OracleDbType.Int16 , Value = (int)status },
                new OracleParameter() { ParameterName="LogID", OracleDbType = OracleDbType.Int64, Value = logID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

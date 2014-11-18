using System;
using System.Collections.Generic;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.BaseInfo.Line;
using System.Linq;

namespace Vancl.TMS.DAL.Oracle.Log
{
    public class OperateLogDAL : BaseDAL, ILogDAL<OperateLogModel>
    {
        /// <summary>
        /// 通用操作日志
        /// </summary>
        private readonly String _commlogSql = @"
SELECT OperateType
,Note
,CASE WHEN  OP.OPERATEBY <= 0 THEN '系统管理员' ELSE E.EMPLOYEENAME END AS Operator
,OperateTime
FROM TMS_OPERATELOG OP
LEFT JOIN EMPLOYEE E ON OP.OPERATEBY = E.EMPLOYEEID
WHERE KeyValue = :KeyValue  AND TableName IN ({0})
ORDER BY OPERATETIME ASC
";
        /// <summary>
        /// 线路操作日志
        /// </summary>
        private readonly String _linelogSql = @"
SELECT *
FROM
(
        SELECT OperateType,Note
        ,CASE WHEN  OP.OPERATEBY <= 0 THEN '系统管理员' ELSE E.EMPLOYEENAME END AS Operator
        ,OperateTime
        FROM TMS_OPERATELOG OP
        LEFT JOIN EMPLOYEE E ON OP.OPERATEBY = E.EMPLOYEEID
        WHERE KeyValue =  :KeyValue  AND TableName IN ({0})
        UNION ALL
        SELECT OperateType,Note
        ,CASE WHEN  OP.OPERATEBY <= 0 THEN '系统管理员' ELSE E.EMPLOYEENAME END AS Operator
        ,OperateTime
        FROM TMS_OPERATELOG OP
        LEFT JOIN EMPLOYEE E ON OP.OPERATEBY = E.EMPLOYEEID
        WHERE KeyValue =  :LineID     AND TableName IN ({0})
        AND OperateType = 4
)
ORDER BY OPERATETIME ASC
";


        #region ILogDAL<OperateLogModel> 成员

        public int Write(OperateLogModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_OperateLog(
                    OLID,
                    OperateType,
                    TableName,
                    KeyValue,
                    Note,
                    OperateBy)
                VALUES(
                    {0},
                    :OperateType,
                    :TableName,
                    :KeyValue,
                    :Note,
                    :OperateBy)
            ", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="OperateType",DbType= DbType.Int32,Value=(int)model.OperateType},
                new OracleParameter() { ParameterName="TableName",DbType= DbType.String,Value=model.TableName},
                new OracleParameter() { ParameterName="KeyValue",DbType= DbType.String,Value=model.KeyValue},
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value=model.Note},
                new OracleParameter() { ParameterName="OperateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        /// <summary>
        /// 取得模块相关的表
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        private String GetModuleTableName(Vancl.TMS.Model.Common.Enums.SysModule module)
        {
            List<String> listTableName = new List<String>();
            switch (module)
            {
                case Enums.SysModule.CarrierModule:
                    listTableName.AddRange(new String[] 
                    {
                        "TMS_Carrier","TMS_Coverage","TMS_DelayCriteria"
                    });
                    break;
                case Enums.SysModule.LineModule:
                    listTableName.AddRange(new String[] 
                    {
                        "TMS_LinePlan","TMS_LineFixedPrice","TMS_LineFormulaPrice","TMS_LineLadderPrice"
                    });
                    break;
                case Enums.SysModule.TransPortPlanModule:
                    listTableName.AddRange(new String[] 
                    {
                       "TMS_TransportPlan","TMS_TransportPlanDetail"
                    });
                    break;
                default:
                    throw new NoNullAllowedException("SysModule is error");
            }
            return "'" + String.Join("','", listTableName) + "'";
        }

        /// <summary>
        /// 取得操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<OperateLogModel> Read(BaseLogSearchModel model)
        {
            if (model == null) throw new ArgumentNullException("OperateLogDAL.Read.BaseLogSearchModel is null");
            if (model is BaseOperateLogSearchModel)
            {
                String sql = _commlogSql;
               List<OracleParameter> arguments = new List<OracleParameter>();
                arguments.Add(new OracleParameter()
                { 
                    ParameterName="KeyValue", 
                    DbType= DbType.String, 
                    Value = (model as BaseOperateLogSearchModel).KeyValue
                });
                if (model is LineOperateLogSearchModel)
                {
                    sql = _linelogSql;
                    arguments.Add(new OracleParameter()
                    {
                        ParameterName = "LineID",
                        DbType = DbType.String,
                        Value = (model as LineOperateLogSearchModel).LineID
                    });
                }
                sql = String.Format(sql, GetModuleTableName((model as BaseOperateLogSearchModel).Module));
                IList<OperateLogModel> listlog = ExecuteSql_ByDataTableReflect<OperateLogModel>(TMSReadOnlyConnection, sql, arguments.ToArray());
                if (listlog != null && listlog.Count > 0)
                {
                    return listlog.ToList();
                }
            }

            return null;
        }

        #endregion
    }
}

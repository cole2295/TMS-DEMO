using System;
using System.Collections.Generic;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Log
{
    public class DeliveryFlowLogDAL : BaseDAL, ILogDAL<DeliveryFlowLogModel>
    {
        #region ILogDAL<DeliveryFlowLogModel> 成员

        public int Write(DeliveryFlowLogModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_DeliveryFlowLog(
                    DFLID,
                    DeliveryNo,
                    FlowType,
                    IsShow,
                    Note,
                    OperateBy)
                VALUES(
                    {0},
                    :DeliveryNo,
                    :FlowType,
                    :IsShow,
                    :Note,
                    :OperateBy)
            ", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNO},
                new OracleParameter() { ParameterName="FlowType",DbType= DbType.Int32,Value=(int)model.FlowType},
                new OracleParameter() { ParameterName="IsShow",DbType= DbType.Byte,Value=model.IsShow},
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value=model.Note},
                new OracleParameter() { ParameterName="OperateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public List<DeliveryFlowLogModel> Read(BaseLogSearchModel model)
        {
            string strSql = @"
                SELECT 
                    dfl.DeliveryNo,
                    dfl.FlowType,
                    dfl.Note,
                    dfl.OperateTime,
                    d.deliveryStatus,
                    ec.CompanyName AS OperateDeptName,
                    d.DistributionName AS OperateCompanyName,
                    e.employeename AS Operator 
                    FROM TMS_DeliveryFlowLog dfl 
                    JOIN employee e ON e.employeeid = dfl.OperateBy 
                    JOIN tms_dispatch d ON d.deliveryNo=dfl.deliveryNo 
                    JOIN ExpressCompany ec ON ec.ExpressCompanyID=e.StationID 
                    JOIN Distribution d ON d.DistributionCode=e.DistributionCode
                    WHERE 
                    dfl.DeliveryNo=:DeliveryNo ORDER BY dfl.OperateTime ASC";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo}
            };
            return ExecuteSql_ByReaderReflect<DeliveryFlowLogModel>(TMSWriteConnection, strSql, arguments) as List<DeliveryFlowLogModel>;
        }

        #endregion
    }
}

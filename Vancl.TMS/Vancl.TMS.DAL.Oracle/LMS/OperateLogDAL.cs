using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class OperateLogDAL : LMSBaseDAL, IOperateLogDAL
    {
        #region IOperateLogDAL 成员
        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行数</returns>
        public int Add(OperateLogEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("OperateLogEntityModel is null");
            String sql = String.Format(@"
INSERT INTO OperateLog(OperateLogID, WaybillNO,LogType,Operation, LogOperator ,OperatorStation ,OperateTime,Reasult ,Status, OldDeliverMan,isSyn, OperateLogKid)
VALUES({0} , :WaybillNO,:LogType,:Operation,:LogOperator,:OperatorStation,SYSDATE ,:Result,:Status,:OldDeliverMan,:isSyn,:OperateLogKid)
",
    model.SequenceNextValue()
 );
            OracleParameter[] parameters = new OracleParameter[] 
            {
					new OracleParameter() { ParameterName="WaybillNO",  OracleDbType = OracleDbType.Int64 , Value = model.WaybillNO },
					new OracleParameter() { ParameterName="LogType" ,  OracleDbType = OracleDbType.Int32 , Value = model.LogType },
					new OracleParameter() { ParameterName = "Operation", OracleDbType = OracleDbType.Varchar2 , Size = 200 , Value = model.Operation },
					new OracleParameter() { ParameterName="LogOperator", OracleDbType = OracleDbType.Varchar2 , Size = 40 , Value = model.LogOperator },
					new OracleParameter() { ParameterName="OperatorStation",  OracleDbType = OracleDbType.Int32 , Value = model.OperatorStation },
					new OracleParameter() { ParameterName = "Result", OracleDbType = OracleDbType.Varchar2 , Size = 800 , Value = model.Result },
					new OracleParameter() { ParameterName = "Status",  OracleDbType = OracleDbType.Int32 , Value = (int)model.Status },
                    new OracleParameter() { ParameterName = "OldDeliverMan", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.OldDeliverMan },
                    new OracleParameter() { ParameterName = "isSyn",  OracleDbType = OracleDbType.Int32 , Value = model.IsSyn },
                    new OracleParameter() { ParameterName="OperateLogKid" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.OperateLogKid }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, parameters);
        }

        #endregion
    }
}

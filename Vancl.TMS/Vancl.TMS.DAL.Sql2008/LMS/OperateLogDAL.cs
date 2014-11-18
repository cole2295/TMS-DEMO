using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;
using System.Data;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class OperateLogDAL : BaseDAL, IOperateLogDAL
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
            String sql = @"
INSERT INTO OperateLog(WaybillNO,LogType,Operation,Operator,OperatorStation,OperateTime,Reasult,Status,OldDeliverMan,isSyn,OperateLogKid)
VALUES(@WaybillNO,@LogType,@Operation,@LogOperator,@OperatorStation,GETDATE(),@Result,@Status,@OldDeliverMan,@isSyn,@OperateLogKid)
";
            SqlParameter[] parameters = new SqlParameter[] 
            {
					new SqlParameter("@WaybillNO", SqlDbType.BigInt,8) { Value = model.WaybillNO },
					new SqlParameter("@LogType", SqlDbType.Int,4) { Value = model.LogType },
					new SqlParameter("@Operation", SqlDbType.NVarChar,100) { Value = model.Operation },
					new SqlParameter("@LogOperator", SqlDbType.NVarChar,20) { Value = model.LogOperator },
					new SqlParameter("@OperatorStation", SqlDbType.Int,4) { Value = model.OperatorStation },
					new SqlParameter("@Result", SqlDbType.NVarChar,400) { Value = model.Result },
					new SqlParameter("@Status", SqlDbType.Int,4) { Value = (int)model.Status },
                    new SqlParameter("@OldDeliverMan",SqlDbType.VarChar,20) { Value = model.OldDeliverMan },
                    new SqlParameter("@isSyn", SqlDbType.Bit) { Value = model.IsSyn },
                    new SqlParameter("@OperateLogKid", SqlDbType.VarChar,20) { Value = model.OperateLogKid }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, parameters);
        }

        #endregion
    }
}

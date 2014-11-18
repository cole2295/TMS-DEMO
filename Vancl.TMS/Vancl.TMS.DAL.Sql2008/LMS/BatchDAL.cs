using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class BatchDAL : BaseDAL, IBatchDAL
    {
        #region IBatchDAL 成员

        public int Add(Model.LMS.BatchEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("BatchEntityModel is null");
            String sql = @"
MERGE INTO Batch AS des
USING 
(
    SELECT @BatchNO as BatchNO, @Operator as Operator, @OperTime as OperTime ,@OperStation as OperStation
    , @ReceiveStation as ReceiveStation, @CreatBy as  CreatBy, @CreatStation  as CreatStation
    , @CreatTime as CreatTime,@UpdateBy as  UpdateBy , @UpdateStation as  UpdateStation, @UpdateTime as UpdateTime
) AS src
 ON des.BatchNO = src.BatchNO
WHEN MATCHED THEN 
    UPDATE SET des.UpdateBy = src.UpdateBy , des.UpdateStation = src.UpdateStation , des.UpdateTime = src.UpdateTime
WHEN NOT MATCHED THEN
INSERT ( BatchNO, Operator, OperTime ,OperStation, ReceiveStation, CreatBy ,CreatStation ,CreatTime ,UpdateBy ,UpdateStation ,UpdateTime ,IsDelete  )
VALUES ( src.BatchNO, src.Operator, src.OperTime ,src.OperStation, src.ReceiveStation, src.CreatBy ,src.CreatStation ,src.CreatTime ,src.UpdateBy ,src.UpdateStation ,src.UpdateTime , 0);
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@BatchNO",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 20,  Value = model.BatchNo },
                new SqlParameter() { ParameterName = "@Operator",   SqlDbType = System.Data.SqlDbType.Int , Value = model.BatchOperator },
                new SqlParameter() { ParameterName = "@OperTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = model.OperTime },
                new SqlParameter() { ParameterName = "@OperStation", SqlDbType = System.Data.SqlDbType.Int , Value = model.OperStation },
                new SqlParameter() { ParameterName = "@ReceiveStation", SqlDbType = System.Data.SqlDbType.Int , Value = model.ReceiveStation },
                new SqlParameter() { ParameterName = "@CreatBy", SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter() { ParameterName = "@CreatStation", SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateDept },
                new SqlParameter() { ParameterName = "@CreatTime", SqlDbType = System.Data.SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter() { ParameterName = "@UpdateBy", SqlDbType = System.Data.SqlDbType.Int , Value = model.UpdateBy },
                new SqlParameter() { ParameterName = "@UpdateStation", SqlDbType = System.Data.SqlDbType.Int , Value = model.UpdateDept },
                new SqlParameter() { ParameterName = "@UpdateTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = model.UpdateTime }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

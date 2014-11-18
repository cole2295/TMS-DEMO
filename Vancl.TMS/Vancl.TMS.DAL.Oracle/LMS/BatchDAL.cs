using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class BatchDAL : BaseDAL, IBatchDAL
    {
        #region IBatchDAL 成员

        public int Add(Model.LMS.BatchEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("BatchEntityModel is null");
            String sql = String.Format(@"
MERGE INTO Batch  des
USING 
(
    SELECT :BatchNO as BatchNO, :BatchOperator as BatchOperator, :OperTime as OperTime ,:OperStation as OperStation
    , :ReceiveStation as ReceiveStation, :CreatBy as  CreatBy, :CreatStation  as CreatStation
    , :CreatTime as CreatTime,:UpdateBy as  UpdateBy , :UpdateStation as  UpdateStation, :UpdateTime as UpdateTime
   FROM dual
)  src
ON ( des.BatchNO = src.BatchNO )
WHEN MATCHED THEN 
    UPDATE SET des.UpdateBy = src.UpdateBy  , des.UpdateStation = src.UpdateStation  , des.UpdateTime = src.UpdateTime 
WHEN NOT MATCHED THEN
INSERT (BatchID, BatchNO, BatchOperator, OperTime ,OperStation, ReceiveStation, CreatBy ,CreatStation ,CreatTime ,UpdateBy ,UpdateStation ,UpdateTime ,IsDelete )
VALUES({0}, src.BatchNO, src.BatchOperator, src.OperTime ,src.OperStation, src.ReceiveStation, src.CreatBy ,src.CreatStation ,src.CreatTime ,src.UpdateBy ,src.UpdateStation ,src.UpdateTime ,0  )
", model.SequenceNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BatchNO", OracleDbType = OracleDbType.Varchar2, Size = 20 , Value = model.BatchNo },
                new OracleParameter() { ParameterName = "BatchOperator", OracleDbType = OracleDbType.Int32 , Value = model.BatchOperator },
                new OracleParameter() { ParameterName = "OperTime",   OracleDbType = OracleDbType.Date , Value = model.OperTime },
                new OracleParameter() { ParameterName = "OperStation",  OracleDbType = OracleDbType.Int32 , Value = model.OperStation },
                new OracleParameter() { ParameterName = "ReceiveStation", OracleDbType = OracleDbType.Int32 , Value = model.ReceiveStation },
                new OracleParameter() { ParameterName = "CreatBy",  OracleDbType = OracleDbType.Int32 , Value = model.CreateBy },
                new OracleParameter() { ParameterName = "CreatStation", OracleDbType = OracleDbType.Int32 , Value = model.CreateDept },
                new OracleParameter() { ParameterName = "CreatTime",  OracleDbType = OracleDbType.Date , Value = model.CreateTime },
                new OracleParameter() { ParameterName = "UpdateBy",  OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy },
                new OracleParameter() { ParameterName = "UpdateStation", OracleDbType = OracleDbType.Int32 , Value = model.UpdateDept },
                new OracleParameter() { ParameterName = "UpdateTime",  OracleDbType = OracleDbType.Date , Value = model.UpdateTime }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }

        #endregion
    }
}

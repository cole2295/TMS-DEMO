using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class WaybillReturnDetailInfoDAL :  BaseDAL,IWaybillReturnDetailInfoDAL
    {
        public long Add(Model.LMS.WaybillReturnDetailInfoEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("WaybillReturnDetailInfoEntityModel is null");
            String sql = @"
MERGE INTO WaybillReturnDetailInfo AS des
USING 
(  
   SELECT @WaybillNo as WaybillNo,@BoxNo as BoxNo,@CreateBy as CreateBy,@CreateTime as CreateTime,
   @CreateDep as CreateDep,@ReturnTo as ReturnTo,@OrderNo as OrderNo,@Weight as Weight
) AS src
ON des.WaybillNo = src.WaybillNo and des.ReturnTo=src.ReturnTo
WHEN MATCHED THEN 
UPDATE SET des.BoxNo=src.BoxNo,des.CreateDep=src.CreateDep,des.ReturnTo=src.ReturnTo,des.Weight=src.Weight
WHEN NOT MATCHED THEN
INSERT(WaybillNo,BoxNo,CreateBy,CreateTime,CreateDep,IsDelete,ReturnTo,OrderNo,Weight)
VALUES(src.WaybillNo,src.BoxNo,src.CreateBy,src.CreateTime,src.CreateDep,0,src.ReturnTo,src.OrderNo,src.Weight);";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@WaybillNo", SqlDbType = System.Data.SqlDbType.BigInt , Value = model.WaybillNo },
                new SqlParameter() { ParameterName = "@BoxNo", SqlDbType = System.Data.SqlDbType.NVarChar , Size = 30 , Value = model.BoxNo },
                new SqlParameter() { ParameterName = "@CreateBy",  SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter() { ParameterName = "@CreateTime", SqlDbType = System.Data.SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter() { ParameterName = "@CreateDep",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.CreateDep },
                new SqlParameter() { ParameterName = "@ReturnTo",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.ReturnTo },
                new SqlParameter() { ParameterName = "@OrderNo",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.OrderNo },
                new SqlParameter() { ParameterName = "@Weight",  SqlDbType = System.Data.SqlDbType.Decimal , Value = model.Weight }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);

        }
    }
}

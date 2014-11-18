using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using System.Data.SqlClient;
using System.Data;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class WaybillThirdPartyInfoDAL :BaseDAL, IWaybillThirdPartyInfoDAL
    {
        public int SaveWeight(string formCode, decimal weight)
        {
            long WayBillNo = long.Parse(formCode);
            string strsql =@"
UPDATE WaybillInfo 
SET WayBillInfoWeight = @Weight
WHERE WaybillNO = @WayBillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName= "@Weight" ,  SqlDbType = SqlDbType.Decimal , Value = weight },
                new SqlParameter() { ParameterName = "@WayBillNo",  SqlDbType = SqlDbType.BigInt , Value = WayBillNo },                
            };
            return ExecuteNonQuery(LMSWriteConnection, strsql, arguments);
        }


        public Model.LMS.WaybillThirdPartyInfoEntityModel GetPackageModel(string formCode, int packageIndex)
        {
            long WayBillNo = long.Parse(formCode);
            string strsql =@"
SELECT WaybillThirdPartyInfoId,WaybillNo,BoxNo,BoxType,Weight,Volume,IsDelete ,CreateBy,CreateTime,UpdateBy,UpdateTime
FROM WaybillThirdPartyInfo (NOLOCK)
WHERE WaybillNO = @WayBillNo and BoxNo = @BoxNo
 ";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName= "@WayBillNo" ,  SqlDbType = SqlDbType.BigInt , Value = WayBillNo },
                new SqlParameter() { ParameterName = "@BoxNo",  SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = packageIndex.ToString() },                
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillThirdPartyInfoEntityModel>(LMSWriteConnection, strsql, arguments);
        }

        public int AddBillPackage(Model.LMS.WaybillThirdPartyInfoEntityModel model)
        {
            string strsql =@"
INSERT INTO [WaybillThirdPartyInfo] ([WaybillNo] ,[BoxNo]  ,[BoxType]  ,[Weight]  ,[Volume]  ,[IsDelete]  ,[CreateBy] ,[CreateTime]  ,[UpdateBy]  ,[UpdateTime])
VALUES (@WaybillNo ,@BoxNo   ,@BoxType  ,@Weight  ,@Volume ,@IsDelete  ,@CreateBy  ,@CreateTime  ,@UpdateBy  ,@UpdateTime )
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@WayBillNo",  SqlDbType = SqlDbType.BigInt , Value = model.WaybillNo },   
                new SqlParameter() { ParameterName = "@BoxNo", SqlDbType = SqlDbType.NVarChar , Size = 50  , Value = model.BoxNo },   
                new SqlParameter() { ParameterName = "@BoxType", SqlDbType = SqlDbType.NVarChar , Size = 50  , Value = model.BoxType },   
                new SqlParameter() { ParameterName = "@Weight",  SqlDbType = SqlDbType.Decimal , Value = model.Weight },   
                new SqlParameter() { ParameterName = "@Volume",  SqlDbType = SqlDbType.Decimal , Value = 0},   
                new SqlParameter() { ParameterName = "@IsDelete",  SqlDbType = SqlDbType.Bit , Value = model.IsDelete },   
                new SqlParameter() { ParameterName = "@CreateBy",  SqlDbType = SqlDbType.Int , Value = model.CreateBy },   
                new SqlParameter() { ParameterName = "@CreateTime",  SqlDbType = SqlDbType.DateTime , Value = model.CreateTime },   
                new SqlParameter() { ParameterName = "@UpdateBy",  SqlDbType = SqlDbType.Int , Value = model.UpdateBy },   
                new SqlParameter() { ParameterName = "@UpdateTime",  SqlDbType = SqlDbType.DateTime , Value = model.UpdateTime },       
            };
            return ExecuteNonQuery(LMSWriteConnection, strsql, arguments);
        }

        public int UpdateBillPackage(Model.LMS.WaybillThirdPartyInfoEntityModel model)
        {
            string strsql =
               @"
UPDATE [WaybillThirdPartyInfo]
   SET [WaybillNo] = @WaybillNo
      ,[BoxNo] = @BoxNo
      ,[BoxType] = @BoxType
      ,[Weight] = @Weight
      ,[Volume] = @Volume
      ,[IsDelete] = @IsDelete
      ,[CreateBy] = @CreateBy
      ,[CreateTime] = @CreateTime
      ,[UpdateBy] = @UpdateBy
      ,[UpdateTime] = @UpdateTime
 WHERE WaybillThirdPartyInfoId=@WaybillThirdPartyInfoId
           ";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@WayBillNo", SqlDbType = SqlDbType.BigInt , Value = model.WaybillNo },   
                new SqlParameter() { ParameterName = "@BoxNo", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.BoxNo },   
                new SqlParameter() { ParameterName = "@BoxType", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.BoxType },   
                new SqlParameter() { ParameterName = "@Weight", SqlDbType = SqlDbType.Decimal , Value = model.Weight },   
                new SqlParameter() { ParameterName = "@Volume",  SqlDbType = SqlDbType.Decimal , Value = 0 },   
                new SqlParameter() { ParameterName = "@IsDelete",  SqlDbType = SqlDbType.Bit , Value = model.IsDelete },   
                new SqlParameter() { ParameterName = "@CreateBy", SqlDbType = SqlDbType.Int , Value = model.CreateBy },   
                new SqlParameter() { ParameterName = "@CreateTime", SqlDbType = SqlDbType.DateTime , Value = model.CreateTime==DateTime.MinValue?DateTime.Now:model.CreateTime },   
                new SqlParameter() { ParameterName = "@UpdateBy",  SqlDbType = SqlDbType.Int  , Value = model.UpdateBy },   
                new SqlParameter() { ParameterName = "@UpdateTime", SqlDbType = SqlDbType.DateTime , Value = model.UpdateTime==DateTime.MinValue?DateTime.Now:model.UpdateTime},       
                new SqlParameter() { ParameterName = "@WaybillThirdPartyInfoId",  SqlDbType = SqlDbType.BigInt , Value = model.WaybillThirdPartyInfoId },       
            };
            return ExecuteNonQuery(LMSWriteConnection, strsql, arguments);
        }
    }
}

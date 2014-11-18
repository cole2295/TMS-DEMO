using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WaybillThirdPartyInfoDAL : LMSBaseDAL,IWaybillThirdPartyInfoDAL
    {
        public int SaveWeight(string formCode, decimal weight)
        {
            long WayBillNo = long.Parse(formCode);
            string strsql =
                @"
                update WaybillInfo 
                set WayBillInfoWeight = :Weight
                where WaybillNO = :WayBillNo
                ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "Weight" , OracleDbType = OracleDbType.Decimal , Value = weight},
                new OracleParameter() { ParameterName = "WayBillNo", OracleDbType = OracleDbType.Int64 , Value = WayBillNo },                
            };

            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strsql, arguments);
        }


        public Model.LMS.WaybillThirdPartyInfoEntityModel GetPackageModel(string formCode, int packageIndex)
        {
            long WayBillNo = long.Parse(formCode);
            string strsql =
                @"
                select WaybillThirdPartyInfoId
                    ,WaybillNo,BoxNo,BoxType,Weight,'Volume',IsDelete
                    ,CreateBy,CreateTime,UpdateBy,UpdateTime
                from WaybillThirdPartyInfo
                where WaybillNO = :WayBillNo and BoxNo = :BoxNo
                ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "WayBillNo" , OracleDbType = OracleDbType.Int64 , Value = WayBillNo},
                new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 100, Value = packageIndex.ToString() },                
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillThirdPartyInfoEntityModel>(LMSOracleWriteConnection, strsql, arguments);
        }

        public int AddBillPackage(Model.LMS.WaybillThirdPartyInfoEntityModel model)
        {
            string strsql =
                @"
                INSERT INTO WaybillThirdPartyInfo
                    (WaybillThirdPartyInfoId
                    ,WaybillNo
                    ,BoxNo
                    ,BoxType
                    ,Weight
                    ,BoxVolume
                    ,IsDelete
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime)
                VALUES
                    (
                    SEQ_WaybillThirdPartyInfo.nextval
                    ,:WaybillNo
                    ,:BoxNo
                    ,:BoxType
                    ,:Weight
                    ,:BoxVolume
                    ,:IsDelete
                    ,:CreateBy
                    ,:CreateTime
                    ,:UpdateBy
                    ,:UpdateTime)
           ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "WayBillNo", OracleDbType = OracleDbType.Int64 , Value = model.WaybillNo },   
                new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 100, Value = model.BoxNo },   
                new OracleParameter() { ParameterName = "BoxType", OracleDbType = OracleDbType.Varchar2 , Size = 100, Value = model.BoxType },   
                new OracleParameter() { ParameterName = "Weight", OracleDbType = OracleDbType.Decimal , Value = model.Weight },   
                new OracleParameter() { ParameterName = "BoxVolume", OracleDbType = OracleDbType.Decimal , Value = 0 },   
                new OracleParameter() { ParameterName = "IsDelete", OracleDbType = OracleDbType.Int16 , Value = Convert.ToInt16(model.IsDelete) },   
                new OracleParameter() { ParameterName = "CreateBy", OracleDbType = OracleDbType.Int32 , Value = model.CreateBy },   
                new OracleParameter() { ParameterName = "CreateTime", OracleDbType = OracleDbType.Date , Value = model.CreateTime },   
                new OracleParameter() { ParameterName = "UpdateBy", OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy },   
                new OracleParameter() { ParameterName = "UpdateTime", OracleDbType = OracleDbType.Date , Value = model.UpdateTime },       
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strsql, arguments);
        }

        public int UpdateBillPackage(Model.LMS.WaybillThirdPartyInfoEntityModel model)
        {
            string strsql =
               @"
UPDATE WaybillThirdPartyInfo
   SET WaybillNo = :WaybillNo
      ,BoxNo = :BoxNo
      ,BoxType = :BoxType
      ,Weight = :Weight
      ,BoxVolume = :BoxVolume
      ,IsDelete = :IsDelete
      ,CreateBy = :CreateBy
      ,CreateTime = :CreateTime
      ,UpdateBy = :UpdateBy
      ,UpdateTime = :UpdateTime
 WHERE WaybillThirdPartyInfoId=:WaybillThirdPartyInfoId
           ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "WayBillNo", OracleDbType = OracleDbType.Int64 , Value = model.WaybillNo },   
                new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 100, Value = model.BoxNo },   
                new OracleParameter() { ParameterName = "BoxType", OracleDbType = OracleDbType.Varchar2 , Size = 100, Value = model.BoxType },   
                new OracleParameter() { ParameterName = "Weight", OracleDbType = OracleDbType.Decimal , Value = model.Weight },   
                new OracleParameter() { ParameterName = "BoxVolume", OracleDbType = OracleDbType.Decimal , Value =0},   
                new OracleParameter() { ParameterName = "IsDelete", OracleDbType = OracleDbType.Int16 , Value =Convert.ToInt16(model.IsDelete) },   
                new OracleParameter() { ParameterName = "CreateBy", OracleDbType = OracleDbType.Int32 , Value = model.CreateBy },   
                new OracleParameter() { ParameterName = "CreateTime", OracleDbType = OracleDbType.Date , Value = model.CreateTime },   
                new OracleParameter() { ParameterName = "UpdateBy", OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy },   
                new OracleParameter() { ParameterName = "UpdateTime", OracleDbType = OracleDbType.Date , Value = model.UpdateTime },       
                new OracleParameter() { ParameterName = "WaybillThirdPartyInfoId", OracleDbType = OracleDbType.Int64 , Value = model.WaybillThirdPartyInfoId },       
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strsql, arguments);
        }

    }
}

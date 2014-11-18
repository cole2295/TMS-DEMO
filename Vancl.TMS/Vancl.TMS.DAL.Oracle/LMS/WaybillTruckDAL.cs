using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WaybillTruckDAL : LMSBaseDAL, IWaybillTruckDAL
    {
        public int Add(WaybillTruckEntityModel model)
        {
            var strSql = String.Format(@"
insert into WaybillTruck(WaybillNOGpsID,WaybillNO,BatchNo,TruckNO,GpsID,CreateTime,CreateBy,UpdateTime,UpdateBy,IsDelete,DriverID)
values  ({0},:WaybillNO,:BatchNo,:TruckNO,:GpsID,:CreateTime,:CreateBy,:UpdateTime,:UpdateBy,:IsDelete,:DriverID)
", model.SequenceNextValue()
 );
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName= "WaybillNO",OracleDbType= OracleDbType.Int64, Value = model.WaybillNO },
                new OracleParameter(){ ParameterName= "BatchNo",OracleDbType= OracleDbType.Int64, Value = model.BatchNO },
                new OracleParameter(){ ParameterName= "TruckNO",OracleDbType= OracleDbType.Varchar2, Size = 40, Value = model.TruckNO },
                new OracleParameter(){ ParameterName= "GpsID",OracleDbType= OracleDbType.Varchar2, Size = 100, Value = model.GpsID },
                new OracleParameter(){ ParameterName= "CreateTime",OracleDbType= OracleDbType.Date, Value = model.CreateTime },
                new OracleParameter(){ ParameterName= "CreateBy",OracleDbType= OracleDbType.Int32, Value = model.CreateBy },
                new OracleParameter(){ ParameterName= "UpdateTime",OracleDbType= OracleDbType.Date, Value = model.UpdateTime },
                new OracleParameter(){ ParameterName= "UpdateBy",OracleDbType= OracleDbType.Int32, Value = model.UpdateBy },
                new OracleParameter(){ ParameterName= "IsDelete",OracleDbType= OracleDbType.Int16, Value = (model.IsDelete ? 1:0) },
                new OracleParameter(){ ParameterName= "DriverID",OracleDbType= OracleDbType.Int32, Value = model.DriverId }
             };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 订单下车
        /// </summary>
        /// <param name="model"></param>
        public int RemoveBillTruck(WaybillTruckEntityModel model)
        {
            string strSQL = @"
UPDATE WaybillTruck
    SET   IsDelete = 1  
WHERE   WaybillNO = :WaybillNO 
    AND   BatchNo = :BatchNo 
    AND   TruckNo = :TruckNo
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName= "WaybillNO", OracleDbType= OracleDbType.Int64 , Value = model.WaybillNO },
                new OracleParameter(){ ParameterName= "BatchNo", OracleDbType= OracleDbType.Int64 , Value = model.BatchNO },
                new OracleParameter(){ ParameterName= "TruckNO", OracleDbType= OracleDbType.Varchar2, Size = 40, Value = model.TruckNO }
             };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strSQL, parameters);
        }

        #region IWaybillTruckDAL 成员


        public bool IsWaybillLoading(long waybillNo, string batchNo)
        {
            if (String.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException("batchNo is null or empty");
            String sql = @"
SELECT WaybillNO
FROM WaybillTruck
WHERE WaybillNO = :WaybillNO
    AND BatchNo = :BatchNo
    AND IsDelete = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "WaybillNO", OracleDbType = OracleDbType.Int64 , Value = waybillNo },
                new OracleParameter() { ParameterName = "BatchNo",  OracleDbType = OracleDbType.Int64 , Value = long.Parse(batchNo) }
            };
            object objValue = ExecuteSqlScalar(LMSOracleWriteConnection, sql, parameters);
            if (objValue != null && objValue != DBNull.Value)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

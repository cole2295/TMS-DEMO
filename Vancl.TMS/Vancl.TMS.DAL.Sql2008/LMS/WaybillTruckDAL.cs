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
    public class WaybillTruckDAL : BaseDAL, IWaybillTruckDAL
    {
        public int Add(WaybillTruckEntityModel model)
        {
            var strSql = @"
INSERT INTO WaybillTruck( WaybillNO ,BatchNo ,TruckNO ,GpsID ,CreateTime ,CreateBy ,UpdateTime ,UpdateBy ,IsDelete ,DriverID )
VALUES  (@WaybillNO ,@BatchNo ,@TruckNO ,@GpsID ,@CreateTime ,@CreateBy ,@UpdateTime ,@UpdateBy ,@IsDelete ,@DriverID)
";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter(){ ParameterName= "@WaybillNO", SqlDbType = SqlDbType.BigInt , Value = model.WaybillNO },
                new SqlParameter(){ ParameterName= "@BatchNo", SqlDbType = SqlDbType.BigInt , Value = model.BatchNO },
                new SqlParameter(){ ParameterName= "@TruckNO", SqlDbType = SqlDbType.NVarChar, Size = 20, Value = model.TruckNO },
                new SqlParameter(){ ParameterName= "@GpsID", SqlDbType = SqlDbType.NVarChar , Size = 50, Value = model.GpsID },
                new SqlParameter(){ ParameterName= "@CreateTime", SqlDbType = SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter(){ ParameterName= "@CreateBy", SqlDbType = SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter(){ ParameterName= "@UpdateTime", SqlDbType = SqlDbType.DateTime , Value = model.UpdateTime },
                new SqlParameter(){ ParameterName= "@UpdateBy", SqlDbType = SqlDbType.Int , Value = model.UpdateBy },
                new SqlParameter(){ ParameterName= "@IsDelete", SqlDbType = SqlDbType.Bit , Value = model.IsDelete },
                new SqlParameter(){ ParameterName= "@DriverID", SqlDbType = SqlDbType.Int , Value = model.DriverId }
             };
            return ExecuteNonQuery(LMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 订单下车
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int RemoveBillTruck(WaybillTruckEntityModel model)
        {
            string strSQL = @"
DELETE  FROM   WaybillTruck
WHERE   WaybillNO = @WaybillNO 
    AND   BatchNo = @BatchNo
    AND   TruckNo = @TruckNO
";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter(){ ParameterName= "@WaybillNO", SqlDbType = SqlDbType.BigInt , Value = model.WaybillNO },
                new SqlParameter(){ ParameterName= "@BatchNo", SqlDbType = SqlDbType.BigInt , Value = model.BatchNO },
                new SqlParameter(){ ParameterName= "@TruckNO", SqlDbType = SqlDbType.NVarChar, Size = 20, Value = model.TruckNO }
            };
            return ExecuteNonQuery(LMSWriteConnection, strSQL, parameters);
        }


        #region IWaybillTruckDAL 成员


        public bool IsWaybillLoading(long waybillNo, string batchNo)
        {
            if (String.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException("batchNo is null or empty");
            String sql = @"
SELECT TOP 1 WaybillNO
FROM WaybillTruck (NOLOCK)
WHERE WaybillNO = @WaybillNO
    AND BatchNo = @BatchNo
    AND IsDelete = 0
";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@WaybillNO", SqlDbType = SqlDbType.BigInt, Value = waybillNo },
                new SqlParameter() { ParameterName = "@BatchNo", SqlDbType = SqlDbType.BigInt , Value = long.Parse(batchNo) }
            };
            object objValue = ExecuteScalar(LMSWriteConnection, sql, parameters);
            if (objValue != null && objValue != DBNull.Value)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}

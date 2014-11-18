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
    public class GpsOrderStatusDAL : BaseDAL, IGpsOrderStatusDAL
    {
        /// <summary>
        /// 添加订单轨迹状态信息
        /// </summary>
        /// <returns></returns>
        public int AddGpsOrderStatus(GPSOrderStatusEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = @"
INSERT INTO GPSOrderStatus (orderno, status, statestr, distributor, distphone, time, createtime, gpsid, truckno, isSyn, station )
VALUES ( @orderno, @status, @statestr, @distributor, @distphone, @time, @createtime, @gpsid, @truckno, @IsSyn, @station )
";
            SqlParameter[] parameters = new SqlParameter[]  
            {
                new SqlParameter(){ ParameterName = "@orderno" , SqlDbType = SqlDbType.NVarChar, Size = 50 , Value = model.orderno },
                new SqlParameter(){ ParameterName = "@status" ,  SqlDbType = SqlDbType.Int , Value = model.status },
                new SqlParameter(){ ParameterName = "@statestr", SqlDbType = SqlDbType.NVarChar, Size = 50  , Value = model.statestr },
                new SqlParameter(){ ParameterName = "@distributor" , SqlDbType = SqlDbType.NVarChar, Size = 20  , Value = model.distributor},
                new SqlParameter(){ ParameterName = "@distphone" , SqlDbType = SqlDbType.NVarChar, Size = 50  , Value = model.distphone },
                new SqlParameter(){ ParameterName = "@time" ,  SqlDbType = SqlDbType.DateTime , Value = model.time },
                new SqlParameter(){ ParameterName = "@createtime" , SqlDbType = SqlDbType.DateTime , Value = model.createtime },
                new SqlParameter(){ ParameterName = "@gpsid" , SqlDbType = SqlDbType.NVarChar, Size = 20  , Value = model.gpsid },
                new SqlParameter(){ ParameterName = "@truckno" , SqlDbType = SqlDbType.Char , Size = 16 ,  Value = model.truckno },
                new SqlParameter(){ ParameterName = "@IsSyn" , SqlDbType = SqlDbType.Bit , Value = model.IsSyn },
                new SqlParameter(){ ParameterName = "@station" , SqlDbType = SqlDbType.VarChar, Size = 10  , Value = model.station }
             };
            return ExecuteNonQuery(LMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 获取订单轨迹状态信息
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        public GPSOrderStatusEntityModel GetGPSOrderStatus(string waybillNo)
        {
            string sql = @"
SELECT TOP 1  id, orderno, status, statestr, distributor, distphone,  time, createtime, gpsid, truckno, IsSyn, station 
FROM   GPSOrderStatus (NOLOCK)
WHERE   orderno = @orderno 
ORDER BY  ID  DESC
";
            SqlParameter[] parameters = new SqlParameter[]  
            {
                new SqlParameter(){ ParameterName="@orderno" , SqlDbType = SqlDbType.NVarChar, Size = 50 , Value = waybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<GPSOrderStatusEntityModel>(LMSWriteConnection, sql, parameters);
        }
    }
}

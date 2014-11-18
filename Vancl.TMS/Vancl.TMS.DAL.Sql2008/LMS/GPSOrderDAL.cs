using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using System.Data.SqlClient;
using Vancl.TMS.Model.LMS;
using System.Data;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class GPSOrderDAL : BaseDAL, IGPSOrderDAL
    {
        public long Add(GPSOrderEntityModel model)
        {
            var strSql = @"
INSERT INTO GPSOrder (orderno ,city ,Warehouse ,stations ,cid ,address ,lng ,lat ,createtime ,gpsid ,time ,truckno ,isSyn)
VALUES (@orderno ,@city ,@warehouse ,@stations ,@cid ,@address ,@lng ,@lat ,@createtime ,@gpsid ,@time ,@truckno ,@IsSyn)
";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter(){ ParameterName = "@orderno", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.orderno },
                    new SqlParameter(){ ParameterName = "@city", SqlDbType = SqlDbType.NVarChar , Size = 50 ,Value = model.city },
                    new SqlParameter(){ ParameterName = "@warehouse", SqlDbType = SqlDbType.NVarChar , Size = 20,Value = model.warehouse },
                    new SqlParameter(){ ParameterName = "@stations", SqlDbType = SqlDbType.NVarChar , Size = 50 ,Value = model.stations },
                    new SqlParameter(){ ParameterName = "@cid", SqlDbType = SqlDbType.Int ,Value = model.cid },
                    new SqlParameter(){ ParameterName = "@address", SqlDbType = SqlDbType.NVarChar , Size = 120 ,Value = model.address },
                    new SqlParameter(){ ParameterName = "@lng", SqlDbType = SqlDbType.Float ,Value = model.lng },
                    new SqlParameter(){ ParameterName = "@lat", SqlDbType = SqlDbType.Float ,Value = model.lat },
                    new SqlParameter(){ ParameterName = "@createtime", SqlDbType = SqlDbType.DateTime ,Value = model.createtime },
                    new SqlParameter(){ ParameterName = "@gpsid", SqlDbType = SqlDbType.NVarChar , Size = 20, Value = model.gpsid },
                    new SqlParameter(){ ParameterName = "@time",  SqlDbType = SqlDbType.DateTime ,Value = model.time },
                    new SqlParameter(){ ParameterName = "@truckno",  SqlDbType = SqlDbType.Char , Size = 16, Value = model.truckno },
                    new SqlParameter(){ ParameterName = "@IsSyn", SqlDbType = SqlDbType.Bit ,Value = model.IsSyn }
             };
            return ExecuteNonQuery(LMSWriteConnection,strSql,parameters);
        }

        /// <summary>
        /// 判断订单轨迹是否存在
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsExist(string orderNo)
        {
            string sql = @"
SELECT COUNT(0) 
FROM GPSOrder(NOLOCK)  
WHERE orderNo = @orderNo
";
            SqlParameter[] parameters = new SqlParameter[]
            {
                   new SqlParameter(){ ParameterName = "@orderno", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = orderNo }
            };
            var i = int.Parse(ExecuteScalar(LMSWriteConnection, sql, parameters).ToString());
            return i > 0;
        }
    }
}

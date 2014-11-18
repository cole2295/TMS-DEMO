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
    public class GpsOrderStatusDAL : LMSBaseDAL, IGpsOrderStatusDAL
    {
        /// <summary>
        /// 添加订单轨迹状态信息
        /// </summary>
        /// <returns></returns>
        public int AddGpsOrderStatus(GPSOrderStatusEntityModel model)
        {
            String sql = String.Format(@"
INSERT INTO GPSOrderStatus ( GPSORDERSTATUSID,orderno,status,statestr,distributor,distphone,GPSORDERSTATUSTIME,createtime,gpsid,truckno,isSyn,station )
VALUES ({0},:orderno,:status,:statestr,:distributor,:distphone,:GPSORDERSTATUSTIME,:createtime,:gpsid,:truckno,:IsSyn,:station )
",
 model.SequenceNextValue()
 );
            OracleParameter[] parameters = new OracleParameter[]  
            {
                new OracleParameter(){ ParameterName="orderno" , OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = model.orderno },
                new OracleParameter(){ ParameterName="status" ,  OracleDbType = OracleDbType.Int32 , Value = model.status },
                new OracleParameter(){ ParameterName="statestr", OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = model.statestr },
                new OracleParameter(){ ParameterName="distributor" , OracleDbType = OracleDbType.Varchar2 , Size = 40 , Value = model.distributor},
                new OracleParameter() { ParameterName = "distphone" , OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = model.distphone },
                new OracleParameter() { ParameterName = "GPSORDERSTATUSTIME" , OracleDbType = OracleDbType.Date , Value = model.time },
                new OracleParameter() { ParameterName = "createtime" ,  OracleDbType = OracleDbType.Date , Value = model.createtime },
                new OracleParameter() { ParameterName = "gpsid" , OracleDbType = OracleDbType.Varchar2 , Size = 40 , Value = model.gpsid },
                new OracleParameter() { ParameterName = "truckno" , OracleDbType = OracleDbType.Char , Size = 16 , Value = model.truckno },
                new OracleParameter(){ ParameterName = "IsSyn" ,  OracleDbType = OracleDbType.Int32 , Value = Convert.ToInt32(model.IsSyn)  },
                new OracleParameter(){ ParameterName = "station" , OracleDbType = OracleDbType.Varchar2 , Size = 10 , Value = model.station }
             };
            return ExecuteSqlNonQuery (LMSOracleWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 获取订单轨迹状态信息
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        public GPSOrderStatusEntityModel GetGPSOrderStatus(string waybillNo)
        {
            string sql = @"
SELECT *
FROM
(
    SELECT   GPSORDERSTATUSID,  orderno,  status,  statestr, distributor,  distphone,  GPSORDERSTATUSTIME time,  createtime, gpsid, truckno, IsSyn, station 
    FROM   GPSOrderStatus
    WHERE   orderno = :orderno 
    ORDER BY   gpsOrderStatusID DESC
)
Where  ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[]  
            {
                new OracleParameter(){ ParameterName="orderno" , OracleDbType = OracleDbType.Varchar2 , Size = 100  , Value = waybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<GPSOrderStatusEntityModel>(LMSOracleWriteConnection, sql, parameters);
        }

    }
}

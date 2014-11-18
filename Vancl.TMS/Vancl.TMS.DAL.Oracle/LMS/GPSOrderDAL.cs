using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class GPSOrderDAL : LMSBaseDAL, IGPSOrderDAL
    {
        public long Add(GPSOrderEntityModel model)
        {
            var strSql = @"
INSERT INTO GPSOrder (orderno,city,Warehouse,stations,cid,address,lng,lat,createtime,gpsid,truckno,isSyn)
VALUES (:orderno,:city,:warehouse,:stations,:cid,:address,:lng,:lat,:createtime,:gpsid,:truckno,:IsSyn)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                    new OracleParameter(){ ParameterName = "orderno", OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = model.orderno },
                    new OracleParameter(){ ParameterName = "city", OracleDbType = OracleDbType.Varchar2 , Size = 100 ,Value = model.city },
                    new OracleParameter(){ ParameterName = "warehouse", OracleDbType = OracleDbType.Varchar2 , Size = 40 ,Value = model.warehouse },
                    new OracleParameter(){ ParameterName = "stations", OracleDbType = OracleDbType.Varchar2 , Size = 100 ,Value = model.stations },
                    new OracleParameter(){ ParameterName = "cid", OracleDbType = OracleDbType.Int32 ,Value = model.cid },
                    new OracleParameter(){ ParameterName = "address", OracleDbType = OracleDbType.Varchar2 , Size = 240, Value = model.address },
                    new OracleParameter(){ ParameterName = "lng", OracleDbType = OracleDbType.Double ,Value = model.lng },
                    new OracleParameter(){ ParameterName = "lat",  OracleDbType = OracleDbType.Double ,Value = model.lat },
                    new OracleParameter(){ ParameterName = "createtime", OracleDbType = OracleDbType.Date ,Value = model.createtime },
                    new OracleParameter(){ ParameterName = "gpsid", OracleDbType = OracleDbType.Varchar2 , Size = 40, Value = model.gpsid },
                    new OracleParameter(){ ParameterName = "truckno",  OracleDbType = OracleDbType.Char , Size = 16 , Value = model.truckno },
                    new OracleParameter(){ ParameterName = "IsSyn", OracleDbType = OracleDbType.Int16 ,Value = Convert.ToInt16(model.IsSyn) }
             };
            if (ExecuteSqlNonQuery(LMSOracleWriteConnection, strSql.ToString(), parameters) > 0)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 判断运单轨迹是否存在
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsExist(string orderNo)
        {
            string sql = @"
SELECT count(0) 
FROM GPSOrder  
WHERE orderNo = :orderNo
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                    new OracleParameter(){ ParameterName = "orderno", OracleDbType = OracleDbType.Varchar2 , Size = 100 ,Value = orderNo }
            };
            var i = int.Parse(ExecuteSqlScalar(LMSOracleWriteConnection, sql, parameters).ToString());
            return i > 0;
        }
    }
}

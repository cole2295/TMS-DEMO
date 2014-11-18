using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Loading;
using Vancl.TMS.Model.Sorting.Loading;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Loading
{
    public class GPSBillDAL : BaseDAL, IGPSBillDAL
    {
        /// <summary>
        /// 添加订单轨迹记录
        /// </summary>
        /// <param name="model"></param>
        public int AddGPSBill(GPSBillModel model)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into SC_GPSBill(");
            strSql.Append("GBID,FormCode,CityName,UpDeptID,DeliverStationID,CustomerAddress,TruckNO,GPSNO,CreateBy,CreateTime)");
            strSql.Append(" values (");
            strSql.Append(":GBID,:FormCode,:CityName,:UpDeptID,:DeliverStationID,:CustomerAddress,:TruckNO,:GPSNO,:CreateBy,:CreateTime)");
            OracleParameter[] parameters = new OracleParameter[]{ 
                  new OracleParameter(){ParameterName = "GBID" ,DbType = DbType.String , Value = model.GBID}
                  ,new OracleParameter(){ParameterName = "FormCode" ,DbType = DbType.String , Value = model.FormCode} 
                  ,new OracleParameter(){ParameterName = "CityName" ,DbType = DbType.String , Value = model.CityName}  
                  ,new OracleParameter(){ParameterName = "UpDeptID" ,DbType = DbType.Int32 , Value = model.UpDeptID}
                  ,new OracleParameter(){ParameterName = "DeliverStationID" ,DbType = DbType.Int32 , Value = model.DeliverStationID}
                  ,new OracleParameter(){ParameterName = "CustomerAddress" ,DbType = DbType.String , Value = model.CustomerAddress}
                  ,new OracleParameter(){ParameterName = "TruckNO" ,DbType = DbType.String , Value = model.TruckNO}
                  ,new OracleParameter(){ParameterName = "GPSNO" ,DbType = DbType.String , Value = model.GPSNO}
                  ,new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = model.CreateBy}
                  ,new OracleParameter(){ParameterName = "CreateTime" ,DbType = DbType.Date , Value = model.CreateTime}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, strSql.ToString(), parameters);
        }

        public bool IsExist(string formCode)
        {
            string sql = @"select GBID from SC_GPSBill where FormCode:FormCode";
            OracleParameter[] parameters = new OracleParameter[]{ 
                  new OracleParameter(){ParameterName = "FormCode" ,DbType = DbType.String , Value = formCode } 
            };
            var result = ExecuteSqlScalar(TMSReadOnlyConnection,sql,parameters);
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}

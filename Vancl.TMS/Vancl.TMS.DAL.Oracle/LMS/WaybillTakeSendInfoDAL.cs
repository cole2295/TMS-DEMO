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
    public class WaybillTakeSendInfoDAL : LMSBaseDAL, IWaybillTakeSendInfoDAL
    {
        /// <summary>
        /// 获取运单扩展信息（客户信息）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public TakeSend_DeliverStationEntityModel GetTakeSendAndDeliverStationInfo(long WaybillNo)
        {
            string sql = @"
SELECT   c.cityname DeliverStationCityName,
        wb.WarehouseId,
        wb.DeliverStationID,
        ec.ParentID DeliverStationParentID, 
        wts.ReceiveAddress 
FROM   WaybillTakeSendInfo wts
    JOIN   waybill wb ON   wb.WaybillNo = wts.WaybillNo
    JOIN   ExpressCompany ec ON   ec.ExpressCompanyID = wb.DeliverStationID
    JOIN   City c ON   c.CityID = ec.CityID
WHERE   wts.WaybillNo = :WaybillNo 
AND   rownum = 1
";

            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName= "WaybillNo",OracleDbType= OracleDbType.Int64, Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<TakeSend_DeliverStationEntityModel>(LMSOracleWriteConnection, sql, parameters);
        }
    }
}

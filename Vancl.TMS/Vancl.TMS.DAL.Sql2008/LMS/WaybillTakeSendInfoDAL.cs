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
    public class WaybillTakeSendInfoDAL : BaseDAL, IWaybillTakeSendInfoDAL
    {
        /// <summary>
        /// 获取运单扩展信息（客户信息）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public TakeSend_DeliverStationEntityModel GetTakeSendAndDeliverStationInfo(long WaybillNo)
        {
            string sql = @"
SELECT   TOP 1 
    c.cityname DeliverStationCityName ,
    wb.WarehouseId ,
    wb.DeliverStationID ,
    ec.ParentID DeliverStationParentID ,
    wts.ReceiveAddress
FROM  Waybill wb(NOLOCK)  
    JOIN   WaybillTakeSendInfo wts(NOLOCK)  ON   wb.WaybillNo = wts.WaybillNo
    JOIN   RFD_PMS.dbo.ExpressCompany ec(NOLOCK)    ON   ec.ExpressCompanyID = wb.DeliverStationID
    JOIN   RFD_PMS.dbo.City c(NOLOCK)   ON   c.CityID = ec.CityID
WHERE   wb.WaybillNo = @WaybillNo
";
            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter(){ ParameterName= "@WaybillNo", SqlDbType = SqlDbType.BigInt, Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<TakeSend_DeliverStationEntityModel>(LMSWriteConnection, sql, parameters);
        }
    }
}

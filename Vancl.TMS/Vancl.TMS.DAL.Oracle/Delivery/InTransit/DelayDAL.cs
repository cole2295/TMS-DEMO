using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Delivery.InTransit;

namespace Vancl.TMS.DAL.Oracle.Delivery.InTransit
{
    public class DelayDAL : BaseDAL, IDelayDAL
    {
        #region IDelayDAL 成员

        public int Add(DelayModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_Delay(
                    DID, 
                    DeliveryNo, 
                    CarrierWaybillno, 
                    DelayType, 
                    DelayReason, 
                    DelayTimespan, 
                    CreateBy,
                    UpdateBy, 
                    IsDeleted
                 )
                VALUES(
                    {0}, 
                    :DeliveryNo, 
                    :CarrierWaybillno, 
                    :DelayType, 
                    :DelayReason, 
                    :DelayTimespan, 
                    :CreateBy,
                    :UpdateBy,  
                    0
                )", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="CarrierWaybillno",DbType= DbType.String,Value=model.CarrierWaybillNo},
                new OracleParameter() { ParameterName="DelayType",DbType= DbType.Int32,Value=(int)model.DelayType},
                new OracleParameter() { ParameterName="DelayReason",DbType= DbType.String,Value=model.DelayReason},
                new OracleParameter() { ParameterName="DelayTimespan",DbType= DbType.Decimal,Value=model.DelayTimeSpan},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public DelayModel GetDelayModel(string deliveryNo)
        {
            string strSql = @"
                SELECT DID,DeliveryNo,CarrierWaybillNo,DelayType
                    ,DelayReason,DelayTimeSpan,CreateBy
                    ,CreateTime,UpdateBy,UpdateTime,IsDeleted
                FROM TMS_Delay
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlSingle_ByReaderReflect<DelayModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public string GetDeliveryNo(long did)
        {
            string strSql = @"
                SELECT DeliveryNo
                FROM TMS_Delay
                WHERE IsDeleted=0
                    AND DID=:DID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DID",DbType= DbType.Int64,Value=did}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToString(o);
            }
            return "";
        }
        #endregion
    }
}

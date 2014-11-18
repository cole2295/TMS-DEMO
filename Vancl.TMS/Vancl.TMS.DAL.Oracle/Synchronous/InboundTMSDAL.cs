using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Converter;

namespace Vancl.TMS.DAL.Oracle.Synchronous
{
    public class InboundTMSDAL : BaseDAL, IInboundTMSDAL
    {
        #region IInboundTMSDAL 成员

        public int UpdateIsArrivedStatusByBoxNo(string boxNo, int arrivalID)
        {
            string strSql = @"
                UPDATE TMS_DispOrderDetail
                SET IsArrived=1
                WHERE IsDeleted=0
                    AND BoxNo=:BoxNo
                    AND ArrivalID=:ArrivalID
                    AND IsArrived=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="BoxNo",DbType= DbType.String,Value=boxNo},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int UpdateIsArrivedStatusByFormCodes(string[] formCodes, int[] arrivalIDs)
        {
            string strSql = @"
                UPDATE TMS_DispOrderDetail
                SET IsArrived=1
                WHERE IsDeleted=0
                    AND FormCode=:FormCodes
                    AND ArrivalID=:ArrivalIDs
                    AND IsArrived=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCodes",DbType= DbType.String,Value=formCodes},
                new OracleParameter() { ParameterName="ArrivalIDs",DbType= DbType.String,Value=arrivalIDs}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, formCodes.Length, arguments);
        }

        public int UpdateDispatchByBoxNo(string boxNo, int arrivalID, DateTime receiveDate)
        {
            string strSql = @"
                UPDATE TMS_Dispatch a
                SET a.DesReceiveDate=:ReceiveDate
                WHERE a.IsDeleted=0
                    AND a.ArrivalID=:ArrivalID
                    AND a.DesReceiveDate=NULL
                    AND a.DeliveryStatus<:DeliveryStatus
                    AND EXISTS
                        (SELECT 1 FROM TMS_DispatchDetail dd
                        WHERE dd.DID=a.DID
                            AND dd.IsDeleted=0
                            AND dd.BoxNo=:BoxNo)";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ReceiveDate",DbType= DbType.DateTime,Value=receiveDate},
                new OracleParameter() { ParameterName="DeliveryStatus",DbType= DbType.Int32,Value=(int)Enums.DeliveryStatus.ArrivedOnTime},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID},
                new OracleParameter() { ParameterName="BoxNo",DbType= DbType.String,Value=boxNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int UpdateDispatchByFormCodes(string[] formCodes, int[] arrivalIDs, DateTime[] receiveDates)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Dispatch a
                SET a.DesReceiveDate=:ReceiveDates
                WHERE a.IsDeleted=0
                    AND a.ArrivalID=:ArrivalIDs
                    AND a.DesReceiveDate IS NULL
                    AND a.DeliveryStatus<{0}
                    AND EXISTS
                        (SELECT 1 FROM TMS_DispOrderDetail dod
                        WHERE dod.DeliveryNo=a.DeliveryNo
                            AND dod.IsDeleted=0
                            AND dod.FormCode IN ({1})
                        )", (int)Enums.DeliveryStatus.ArrivedOnTime
                        , "'" + string.Join("','", formCodes) + "'");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ReceiveDates",DbType= DbType.DateTime,Value=receiveDates},
                new OracleParameter() { ParameterName="ArrivalIDs",DbType= DbType.Int32,Value=arrivalIDs}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, arrivalIDs.Length, arguments);
        }

        public int UpdateOrderTMSStatusByBoxNo(string boxNo, int arrivalID)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Order o
                SET o.OrderTMSStatus={0}
                WHERE o.IsDeleted=0
                    AND o.OrderTMSStatus={1}
                    AND o.ArrivalID=:ArrivalID
                    AND EXISTS (
                        SELECT 1
                        FROM TMS_BoxDetail bd
                        WHERE bd.BoxNo=:BoxNo
                            AND bd.FormCode=o.FormCode
                    )"
                , (int)Enums.OrderTMSStatus.Finished
                , (int)Enums.OrderTMSStatus.Normal);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="BoxNo",DbType= DbType.String,Value=boxNo},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int UpdateOrderTMSStatusByFormCodes(string[] formCodes, int[] arrivalIDs)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Order
                SET OrderTMSStatus={0}
                WHERE IsDeleted=0
                    AND OrderTMSStatus={1}
                    AND FormCode=:FormCodes
                    AND ArrivalID=:ArrivalIDs"
                , (int)Enums.OrderTMSStatus.Finished
                , (int)Enums.OrderTMSStatus.Normal);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCodes",DbType= DbType.String,Value=formCodes},
                new OracleParameter() { ParameterName="ArrivalIDs",DbType= DbType.Int32,Value=arrivalIDs}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, arrivalIDs.Length, arguments);
        }
        #endregion
    }
}

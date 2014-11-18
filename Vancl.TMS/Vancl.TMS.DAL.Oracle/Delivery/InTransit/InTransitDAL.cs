using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.Model.Delivery.InTransit;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Delivery.InTransit
{
    public class InTransitDAL : BaseDAL, IInTransitDAL
    {
        #region IInTransitDAL 成员

        public Util.Pager.PagedList<ViewInTransitModel> Search(InTransitSearchModel searchModel)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
SELECT 
    d.DID as DispatchID,
    d.CreateTime,
    d.DepartureID,
    d.ProtectedPrice ,
    ecd.Companyname as DepartureName,
    d.ArrivalID,
    eca.Companyname as ArrivalName,
    City.Cityname as ArrivalCity,
    d.BoxCount,
    d.CarrierID,
    c.CarrierName,
    d.TransPortType,
    d.ArrivalTiming,
    d.DeliveryNo,
    cw.WaybillNo,
    d.LineGoodsType,
    d.TotalAmount,
    d.ExpectArrivalDate,
    d.DesReceiveDate,
    d.DeliveryStatus,
    cw.Weight,
    cw.TotalCount,
    d.ConfirmExpArrivalDate,
    sa.LeaveTime as DeliveryTime
FROM TMS_Dispatch d
    JOIN ExpressCompany ecd ON ecd.ExpresscompanyID = d.DepartureID
    JOIN ExpressCompany eca ON eca.Expresscompanyid = d.arrivalid
    JOIN City ON City.CityID = eca.CityID
    JOIN TMS_CarrierWaybill cw ON cw.CWID = d.CarrierWaybillID
    JOIN TMS_Carrier c ON c.CarrierID = d.CarrierID AND c.IsDeleted = 0
    JOIN TMS_SiteAssessment sa ON sa.DeliveryNO = d.DeliveryNO AND sa.IsDeleted=0
WHERE d.IsDeleted = 0 
    AND d.DeliveryStatus = {0}
"
                , (int)Enums.DeliveryStatus.InTransit);
            var paramList = new List<OracleParameter>();
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                sbSql.Append(" AND d.DeliveryNO like :DeliveryNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "DeliveryNO", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = searchModel.DeliveryNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.WaybillNO))
            {
                sbSql.Append(" AND cw.WaybillNO like :WaybillNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "WaybillNO", OracleDbType = OracleDbType.Varchar2, Size = 50 , Value = searchModel.WaybillNO });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sbSql.Append(" AND d.CarrierID = :CarrierID ");
                paramList.Add(new OracleParameter() { ParameterName = "CarrierID", OracleDbType = OracleDbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.ArrivalCity))
            {
                sbSql.Append(" AND City.CityName like :CityName || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "CityName", OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = searchModel.ArrivalCity });
            }
            if (searchModel.DeliveryTimeBegin.HasValue)
            {
                sbSql.Append(" AND sa.LeaveTime >= :DeliveryTimeBegin");
                paramList.Add(new OracleParameter() { ParameterName = "DeliveryTimeBegin", OracleDbType = OracleDbType.Date, Value = searchModel.DeliveryTimeBegin.Value });
            }
            if (searchModel.DeliveryTimeEnd.HasValue)
            {
                sbSql.Append(" AND sa.LeaveTime <= :DeliveryTimeEnd");
                paramList.Add(new OracleParameter() { ParameterName = "DeliveryTimeEnd", OracleDbType = OracleDbType.Date, Value = Convert.ToDateTime(searchModel.DeliveryTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            if (searchModel.ExpectTimeBegin.HasValue)
            {
                sbSql.Append(" AND d.ConfirmExpArrivalDate >= :ExpectTimeBegin");
                paramList.Add(new OracleParameter() { ParameterName = "ExpectTimeBegin", OracleDbType = OracleDbType.Date, Value = searchModel.ExpectTimeBegin.Value });
            }
            if (searchModel.ExpectTimeEnd.HasValue)
            {
                sbSql.Append(" AND d.ConfirmExpArrivalDate <= :ExpectTimeEnd");
                paramList.Add(new OracleParameter() { ParameterName = "ExpectTimeEnd", OracleDbType = OracleDbType.Date, Value = Convert.ToDateTime(searchModel.ExpectTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            sbSql.Append(" ORDER BY sa.LeaveTime");
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewInTransitModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, paramList.ToArray());
        }


        public ViewInTransitModel Get(long dispatchID)
        {
            string sql = @"
               SELECT 
                   TMS_Dispatch.DID as DispatchID,
                   TMS_Dispatch.CreateTime,
                   TMS_Dispatch.DepartureID,
                   Departure.Companyname as DepartureName,
                   TMS_Dispatch.ArrivalID,
                   Arrival.Companyname as ArrivalName,
                   City.Cityname as ArrivalCity,
                   TMS_Dispatch.BoxCount,
                   TMS_Dispatch.CarrierID,
                   TMS_Carrier.CarrierName,
                   TMS_Dispatch.TransPortType,
                   TMS_Dispatch.ArrivalTiming,
                   TMS_Dispatch.DeliveryNo,
                   TMS_Carrierwaybill.WaybillNo,
                   TMS_Dispatch.LineGoodsType,
                   TMS_Dispatch.TotalAmount,
                   TMS_Dispatch.ExpectArrivalDate,
                   TMS_Dispatch.DesReceiveDate,
                   TMS_Dispatch.ConfirmExpArrivalDate,
                   TMS_Dispatch.DeliveryStatus  

            FROM TMS_Dispatch
            JOIN ExpressCompany Departure on Departure.ExpresscompanyID = TMS_Dispatch.DepartureID
            JOIN ExpressCompany Arrival on arrival.Expresscompanyid = TMS_Dispatch.arrivalid
            JOIN City on City.cityid=Arrival.CityID
            JOIN TMS_Carrierwaybill on TMS_carrierwaybill.CWID = TMS_Dispatch.CarrierWaybillID 
            JOIN TMS_Carrier on TMS_Carrier.carrierid = TMS_Dispatch.CarrierID AND TMS_Carrier.IsDeleted=0

            WHERE TMS_Dispatch.IsDeleted=0 AND TMS_Dispatch.DID=:DispatchID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DispatchID",DbType= DbType.Int64,Value=dispatchID}};
            return ExecuteSqlSingle_ByReaderReflect<ViewInTransitModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public ViewSetArriveModel GetSetArriveView(long dispatchID)
        {
            string strSql = string.Format(@"
                SELECT 
                    d.DID as DispatchID
                    ,d.DeliveryNo
                    ,cb.WaybillNo
                    ,d.ExpectArrivalDate
                    ,d.DesReceiveDate
                    ,d.ConfirmExpArrivalDate
                    ,CASE WHEN ed.EDID IS NULL THEN 0
                    ELSE 1 END AS IsExpectDelay
                    ,ed.DelayTime as ExpectDelayTime
                    ,ed.ExpectDelayType as ExpectDelayType
                    ,ed.DelayDesc as ExpectDelayDesc
                FROM TMS_Dispatch d
                JOIN TMS_Carrierwaybill cb 
                    ON cb.CWID = d.CarrierWaybillID
                LEFT JOIN TMS_ExpectDelay ed
                    ON ed.DeliveryNo=d.DeliveryNo
                        AND ed.IsDeleted=0
                        AND ed.ApproveStatus={0}
                WHERE d.IsDeleted=0 
                    AND d.DID=:DispatchID", (int)Enums.ApproveStatus.Approved);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DispatchID",DbType= DbType.Int64,Value=dispatchID}};
            return ExecuteSqlSingle_ByReaderReflect<ViewSetArriveModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int UpdateOrderTMSStatusToFinished(long dispatchID)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Order o
                SET o.OrderTMSStatus={0}
                    ,o.UpdateTime=sysdate
                    ,o.UpdateBy=:UpdateBy
                WHERE o.IsDeleted=0
                    AND o.OrderTMSStatus={1}
                    AND EXISTS (
                        --在提货单里存在并且不被认为是已经丢失
                        SELECT 1
                        FROM TMS_DispOrderDetail dod
                        JOIN TMS_Dispatch d
                            ON dod.DeliveryNo=d.DeliveryNo
                                AND d.DID=:DID
                                AND d.IsDeleted=0
                        WHERE dod.IsDeleted=0
                            AND dod.FormCode=o.FormCode
                            AND dod.ArrivalID=o.ArrivalID
                            --不在丢失表里存在
                            AND NOT EXISTS (
                                SELECT 1
                                FROM TMS_Lost l
                                WHERE l.IsDeleted=0
                                    AND l.DeliveryNo=d.DeliveryNo
                                    AND l.IsAllLost=1
                                    AND l.ApproveStatus={2}
                                UNION ALL
                                SELECT 1
                                FROM TMS_Lost l
                                JOIN TMS_LostDetail ld
                                    ON ld.LID=l.LID
                                        AND ld.IsDeleted=0
                                WHERE l.IsDeleted=0
                                    AND l.DeliveryNo=d.DeliveryNo
                                    AND l.IsAllLost=0
                                    AND l.ApproveStatus={2}
                                    AND ld.FormCode=dod.FormCode
                            )
                    )", (int)Enums.OrderTMSStatus.Finished
                      , (int)Enums.OrderTMSStatus.Normal
                      , (int)Enums.ApproveStatus.Approved);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DID",DbType= DbType.Int64,Value=dispatchID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        #endregion
    }
}

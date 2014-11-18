using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.OutServiceProxy;

namespace Vancl.TMS.DAL.Oracle.Transport.Dispatch
{
    public class DispatchDAL : BaseDAL, IDispatchDAL
    {
        #region IDispatchDAL 成员

        public int Add(DispatchModel model)
        {
            string sql = @"
                insert into TMS_Dispatch
                  (DID, DeliveryNo, BoxCount, TotalAmount, ProtectedPrice , DepartureID, ArrivalID, CarrierID, LPID, TransportType, LineGoodsType, ArrivalTiming, DeliveryStatus, CarrierWaybillID, CreateBy, UpdateBy)
                values
                  (:DID, :DeliveryNo, :BoxCount, :TotalAmount, :ProtectedPrice ,:DepartureID, :ArrivalID, :CarrierID, :LPID, :TransportType, :LineGoodsType, :ArrivalTiming,  :DeliveryStatus, :CarrierWaybillID, :CreateBy, :UpdateBy)
                ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DID",DbType= DbType.Int64,Value=model.DID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="BoxCount",DbType= DbType.Int32,Value=model.BoxCount},
                new OracleParameter() { ParameterName="TotalAmount",DbType= DbType.Int32,Value=model.TotalAmount},
                new OracleParameter() { ParameterName="ProtectedPrice", DbType= DbType.Decimal, Value= model.ProtectedPrice},
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=model.DepartureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=model.ArrivalID},
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=model.CarrierID},
                new OracleParameter() { ParameterName="LPID",DbType= DbType.Int32,Value=model.LPID},
                new OracleParameter() { ParameterName="TransportType",DbType= DbType.Int32,Value=model.TransportType},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=model.LineGoodsType},
                new OracleParameter() { ParameterName="ArrivalTiming",DbType= DbType.Int32,Value=model.ArrivalTiming},
                new OracleParameter() { ParameterName="DeliveryStatus",DbType= DbType.Int32,Value = (int)model.DeliveryStatus},
                new OracleParameter() { ParameterName="CarrierWaybillID",DbType= DbType.Int32,Value=model.CarrierWaybillID},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int AddEx(DispatchModel model)
        {
            string sql = @"
                insert into TMS_Dispatch
                  (DID, DeliveryNo, BoxCount, TotalAmount, ProtectedPrice , DepartureID, ArrivalID, CarrierID, LPID, TransportType, LineGoodsType, ArrivalTiming, DeliveryStatus, CarrierWaybillID, CreateBy, UpdateBy, DELIVERYSOURCE,BatchNo)
                values
                  (:DID, :DeliveryNo, :BoxCount, :TotalAmount, :ProtectedPrice ,:DepartureID, :ArrivalID, :CarrierID, :LPID, :TransportType, :LineGoodsType, :ArrivalTiming,  :DeliveryStatus, :CarrierWaybillID, :CreateBy, :UpdateBy, :DELIVERYSOURCE,:BatchNo)
                ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DID",OracleDbType= OracleDbType.Int64,Value=model.DID},
                new OracleParameter() { ParameterName="DeliveryNo",OracleDbType= OracleDbType.Varchar2, Size=50,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="BoxCount",OracleDbType= OracleDbType.Int32,Value=model.BoxCount},
                new OracleParameter() { ParameterName="TotalAmount",OracleDbType= OracleDbType.Decimal,Value=model.TotalAmount},
                new OracleParameter() { ParameterName="ProtectedPrice", OracleDbType= OracleDbType.Decimal, Value= model.ProtectedPrice},
                new OracleParameter() { ParameterName="DepartureID",OracleDbType= OracleDbType.Int32,Value=model.DepartureID},
                new OracleParameter() { ParameterName="ArrivalID",OracleDbType= OracleDbType.Int32,Value=model.ArrivalID},
                new OracleParameter() { ParameterName="CarrierID",OracleDbType= OracleDbType.Int32,Value=model.CarrierID},
                new OracleParameter() { ParameterName="LPID",OracleDbType= OracleDbType.Int32,Value=model.LPID},
                new OracleParameter() { ParameterName="TransportType",OracleDbType= OracleDbType.Int32,Value=Convert.ToInt32(model.TransportType)},
                new OracleParameter() { ParameterName="LineGoodsType",OracleDbType= OracleDbType.Int32,Value=Convert.ToInt32(model.LineGoodsType)},
                new OracleParameter() { ParameterName="ArrivalTiming",OracleDbType= OracleDbType.Decimal,Value=model.ArrivalTiming},
                new OracleParameter() { ParameterName="DeliveryStatus",OracleDbType= OracleDbType.Int32,Value = Convert.ToInt32(model.DeliveryStatus)},
                new OracleParameter() { ParameterName="CarrierWaybillID",OracleDbType= OracleDbType.Int64,Value=model.CarrierWaybillID},
                new OracleParameter() { ParameterName="CreateBy",OracleDbType= OracleDbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",OracleDbType= OracleDbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DELIVERYSOURCE",OracleDbType= OracleDbType.Int32,Value=Convert.ToInt32(model.DeliverySource)},
                new OracleParameter() { ParameterName="BatchNo",OracleDbType= OracleDbType.Varchar2, Size=50,Value=model.BatchNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Update(DispatchModel model)
        {
            StringBuilder sb = new StringBuilder(@"
                   UPDATE TMS_Dispatch
                   SET ");
            List<OracleParameter> argList = new List<OracleParameter>();
            if (model.ExpectArrivalDate.HasValue)
            {
                sb.Append(" ExpectArrivalDate = :ExpectArrivalDate,");
                argList.Add(new OracleParameter() { ParameterName = "ExpectArrivalDate", OracleDbType = OracleDbType.Date, Value = model.ExpectArrivalDate.Value });
            }
            if (model.ConfirmExpArrivalDate.HasValue)
            {
                sb.Append(" ConfirmExpArrivalDate = :ConfirmExpArrivalDate,");
                argList.Add(new OracleParameter() { ParameterName = "ConfirmExpArrivalDate", OracleDbType = OracleDbType.Date, Value = model.ConfirmExpArrivalDate.Value });
            }
            if (model.DesReceiveDate.HasValue)
            {
                sb.Append(" DesReceiveDate = :DesReceiveDate,");
                argList.Add(new OracleParameter() { ParameterName = "DesReceiveDate", OracleDbType = OracleDbType.Date, Value = model.DesReceiveDate.Value });
            }

            sb.Append(@"
                       DeliveryStatus = :DeliveryStatus,
                       CarrierWaybillID = :CarrierWaybillID,
                       AccountNo = :AccountNo,
                       UpdateBy = :UpdateBy,
                       UpdateTime = sysdate
                 WHERE DID = :DID");
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DeliveryStatus",OracleDbType= OracleDbType.Int32,Value =Convert.ToInt32(model.DeliveryStatus)},
                new OracleParameter() { ParameterName="CarrierWaybillID",OracleDbType= OracleDbType.Int64,Value=model.CarrierWaybillID},
                new OracleParameter() { ParameterName="AccountNo",OracleDbType= OracleDbType.Varchar2, Size=20,Value=model.AccountNo},
                new OracleParameter() { ParameterName="UpdateBy",OracleDbType= OracleDbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DID",OracleDbType= OracleDbType.Int64,Value=model.DID},
            };
            argList.AddRange(arguments.ToList());
            return ExecuteSqlNonQuery(TMSWriteConnection, sb.ToString(), arguments);
        }

        public int Delete(long did)
        {
            string sql = @"
UPDATE TMS_Dispatch
SET IsDeleted = 1 ,UpdateTime = sysdate , UpdateBy = :UpdateBy
WHERE DID = :DID AND  IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DID",DbType= DbType.Int64,Value=did},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public DispatchModel Get(long did)
        {
            string sql = @"
SELECT
    DID, DeliveryNo, BoxCount, TotalAmount, DepartureID, ArrivalID, CarrierID, LPID, TransportType
    , LineGoodsType, ArrivalTiming, ExpectArrivalDate, ConfirmExpArrivalDate, DesReceiveDate, DeliveryStatus
    , CarrierWaybillID, AccountNo, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted, DELIVERYSOURCE
    , ProtectedPrice
FROM TMS_Dispatch
WHERE DID = :DID
    AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DID", OracleDbType = OracleDbType.Int64 ,Value = did},
            };
            return ExecuteSqlSingle_ByReaderReflect<DispatchModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public DispatchModel Get(string deliveryNo)
        {
            string sql = @"
SELECT
    DID, DeliveryNo, BoxCount, TotalAmount, DepartureID, ArrivalID
    , CarrierID, LPID, TransportType, LineGoodsType, ArrivalTiming
    , ExpectArrivalDate, ConfirmExpArrivalDate, DesReceiveDate, DeliveryStatus
    , CarrierWaybillID, AccountNo, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted, DELIVERYSOURCE
    , ProtectedPrice
FROM TMS_Dispatch
WHERE DeliveryNo = :DeliveryNo
    AND IsDeleted=0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DeliveryNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = deliveryNo},
            };
            return ExecuteSqlSingle_ByReaderReflect<DispatchModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public int UpdateDeliveryStatus(string deliveryNO, Enums.DeliveryStatus deliveryStatus)
        {
            string sql = @"
                    UPDATE TMS_Dispatch
                        SET
                            DeliveryStatus = :DeliveryStatus,
                            UpdateBy = :UpdateBy,
                            UpdateTime = sysdate
                        WHERE DeliveryNo = :DeliveryNo
                            AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",OracleDbType= OracleDbType.Varchar2, Size=50,Value=deliveryNO},
                new OracleParameter() { ParameterName="DeliveryStatus",OracleDbType= OracleDbType.Int32,Value=Convert.ToInt32(deliveryStatus)},
                new OracleParameter() { ParameterName="UpdateBy",OracleDbType= OracleDbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 确认提货单到货
        /// </summary>
        /// <param name="isConfirmLimited">是否限制确认到货</param>
        /// <param name="deliveryNO">提货单对象</param>
        /// <param name="deliveryStatus">提货单状态</param>
        /// <returns></returns>
        public int ConfirmDeliveryArrived(bool isConfirmLimited, DispatchModel dispModel, Enums.DeliveryStatus deliveryStatus)
        {
            List<OracleParameter> arguments = new List<OracleParameter>();
            string strSql = @"
UPDATE TMS_Dispatch
SET
    DeliveryStatus = :DeliveryStatus
    ,SignedUser = :SignedUser
    ,IsDelay = :IsDelay
    ,UpdateBy = :UpdateBy
    ,UpdateTime = sysdate
";
            if (!isConfirmLimited)
            {
                strSql += @"
                    ,DesReceiveDate = :DesReceiveDate";
                arguments.Add(new OracleParameter() { ParameterName = "DesReceiveDate", DbType = DbType.Date, Value = dispModel.DesReceiveDate });
            }
            strSql += @"
                WHERE DeliveryNo = :DeliveryNo
                    AND IsDeleted=0";
            arguments.Add(new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = dispModel.DeliveryNo });
            arguments.Add(new OracleParameter() { ParameterName = "DeliveryStatus", DbType = DbType.Int16, Value = (int)deliveryStatus });
            arguments.Add(new OracleParameter() { ParameterName = "SignedUser", DbType = DbType.String, Value = dispModel.SignedUser });
            arguments.Add(new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID });
            arguments.Add(new OracleParameter() { ParameterName = "IsDelay", DbType = DbType.Byte, Value = dispModel.IsDelay });
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments.ToArray());
        }


        public int UpdateDispatchCofirmExpArrivalDate(long did, DateTime dt)
        {
            string sql = @"
UPDATE TMS_Dispatch
SET
        CONFIRMEXPARRIVALDATE = :CONFIRMEXPARRIVALDATE,
        UpdateBy = :UpdateBy,
        UpdateTime = sysdate
WHERE DID = :DID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="CONFIRMEXPARRIVALDATE", DbType = DbType.Date,Value = dt },
                new OracleParameter() { ParameterName="UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DID", DbType = DbType.Int64, Value=did}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int RejectDispatchEx(long did, Enums.DeliveryStatus DeliveryStatus)
        {
            string sql = @"
                    UPDATE TMS_Dispatch
                        SET
                            DeliveryNo = ' ',
                            UpdateBy = :UpdateBy,
                            UpdateTime = sysdate,
                            DeliveryStatus=:DeliveryStatus,
                            DispatchTime=null
                        WHERE DID = :DID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DID",DbType= DbType.Int64,Value=did},
                new OracleParameter() { ParameterName="DeliveryStatus",DbType= DbType.Int32,Value=(int)DeliveryStatus}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public PagedList<ViewDispatchModel> Search(DispatchSearchModel searchModel)
        {
            List<OracleParameter> arguments = new List<OracleParameter>();
            StringBuilder sbWhere = new StringBuilder();
            StringBuilder sbOrderWhere = new StringBuilder();
            StringBuilder sbPreDispatchedSql = new StringBuilder();
            StringBuilder sbCannotRecognizeSql = new StringBuilder();
            StringBuilder sbDispatchedSql = new StringBuilder();
            string strSql = "";
            if (searchModel.ArrivalID.HasValue)
            {
                sbWhere.Append(" AND {0}.ArrivalID=:ArrivalID");
                sbOrderWhere.Append(" AND {0}.ArrivalID=:ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sbWhere.Append(" AND {0}.DepartureID=:DepartureID");
                sbOrderWhere.Append(" AND {0}.DepartureID=:DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue
                 && (!searchModel.DispatchingPageStatus.HasValue || searchModel.DispatchingPageStatus.Value != Enums.DispatchingPageStatus.CannotRecognize))
            {
                sbWhere.Append(" AND {1}.CarrierID=:CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sbWhere.Append(" AND {0}.CreateTime<=:EndTime");
                sbOrderWhere.Append(" AND {0}.CreateTime<=:EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sbWhere.Append(" AND {0}.CreateTime>=:StartTime");
                sbOrderWhere.Append(" AND {0}.CreateTime>=:StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue
                && (!searchModel.DispatchingPageStatus.HasValue || searchModel.DispatchingPageStatus.Value != Enums.DispatchingPageStatus.CannotRecognize))
            {
                sbWhere.Append(" AND {1}.TransportType=:TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            //等待调度的sql
            //有运输计划的sql
            sbPreDispatchedSql.AppendFormat(@"
                WITH cte_pre as
                (
                    SELECT pd.CreateTime,ecd.CompanyName DepartureName
                        ,eca.CompanyName ArrivalName,City.CityName ArrivalCity
                        ,c.CarrierName,lp.TransportType
                        ,ArrivalTiming,pd.LineGoodsType
                        ,sysdate+ArrivalTiming/24 ConfirmExpArrivalDate
                        ,pd.LPID,pd.BoxNo,pd.DepartureID,pd.ArrivalID
                    FROM TMS_PreDispatch pd
                    JOIN TMS_LinePlan lp   ON lp.IsDeleted=0  AND lp.Status={0}  AND lp.LPID = pd.LPID     AND lp.IsEnabled=1
                    JOIN EXPRESSCOMPANY ecd  ON pd.DepartureID = ecd.EXPRESSCOMPANYID
                    JOIN EXPRESSCOMPANY eca  ON pd.ArrivalID = eca.EXPRESSCOMPANYID
                    JOIN City   ON City.CityID=eca.CityID
                    JOIN TMS_CARRIER c   ON c.CarrierID=lp.CarrierID   AND c.IsDeleted=0   AND c.Status={1}
                    WHERE pd.IsDeleted=0  AND pd.DispatchStatus={2}"
                    , (int)Enums.LineStatus.Effective
                    , (int)Enums.CarrierStatus.Valid
                    , (int)Enums.DispatchStatus.CanDispatch).AppendFormat(sbWhere.ToString(), "pd", "lp")
                .AppendFormat(@"
                )
                SELECT MIN(CreateTime) CreateTime,MAX(DepartureName) DepartureName
                    ,MAX(ArrivalName) ArrivalName,MAX(ArrivalCity) ArrivalCity
                    ,MAX(CarrierName) CarrierName,MAX(TransportType) TransportType
                    ,MAX(ArrivalTiming) ArrivalTiming,'' DeliveryNo,'' WaybillNo
                    ,MAX(LineGoodsType) LineGoodsType,MIN(ConfirmExpArrivalDate) ConfirmExpArrivalDate
                    ,{0} DispatchingPageStatus,LPID,SUM(BoxCount) BoxCount
                    ,SUM(OrderCount) OrderCount,SUM(TotalAmount) TotalAmount,MIN(ReceiveOrderTime) ReceiveOrderTime
                    ,MAX(DepartureID) DepartureID,MAX(ArrivalID) ArrivalID
                FROM (
                    SELECT CreateTime,DepartureName
                        ,ArrivalName,ArrivalCity
                        ,CarrierName,TransportType
                        ,ArrivalTiming,LineGoodsType
                        ,ConfirmExpArrivalDate
                        ,LPID
                        ,(
                            SELECT COUNT(1) CC
                            FROM TMS_Box b
                            WHERE b.IsDeleted=0 AND b.BoxNo=pre.BoxNo
                        ) BoxCount
                        ,(
                            SELECT COUNT(1) CC
                            FROM TMS_BoxDetail bd
                            JOIN TMS_Order o  ON o.IsDeleted=0  AND o.FormCode=bd.FormCode   AND o.OrderTMSStatus={1}
                            WHERE bd.BoxNo=pre.BoxNo
                        ) OrderCount
                        ,(
                            SELECT SUM(o.Price) Amount
                            FROM TMS_BoxDetail bd
                            JOIN TMS_Order o  ON o.IsDeleted=0  AND o.FormCode=bd.FormCode  AND o.OrderTMSStatus={1}
                            WHERE bd.BoxNo=pre.BoxNo
                        ) TotalAmount
                        ,(
                            SELECT MIN(o.CreateTime) CreateTime
                            FROM TMS_BoxDetail bd
                            JOIN TMS_Order o  ON o.IsDeleted=0  AND o.FormCode=bd.FormCode  AND o.OrderTMSStatus={1}
                            WHERE bd.BoxNo=pre.BoxNo
                        ) ReceiveOrderTime
                        ,DepartureID
                        ,ArrivalID
                    FROM cte_pre pre
                ) t
                GROUP BY LPID "
                , (int)Enums.DispatchingPageStatus.PreDispatched
                , (int)Enums.OrderTMSStatus.Normal);

            //无运输计划的sql
            sbCannotRecognizeSql.AppendFormat(@"
                SELECT MIN(b.CreateTime) CreateTime,MAX(ecd.CompanyName) DepartureName
                    ,MAX(eca.CompanyName) ArrivalName,MAX(City.CityName) ArrivalCity
                    ,'' CarrierName,0 TransportType
                    ,null ArrivalTiming,'' DeliveryNo,'' WaybillNo
                    ,MAX(b.ContentType) LineGoodsType,null ConfirmExpArrivalDate
                    ,{0} DispatchingPageStatus,0 LPID,COUNT(1) BoxCount
                    ,SUM(b.TotalCount) OrderCount,SUM(b.TotalAmount) TotalAmount
                    ,MIN(b.CreateTime) ReceiveOrderTime
                    ,b.DepartureID,b.ArrivalID
                FROM TMS_Box b
                JOIN EXPRESSCOMPANY ecd   ON b.DEPARTUREID=ecd.EXPRESSCOMPANYID
                JOIN EXPRESSCOMPANY eca   ON b.ARRIVALID=eca.EXPRESSCOMPANYID 
                JOIN City  ON City.CityID=eca.CityID
                WHERE b.IsDeleted=0
                    AND EXISTS(
                            SELECT 1
                            FROM TMS_BoxDetail bd
                            JOIN TMS_Order o ON o.IsDeleted=0   AND o.OrderTMSStatus={1}  AND o.FormCode=bd.FormCode
                            WHERE bd.BoxNo=b.BoxNo
                    )
                    AND NOT EXISTS (
                            SELECT 1
                            FROM TMS_PreDispatch pd
                            JOIN TMS_LinePlan lp  ON lp.IsDeleted=0  AND lp.Status={2}  AND lp.LPID=pd.LPID  AND lp.IsEnabled=1
                            JOIN TMS_CARRIER c   ON c.CarrierID=lp.CarrierID   AND c.IsDeleted=0  AND c.Status={3}
                            WHERE pd.IsDeleted=0   AND pd.BoxNo=b.BoxNo    AND pd.DispatchStatus={4}
                            UNION ALL
                            SELECT 1
                            FROM TMS_DispatchDetail dd
                            JOIN TMS_Dispatch d    ON d.IsDeleted=0    AND d.DID=dd.DID    AND d.DeliveryStatus IN ({5})
                            WHERE dd.IsDeleted=0   AND dd.BoxNo=b.BoxNo
                        )"
                , (int)Enums.DispatchingPageStatus.CannotRecognize
                , (int)Enums.OrderTMSStatus.Normal
                , (int)Enums.LineStatus.Effective
                , (int)Enums.CarrierStatus.Valid
                , (int)Enums.DispatchStatus.CanDispatch
                , string.Join(",", base.GetDeliveryNotFinishStatus()))
                .AppendFormat(sbOrderWhere.ToString(), "b")
                .Append(@"
                GROUP BY b.DepartureID,b.ArrivalID,b.ContentType");

            //已调度的sql
            sbDispatchedSql.AppendFormat(@"
                SELECT d.CreateTime,ecd.CompanyName DepartureName
                    ,eca.CompanyName ArrivalName,City.CityName ArrivalCity
                    ,c.CarrierName,d.TransportType
                    ,d.ArrivalTiming,d.DeliveryNo,cw.WaybillNo
                    ,d.LineGoodsType,d.ConfirmExpArrivalDate
                    ,{0} DispatchingPageStatus,d.LPID,d.BoxCount
                    ,(
                        SELECT COUNT(1) CC
                        FROM TMS_DispOrderDetail dod
                        WHERE dod.IsDeleted=0   AND dod.DeliveryNo=d.DeliveryNo
                    ) OrderCount
                    ,d.TotalAmount
                    ,(
                        SELECT MIN(bd.CreateTime) CreateTime
                        FROM TMS_DispatchDetail dd  
                        JOIN TMS_Box bd    ON bd.BoxNo = dd.BoxNo
                        WHERE dd.DID = d.DID AND dd.IsDeleted = 0
                    ) ReceiveOrderTime
                    ,d.DepartureID,d.ArrivalID
                FROM TMS_Dispatch d   
                JOIN TMS_CarrierWaybill cw   ON cw.IsDeleted=0  AND cw.CWID = d.CarrierWaybillID
                JOIN EXPRESSCOMPANY ecd   ON d.DepartureID = ecd.EXPRESSCOMPANYID
                JOIN EXPRESSCOMPANY eca    ON d.ArrivalID = eca.EXPRESSCOMPANYID
                JOIN City     ON City.CityID = eca.CityID
                JOIN TMS_CARRIER c   ON c.CarrierID = d.CarrierID   AND c.IsDeleted = 0   AND c.Status = {2}
                WHERE d.IsDeleted = 0   AND d.DeliveryStatus={3}"
                , (int)Enums.DispatchingPageStatus.Dispatched
                , (int)Enums.OrderTMSStatus.Normal
                , (int)Enums.CarrierStatus.Valid
                , (int)Enums.DeliveryStatus.Dispatched)
            .AppendFormat(sbWhere.ToString(), "d", "d");
            if (searchModel.DispatchingPageStatus.HasValue)
            {
                switch (searchModel.DispatchingPageStatus.Value)
                {
                    case Enums.DispatchingPageStatus.PreDispatched:
                        strSql = sbPreDispatchedSql.ToString();
                        break;
                    case Enums.DispatchingPageStatus.Dispatched:
                        strSql = sbDispatchedSql.ToString();
                        break;
                    case Enums.DispatchingPageStatus.CannotRecognize:
                        strSql = sbCannotRecognizeSql.ToString();
                        break;
                }
            }
            else
            {
                strSql = sbPreDispatchedSql.Append(@"
                    UNION ALL
                ").Append(sbCannotRecognizeSql).Append(@"
                    UNION ALL
                ").Append(sbDispatchedSql).ToString();
            }
            searchModel.OrderByString = "CreateTime";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDispatchModel>(TMSReadOnlyConnection, strSql, searchModel, arguments.ToArray());
        }

        public PagedList<ViewDeliveryPrintModel> Search(DeliveryPrintSearchModel searchModel)
        {
            List<OracleParameter> arguments = new List<OracleParameter>();
            StringBuilder sbWhere = new StringBuilder();
            if (searchModel.ArrivalID.HasValue)
            {
                sbWhere.Append(" AND d.ArrivalID=:ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sbWhere.Append(" AND d.DepartureID=:DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sbWhere.Append(" AND d.CarrierID=:CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sbWhere.Append(" AND d.CreateTime<=:EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sbWhere.Append(" AND d.CreateTime>=:StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                sbWhere.Append(" AND d.TransportType=:TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                sbWhere.Append(" AND d.DeliveryStatus=:DeliveryStatus");
                arguments.Add(new OracleParameter() { ParameterName = "DeliveryStatus", DbType = DbType.Int32, Value = (int)searchModel.DeliveryStatus.Value });
            }
            //已调度的sql
            string strSql = @"
                SELECT d.CreateTime,ecd.CompanyName DepartureName
                    ,eca.CompanyName ArrivalName,City.CityName ArrivalCity
                    ,c.CarrierName,d.TransportType
                    ,d.ArrivalTiming,d.DeliveryNo,cw.WaybillNo
                    ,d.LineGoodsType,nvl(ConfirmExpArrivalDate, sysdate+d.ArrivalTiming/24) as ConfirmExpArrivalDate
                    ,d.LPID,d.BoxCount
                    ,(
                        SELECT COUNT(1) CC
                        FROM TMS_DispOrderDetail dod
                        WHERE dod.IsDeleted=0
                            AND dod.DeliveryNo=d.DeliveryNo
                    ) OrderCount
                    ,d.TotalAmount
                    ,(
                        SELECT MIN(o.CreateTime) CreateTime
                        FROM TMS_DispatchDetail dd
                        JOIN TMS_BoxDetail bd
                            ON bd.BoxNo=dd.BoxNo
                        JOIN TMS_Order o
                            ON o.IsDeleted=0
                                AND o.FormCode=bd.FormCode
                        WHERE dd.IsDeleted=0
                            AND dd.DID=d.DID
                    ) ReceiveOrderTime
                    ,d.DepartureID,d.ArrivalID,d.DeliveryStatus					
                    ,tran.PlateNo
                    ,tran.Consignor
                    ,tran.Consignee
                    ,tran.ConsigneePhone
                    ,tran.ReceiveAddress
                FROM TMS_Dispatch d
                JOIN TMS_CarrierWaybill cw
                    ON cw.IsDeleted=0
                        AND cw.CWID=d.CarrierWaybillID
                JOIN EXPRESSCOMPANY ecd 
                    ON d.DepartureID=ecd.EXPRESSCOMPANYID
                JOIN EXPRESSCOMPANY eca
                    ON d.ArrivalID=eca.EXPRESSCOMPANYID
                JOIN City 
                    ON City.CityID=eca.CityID
                JOIN TMS_CARRIER c 
                    ON c.CarrierID=d.CarrierID 
                        AND c.IsDeleted=0
                Left JOIN TMS_DispTransition tran
                    ON tran.DeliveryNo = d.DeliveryNo
                WHERE d.IsDeleted=0"
            + sbWhere.ToString();
            searchModel.OrderByString = "DeliveryNo";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDeliveryPrintModel>(TMSReadOnlyConnection, strSql, searchModel, arguments.ToArray());

        }

        public ViewDispatchStatisticModel GetStatisticInfo(DispatchSearchModel searchModel)
        {
            List<OracleParameter> arguments = new List<OracleParameter>();
            StringBuilder sbDispatchedWhere = new StringBuilder();
            StringBuilder sbOrderWhere = new StringBuilder();
            StringBuilder sbTotalWhere = new StringBuilder();
            StringBuilder sbPreWhere = new StringBuilder();
            StringBuilder sbLineWhere = new StringBuilder();
            if (searchModel.ArrivalID.HasValue)
            {
                sbOrderWhere.Append(" AND {0}.ArrivalID=:ArrivalID");
                sbPreWhere.Append(" AND {0}.ArrivalID=:ArrivalID");
                sbDispatchedWhere.Append(" AND {0}.ArrivalID=:ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sbOrderWhere.Append(" AND {0}.DepartureID=:DepartureID");
                sbTotalWhere.Append(" AND {0}.ArrivalID=:DepartureID");
                sbPreWhere.Append(" AND {0}.DepartureID=:DepartureID");
                sbDispatchedWhere.Append(" AND {0}.DepartureID=:DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sbDispatchedWhere.Append(" AND {0}.CarrierID=:CarrierID");
                sbTotalWhere.Append(" AND {0}.CarrierID=:CarrierID");
                sbLineWhere.Append(" AND {0}.CarrierID=:CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sbOrderWhere.Append(" AND {0}.CreateTime<=:EndTime");
                sbDispatchedWhere.Append(" AND {0}.CreateTime<=:EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sbOrderWhere.Append(" AND {0}.CreateTime>=:StartTime");
                sbDispatchedWhere.Append(" AND {0}.CreateTime>=:StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                sbDispatchedWhere.Append(" AND {0}.TransportType=:TransportType");
                sbTotalWhere.Append(" AND {0}.TransportType=:TransportType");
                sbLineWhere.Append(" AND {0}.TransportType=:TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            //统计总单量sql
            StringBuilder sbTotal = new StringBuilder();
            sbTotal.AppendFormat(@"
                SELECT COUNT(1) TotalCount
                    ,0 DispatchedCount
                    ,0 PreDispatchCount
                    ,0 UnrecognizedCount
                FROM
                (
                    SELECT /*+ use_hash(o)*/  o.FormCode   --正常未完成处于调度以及待调度,无法识别
                    FROM TMS_Order o
                    WHERE o.IsDeleted=0
                        AND o.OrderTMSStatus={0}
                        AND NOT EXISTS (
                            SELECT 1
                            FROM TMS_DispOrderDetail dod
                                JOIN TMS_Dispatch d   ON d.IsDeleted=0  AND d.DeliveryNo=dod.DeliveryNo  AND d.DeliveryStatus > {1}
                            WHERE dod.IsDeleted=0  AND dod.FormCode=o.FormCode
                        )"
                    , (int)Enums.OrderTMSStatus.Normal
                    , (int)Enums.DeliveryStatus.Dispatched)
                .AppendFormat(sbOrderWhere.ToString(), "o")
                .AppendFormat(@"
                    UNION ALL   --中转未完成
                    SELECT o.FormCode
                    FROM TMS_Order o
                    JOIN TMS_DispOrderDetail dod   ON dod.IsDeleted=0   AND dod.FormCode=o.FormCode
                    JOIN TMS_Dispatch d  ON d.IsDeleted=0 AND d.DeliveryNo=dod.DeliveryNo AND d.DeliveryStatus IN ({0})"
                    , string.Join(",", base.GetDeliveryFinishedStatus()))
                .AppendFormat(sbTotalWhere.ToString(), "d")
                .AppendFormat(@"
                    WHERE o.IsDeleted=0
                        AND o.OrderTMSStatus={0}"
                    , (int)Enums.OrderTMSStatus.Normal)
                .Append(@"
                ) t");
            //统计已调度sql
            StringBuilder sbDispatched = new StringBuilder();
            sbDispatched.AppendFormat(@"
                SELECT
                    0 TotalCount
                    ,COUNT(1) DispatchedCount
                    ,0 PreDispatchCount
                    ,0 UnrecognizedCount
                FROM TMS_DispOrderDetail dod
                JOIN TMS_Box b  ON b.IsDeleted=0   AND b.BoxNo=dod.BoxNo  AND b.UpdateTime >= sysdate-2
                JOIN TMS_Dispatch d  ON d.IsDeleted=0   AND d.DeliveryNo=dod.DeliveryNo  AND d.DeliveryStatus={0}"
                , (int)Enums.DeliveryStatus.Dispatched)
                .AppendFormat(sbDispatchedWhere.ToString(), "d")
                .Append(@"
                WHERE dod.IsDeleted=0");
            //统计待调度sql
            StringBuilder sbPreDispatch = new StringBuilder();
            sbPreDispatch.AppendFormat(@"
                SELECT 
                    0 TotalCount
                    ,0 DispatchedCount
                    ,COUNT(1) PreDispatchCount
                    ,0 UnrecognizedCount
                FROM TMS_Order o
                JOIN TMS_BoxDetail bd  ON bd.FormCode=o.FormCode
                JOIN TMS_Box b  ON b.IsDeleted=0 AND b.BoxNo=bd.BoxNo  AND b.UpdateTime>=sysdate-2
                JOIN TMS_PreDispatch pd ON pd.IsDeleted=0  AND pd.BoxNo=bd.BoxNo  AND pd.DispatchStatus={0}"
                , (int)Enums.DispatchStatus.CanDispatch)
                .AppendFormat(sbPreWhere.ToString(), "pd")
                .AppendFormat(@"
                JOIN TMS_LinePlan lp   ON lp.IsDeleted=0 AND lp.LPID=pd.LPID  AND lp.Status={0}  AND lp.IsEnabled=1"
                , (int)Enums.LineStatus.Effective)
                .AppendFormat(sbLineWhere.ToString(), "lp")
                .AppendFormat(@"
                JOIN TMS_Carrier c ON c.IsDeleted=0  AND c.CarrierID=lp.CarrierID  AND c.Status={0}"
                , (int)Enums.CarrierStatus.Valid)
                .AppendFormat(@"
                WHERE o.IsDeleted=0
                    AND o.OrderTMSStatus={0}"
                , (int)Enums.OrderTMSStatus.Normal)
                .AppendFormat(sbOrderWhere.ToString(), "o");
            //统计无法识别sql
            StringBuilder sbUnrecognized = new StringBuilder();
            sbUnrecognized.AppendFormat(@"
                SELECT 0 TotalCount
                    ,0 DispatchedCount
                    ,0 PreDispatchCount
                    ,COUNT(1) UnrecognizedCount
                FROM TMS_Order o
                JOIN TMS_BoxDetail bd  ON bd.FormCode=o.FormCode
                JOIN TMS_Box b  ON b.IsDeleted=0  AND b.BoxNo=bd.BoxNo   AND b.UpdateTime>=sysdate-2
                WHERE o.IsDeleted=0   AND o.OrderTMSStatus={0}
                    AND NOT EXISTS(
                        SELECT 1
                        FROM TMS_PreDispatch pd
                        JOIN TMS_LinePlan lp    ON lp.IsDeleted=0  AND lp.Status={1}  AND lp.LPID=pd.LPID   AND lp.IsEnabled=1
                        JOIN TMS_CARRIER c   ON c.CarrierID=lp.CarrierID   AND c.IsDeleted=0  AND c.Status={2}
                        WHERE pd.IsDeleted=0
                            AND pd.BoxNo=b.BoxNo
                            AND pd.DispatchStatus={3}
                        UNION ALL
                        SELECT 1
                        FROM TMS_DispatchDetail dd
                        JOIN TMS_Dispatch d ON d.IsDeleted=0   AND d.DID=dd.DID   AND d.DeliveryStatus IN ({4})
                        WHERE dd.IsDeleted=0
                            AND dd.BoxNo=b.BoxNo
                        )"
                , (int)Enums.OrderTMSStatus.Normal
                , (int)Enums.LineStatus.Effective
                , (int)Enums.CarrierStatus.Valid
                , (int)Enums.DispatchStatus.CanDispatch
                , string.Join(",", base.GetDeliveryNotFinishStatus()))
                .AppendFormat(sbOrderWhere.ToString(), "o");

            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
                SELECT SUM(TotalCount) TotalCount
                    ,SUM(DispatchedCount) DispatchedCount
                    ,SUM(PreDispatchCount) PreDispatchCount
                    ,SUM(UnrecognizedCount) UnrecognizedCount
                    ,SUM(TotalCount)-SUM(DispatchedCount)-SUM(PreDispatchCount)-SUM(UnrecognizedCount) ExtendedStayCount
                FROM
                (
            ").Append(sbTotal).Append(@"
                    UNION ALL
            ").Append(sbDispatched).Append(@"
                    UNION ALL
            ").Append(sbPreDispatch).Append(@"
                    UNION ALL
            ").Append(sbUnrecognized).Append(@"
                ) t
            ");
            return ExecuteSqlSingle_ByReaderReflect<ViewDispatchStatisticModel>(TMSReadOnlyConnection, sbSql.ToString(), arguments.ToArray());
        }

        public DataTable ExportReport(DispatchSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 运输调度取得已验证的可添加的箱列表信息
        /// </summary>
        /// <param name="DepartureID">出发地ID</param>
        /// <param name="disPatchedBox">已添加到调度的箱子</param>
        /// <param name="disPatchedBox">出发地名称</param>
        /// <param name="disPatchedBox">目的地名称</param>
        /// <returns></returns>
        public List<ViewDispatchBoxModel> GetValidBoxList(int DepartureID, string[] disPatchedBox, string departureName, string arrivalName)
        {
            string sql = String.Format(@"
SELECT tmp.BoxNo,tmp.DepartureID,exD.Companyname AS DepartureName ,tmp.ArrivalID,exA.Companyname AS ArrivalName ,OrderCount,ContentType
FROM
(
    SELECT box.BoxNo,box.DepartureID,box.ArrivalID,COUNT(*) AS OrderCount,MAX(box.ContentType) AS ContentType
    FROM TMS_BOX box
    JOIN TMS_BoxDetail bd
        ON bd.BoxNo=box.Boxno
    JOIN TMS_ORDER ord 
        ON bd.FormCode = ord.FormCode AND ord.ordertmsstatus = {0}
    WHERE box.DepartureID = :DepartureID 
        AND box.IsDeleted = 0 
        AND ord.Isdeleted = 0
    GROUP BY box.BoxNo,box.DepartureID,box.ArrivalID
    UNION ALL
    SELECT box.BoxNo,box.DepartureID,box.ArrivalID,COUNT(*) AS OrderCount,MAX(box.ContentType) AS ContentType
    FROM 
        TMS_DISPATCH disp 
        JOIN TMS_DISPATCHDETAIL dispde ON disp.did = dispde.did
        JOIN TMS_BOX box on dispde.Boxno = box.Boxno
        JOIN TMS_BoxDetail bd ON bd.BoxNo=box.BoxNo
        JOIN TMS_ORDER ord ON bd.FormCode = ord.FormCode AND ord.ordertmsstatus = {0}
    WHERE disp.ArrivalID = :DepartureID AND disp.deliverystatus in ({1})
        AND disp.Isdeleted = 0 AND dispde.Isdeleted = 0
        AND box.IsDeleted = 0 AND ord.Isdeleted = 0
    GROUP BY box.BoxNo,box.DepartureID,box.ArrivalID
) tmp
   JOIN ExpressCompany exD ON tmp.DepartureID = exD.ExpressCompanyID AND exD.Isdeleted = 0
   JOIN ExpressCompany exA ON tmp.ArrivalID = exA.ExpressCompanyID AND exA.Isdeleted = 0
WHERE NOT EXISTS(
              SELECT *
              FROM TMS_DISPATCH Arrdisp
              JOIN TMS_DISPATCHDETAIL Arrdispde ON Arrdisp.did = Arrdispde.did  
              WHERE Arrdisp.DepartureID =  :DepartureID AND Arrdisp.IsDeleted = 0 AND Arrdispde.BoxNo = tmp.Boxno 
          )
",
    (int)Enums.OrderTMSStatus.Normal,
    String.Join(",", base.GetDeliveryFinishedStatus())
 );
            List<OracleParameter> lstParam = new List<OracleParameter>();
            lstParam.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = DepartureID });
            if (!string.IsNullOrWhiteSpace(departureName))
            {
                sql += @" AND exD.Companyname LIKE :DepartureName || '%'";
                lstParam.Add(new OracleParameter() { ParameterName = "DepartureName", DbType = DbType.String, Value = departureName });
            }
            if (!string.IsNullOrWhiteSpace(departureName))
            {
                sql += @" AND exA.Companyname LIKE :ArrivalName || '%'";
                lstParam.Add(new OracleParameter() { ParameterName = "ArrivalName", DbType = DbType.String, Value = arrivalName });
            }
            IList<ViewDispatchBoxModel> ilist = ExecuteSql_ByReaderReflect<ViewDispatchBoxModel>(TMSReadOnlyConnection, sql, lstParam.ToArray());
            if (ilist != null && ilist.Count > 0)
            {
                if (disPatchedBox == null || disPatchedBox.Length < 1)
                {
                    return ilist.ToList();
                }
                var result = ilist.ToLookup(p => disPatchedBox.Contains(p.BoxNo));      //contain
                if (result != null)
                {
                    return result[false].ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// 构建箱号CTE SQL字符串
        /// </summary>
        /// <param name="arrBox"></param>
        /// <returns></returns>
        private string BuildingBoxCTEsql(string[] arrBox)
        {
            StringBuilder strBuffer = new StringBuilder();
            string dual = " FROM DUAL ";
            string unionall = " UNION ALL ";
            string select = " SELECT ";
            for (int i = 0; i < arrBox.Length; i++)
            {
                strBuffer.Append(select);
                strBuffer.Append
                    (
                        string.Format(@" '{0}' AS BOXNO "
                        , arrBox[i].Replace("'", "")
                        )
                    );
                strBuffer.Append(dual);
                strBuffer.Append(unionall);
            }
            return strBuffer.ToString().Substring(0, strBuffer.Length - unionall.Length);
        }

        /// <summary>
        /// 该出发地箱号的已有的调度箱明细记录
        /// </summary>
        /// <param name="DepartureID">出发地</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        public List<DispatchDetailModel> DispatchBoxList(int DepartureID, string[] disPatchedBox)
        {
            string sql = @"
SELECT dispde.ddid, dispde.boxno, dispde.deliveryno,dispde.isplan , dispde.ordercount
FROM TMS_DISPATCH disp
JOIN TMS_DISPATCHDETAIL dispde ON disp.did = dispde.did
JOIN 
(
    SELECT REGEXP_SUBSTR(:listBatchNoStr, '[^,]+', 1, LEVEL) AS BatchNo
    FROM DUAL
    CONNECT BY LEVEL <=
    LENGTH(TRIM(TRANSLATE(:listBatchNoStr,TRANSLATE(:listBatchNoStr, ',', ' '), ' '))) + 1
) tmp ON dispde.BoxNo = tmp.BatchNo
WHERE DepartureID = :DepartureID  AND disp.Isdeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value = DepartureID},
                new OracleParameter() { ParameterName="listBatchNoStr", DbType = DbType.String, Value = String.Join(",",disPatchedBox)}
            };
            IList<DispatchDetailModel> ilist = ExecuteSql_ByDataTableReflect<DispatchDetailModel>(TMSWriteConnection, sql, arguments);
            if (ilist != null)
            {
                return ilist.ToList();
            }
            return null;
        }

        public int Delete(string DeliveryNo)
        {
            string sql = @"
UPDATE TMS_Dispatch
SET IsDeleted = 1 , UpdateTime = sysdate  
WHERE DeliveryNo = :DeliveryNo AND  IsDeleted = 0
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value = DeliveryNo},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 取得运输调度中存在运输计划预调度的PDID
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        public List<long> GetDispatchIsPlanedPDID(string DeliveryNo)
        {
            string sql = string.Format(@"
SELECT dispde.pdid AS PDID
FROM TMS_DISPATCH disp
JOIN TMS_DISPATCHDETAIL dispde ON disp.did = dispde.did and dispde.PDID > 0 
where disp.deliveryno = :DeliveryNo and disp.Isdeleted = 0
");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value = DeliveryNo},
            };
            DataTable dtResult = ExecuteSqlDataTable(TMSReadOnlyConnection, sql, arguments);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<long> listPDID = new List<long>();
                foreach (DataRow item in dtResult.Rows)
                {
                    listPDID.Add(Convert.ToInt32(item["PDID"].ToString()));
                }
                return listPDID;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>modify by weiminga 20121203,修改预计到货时间计算逻辑:预计到货时间=离库时间+线路到货时效</remarks>
        /// <param name="deliveryNO"></param>
        /// <param name="leaveTime"></param>
        /// <returns></returns>
        public int UpdateDispatchExpectArrivalDate(string deliveryNO, DateTime leaveTime)
        {
            string strSql = @"
                UPDATE TMS_Dispatch d
                SET
                    (d.ExpectArrivalDate,d.ConfirmExpArrivalDate)=
                        (
                            SELECT to_date(to_char(:LeaveTime,'yyyy-mm-dd')||' '||to_char(lp.LeaveAssessmentTime,'hh24:mi'),'yyyy-mm-dd hh24:mi')+lp.ArrivalTiming/24,
                                   to_date(to_char(:LeaveTime,'yyyy-mm-dd')||' '||to_char(lp.LeaveAssessmentTime,'hh24:mi'),'yyyy-mm-dd hh24:mi')+lp.ArrivalTiming/24
                            FROM TMS_LinePlan lp
                            WHERE lp.LPID=d.LPID
                        )
                    ,d.UpdateBy = :UpdateBy
                    ,d.UpdateTime = sysdate
                WHERE d.DeliveryNo = :DeliveryNo
                    AND d.IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNO},
                new OracleParameter() { ParameterName="LeaveTime",DbType= DbType.DateTime,Value=leaveTime}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public List<ViewDispatchBoxModel> GetDispatchedBoxList(string deliveryNo)
        {
            string strSql = @"
                SELECT d.DepartureID,ecd.CompanyName DepartureName
                    ,d.ArrivalID,eca.CompanyName ArrivalName,dd.BoxNo
                    ,dd.OrderCount
                FROM TMS_DispatchDetail dd
                JOIN TMS_Dispatch d
                    ON d.DID=dd.DID
                        AND d.IsDeleted=0
                JOIN ExpressCompany ecd 
                    ON d.DepartureID=ecd.ExpressCompanyID
                JOIN ExpressCompany eca 
                    ON d.ArrivalID=eca.ExpressCompanyID
                WHERE dd.IsDeleted=0
                    AND dd.DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return (List<ViewDispatchBoxModel>)ExecuteSql_ByReaderReflect<ViewDispatchBoxModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public PrintDeliveryNoModel GetPrintDeliveryInfo(string deliveryNo)
        {
            string strSql = @"
                SELECT sysdate as PrintTime,d.DeliveryNo,cw.WaybillNo
                    ,d.CarrierID,c.CarrierName
                    ,d.DepartureID,ecd.CompanyName as DepartureName
                    ,d.ArrivalID,eca.CompanyName as ArrivalName
                    ,d.BoxCount
                    ,cw.TotalCount TotalOrderCount
                    ,cw.Weight TotalWeight
                    ,d.TotalAmount
                    ,tran.PlateNo
                    ,tran.Consignor
                    ,tran.Consignee
                    ,tran.ConsigneePhone
                    ,tran.ReceiveAddress
                FROM TMS_Dispatch d
                JOIN TMS_CarrierWaybill cw
                    ON cw.IsDeleted=0
                        AND cw.CWID=d.CarrierWaybillID
                JOIN TMS_Carrier c
                    ON c.IsDeleted=0
                        AND c.CarrierID=d.CarrierID
                JOIN ExpressCompany ecd
                    ON ecd.ExpressCompanyID=d.DepartureID
                JOIN ExpressCompany eca
                    ON eca.ExpressCompanyID=d.ArrivalID
                Left JOIN TMS_DispTransition tran
                    ON tran.DeliveryNo = d.DeliveryNo
                WHERE d.IsDeleted=0
                    AND d.DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlSingle_ByReaderReflect<PrintDeliveryNoModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<PrintDeliveryNoModel> GetPrintDeliveryInfo(IList<string> deliveryNoList)
        {
            if (deliveryNoList == null) throw new ArgumentNullException("deliveryNoList");
            if (deliveryNoList.Count == 0) throw new ArgumentException("打印提货单不能为空", "deliveryNoList");

            string strSql = @"
                SELECT sysdate as PrintTime,d.DeliveryNo,cw.WaybillNo
                    ,d.CarrierID,c.CarrierName
                    ,d.DepartureID,ecd.CompanyName as DepartureName
                    ,d.ArrivalID,eca.CompanyName as ArrivalName
                    ,d.BoxCount
                    ,cw.TotalCount TotalOrderCount
                    ,cw.Weight TotalWeight
                    ,d.TotalAmount
                    ,tran.PlateNo
                    ,tran.Consignor
                    ,tran.Consignee
                    ,tran.ConsigneePhone
                    ,tran.ReceiveAddress
                FROM TMS_Dispatch d
                JOIN TMS_CarrierWaybill cw
                    ON cw.IsDeleted=0
                        AND cw.CWID=d.CarrierWaybillID
                JOIN TMS_Carrier c
                    ON c.IsDeleted=0
                        AND c.CarrierID=d.CarrierID
                JOIN ExpressCompany ecd
                    ON ecd.ExpressCompanyID=d.DepartureID
                JOIN ExpressCompany eca
                    ON eca.ExpressCompanyID=d.ArrivalID
                Left JOIN TMS_DispTransition tran
                    ON tran.DeliveryNo = d.DeliveryNo
                WHERE d.IsDeleted=0
                    AND d.DeliveryNo in ({0})";
            string deliveryStr = string.Empty;
            deliveryNoList.ToList().ForEach(x =>
            {
                deliveryStr += string.Format("'{0}',", x);
            });
            deliveryStr = deliveryStr.TrimEnd(',');
            strSql = string.Format(strSql, deliveryStr);

            return ExecuteSql_ByReaderReflect<PrintDeliveryNoModel>(TMSReadOnlyConnection, strSql, null);
        }

        public List<ViewDispatchBoxModel> GetDispatchBoxList(int departureID, int arrivalID, Enums.GoodsType lineGoodsType)
        {
            string strSql = string.Format(@"
            SELECT b.DepartureID,ecd.CompanyName as DepartureName
                ,b.ArrivalID,eca.CompanyName as ArrivalName
                ,b.BoxNo,b.TotalCount as OrderCount
            FROM TMS_Box b
            JOIN EXPRESSCOMPANY ecd 
                ON b.DEPARTUREID=ecd.EXPRESSCOMPANYID
            JOIN EXPRESSCOMPANY eca
                ON b.ARRIVALID=eca.EXPRESSCOMPANYID 
            JOIN City 
                ON City.CityID=eca.CityID
            WHERE b.IsDeleted=0
                AND b.DepartureID=:DepartureID
                AND b.ArrivalID=:ArrivalID
                AND b.ContentType=:ContentType
                AND EXISTS(
                    SELECT 1
                    FROM TMS_BoxDetail bd
                    JOIN TMS_Order o
                        ON o.IsDeleted=0
                        AND o.OrderTMSStatus={0}
                        AND bd.FormCode=o.FormCode
                    WHERE bd.BoxNo=b.BoxNo
                )
                AND NOT EXISTS (
                    SELECT 1
                    FROM TMS_PreDispatch pd
                    JOIN TMS_LinePlan lp
                        ON lp.IsDeleted=0
                            AND lp.Status={1}
                            AND lp.LPID=pd.LPID
                            AND lp.IsEnabled=1
                    JOIN TMS_CARRIER c 
                        ON c.CarrierID=lp.CarrierID 
                            AND c.IsDeleted=0
                            AND c.Status={2}
                    WHERE pd.IsDeleted=0
                        AND pd.BoxNo=b.BoxNo
                        AND pd.DispatchStatus={3}
                    UNION ALL
                    SELECT 1
                    FROM TMS_DispatchDetail dd
                    JOIN TMS_Dispatch d
                        ON d.IsDeleted=0
                            AND d.DID=dd.DID
                            AND d.DeliveryStatus IN ({4})
                    WHERE dd.IsDeleted=0
                        AND dd.BoxNo=b.BoxNo
                    )"
            , (int)Enums.OrderTMSStatus.Normal
            , (int)Enums.LineStatus.Effective
            , (int)Enums.CarrierStatus.Valid
            , (int)Enums.DispatchStatus.CanDispatch
            , string.Join(",", base.GetDeliveryNotFinishStatus()));
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=departureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID},
                new OracleParameter() { ParameterName="ContentType",DbType= DbType.Int32,Value= (int)lineGoodsType}
            };
            return (List<ViewDispatchBoxModel>)ExecuteSql_ByReaderReflect<ViewDispatchBoxModel>(TMSReadOnlyConnection, strSql, arguments);
        }
        #endregion


        #region IDispatchDAL 成员

        /// <summary>
        /// 更新提货单为没有延误(复议审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        public int UpdateDeliveryToNoDelay(string deliveryNO)
        {
            if (String.IsNullOrWhiteSpace(deliveryNO)) throw new ArgumentNullException("deliveryNO is empty");
            string sql = @"
                    UPDATE TMS_Dispatch
                        SET
                            IsDelay = :IsDelay,
                            UpdateBy = :UpdateBy,
                            UpdateTime = sysdate
                        WHERE DeliveryNo = :DeliveryNo
                            AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNO},
                new OracleParameter() { ParameterName="IsDelay",DbType= DbType.Byte,Value= false},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 更新提货单为存在丢失(丢失审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        public int UpdateDeliveryToExistsLost(string deliveryNO)
        {
            if (String.IsNullOrWhiteSpace(deliveryNO)) throw new ArgumentNullException("deliveryNO is empty");
            string sql = @"
                    UPDATE TMS_Dispatch
                        SET
                            IsExistsLost = :IsExistsLost,
                            UpdateBy = :UpdateBy,
                            UpdateTime = sysdate
                        WHERE DeliveryNo = :DeliveryNo
                            AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNO},
                new OracleParameter() { ParameterName="IsExistsLost",DbType= DbType.Byte,Value= true},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IDispatchDAL 成员


        public PagedList<ViewDispatchModel> SearchEx(DispatchSearchModel searchModel)
        {
            StringBuilder sb = new StringBuilder(@"SELECT dp.CreateTime
                                                          ,dp.departureid
                                                          ,ec1.companyName DepartureName
                                                          ,dp.ArrivalID
                                                          ,ec.companyName ArrivalName
                                                          ,ct.cityName ArrivalCity
                                                          ,dp.BoxCount
                                                          ,cw.TotalCount OrderCount
                                                          ,cw.Weight TotalWeight
                                                          ,dp.CarrierID
                                                          ,c.CarrierName
                                                          ,lp.TransportType
                                                          ,lp.ArrivalTiming
                                                          ,dp.DeliveryNo
                                                          ,cw.WaybillNo
                                                          ,dp.linegoodstype
                                                          ,dp.Totalamount
                                                          ,dp.confirmexparrivaldate
                                                          ,lp.LPID
                                                          ,dp.DeliverySource DeliverySource
                                                          ,dp.DeliveryStatus+1 DispatchingPageStatus
                                                          ,dp.did
                                                          ,dp.DispatchTime
                                        FROM tms_dispatch dp 
                                        JOIN tms_carrierwaybill cw ON cw.cwid=dp.carrierwaybillid
                                        JOIN tms_lineplan lp ON lp.lpid=dp.lpid
                                        JOIN tms_carrier c ON c.carrierid=dp.carrierid
                                        JOIN expresscompany ec ON ec.expresscompanyid=dp.arrivalid
                                        JOIN expresscompany ec1 ON ec1.expresscompanyid=dp.departureid
                                        JOIN city ct ON ct.cityid=ec.cityid
                                        WHERE dp.isdeleted=0 AND dp.DeliveryStatus in (0,1)");
            List<OracleParameter> arguments = new List<OracleParameter>();
            if (searchModel.ArrivalID.HasValue)
            {
                sb.Append(" AND dp.arrivalid=:ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sb.Append(" AND dp.departureid=:DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sb.Append(" AND dp.carrierid=:CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sb.Append(" AND dp.createtime <= :EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sb.Append(" AND dp.createtime >= :StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                sb.Append(" AND lp.transporttype=:TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            if (searchModel.DispatchingPageStatus.HasValue)
            {
                sb.Append(" AND dp.deliverystatus=:deliverystatus");
                arguments.Add(new OracleParameter() { ParameterName = "deliverystatus", DbType = DbType.Int32, Value = (int)searchModel.DispatchingPageStatus.Value - 1 });
            }

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDispatchModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, arguments.ToArray());
        }

        public PagedList<ViewDeliveryPrintModel> SearchEx(DeliveryPrintSearchModel searchModel)
        {
            StringBuilder sb = new StringBuilder(@"
SELECT dp.DispatchTime AS DispatchTime
,dp.departureid
,ec1.companyName DepartureName
,dp.ArrivalID
,ec.companyName ArrivalName
,ct.cityName ArrivalCity
,dp.BoxCount
,cw.TotalCount OrderCount
,cw.Weight TotalWeight
,dp.CarrierID
,c.CarrierName
,lp.TransportType
,lp.ArrivalTiming
,dp.DeliveryNo
,cw.WaybillNo
,dp.linegoodstype
,dp.Totalamount
,dp.ProtectedPrice
,dp.confirmexparrivaldate
,dp.ExpectArrivalDate
,lp.LPID
,dp.DeliverySource DeliverySource
,dp.DeliveryStatus 
,dp.did
,tran.PlateNo
,tran.Consignor
,tran.Consignee
,tran.ConsigneePhone
,tran.ReceiveAddress
FROM tms_dispatch dp 
JOIN tms_carrierwaybill cw ON cw.cwid = dp.carrierwaybillid
JOIN tms_lineplan lp ON lp.lpid = dp.lpid
JOIN tms_carrier c ON c.carrierid = dp.carrierid
JOIN expresscompany ec ON ec.expresscompanyid = dp.arrivalid
JOIN expresscompany ec1 ON ec1.expresscompanyid = dp.departureid
JOIN city ct ON ct.cityid = ec.cityid
Left JOIN TMS_DispTransition tran ON tran.DeliveryNo = dp.DeliveryNo
WHERE dp.isdeleted = 0 AND dp.deliverystatus > 0
");
            List<OracleParameter> arguments = new List<OracleParameter>();
            if (searchModel.ArrivalID.HasValue)
            {
                sb.Append(" AND dp.arrivalid = :ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", OracleDbType = OracleDbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sb.Append(" AND dp.departureid = :DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", OracleDbType = OracleDbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sb.Append(" AND dp.carrierid = :CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", OracleDbType = OracleDbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sb.Append(" AND dp.DispatchTime <= :EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", OracleDbType = OracleDbType.Date, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sb.Append(" AND dp.DispatchTime >= :StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", OracleDbType = OracleDbType.Date, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                sb.Append(" AND lp.transporttype = :TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", OracleDbType = OracleDbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                sb.Append(" AND dp.deliverystatus = :deliverystatus");
                arguments.Add(new OracleParameter() { ParameterName = "deliverystatus", OracleDbType = OracleDbType.Int32, Value = (int)searchModel.DeliveryStatus.Value });
            }

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDeliveryPrintModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, arguments.ToArray());
        }

        public ViewDispatchStatisticModel GetStatisticInfoEx(DispatchSearchModel searchModel)
        {
            StringBuilder sb = new StringBuilder(@"SELECT dp.CreateTime ReceiveOrderTime
                                                          ,dp.departureid
                                                          ,ec1.companyName DepartureName
                                                          ,dp.ArrivalID
                                                          ,ec.companyName ArrivalName
                                                          ,ct.cityName ArrivalCity
                                                          ,dp.BoxCount
                                                          ,cw.TotalCount OrderCount
                                                          ,cw.Weight TotalWeight
                                                          ,dp.CarrierID
                                                          ,c.CarrierName
                                                          ,lp.TransportType
                                                          ,lp.ArrivalTiming
                                                          ,dp.DeliveryNo
                                                          ,cw.WaybillNo
                                                          ,dp.linegoodstype
                                                          ,dp.Totalamount
                                                          ,dp.confirmexparrivaldate
                                                          ,lp.LPID
                                                          ,dp.DeliverySource DeliverySource
                                                          ,dp.DeliveryStatus+1 DispatchingPageStatus
                                                          ,dp.did
                                        FROM tms_dispatch dp 
                                        JOIN tms_carrierwaybill cw ON cw.cwid=dp.carrierwaybillid
                                        JOIN tms_lineplan lp ON lp.lpid=dp.lpid
                                        JOIN tms_carrier c ON c.carrierid=dp.carrierid
                                        JOIN expresscompany ec ON ec.expresscompanyid=dp.arrivalid
                                        JOIN expresscompany ec1 ON ec1.expresscompanyid=dp.departureid
                                        JOIN city ct ON ct.cityid=ec.cityid
                                        WHERE dp.isdeleted=0  AND dp.DeliveryStatus in (0,1)");
            List<OracleParameter> arguments = new List<OracleParameter>();
            if (searchModel.ArrivalID.HasValue)
            {
                sb.Append(" AND dp.arrivalid=:ArrivalID");
                arguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sb.Append(" AND dp.departureid=:DepartureID");
                arguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                sb.Append(" AND dp.carrierid=:CarrierID");
                arguments.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                sb.Append(" AND dp.createtime <= :EndTime");
                arguments.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime.Value });
            }
            if (searchModel.StartTime.HasValue)
            {
                sb.Append(" AND dp.createtime >= :StartTime");
                arguments.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.StartTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                sb.Append(" AND lp.transporttype=:TransportType");
                arguments.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType.Value });
            }
            if (searchModel.DispatchingPageStatus.HasValue)
            {
                sb.Append(" AND dp.deliverystatus=:deliverystatus");
                arguments.Add(new OracleParameter() { ParameterName = "deliverystatus", DbType = DbType.Int32, Value = (int)searchModel.DispatchingPageStatus.Value });
            }

            List<ViewDispatchModel> list = ExecuteSql_ByReaderReflect<ViewDispatchModel>(TMSReadOnlyConnection, sb.ToString(), arguments.ToArray()) as List<ViewDispatchModel>;
            ViewDispatchStatisticModel vdsModel = new ViewDispatchStatisticModel();
            vdsModel.PreDispatchCount = list.FindAll(m => m.DispatchingPageStatus == Enums.DispatchingPageStatus.PreDispatched).Count;
            vdsModel.TotalCount = list.Count;
            vdsModel.DispatchedCount = list.FindAll(m => m.DispatchingPageStatus == Enums.DispatchingPageStatus.Dispatched).Count;

            return vdsModel;


        }

        #endregion

        #region IDispatchDAL 成员


        public int UpdateEx(DispatchModel model)
        {
            string sql = @"UPDATE tms_dispatch SET 
                                 DeliveryNo=:DeliveryNo
                                ,TotalAmount=:TotalAmount
                                ,BoxCount=:BoxCount 
                                ,lpid=:lpid 
                                ,UpdateTime=sysdate
                                ,UpdateBy=:UpdateBy
                                ,DeliveryStatus=:DeliveryStatus
                                ,CarrierID=:CarrierID
                                ,TransportType=:TransportType
                                ,ProtectedPrice=:ProtectedPrice
                                ,LineGoodsType=:LineGoodsType
                                ,DispatchTime=:DispatchTime
                                WHERE did=:did AND IsDeleted=0";

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="TotalAmount",DbType= DbType.Decimal,Value= model.TotalAmount},
                new OracleParameter() { ParameterName="BoxCount",DbType= DbType.Int32,Value=model.BoxCount},
                new OracleParameter() { ParameterName="lpid",DbType= DbType.Int32,Value=model.LPID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Decimal,Value= model.UpdateBy},
                new OracleParameter() { ParameterName="did",DbType= DbType.Int32,Value=model.DID},
                new OracleParameter() { ParameterName="DeliveryStatus",DbType= DbType.Int32,Value=(int)model.DeliveryStatus},
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=model.CarrierID},
                new OracleParameter() { ParameterName="TransportType",DbType= DbType.Int32,Value=(int)model.TransportType},
                new OracleParameter() { ParameterName="ProtectedPrice",DbType= DbType.Decimal,Value= model.ProtectedPrice},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=(int)model.LineGoodsType},
                new OracleParameter() { ParameterName="DispatchTime",DbType= DbType.DateTime,Value=model.DispatchTime.HasValue?model.DispatchTime.Value:DateTime.Now}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IDispatchDAL 成员


        public List<long> GetDispatchDetailKeyIDList(long DID)
        {
            String sql = @"
SELECT dispDetail.DDID
FROM TMS_Dispatch disp
    JOIN TMS_DispatchDetail dispDetail ON disp.DID = dispDetail.DID
WHERE disp.DID = :DID AND disp.Isdeleted = 0 AND dispDetail.Isdeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "DID",  DbType = DbType.Int64, Value = DID }
            };
            var dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, parameters);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<long> listResult = new List<long>();
                foreach (DataRow item in dtResult.Rows)
                {
                    listResult.Add(long.Parse(item[0].ToString()));
                }
                return listResult;
            }
            return null;
        }

        #endregion

        #region IDispatchDAL 成员


        public List<long> GetDispatchIsPlanedPDID(long did)
        {
            string sql = string.Format(@"
SELECT dispde.pdid AS PDID
FROM TMS_DISPATCH disp
JOIN TMS_DISPATCHDETAIL dispde ON disp.did = dispde.did and dispde.PDID > 0 
where disp.did = :did and disp.Isdeleted = 0
");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="did",DbType= DbType.Int64,Value = did},
            };
            DataTable dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, arguments);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<long> listPDID = new List<long>();
                foreach (DataRow item in dtResult.Rows)
                {
                    listPDID.Add(Convert.ToInt32(item["PDID"].ToString()));
                }
                return listPDID;
            }
            return null;
        }

        #endregion

        #region IDispatchDAL 成员


        public int UpdateTotalAmountByID(long did, decimal price, decimal protectedPrice)
        {
            string sql = @"
UPDATE TMS_Dispatch 
SET TotalAmount = :TotalAmount 
    ,ProtectedPrice = :ProtectedPrice 
WHERE did=:did
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="did", OracleDbType = OracleDbType.Int64 ,Value = did },
                new OracleParameter() { ParameterName="TotalAmount", OracleDbType = OracleDbType.Decimal,Value = price},
                new OracleParameter() { ParameterName="ProtectedPrice", OracleDbType = OracleDbType.Decimal,Value = protectedPrice}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IDispatchDAL 成员


        public List<Model.Log.DeliveryFlowLogModel> GetDeliveryInfo4TMS2ThridParty(Model.Synchronous.OutSync.Tms2ThridPartyJobArgs args)
        {
            if (args == null) throw new ArgumentNullException("args is null");
            String sql = String.Format(@"
SELECT *
FROM
(
    SELECT DFLID, DELIVERYNO, FLOWTYPE, OPERATEBY, OPERATETIME
    FROM TMS_DELIVERYFLOWLOG
    WHERE OPERATETIME > to_date(:SyncTime,'yyyy-mm-dd hh24:mi:ss')
          AND   SYNCTOLMS = :SyncStatus
          AND FLOWTYPE IN ({0},{1})
    ORDER BY   OPERATETIME ASC
)  T
WHERE ROWNUM < :TopCount
",
 (int)Enums.DeliverFlowType.Outbound,
 (int)Enums.DeliverFlowType.Inbound
 );
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="SyncTime" , DbType = System.Data.DbType.String, Value = args.SyncTime.ToString("yyyy-MM-dd HH:mm:ss")  },
                new OracleParameter() { ParameterName="SyncStatus" , DbType = System.Data.DbType.Int16, Value = (int)Enums.SyncStatus.NotYet   },
                new OracleParameter() { ParameterName="TopCount" , DbType = System.Data.DbType.Int32, Value = args.TopCount  }
            };
            var result = ExecuteSql_ByReaderReflect<DeliveryFlowLogModel>(TMSWriteConnection, sql, arguments);
            if (result != null)
            {
                return result.ToList();
            }
            return null;
        }

        /// <summary>
        /// 更新TMS提货单日志表同步标记
        /// </summary>
        /// <param name="keyID"></param>
        public void UpdateDeliveryFlowSyncFlag4TMS2ThridParty(long keyID, Enums.SyncStatus sync)
        {
            String sql = @"
UPDATE TMS_DELIVERYFLOWLOG
SET SYNCTOLMS = :SyncStatus, SyncTime = SYSDATE
WHERE DFLID = :DFLID
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="SyncStatus" , DbType = System.Data.DbType.Int16, Value = (int)sync   },
                new OracleParameter() { ParameterName="DFLID" , DbType = System.Data.DbType.Int64, Value = keyID }
            };
            ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public List<AgingMonitoringLogProxyModel> GetTMSAgingMonitoringLogList(DispatchModel dispModel, Enums.DeliverFlowType FlowType)
        {
            if (dispModel == null) throw new ArgumentNullException("DispatchModel IS NULL");
            String sql = String.Format(@"
SELECT dispordetail.DODID, dispordetail.FORMCODE, disp.transporttype, od.LMSWaybillType AS BillType, {0} AS TransportStationID
FROM TMS_DISPATCH disp
     JOIN TMS_DISPORDERDETAIL dispordetail ON disp.deliveryno = dispordetail.deliveryno
     JOIN TMS_Order od ON dispordetail.FORMCODE = od.FORMCODE
WHERE disp.DELIVERYNO = :DELIVERYNO
AND disp.IsDeleted = 0
AND dispordetail.isdeleted = 0
", FlowType == Enums.DeliverFlowType.Outbound ? "disp.DepartureID" : "disp.ArrivalID"
 );
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="DELIVERYNO" , DbType = System.Data.DbType.String, Value =  dispModel.DeliveryNo  }
            };
            var listResult = ExecuteSql_ByReaderReflect<AgingMonitoringLogProxyModel>(TMSWriteConnection, sql, arguments);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion

        #region IDispatchDAL 成员
        public int UpdateDispatchBoxCount(DispatchModel model)
        {
            String strSql = @"UPDATE TMS_Dispatch SET Boxcount=:Boxcount,UpdateBy=:UpdateBy,UpdateTime=SysDate WHERE DID=:DID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DID",DbType= DbType.Decimal,Value=model.DID},
                new OracleParameter() { ParameterName="Boxcount",DbType= DbType.Decimal,Value=model.BoxCount},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Decimal,Value=model.UpdateBy},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }
        #endregion
    }
}

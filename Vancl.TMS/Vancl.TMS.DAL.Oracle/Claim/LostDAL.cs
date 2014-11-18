using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Claim.Lost;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Claim
{
    public class LostDAL : BaseDAL, ILostDAL
    {
        #region ILostDAL 成员

        public bool IsExistNotApproveInfo(string deliveryNo)
        {
            string strSql = string.Format(@"
                SELECT COUNT(*) CC
                FROM TMS_Lost
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0
                    AND ApproveStatus IN ({0},{1})
                ", (int)Enums.ApproveStatus.Dismissed, (int)Enums.ApproveStatus.NotApprove);
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                if (Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsExistNotInputLostInfo(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(*) CC
                FROM TMS_DISPORDERDETAIL
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0
                    AND IsArrived=0";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                if (Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public IList<ViewLostModel> Search(LostSearchModel searchModel)
        {
            List<OracleParameter> arguments = new List<OracleParameter>();
            StringBuilder sbWhere = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                sbWhere.Append(" AND A.DeliveryNO=:DeliveryNO");
                arguments.Add(new OracleParameter() { ParameterName = "DeliveryNO", DbType = DbType.String, Value = searchModel.DeliveryNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierWaybillNO))
            {
                sbWhere.Append(" AND W.WAYBILLNO=:WaybillNo");
                arguments.Add(new OracleParameter() { ParameterName = "WaybillNo", DbType = DbType.String, Value = searchModel.CarrierWaybillNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.ArrivalCity))
            {
                sbWhere.Append(" AND City.CityName like :CityName || '%' ");
                arguments.Add(new OracleParameter() { ParameterName = "CityName", DbType = DbType.String, Value = searchModel.ArrivalCity });
            }
            if (searchModel.CreateDateBegin.HasValue)
            {
                sbWhere.Append(" AND siteass.leavetime  >= :CreateDateBegin");
                arguments.Add(new OracleParameter() { ParameterName = "CreateDateBegin", DbType = DbType.DateTime, Value = searchModel.CreateDateBegin.Value });
            }
            if (searchModel.CreateDateEnd.HasValue)
            {
                sbWhere.Append(" AND  siteass.leavetime <= :CreateDateEnd");
                arguments.Add(new OracleParameter() { ParameterName = "CreateDateEnd", DbType = DbType.DateTime, Value = searchModel.CreateDateEnd.Value });
            }
            if (searchModel.ApproveStatus.HasValue)
            {
                sbWhere.Append(" AND L.ApproveStatus = :ApproveStatus");
                arguments.Add(new OracleParameter() { ParameterName = "ApproveStatus", DbType = DbType.Int32, Value = (int)searchModel.ApproveStatus });
            }
            StringBuilder sbSql = new StringBuilder();
            string strSql_Lost = string.Format(@"
SELECT  siteass.leavetime AS DepartureTime, E1.COMPANYNAME DepartureName,E2.COMPANYNAME ArrivalName
    ,City.CityName ArrivalCity,A.BOXCOUNT TotalCount,C.CarrierName 
    ,A.TransPortType,A.ArrivalTiming,A.DeliveryNO,W.WAYBILLNO CARRIERWAYBILLNO
    ,A.LineGoodsType,A.TotalAmount,A.ConfirmExpArrivalDate,A.DeliveryStatus
    ,1 IsAddedLost,L.ApproveStatus,L.ISALLLOST,L.LOSTAMOUNT 
FROM TMS_Lost L
    JOIN TMS_DISPATCH A ON A.DeliveryNo=L.DeliveryNo AND A.IsDeleted=0 AND A.DeliveryStatus = {0}
    JOIN TMS_SITEASSESSMENT siteAss ON A.deliveryno = siteAss.Deliveryno
    JOIN EXPRESSCOMPANY E1 ON A.DEPARTUREID = E1.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY E2 ON A.ARRIVALID = E2.EXPRESSCOMPANYID
    JOIN City ON City.CityID = E2.CityID
    JOIN TMS_CARRIER C ON A.CARRIERID = C.CARRIERID AND C.IsDeleted = 0
    JOIN TMS_CARRIERWAYBILL W ON W.CWID = A.CARRIERWAYBILLID AND W.IsDeleted = 0
WHERE L.IsDeleted = 0
", (int)Enums.DeliveryStatus.InTransit);
            strSql_Lost += sbWhere.ToString();
            if (searchModel.IsInput.HasValue && searchModel.IsInput.Value)
            {
                sbWhere.Append(" AND 1=2 ");
            }
            string strSql_Dispatch = string.Format(@"
SELECT  siteass.leavetime AS DepartureTime, E1.COMPANYNAME DepartureName,E2.COMPANYNAME ArrivalName
    ,City.CityName ArrivalCity,A.BOXCOUNT TotalCount,C.CarrierName
    ,A.TransPortType,A.ArrivalTiming,A.DeliveryNO,W.WAYBILLNO CARRIERWAYBILLNO
    ,A.LineGoodsType,A.TotalAmount,A.ConfirmExpArrivalDate,A.DeliveryStatus
    ,0 IsAddedLost,NULL ApproveStatus,1 ISALLLOST,0 LOSTAMOUNT
FROM TMS_DISPATCH A
JOIN TMS_SITEASSESSMENT siteAss ON A.deliveryno = siteAss.Deliveryno
    JOIN EXPRESSCOMPANY E1 ON A.DEPARTUREID = E1.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY E2 ON A.ARRIVALID = E2.EXPRESSCOMPANYID
    JOIN City ON City.CityID = E2.CityID
    JOIN TMS_CARRIER C ON A.CARRIERID = C.CARRIERID AND C.IsDeleted = 0
    JOIN TMS_CARRIERWAYBILL W ON W.CWID = A.CARRIERWAYBILLID AND W.IsDeleted = 0
WHERE A.IsDeleted = 0
    AND A.DeliveryStatus = {0}
", (int)Enums.DeliveryStatus.InTransit);
            strSql_Dispatch += sbWhere.ToString();
            if (searchModel.IsInput.HasValue && !searchModel.IsInput.Value)
            {
                strSql_Dispatch += @"
                    AND NOT EXISTS (
                        SELECT 1
                        FROM TMS_Lost l
                        WHERE l.DeliveryNo = A.DeliveryNo
                            AND l.IsDeleted = 0
                    )
                ";
            }
            string strSql_Head = @"
SELECT MAX(DepartureTime) DepartureTime,MAX(DepartureName) DepartureName
    ,MAX(ArrivalName) ArrivalName,MAX(ArrivalCity) ArrivalCity
    ,MAX(TotalCount) TotalCount,MAX(CarrierName) CarrierName
    ,MAX(TransPortType) TransPortType,MAX(ArrivalTiming) ArrivalTiming
    ,DeliveryNO,MAX(CARRIERWAYBILLNO) CARRIERWAYBILLNO
    ,MAX(LineGoodsType) LineGoodsType,MAX(TotalAmount) TotalAmount
    ,MAX(ConfirmExpArrivalDate) ConfirmExpArrivalDate,MAX(DeliveryStatus) DeliveryStatus
    ,MAX(IsAddedLost) IsAddedLost,MAX(ApproveStatus) ApproveStatus,MAX(ISALLLOST) ISALLLOST 
    ,MAX(LOSTAMOUNT) LOSTAMOUNT
FROM (
";
            string strSql_GroupBy = @"
                ) t
                GROUP BY DeliveryNO";
            if (searchModel.IsAddLost)
            {
                if (searchModel.IsInput.HasValue)
                {
                    if (searchModel.IsInput.Value)
                    {
                        sbSql.Append(strSql_Lost);
                    }
                    else
                    {
                        sbSql.Append(strSql_Dispatch);
                    }
                }
                else
                {
                    sbSql.Append(strSql_Head)
                        .Append(strSql_Lost)
                        .Append(@"
                            UNION ALL
                        ")
                        .Append(strSql_Dispatch)
                        .Append(strSql_GroupBy);
                }
            }
            else
            {
                sbSql.Append(strSql_Lost);
            }
            searchModel.OrderByString = "DELIVERYNO";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewLostModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, arguments.ToArray());
        }

        public IList<ViewLostBoxModel> GetBoxDetail(string deliveryNo)
        {
            string strSql = @"
SELECT box.BoxNo , box.CustomerBatchNo,box.TotalCount AS OrderCount ,box.TotalAmount,0 as BoxLostStatus
FROM TMS_Dispatch Disp
JOIN TMS_DispatchDetail DispDetail ON Disp.Did = DispDetail.Did AND Disp.DeliveryNo = DispDetail.DeliveryNo 
JOIN TMS_BOX  box ON DispDetail.BOXNO = box.BOXNO AND box.departureID = Disp.departureID AND box.ArrivalID = Disp.ArrivalID
WHERE Disp.IsDeleted = 0
    AND DispDetail.Isdeleted = 0
    AND Disp.DeliveryNo = :DeliveryNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSql_ByReaderReflect<ViewLostBoxModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<LostOrderDetailModel> GetPreLostOrderDetail(string deliveryNo)
        {
            string strSql = @"
SELECT box.BoxNo , box.CustomerBatchNo BoxNo, DispOrDetail.FormCode
FROM TMS_Dispatch Disp
JOIN TMS_DispatchDetail DispDetail ON Disp.Did = DispDetail.Did AND Disp.DeliveryNo = DispDetail.DeliveryNo 
JOIN TMS_DispOrderDetail DispOrDetail ON DispDetail.DDID = DispOrDetail.DDID AND  DispOrDetail.DeliveryNo = DispDetail.DeliveryNo 
JOIN TMS_BOX  box ON DispDetail.BOXNO = box.BOXNO AND box.departureID = Disp.departureID AND box.ArrivalID = Disp.ArrivalID
WHERE Disp.IsDeleted = 0
    AND DispDetail.Isdeleted = 0
    AND Disp.DeliveryNo = :DeliveryNo
    AND DispOrDetail.IsArrived = 0
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSql_ByReaderReflect<LostOrderDetailModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<LostOrderDetailModel> GetLostOrderDetail(string deliveryNo)
        {
            string strSql = @"
                SELECT ld.BoxNo,ld.FormCode
                FROM TMS_LostDetail ld
                JOIN TMS_Lost l
                    ON l.LID=ld.LID
                        AND l.DeliveryNo=:DeliveryNo
                        AND l.IsDeleted=0
                        AND l.IsAllLost=0
                WHERE ld.IsDeleted=0
                UNION ALL
                SELECT d.BoxNo,d.FormCode
                FROM TMS_DispOrderDetail d
                JOIN TMS_Lost l
                    ON d.DeliveryNo=l.DeliveryNo
                        AND l.IsDeleted=0
                        AND l.IsAllLost=1
                WHERE d.IsDeleted=0
                    AND d.DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSql_ByReaderReflect<LostOrderDetailModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int Add(LostModel model)
        {
            string strSql = @"
                INSERT INTO TMS_Lost(
                    LID
                    ,DeliveryNo
                    ,DepartureID
                    ,ArrivalID
                    ,IsAllLost
                    ,LostAmount
                    ,ProtectedPrice
                    ,ApproveStatus
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted)
                VALUES(
                    :LID
                    ,:DeliveryNo
                    ,:DepartureID
                    ,:ArrivalID
                    ,:IsAllLost
                    ,:LostAmount
                    ,:ProtectedPrice
                    ,:ApproveStatus
                    ,:CreateBy
                    ,:UpdateBy
                    ,0)";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "LID", DbType = DbType.Int64, Value = model.LID },
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = model.DeliveryNo },
                new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = model.DepartureID },
                new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = model.ArrivalID },
                new OracleParameter() { ParameterName = "IsAllLost", DbType = DbType.Byte, Value = model.IsAllLost },
                new OracleParameter() { ParameterName = "LostAmount", DbType = DbType.Decimal, Value = model.LostAmount },
                new OracleParameter() { ParameterName = "ProtectedPrice", DbType = DbType.Decimal, Value = model.ProtectedPrice },
                new OracleParameter() { ParameterName = "ApproveStatus", DbType = DbType.Int32, Value = (int)model.ApproveStatus },
                new OracleParameter() { ParameterName = "CreateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public IList<LostOrderDetailModel> GetOrderDetail(string deliveryNo, List<string> lstBoxNo)
        {
            string strSql = string.Format(@"
                SELECT BoxNo,FormCode
                FROM TMS_DISPORDERDETAIL
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo
                    AND BoxNo IN ({0})", "'" + string.Join("','", lstBoxNo) + "'");
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSql_ByReaderReflect<LostOrderDetailModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<ViewLostOrderModel> GetOrderList(string boxNo)
        {
            string strSql = @"
SELECT dd.BoxNo, dd.FormCode, dd.Price ,dd.ProtectedPrice,b.CustomerBatchNo CustomerBoxNo
FROM TMS_DispOrderDetail dd
LEFT JOIN TMS_Box b ON b.BoxNo=dd.BoxNo
WHERE dd.IsDeleted = 0
    AND dd.BoxNo = :BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = boxNo } 
            };
            return ExecuteSql_ByReaderReflect<ViewLostOrderModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<ViewLostOrderModel> GetLostOrderList(string boxNo)
        {
            string strSql = @"
                SELECT dd.BoxNo,dd.FormCode,dd.Price,dd.ProtectedPrice,b.CustomerBatchNo CustomerBoxNo
                    FROM TMS_DispOrderDetail dd
                    LEFT JOIN TMS_Box b ON b.BoxNo=dd.BoxNo
                    WHERE dd.IsDeleted=0
                        AND dd.BoxNo = :BoxNo
                        AND dd.IsArrived = 0";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = boxNo } 
            };
            return ExecuteSql_ByReaderReflect<ViewLostOrderModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public bool IsExistLost(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(*) CC
                FROM TMS_Lost
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                if (Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public LostModel Get(string deliveryNo)
        {
            string strSql = @"
                SELECT LID,DeliveryNo,DepartureID,ArrivalID
                    ,IsAllLost,LostAmount,ProtectedPrice,ApproveBy,ApproveTime
                    ,ApproveStatus,CreateBy,UpdateBy,CreateTime
                    ,UpdateTime,IsDeleted
                FROM TMS_Lost
                WHERE IsDeleted=0
                    AND DeliveryNo = :DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSqlSingle_ByReaderReflect<LostModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int Approve(string deliveryNo, Enums.ApproveStatus approveStatus)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Lost
                SET ApproveStatus=:ApproveStatus
                    ,ApproveTime=sysdate
                    ,ApproveBy=:ApproveBy
                WHERE IsDeleted=0
                    AND DeliveryNo = :DeliveryNo
                    AND ApproveStatus={0}", (int)Enums.ApproveStatus.NotApprove);
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ApproveStatus", DbType = DbType.Int32, Value = (int)approveStatus },
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo },
                new OracleParameter() { ParameterName = "ApproveBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(string deliveryNo)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Lost
                SET IsDeleted=1
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                WHERE IsDeleted=0
                    AND DeliveryNo = :DeliveryNo");
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo },
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int UpdateOrderTMSStatus(string deliveryNo, bool isAllLost)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Order o
                SET o.OrderTMSStatus={0}
                    ,o.UpdateBy=:UpdateBy
                    ,o.UpdateTime=sysdate
                WHERE o.IsDeleted=0
                    AND o.OrderTMSStatus={1}
                    AND EXISTS (
                        SELECT 1
                        FROM {2} d
                        WHERE d.IsDeleted=0
                            AND d.DeliveryNo=:DeliveryNo
                            AND d.FormCode=o.FormCode
                    )"
                 , (int)Enums.OrderTMSStatus.Lost
                 , (int)Enums.OrderTMSStatus.Normal
                 , isAllLost ? "TMS_DispOrderDetail" : "TMS_LostDetail");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo },
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public decimal GetLostDeduction(string deliveryNo)
        {
            string strSql = @"
                SELECT ProtectedPrice
                FROM TMS_Lost
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo }
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToDecimal(o);
            }
            return 0;
        }

        #endregion
    }
}

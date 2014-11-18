using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.Model.Claim;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.DAL.Oracle.Claim
{
    public class ExpectDelayDAL : BaseDAL, IExpectDelayDAL
    {
        #region IExpectDelayDAL 成员

        public PagedList<Model.Claim.ViewExpectDelayModel> Search(ExpectDelaySearchModel searchModel)
        {
            List<OracleParameter> OracleParameterList = new List<OracleParameter>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
SELECT 
        d.DID as DispatchID,
        d.DeliveryNo,
        ed.CarrierWaybillNo as WaybillNo,
        siteass.leavetime AS DepartureTime,
        d.DepartureID,
        ecd.Companyname as DepartureName,
        d.ArrivalID,
        eca.Companyname as ArrivalName,
        City.Cityname as ArrivalCity,
        cw.TotalCount,
        d.CarrierID,
        c.CarrierName,
        d.TransportType,
        d.ArrivalTiming,
        d.LineGoodsType,
        d.TotalAmount,
        d.ExpectArrivalDate,
        d.DeliveryStatus,
        ed.EDID as ExpectDelayID,
        ed.DelayTime,
        ed.Delaydesc,
        ed.ExpectDelayType,
        d.ConfirmExpArrivalDate,
        ed.ApproveStatus
FROM TMS_Dispatch d
JOIN TMS_SITEASSESSMENT siteAss ON d.deliveryno = siteAss.Deliveryno
LEFT JOIN TMS_ExpectDelay ed ON ed.DeliveryNo = d.DeliveryNo  AND ed.IsDeleted=0
JOIN ExpressCompany ecd  ON ecd.ExpresscompanyID = d.DepartureID
JOIN ExpressCompany eca  ON eca.ExpresscompanyID = d.ArrivalID
JOIN City   ON city.CityID=eca.CityID
JOIN TMS_CarrierWaybill cw ON cw.CWID = d.CarrierWaybillID 
JOIN TMS_Carrier c  ON c.CarrierID = d.CarrierID  AND c.IsDeleted=0
WHERE d.IsDeleted=0
    AND d.DeliveryStatus={0}   
", (int)Enums.DeliveryStatus.InTransit);

            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                sbSql.Append(" AND d.DeliveryNO = :DeliveryNO");
                OracleParameterList.Add(new OracleParameter { ParameterName = "DeliveryNO", DbType = DbType.String, Value = searchModel.DeliveryNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierWaybillNO))
            {
                sbSql.Append(" AND ed.CarrierWaybillNO = :CarrierWaybillNO");
                OracleParameterList.Add(new OracleParameter { ParameterName = "CarrierWaybillNO", DbType = DbType.String, Value = searchModel.CarrierWaybillNO });
            }
            if (searchModel.CreateDateBegin.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime >= :CreateDateBegin");
                OracleParameterList.Add(new OracleParameter { ParameterName = "CreateDateBegin", DbType = DbType.DateTime, Value = searchModel.CreateDateBegin.Value });
            }
            if (searchModel.CreateDateEnd.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime <= :CreateDateEnd");
                OracleParameterList.Add(new OracleParameter { ParameterName = "CreateDateEnd", DbType = DbType.DateTime, Value = Convert.ToDateTime(searchModel.CreateDateEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            if (searchModel.ApproveStatus.HasValue)
            {//审核状态
                sbSql.AppendFormat(" AND ed.ApproveStatus  = {0}", (int)searchModel.ApproveStatus.Value);
            }
            if (searchModel.IsApply)
            {//申请
                sbSql.AppendFormat(" AND (ed.ApproveStatus IS NULL OR ed.ApproveStatus IN ({0}))", (int)Enums.ApproveStatus.NotApprove + "," + (int)Enums.ApproveStatus.Dismissed);
            }
            else
            {//审核
                sbSql.Append(" AND ed.ApproveStatus IS NOT NULL ");
            }
            if (searchModel.ArrivalTimeBegin.HasValue)
            {
                sbSql.Append(" AND d.CONFIRMEXPARRIVALDATE>=:ArrivalTimeBegin");
                OracleParameterList.Add(new OracleParameter { ParameterName = "ArrivalTimeBegin", DbType = DbType.DateTime, Value = searchModel.ArrivalTimeBegin.Value });
            }
            if (searchModel.ArrivalTimeEnd.HasValue)
            {
                sbSql.Append(" AND d.CONFIRMEXPARRIVALDATE<=:ArrivalTimeEnd");
                OracleParameterList.Add(new OracleParameter { ParameterName = "ArrivalTimeEnd", DbType = DbType.DateTime, Value = Convert.ToDateTime(searchModel.ArrivalTimeEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            if (searchModel.DepartureID.HasValue)
            {
                sbSql.Append(" AND d.DepartureID=:DepartureID");
                OracleParameterList.Add(new OracleParameter { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.ArrivalID.HasValue)
            {
                sbSql.Append(" AND d.ArrivalID=:ArrivalID");
                OracleParameterList.Add(new OracleParameter { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.IsInput.HasValue)
            {
                if (searchModel.IsInput.Value)
                {
                    sbSql.Append(" AND ed.EDID IS NOT NULL");
                }
                else
                {
                    sbSql.Append(" AND ed.EDID IS NULL");
                }
            }
            sbSql.Append(" ORDER BY ed.CreateTime DESC");

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewExpectDelayModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, OracleParameterList.ToArray());
        }

        public ViewExpectDelayModel GetViewExpectDelayModel(long dispatchID)
        {
            string sql = @"
            SELECT 
                   TMS_Dispatch.DID as DispatchID,
                   TMS_Dispatch.DeliveryNo,
                   TMS_ExpectDelay.CarrierWaybillNo as WaybillNo,
                   TMS_Dispatch.CreateTime,
                   TMS_Dispatch.DepartureID,
                   departure.Companyname as DepartureName,
                   TMS_dispatch.ArrivalID,
                   arrival.Companyname as ArrivalName,
                   City.Cityname as ArrivalCity,
                   TMS_CarrierWaybill.TotalCount,
                   TMS_Dispatch.CarrierID,
                   TMS_Carrier.CarrierName,
                   TMS_dispatch.TransportType,
                   TMS_dispatch.ArrivalTiming,
                   TMS_dispatch.LineGoodsType,
                   TMS_dispatch.TotalAmount,
                   TMS_dispatch.ExpectArrivalDate,
                   TMS_dispatch.DeliveryStatus,
                   TMS_ExpectDelay.DelayTime,
                   TMS_ExpectDelay.Delaydesc,
                   TMS_ExpectDelay.ExpectDelayType,
                   TMS_Dispatch.ConfirmExpArrivalDate,
                   TMS_Expectdelay.ApproveStatus       
            FROM TMS_Dispatch
            LEFT JOIN TMS_expectdelay on TMS_expectdelay.deliveryno = TMS_dispatch.deliveryno and TMS_expectdelay.IsDeleted=0
            JOIN ExpressCompany departure on departure.Expresscompanyid = TMS_dispatch.DepartureID
            JOIN ExpressCompany arrival on arrival.Expresscompanyid = TMS_dispatch.arrivalid
            JOIN City on city.cityid=arrival.cityid
            JOIN TMS_carrierwaybill on TMS_carrierwaybill.CWID = TMS_dispatch.CarrierWaybillID 
            JOIN TMS_carrier on TMS_carrier.carrierid = TMS_dispatch.CarrierID AND TMS_carrier.IsDeleted=0
            WHERE TMS_Dispatch.DID = :DispatchID
        ";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DispatchID", DbType = DbType.Int32, Value = dispatchID } 
            };

            var list = ExecuteSql_ByReaderReflect<ViewExpectDelayModel>(TMSReadOnlyConnection, sql, arguments);
            return list.Count > 0 ? list[0] : null;
        }

        public int Add(ExpectDelayModel model)
        {
            string sql = string.Format(@"
                    insert into TMS_ExpectDelay
                      (EDID, DeliveryNo, CarrierWaybillNo, ExpectDelayType, DelayTime, DelayDesc, ExpectTime, ApproveStatus, CreateBy, UpdateBy)
                    values
                      ({0}, :DeliveryNo, :CarrierWaybillNo, :ExpectDelayType, :DelayTime, :DelayDesc, :ExpectTime, :ApproveStatus, :CreateBy,:UpdateBy)
                ", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="CarrierWaybillNo",DbType= DbType.String,Value=model.CarrierWaybillNo},
                new OracleParameter() { ParameterName="ExpectDelayType",DbType= DbType.Int32,Value=model.ExpectDelayType},
                new OracleParameter() { ParameterName="DelayTime",DbType= DbType.Int32,Value=model.DelayTime},
                new OracleParameter() { ParameterName="DelayDesc",DbType= DbType.String,Value=model.DelayDesc},
                new OracleParameter() { ParameterName="ExpectTime",DbType= DbType.DateTime,Value=model.ExpectTime},
                new OracleParameter() { ParameterName="ApproveStatus",DbType= DbType.Int32,Value=model.ApproveStatus},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Update(ExpectDelayModel model)
        {
            string sql = @"
                UPDATE TMS_Expectdelay
                   SET
                       ExpectDelayType = :ExpectDelayType,
                       DelayTime = :DelayTime,
                       DelayDesc = :DelayDesc,
                       ExpectTime = :ExpectTime,
                       ApproveStatus = :ApproveStatus,
                       ApproveBy = :ApproveBy,
                       ApproveTime = :ApproveTime,
                       UpdateBy = :UpdateBy,
                       UpdateTime = sysdate
                 WHERE EDID = :EDID
                    AND IsDeleted=0
             ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ExpectDelayType",DbType= DbType.Int32,Value=(int)model.ExpectDelayType},
                new OracleParameter() { ParameterName="DelayTime",DbType= DbType.Int32,Value=model.DelayTime},
                new OracleParameter() { ParameterName="DelayDesc",DbType= DbType.String,Value=model.DelayDesc},
                new OracleParameter() { ParameterName="ExpectTime",DbType= DbType.DateTime,Value=model.ExpectTime},
                new OracleParameter() { ParameterName="ApproveStatus",DbType= DbType.Int32,Value=(int)model.ApproveStatus},
                new OracleParameter() { ParameterName="ApproveBy",DbType= DbType.Int32,Value=model.ApproveBy},
                new OracleParameter() { ParameterName="ApproveTime",DbType= DbType.DateTime,Value=model.ApproveTime},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="EDID",DbType= DbType.Int32,Value=model.EDID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Approve(ExpectDelayModel model)
        {
            string sql = @"
                UPDATE TMS_Expectdelay
                   SET
                       ApproveStatus = :ApproveStatus,
                       ApproveBy = :ApproveBy,
                       ApproveTime = sysdate,
                       UpdateBy = :UpdateBy,
                       UpdateTime = sysdate
                 WHERE EDID = :EDID
                    AND IsDeleted=0
             ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ApproveStatus",DbType= DbType.Int32,Value=(int)model.ApproveStatus},
                new OracleParameter() { ParameterName="ApproveBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="EDID",DbType= DbType.Int32,Value=model.EDID},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Delete(long id)
        {
            string sql = @"
                UPDATE TMS_ExpectDelay
                SET IsDeleted = 1
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                 WHERE EDID = :EDID
             ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="EDID",DbType= DbType.Int32,Value=id}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public ExpectDelayModel Get(long id)
        {
            string sql = @"
                SELECT
                    EDID, DeliveryNo, CarrierWaybillNo, ExpectDelayType, DelayTime, DelayDesc, ExpectTime, ApproveStatus, ApproveBy, ApproveTime, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted
                FROM TMS_ExpectDelay
                WHERE EDID=:EDID
                    AND IsDeleted=0";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "EDID", DbType = DbType.Int64, Value = id } 
            };
            return ExecuteSqlSingle_ByReaderReflect<ExpectDelayModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public ExpectDelayModel GetByDeliveryNo(string deliveryNo)
        {
            string sql = @"
                SELECT
                    EDID, DeliveryNo, CarrierWaybillNo, ExpectDelayType, DelayTime, DelayDesc, ExpectTime, ApproveStatus, ApproveBy, ApproveTime, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted
                FROM TMS_ExpectDelay
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo } 
            };
            return ExecuteSqlSingle_ByReaderReflect<ExpectDelayModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public bool IsExistExpectDelayInfo(string deliveryNo)
        {
            string strSql = string.Format(@"
                SELECT COUNT(*) CC
                FROM TMS_ExpectDelay
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

        #endregion
    }
}

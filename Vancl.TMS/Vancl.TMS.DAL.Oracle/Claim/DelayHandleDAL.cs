using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IDAL.Claim;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;


namespace Vancl.TMS.DAL.Oracle.Claim
{
    public class DelayHandleDAL : BaseDAL, IDelayHandleDAL
    {
        public PagedList<ViewDelayHandleModel> Search(DelayHandleSearchModel searchModel)
        {
            var paramList = new List<OracleParameter>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
SELECT  A.DID AS DispatchID,D.DID DelayID,D.DELAYTIMESPAN,A.DELIVERYNO, BOXCOUNT, TOTALAMOUNT, E1.COMPANYNAME DEPARTURENAME,
    E2.COMPANYNAME ARRIVALNAME,C.CARRIERNAME CARRIERname, TRANSPORTTYPE, LINEGOODSTYPE, 
    ARRIVALTIMING,W.WAYBILLNO CARRIERWAYBILLNO, A.DELIVERYSTATUS
FROM TMS_DELAY D
    JOIN TMS_DISPATCH A ON D.DELIVERYNO=A.DELIVERYNO AND A.DELIVERYSTATUS={0}
    JOIN TMS_SITEASSESSMENT siteAss ON a.deliveryno = siteAss.Deliveryno
    JOIN EXPRESSCOMPANY E1 ON A.DEPARTUREID=E1.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY E2 ON A.ARRIVALID=E2.EXPRESSCOMPANYID
    JOIN TMS_CARRIER C ON A.CARRIERID=C.CARRIERID 
    JOIN TMS_CARRIERWAYBILL W ON W.CWID = A.CARRIERWAYBILLID
WHERE A.IsDeleted=0
", (int)Enums.DeliveryStatus.ArrivedDelay);

            if (searchModel.CreateDateBegin.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime >= :CreateDateBegin");
                paramList.Add(new OracleParameter { ParameterName = "CreateDateBegin", DbType = DbType.DateTime, Value = searchModel.CreateDateBegin.Value });
            }
            if (searchModel.CreateDateEnd.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime <= :CreateDateEnd");
                paramList.Add(new OracleParameter { ParameterName = "CreateDateEnd", DbType = DbType.DateTime, Value = Convert.ToDateTime(searchModel.CreateDateEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                sbSql.Append(" AND A.DeliveryNO  like :DeliveryNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "DeliveryNO", DbType = DbType.String, Value = searchModel.DeliveryNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierWaybillNO))
            {
                sbSql.Append(" AND W.WAYBILLNO  like :WAYBILLNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "WAYBILLNO", DbType = DbType.String, Value = searchModel.CarrierWaybillNO });
            }
            if (searchModel.IsInput.HasValue)
            {
                if (searchModel.IsInput.Value)
                {
                    sbSql.Append(@" AND EXISTS (
                                SELECT 1
                                FROM TMS_DelayHandle dh
                                WHERE dh.DelayID=d.DID
                                    AND dh.IsDeleted=0
                                )");
                }
                else
                {
                    sbSql.Append(@" AND NOT EXISTS (
                                SELECT 1
                                FROM TMS_DelayHandle dh
                                WHERE dh.DelayID=d.DID
                                    AND dh.IsDeleted=0
                                )");
                }
            }
            searchModel.OrderByString = "DELIVERYNO";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDelayHandleModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, paramList.ToArray());
        }

        public PagedList<ViewDelayHandleModel> SearchDelayHandleApply(DelayHandleSearchModel searchModel)
        {
            var paramList = new List<OracleParameter>();
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendFormat(@"
SELECT  A.DID AS DispatchID,D.DID DelayID,D.DELAYTIMESPAN,A.DELIVERYNO, BOXCOUNT, TOTALAMOUNT, E1.COMPANYNAME DEPARTURENAME,
    E2.COMPANYNAME ARRIVALNAME,C.CARRIERNAME, TRANSPORTTYPE, LINEGOODSTYPE, 
    ARRIVALTIMING, W.WAYBILLNO CARRIERWAYBILLNO,DH.NOTE DelayHandleNote,DH.DHID
    ,A.DELIVERYSTATUS
FROM TMS_DELAYHANDLE DH
    JOIN TMS_DELAY D ON DH.DELAYID = D.DID
    JOIN TMS_DISPATCH A ON D.DELIVERYNO=A.DELIVERYNO 
    JOIN TMS_SITEASSESSMENT siteAss ON a.deliveryno = siteAss.Deliveryno
    JOIN EXPRESSCOMPANY E1 ON A.DEPARTUREID=E1.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY E2 ON A.ARRIVALID=E2.EXPRESSCOMPANYID
    JOIN TMS_CARRIER C ON A.CARRIERID=C.CARRIERID 
    JOIN TMS_CARRIERWAYBILL W ON W.CWID = A.CARRIERWAYBILLID
WHERE A.IsDeleted=0
");
            if (searchModel.CreateDateBegin.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime >= :CreateDateBegin");
                paramList.Add(new OracleParameter { ParameterName = "CreateDateBegin", DbType = DbType.DateTime, Value = searchModel.CreateDateBegin.Value });
            }
            if (searchModel.CreateDateEnd.HasValue)
            {
                sbSql.Append(" AND siteass.leavetime <= :CreateDateEnd");
                paramList.Add(new OracleParameter { ParameterName = "CreateDateEnd", DbType = DbType.DateTime, Value = Convert.ToDateTime(searchModel.CreateDateEnd.Value.ToString("yyyy-MM-dd 23:59:59")) });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.DeliveryNO))
            {
                sbSql.Append(" AND A.DeliveryNO  like :DeliveryNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "DeliveryNO", DbType = DbType.String, Value = searchModel.DeliveryNO });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierWaybillNO))
            {
                sbSql.Append(" AND W.WAYBILLNO  like :WAYBILLNO || '%' ");
                paramList.Add(new OracleParameter() { ParameterName = "WAYBILLNO", DbType = DbType.String, Value = searchModel.CarrierWaybillNO });
            }
            if (searchModel.ApproveStatus.HasValue)
            {
                sbSql.Append(" AND APPROVESTATUS=:APPROVESTATUS ");
                paramList.Add(new OracleParameter() { ParameterName = "APPROVESTATUS", DbType = DbType.Int32, Value = (int)searchModel.ApproveStatus });
            }
            searchModel.OrderByString = "DELIVERYNO";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDelayHandleModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, paramList.ToArray());
        }

        public int Add(DelayHandleModel model)
        {
            string sql = string.Format(@"
                INSERT INTO TMS_DELAYHANDLE(
                        DHID,
                        DELAYID,
                        NOTE,
                        CREATEBY,
                        UPDATEBY,
                        ISDELETED
                    )
                VALUES(
                        {0},
                        :DELAYID,
                        :NOTE,
                        :CREATEBY,
                        :UPDATEBY,
                        0
                    )", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DELAYID",DbType= DbType.Int64,Value=model.DelayID},
                new OracleParameter() { ParameterName="NOTE",DbType= DbType.String,Value=model.NOTE},
                new OracleParameter() { ParameterName="CREATEBY",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UPDATEBY",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Approve(DelayHandleModel model)
        {
            string sql = @"
                UPDATE TMS_DELAYHANDLE SET
                     APPROVESTATUS = :APPROVESTATUS,
                     APPROVEBY = :APPROVEBY,
                     APPROVETIME = SYSDATE
                WHERE DHID = :DHID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="APPROVESTATUS",DbType= DbType.Int32,Value=(int)model.ApproveStatus},
                new OracleParameter() { ParameterName="APPROVEBY",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DHID",DbType= DbType.Int32,Value=model.DHID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public DelayHandleModel GetDelayHandle(long delayID, Enums.ApproveStatus approveStatus)
        {
            string strSql = @"
                SELECT DHID,DELAYID,NOTE,APPROVESTATUS,APPROVEBY
                    ,APPROVETIME,CREATEBY,CREATETIME,UPDATEBY,UPDATETIME,ISDELETED
                FROM TMS_DELAYHANDLE
                WHERE ISDELETED=0
                    AND DELAYID=:DELAYID
                    AND APPROVESTATUS=:APPROVESTATUS";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DELAYID",DbType= DbType.Int64,Value=delayID},
                new OracleParameter() { ParameterName="APPROVESTATUS",DbType= DbType.Int32,Value=(int)approveStatus}
            };
            return ExecuteSqlSingle_ByReaderReflect<DelayHandleModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int UpdateDelayHandleApplyReason(DelayHandleModel model)
        {
            string strSql = @"
                UPDATE TMS_DELAYHANDLE 
                SET NOTE = :NOTE,
                    UPDATEBY = :UPDATEBY,
                    UPDATETIME = SYSDATE
                WHERE DHID = :DHID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="NOTE",DbType= DbType.String,Value=model.NOTE},
                new OracleParameter() { ParameterName="UPDATEBY",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DHID",DbType= DbType.Int64,Value=model.DHID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }
    }


}

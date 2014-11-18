using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Dispatch;
using System.Data;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.DAL.Oracle.Transport.Dispatch
{
    public partial class PreDispatchDAL
    {
        /// <summary>
        /// 待调度批次查询
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public List<ViewPreDispatchModel> SearchPreDispatchInfoV1(PreDispatchSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("PreDispatchSearchModel is null");
            String sql = String.Format(@"
SELECT box.BoxNo AS BatchNo, box.Source,  box.CustomerBatchNo, predis.DepartureID, ecd.CompanyName AS DepartureName,  predis.ArrivalID,  eca.CompanyName AS ArrivalName
,predis.PDID, predis.TPID, predis.LPID, lp.TransportType,box.CreateTime as OutBoundTime
,CASE WHEN box.SOURCE=0 THEN 'VANCL' WHEN box.SOURCE=1 THEN 'VJIA' ELSE (SELECT CASE WHEN count(m.merchantname)=1 THEN max(m.merchantname) ELSE '混装' end
  FROM tms_boxdetail tb
JOIN sc_bill sb ON sb.formcode=tb.formcode
JOIN ps_Pms.merchantbaseinfo m ON sb.merchantid=m.ID 
 WHERE tb.boxno=box.boxno) END MerchantName
FROM TMS_Box box
    JOIN TMS_PREDISPATCH_V1 predis ON box.BoxNo = predis.BoxNo
    JOIN TMS_LinePlan lp ON predis.LPID = lp.LPID
    JOIN ExpressCompany ecd ON predis.DepartureID = ecd.ExpressCompanyID
    JOIN ExpressCompany eca ON predis.ArrivalID = eca.ExpressCompanyID 
WHERE  predis.DepartureID = :DepartureID
    AND predis.ArrivalID = :ArrivalID
    AND predis.DispatchStatus = {0}
    AND box.IsDeleted = 0
    AND predis.IsDeleted = 0
",
 (int)Enums.DispatchStatus.CanDispatch
 );
            List<OracleParameter> listParameter = new List<OracleParameter>();
            listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchmodel.DepartureID });
            listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchmodel.ArrivalID });
            if (searchmodel.Source.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "Source", DbType = DbType.Int32, Value = (int)searchmodel.Source.Value });
                sql += @"
    AND box.Source = :Source
";
            }
            if (searchmodel.TransportType.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchmodel.TransportType.Value });
                sql += @"
    AND lp.TransportType = :TransportType
";
            }
            if (searchmodel.BeginDate.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "BeginDate", DbType = DbType.DateTime, Value = searchmodel.BeginDate.Value });
                sql += @"
    AND box.CreateTime >= :BeginDate
";
            }
            if (searchmodel.EndDate.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "EndDate", DbType = DbType.DateTime, Value = searchmodel.EndDate.Value });
                sql += @"
    AND box.CreateTime <= :EndDate
";
            }

            sql += " ORDER BY box.CreateTime DESC";
            var listResult = ExecuteSql_ByDataTableReflect<ViewPreDispatchModel>(TMSWriteConnection, sql, listParameter.ToArray());
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }
        public List<ViewPreDispatchModel> GetPreDispatchInfoV1(PreDispatchSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("PreDispatchSearchModel is null");
            if (String.IsNullOrWhiteSpace(searchmodel.CustomerBatchNo)) throw new ArgumentNullException("PreDispatchSearchModel.CustomerBatchNo is null or empty.");
            String sql = String.Format(@"
SELECT box.BoxNo AS BatchNo, box.Source,  box.CustomerBatchNo, predis.DepartureID, ecd.CompanyName AS DepartureName,  predis.ArrivalID,  eca.CompanyName AS ArrivalName
,predis.PDID, predis.TPID, predis.LPID, lp.TransportType,box.CreateTime as OutBoundTime
,CASE WHEN box.SOURCE=0 THEN 'VANCL' WHEN box.SOURCE=1 THEN 'VJIA' ELSE (SELECT CASE WHEN count(m.merchantname)=1 THEN max(m.merchantname) ELSE '混装' end
  FROM tms_boxdetail tb
JOIN sc_bill sb ON sb.formcode=tb.formcode
JOIN ps_Pms.merchantbaseinfo m ON sb.merchantid=m.ID 
 WHERE tb.boxno=box.boxno) END MerchantName
FROM TMS_Box box
    JOIN TMS_PREDISPATCH_V1 predis ON box.BoxNo = predis.BoxNo
    JOIN TMS_LinePlan lp ON predis.LPID = lp.LPID
    JOIN ExpressCompany ecd ON predis.DepartureID = ecd.ExpressCompanyID
    JOIN ExpressCompany eca ON predis.ArrivalID = eca.ExpressCompanyID 
WHERE box.CustomerBatchNo = :CustomerBatchNo
    AND predis.DepartureID = :DepartureID
    AND predis.ArrivalID = :ArrivalID
    AND predis.DispatchStatus = {0}
    AND box.IsDeleted = 0
    AND predis.IsDeleted = 0
",
 (int)Enums.DispatchStatus.CanDispatch
 );
            List<OracleParameter> listParameter = new List<OracleParameter>();
            listParameter.Add(new OracleParameter() { ParameterName = "CustomerBatchNo", DbType = DbType.String, Value = searchmodel.CustomerBatchNo });
            listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchmodel.DepartureID });
            listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchmodel.ArrivalID });
            if (searchmodel.Source.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "Source", DbType = DbType.Int32, Value = (int)searchmodel.Source.Value });
                sql += @"
    AND box.Source = :Source
";
            }
            if (searchmodel.TransportType.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchmodel.TransportType.Value });
                sql += @"
    AND lp.TransportType = :TransportType
";
            }
            var listResult = ExecuteSql_ByDataTableReflect<ViewPreDispatchModel>(TMSWriteConnection, sql, listParameter.ToArray());
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        public List<ViewDispatchBoxModel> GetPreDispatchBoxListV1(int LPID)
        {
            string strSql = String.Format(@"
                SELECT pd.DepartureID,ecd.CompanyName DepartureName
                    ,pd.ArrivalID,eca.CompanyName ArrivalName,pd.BoxNo
                    ,(
                        SELECT COUNT(1) CC
                        FROM TMS_BoxDetail bd
                        JOIN TMS_Order o
                            ON o.IsDeleted=0
                                AND o.FormCode=bd.FormCode
                                AND o.OrderTMSStatus={0}
                        WHERE bd.BoxNo=pd.BoxNo
                    ) OrderCount
                FROM TMS_PREDISPATCH_V1 pd
                JOIN ExpressCompany ecd ON pd.DepartureID=ecd.ExpressCompanyID
                JOIN ExpressCompany eca ON pd.ArrivalID=eca.ExpressCompanyID
                WHERE pd.IsDeleted=0
                    AND pd.DispatchStatus={1}
                    AND pd.LPID=:LPID"
                , (int)Enums.OrderTMSStatus.Normal
                , (int)Enums.DispatchStatus.CanDispatch);
            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="LPID", DbType= DbType.Int64, Value = LPID}
            };
            return (List<ViewDispatchBoxModel>)ExecuteSql_ByReaderReflect<ViewDispatchBoxModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int UpdateToDisabledDispatchV1(List<long> listPDID, Enums.DispatchStatus UpdateStatus)
        {
            if (listPDID.Count == 0)
            {
                return 1;
            }
            string sql = @"
UPDATE TMS_PREDISPATCH_V1
SET DispatchStatus = :DispatchStatus ,UpdateTime = sysdate  , UpdateBy = :UpdateBy 
WHERE PDID = :PDID AND IsDeleted = 0 
";
            var userID = UserContext.CurrentUser.ID;
            List<int> Users = new List<int>(listPDID.Count);
            List<int> listDispatchStatus = new List<int>(listPDID.Count);
            for (int i = 0; i < listPDID.Count; i++)
            {
                Users.Add(userID);
                listDispatchStatus.Add((int)UpdateStatus);
            }
            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="PDID", DbType= DbType.Int64, Value = listPDID.ToArray() },
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = Users.ToArray() },
                new OracleParameter() { ParameterName="DispatchStatus", DbType= DbType.Int32, Value = listDispatchStatus.ToArray() }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listPDID.Count, arguments);
        }

        public List<PreDispatchModel> GetModelByDispatch(PreDispatchPublicQueryModel QueryModel)
        {
            string strSql =
                @"select pd.* from tms_dispatch d 
            join tms_dispatchdetail dd on dd.did=d.did
            join TMS_PREDISPATCH_V1 pd on pd.BOXNO=dd.BOXNO and pd.dispatchstatus=:status
            where d.did=:did";

            OracleParameter[] arguments = new OracleParameter[]
                                             {
                                                 new OracleParameter()
                                                     {
                                                         ParameterName = "status",
                                                         DbType = DbType.Int32,
                                                         Value = (int) QueryModel.Status
                                                     },
                                                 new OracleParameter()
                                                     {
                                                         ParameterName = "did",
                                                         DbType = DbType.Int64,
                                                         Value = QueryModel.DID
                                                     }
                                             };
            return
                (List<PreDispatchModel>)
                ExecuteSql_ByReaderReflect<PreDispatchModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public List<PreDispatchModel> GetCurPreDispatchListByBoxNos(string[] box, Enums.DispatchStatus UpdateStatus)
        {
            if (box == null || box.Length == 0)
            {
                return null;
            }
            StringBuilder sbCTE = new StringBuilder();
            foreach (string b in box)
            {
                sbCTE.AppendFormat(@"
                    SELECT '{0}' AS BoxNo FROM DUAL
                    UNION ALL ", b);
            }
            sbCTE.Remove(sbCTE.Length - 10, 10);
            string strSql = String.Format(@"
                WITH tmp AS
                (
                    {0}
                )
                SELECT pd.PDID,pd.BoxNo,pd.LPID,pd.SeqNo,pd.DispatchStatus
                    ,pd.DepartureID,pd.ArrivalID,pd.LineGoodsType
                FROM TMS_PREDISPATCH_V1 pd
                JOIN tmp t
                    ON t.BoxNo=pd.BoxNo
                WHERE pd.IsDeleted=0
                    AND pd.DispatchStatus IN ({1})"
                , sbCTE.ToString()
                , ((int)UpdateStatus).ToString());
            return (List<PreDispatchModel>)ExecuteSql_ByReaderReflect<PreDispatchModel>(TMSReadOnlyConnection, strSql);
        }

        public List<PreDispatchModel> GetNextPreDispatchListByBoxNos(List<string> box, int iNext)
        {
            if (box == null || box.Count == 0)
            {
                return null;
            }
            StringBuilder sbCTE = new StringBuilder();
            foreach (string b in box)
            {
                sbCTE.AppendFormat(@"
                    SELECT '{0}' AS BoxNo FROM DUAL
                    UNION ALL ", b);
            }
            sbCTE.Remove(sbCTE.Length - 10, 10);
            string strSql = String.Format(@"
                WITH tmp AS
                (
                    {0}
                )
                SELECT pd.PDID,pd.BoxNo,pd.LPID,pd.SeqNo,pd.DispatchStatus
                    ,pd.DepartureID,pd.ArrivalID,pd.LineGoodsType
                FROM TMS_PREDISPATCH_V1 pd
                JOIN tmp t
                    ON t.BoxNo=pd.BoxNo
                WHERE pd.IsDeleted=0
                    AND pd.SEQNO IN ({1})"
                , sbCTE.ToString()
                , iNext.ToString());
            return (List<PreDispatchModel>)ExecuteSql_ByReaderReflect<PreDispatchModel>(TMSReadOnlyConnection, strSql);
        }
        public PreDispatchModel GetPreDispatchModelByPDID(long PDID)
        {
            string strSql = @"SELECT tp.* FROM TMS_PREDISPATCH_V1 tp WHERE tp.PDID=:PDID";
            OracleParameter[] arguments = new OracleParameter[]
                                              {
                                                  new OracleParameter()
                                                      {ParameterName = "PDID", DbType = DbType.Int64, Value = PDID}
                                              };

            return ExecuteSqlSingle_ByReaderReflect<PreDispatchModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int UpdateToCanDispatchV1(List<long> listPDID)
        {
            string sql = @"
                    UPDATE TMS_PREDISPATCH_V1
                    SET DispatchStatus = :DispatchStatus ,UpdateTime = sysdate  , UpdateBy = :UpdateBy 
                    WHERE PDID = :PDID AND IsDeleted = 0 
                    ";
            var userID = UserContext.CurrentUser.ID;
            List<int> Users = new List<int>(listPDID.Count);
            List<int> listDispatchStatus = new List<int>(listPDID.Count);
            for (int i = 0; i < listPDID.Count; i++)
            {
                Users.Add(userID);
                listDispatchStatus.Add((int)Enums.DispatchStatus.CanDispatch);
            }
            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="PDID", DbType= DbType.Int64, Value = listPDID.ToArray() },
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = Users.ToArray() },
                new OracleParameter() { ParameterName="DispatchStatus", DbType= DbType.Int32, Value = listDispatchStatus.ToArray() }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listPDID.Count, arguments);
        }

        // public 


    }
}

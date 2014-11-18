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
    public partial class PreDispatchDAL : BaseDAL, IPreDispatchDAL
    {
        #region IPreDispatchDAL 成员

        public DataTable GetValidBoxNoAndTPID(int count)
        {
            string strSql = string.Format(@"
                WITH cte_tmp AS
                (
                    SELECT tb.BoxNo,tb.DepartureID,tb.ArrivalID,tb.ContentType,ttp.TPID
                    FROM TMS_Box tb
                    JOIN TMS_TransportPlan ttp
                        ON ttp.IsDeleted=0
                            AND ttp.Status={0}
                            AND ttp.IsEnabled=1
                            AND ttp.Deadline>=sysdate
                            AND ttp.DepartureID=tb.DepartureID
                            AND ttp.ArrivalID=tb.ArrivalID
                            AND (ttp.LineGoodsType+tb.ContentType)-BITAND(ttp.LineGoodsType,tb.ContentType)=ttp.LineGoodsType
                    WHERE tb.IsDeleted=0 
                        AND tb.IsPreDispatch=0
                        --没有运输计划，无法产生预调度信息，但被用户更改运输计划进行调度了，这种不能再次预调度了
                        AND NOT EXISTS(
                            SELECT 1
                            FROM TMS_DispatchDetail tdd
                            WHERE tdd.BoxNo=tb.BoxNo
                                AND tdd.IsDeleted=0
                        )
                    ORDER BY ttp.LineGoodsType desc
                )
                SELECT a.BoxNo,a.TPID
                FROM cte_tmp a
                JOIN TMS_TransportPlanDetail ttpd
                    ON ttpd.TPID=a.TPID
                        AND ttpd.IsDeleted=0
                JOIN TMS_LinePlan tlp
                    ON tlp.LineID=ttpd.LineID
                        AND tlp.IsDeleted=0
                        AND tlp.Status={1}
                        AND tlp.IsEnabled=1
                JOIN TMS_Carrier tc
                    ON tc.IsDeleted=0
                        AND tc.Status={2}
                        AND tc.CarrierID=tlp.CarrierID
                WHERE (a.DepartureID,a.ArrivalID,a.ContentType)
                        =(SELECT b.DepartureID,b.ArrivalID,b.ContentType
                        FROM cte_tmp b
                        WHERE rownum=1) 
                    AND rownum<={3}
                GROUP BY a.BoxNo,a.TPID
                ", (int)Enums.TransportStatus.Effective
                 , (int)Enums.LineStatus.Effective
                 , (int)Enums.CarrierStatus.Valid
                 , count);
            return ExecuteSqlDataTable(TMSReadOnlyConnection, strSql);
        }

        public int Add(string BoxNos, int TPID)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_PreDispatch
                    (PDID
                    ,BoxNo
                    ,TPID
                    ,LPID
                    ,SeqNo
                    ,DispatchStatus
                    ,DepartureID
                    ,ArrivalID
                    ,LineGoodsType
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted)
                SELECT SEQ_TMS_PreDispatch_PDID.NEXTVAL
                    ,tb.BoxNo
                    ,:TPID
                    ,tlp.LPID
                    ,ttpd.SeqNo
                    ,CASE ttpd.SeqNo WHEN 1 THEN {0} ELSE {1} END
                    ,tlp.DepartureID
                    ,tlp.ArrivalID
                    ,tlp.LineGoodsType
                    ,0
                    ,0
                    ,0
                FROM TMS_Box tb
                JOIN TMS_TransportPlanDetail ttpd
                    ON ttpd.IsDeleted=0
                        AND TPID=:TPID
                JOIN TMS_LinePlan tlp
                    ON tlp.IsDeleted=0
                        AND tlp.Status={2}
                        AND tlp.LineID=ttpd.LineID
                WHERE tb.IsDeleted=0 
                        AND tb.IsPreDispatch=0 
                        AND tb.BoxNo IN ({3})
            ", (int)Enums.DispatchStatus.CanDispatch
             , (int)Enums.DispatchStatus.CanNotDispatch
             , (int)Enums.LineStatus.Effective
             , BoxNos);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value=TPID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }


        public List<ViewDispatchBoxModel> GetPreDispatchBoxList(int LPID)
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
                FROM TMS_PreDispatch pd
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

        public List<PreDispatchModel> GetCurPreDispatchList(string[] box, bool isContainDispatched)
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
                FROM TMS_PreDispatch pd
                JOIN tmp t
                    ON t.BoxNo=pd.BoxNo
                WHERE pd.IsDeleted=0
                    AND pd.DispatchStatus IN ({1})"
                , sbCTE.ToString()
                , isContainDispatched ? (int)Enums.DispatchStatus.Dispatched + "," + (int)Enums.DispatchStatus.CanDispatch
                    : ((int)Enums.DispatchStatus.CanDispatch).ToString());
            return (List<PreDispatchModel>)ExecuteSql_ByReaderReflect<PreDispatchModel>(TMSReadOnlyConnection, strSql);
        }

        /// <summary>
        /// 可调度状态更新为已调度状态
        /// </summary>
        /// <param name="listPDID">主键ID列表</param>
        /// <returns></returns>
        public int UpdateToDispatched(List<long> listPDID)
        {
            string sql = String.Format(@"
UPDATE TMS_PreDispatch
SET DispatchStatus = {1},UpdateTime = sysdate ,UpdateBy=:UpdateBy
WHERE PDID=:PDID AND IsDeleted = 0 AND DispatchStatus={0}
"
                , (int)Enums.DispatchStatus.CanDispatch
                , (int)Enums.DispatchStatus.Dispatched
                );

            var Users = listPDID.Select(x =>
            {
                return UserContext.CurrentUser.ID;
            }).ToArray();

            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="PDID", DbType= DbType.Int64, Value = listPDID.ToArray()},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = Users}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listPDID.Count, arguments);
        }

        /// <summary>
        /// 可调度状态更新为已作废状态
        /// </summary>
        /// <param name="listPDID">主键ID列表</param>
        /// <returns></returns>
        public int UpdateToInvalid(List<long> listPDID)
        {
            string sql = String.Format(@"
UPDATE TMS_PreDispatch
SET DispatchStatus = {1},UpdateTime = sysdate ,UpdateBy=:UpdateBy 
WHERE PDID=:PDID AND IsDeleted = 0 AND DispatchStatus={0}
"
    , (int)Enums.DispatchStatus.CanDispatch
    , (int)Enums.DispatchStatus.Invalid
    );
            var Users = listPDID.Select(x =>
            {
                return UserContext.CurrentUser.ID;
            }).ToArray();
            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="PDID", DbType= DbType.Int64, Value = listPDID.ToArray()},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = Users}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listPDID.Count, arguments);
        }

        public int UpdateToCanDispatch(List<long> listPDID)
        {
            string sql = @"
UPDATE TMS_PreDispatch
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

        #endregion

        #region IPreDispatchDAL 成员

        public List<ViewPreDispatchModel> GetPreDispatchInfo(PreDispatchSearchModel searchmodel)
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
    JOIN TMS_PreDispatch predis ON box.BoxNo = predis.BoxNo
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


        public List<ViewPreDispatchModel> SearchPreDispatchInfo(PreDispatchSearchModel searchmodel)
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
    JOIN TMS_PreDispatch predis ON box.BoxNo = predis.BoxNo
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

        public int UpdateToDisabledDispatch(List<long> listPDID)
        {
            string sql = @"
UPDATE TMS_PreDispatch
SET DispatchStatus = :DispatchStatus ,UpdateTime = sysdate  , UpdateBy = :UpdateBy 
WHERE PDID = :PDID AND IsDeleted = 0 
";
            var userID = UserContext.CurrentUser.ID;
            List<int> Users = new List<int>(listPDID.Count);
            List<int> listDispatchStatus = new List<int>(listPDID.Count);
            for (int i = 0; i < listPDID.Count; i++)
            {
                Users.Add(userID);
                listDispatchStatus.Add((int)Enums.DispatchStatus.CanNotDispatch);
            }
            OracleParameter[] arguments = new OracleParameter[]{
                new OracleParameter(){ ParameterName="PDID", DbType= DbType.Int64, Value = listPDID.ToArray() },
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = Users.ToArray() },
                new OracleParameter() { ParameterName="DispatchStatus", DbType= DbType.Int32, Value = listDispatchStatus.ToArray() }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listPDID.Count, arguments);
        }

        #endregion

        #region IPreDispatchDAL 成员

        public List<BoxModel> GetNeededPreDispatchBatchList(PreDispatchJobArgModel arguments)
        {
            if (arguments == null) throw new ArgumentNullException("PreDispatchJobArgModel is null");
            String sql = @"
SELECT T.*
FROM
(
    SELECT BID , BoxNo, DepartureID, ArrivalID, IsPreDispatch, ContentType, CustomerBatchNo
    FROM TMS_Box
    WHERE CreateTime > :SyncTime
    AND IsDeleted = 0 
    AND IsPreDispatch = 0
    ORDER BY CreateTime ASC
) T
WHERE ROWNUM <= :TopCount
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="SyncTime", DbType = DbType.DateTime, Value = arguments.SyncTime },
                new OracleParameter() { ParameterName="TopCount", DbType = DbType.Int32, Value = arguments.PerBatchCount }
            };
            var listResult = ExecuteSql_ByReaderReflect<BoxModel>(TMSWriteConnection, sql, parameters);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        public BoxModel GetAbnormalPreDispatchByBID(Int64 bid)
        {
            if (bid == null) throw new ArgumentNullException("bid is null");
            String sql = @"
    SELECT BID , BoxNo, DepartureID, ArrivalID, IsPreDispatch, ContentType, CustomerBatchNo
    FROM TMS_Box
    WHERE 1=1
    AND IsDeleted = 0 
    AND IsPreDispatch = :IsPreDispatch
    AND BID=:BID
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IsPreDispatch", DbType = DbType.Int32, Value = (int)Vancl.TMS.Model.Common.Enums.BatchPreDispatchedStatus.DispatchedError },
                new OracleParameter() { ParameterName="BID", DbType = DbType.Int64, Value = bid }
            };
             var listResult = ExecuteSqlSingle_ByReaderReflect<BoxModel>(TMSWriteConnection, sql, parameters);
            if (listResult != null)
            {
                return listResult;
            }
            return null;
        }

        public int Add(PreDispatchModel model)
        {
            throw new NotImplementedException();
        }


        public int BatchAdd(List<PreDispatchModel> model)
        {
            if (model == null || model.Count <= 0)
            {
                return 0;
            }
            String sql = String.Format(@"
INSERT INTO TMS_PreDispatch (PDID, BoxNo ,TPID ,LPID  ,SeqNo  ,DispatchStatus  ,DepartureID   ,ArrivalID  ,LineGoodsType  ,CreateBy  ,UpdateBy ,IsDeleted)
VALUES({0},  :BoxNo, :TPID  , :LPID , :SeqNo , :DispatchStatus, :DepartureID, :ArrivalID, :LineGoodsType, :CreateBy, :UpdateBy, 0)
",
 model[0].SequenceNextValue()
 );
            String[] arrBoxNo = new String[model.Count];
            int[] arrTPID = new int[model.Count];
            int[] arrLPID = new int[model.Count];
            int[] arrSeqNo = new int[model.Count];
            int[] arrDispatchStatus = new int[model.Count];
            int[] arrDepartureID = new int[model.Count];
            int[] arrArrivalID = new int[model.Count];
            int[] arrLineGoodsType = new int[model.Count];
            int[] arrCreateBy = new int[model.Count];
            int[] arrUpdateBy = new int[model.Count];
            int nPos = 0;
            model.ForEach(p =>
            {
                arrBoxNo[nPos] = p.BoxNo;
                arrTPID[nPos] = p.TPID;
                arrLPID[nPos] = p.LPID;
                arrSeqNo[nPos] = p.SeqNo;
                arrDispatchStatus[nPos] = (int)p.DispatchStatus;
                arrDepartureID[nPos] = p.DepartureID;
                arrArrivalID[nPos] = p.ArrivalID;
                arrLineGoodsType[nPos] = (int)p.LineGoodsType;
                arrCreateBy[nPos] = p.CreateBy;
                arrUpdateBy[nPos] = p.UpdateBy;
                nPos++;
            });
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = arrBoxNo },
                new OracleParameter() { ParameterName = "TPID", DbType = DbType.Int32, Value = arrTPID },
                new OracleParameter() { ParameterName = "LPID", DbType = DbType.Int32, Value = arrLPID },
                new OracleParameter() { ParameterName = "SeqNo", DbType = DbType.Int32, Value = arrSeqNo },
                new OracleParameter() { ParameterName = "DispatchStatus", DbType = DbType.Int32, Value = arrDispatchStatus },
                new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = arrDepartureID },
                new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = arrArrivalID },
                new OracleParameter() { ParameterName = "LineGoodsType", DbType = DbType.Int32, Value = arrLineGoodsType },
                new OracleParameter() { ParameterName = "CreateBy", DbType = DbType.Int32, Value = arrCreateBy },
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = arrUpdateBy }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, model.Count, parameters);
        }

        #endregion

        #region IPreDispatchDAL 成员

        public int BatchAddPreDispatchLog(List<PreDispatchLogEntityModel> listLogModel)
        {
            if (listLogModel == null || listLogModel.Count <= 0)
            {
                return 0;
            }
            String sql = String.Format(@"
INSERT INTO TMS_PreDispatchLog( PDLID, BatchNo, CustomerBatchNo, DepartureID, ArrivalID, Note, IsDeleted )
VALUES({0},  :BatchNo, :CustomerBatchNo, :DepartureID, :ArrivalID, :Note,  0 )
",
 listLogModel[0].SequenceNextValue()
 );
            String[] arrBatchNo = new String[listLogModel.Count];
            String[] arrCustomerBatchNo = new String[listLogModel.Count];
            int[] arrDepartureID = new int[listLogModel.Count];
            int[] arrArrivalID = new int[listLogModel.Count];
            String[] arrNote = new String[listLogModel.Count];
            int nPos = 0;
            listLogModel.ForEach(p =>
            {
                arrBatchNo[nPos] = p.BatchNo;
                arrCustomerBatchNo[nPos] = p.CustomerBatchNo;
                arrDepartureID[nPos] = p.DepartureID;
                arrArrivalID[nPos] = p.ArrivalID;
                arrNote[nPos] = p.Note;
                nPos++;
            });
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BatchNo", DbType = DbType.String, Value = arrBatchNo },
                new OracleParameter() { ParameterName="CustomerBatchNo", DbType = DbType.String, Value = arrCustomerBatchNo },
                new OracleParameter() { ParameterName="DepartureID", DbType = DbType.Int32, Value = arrDepartureID },
                new OracleParameter() { ParameterName="ArrivalID", DbType = DbType.Int32, Value = arrArrivalID } ,
                new OracleParameter() { ParameterName="Note", DbType = DbType.String, Value = arrNote }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listLogModel.Count, arguments);
        }

        public int AddPreDispatchLog(PreDispatchLogEntityModel LogModel)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPreDispatchDAL 成员

        public Util.Pager.PagedList<ViewPreDispatchLogModel> SearchPreDispatchLog(PreDispatchLogSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("PreDispatchLogSearchModel is null.");
            String sql = @"
SELECT box.bid, box.contenttype AS GoodsType, box.Source,  box.CustomerBatchNo, box.DepartureID, ecd.CompanyName AS DepartureName,  box.ArrivalID,  eca.CompanyName AS ArrivalName, predispLog.Note
FROM TMS_Box box
    JOIN TMS_PREDISPATCHLOG predispLog ON box.boxno = predispLog.Batchno
    JOIN ExpressCompany ecd ON box.DepartureID = ecd.ExpressCompanyID
    JOIN ExpressCompany eca ON box.ArrivalID = eca.ExpressCompanyID 
WHERE  box.DepartureID =  :DepartureID
    AND box.ArrivalID = :ArrivalID
    AND box.ispredispatch = :predispatch
    AND box.IsDeleted = 0
";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            parameters.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID });
            parameters.Add(new OracleParameter() { ParameterName = "predispatch", DbType = DbType.Int32, Value = (int)Enums.BatchPreDispatchedStatus.DispatchedError });
            if (searchModel.GoodsType.HasValue)
            {
                sql += @"
    AND box.contenttype = :GoodsType
";
                parameters.Add(new OracleParameter() { ParameterName = "GoodsType", DbType = DbType.Int32, Value = (int)searchModel.GoodsType });
            }
            searchModel.OrderByString = "CustomerBatchNo";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewPreDispatchLogModel>(TMSWriteConnection, sql, searchModel, parameters.ToArray());
        }

        public List<ViewPreDispatchLogStatisticModel> SearchPreDispatchStatisticLog(PreDispatchLogSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("PreDispatchLogSearchModel is null.");
            StringBuilder sql = new StringBuilder(@"
SELECT box.DepartureID, MAX(ecd.CompanyName) AS DepartureName,  box.ArrivalID, box.contenttype AS GoodsType,  MAX(eca.CompanyName) AS ArrivalName,COUNT(*) AS TotalCount,SUM(box.TotalCount) AS TotalOrderCount
FROM TMS_Box box
    JOIN TMS_PREDISPATCHLOG predispLog ON box.boxno = predispLog.Batchno
    JOIN ExpressCompany ecd ON box.DepartureID = ecd.ExpressCompanyID
    JOIN ExpressCompany eca ON box.ArrivalID = eca.ExpressCompanyID 
WHERE  box.DepartureID =  :DepartureID
    AND box.ArrivalID = :ArrivalID
    AND box.ispredispatch = :predispatch
    AND box.IsDeleted = 0
");
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            parameters.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID });
            parameters.Add(new OracleParameter() { ParameterName = "predispatch", DbType = DbType.Int32, Value = (int)Enums.BatchPreDispatchedStatus.DispatchedError });
            if (searchModel.GoodsType.HasValue)
            {
                sql.Append(@"
    AND box.contenttype = :GoodsType
");
                parameters.Add(new OracleParameter() { ParameterName = "GoodsType", DbType = DbType.Int32, Value = (int)searchModel.GoodsType });
            }
            sql.Append(@"
GROUP BY box.DepartureID, box.ArrivalID, box.contenttype
");
            var listResult = ExecuteSql_ByReaderReflect<ViewPreDispatchLogStatisticModel>(TMSWriteConnection, sql.ToString(), parameters.ToArray());
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion

        #region IPreDispatchDAL 成员

        /// <summary>
        /// 更新城际批次为待预调度状态
        /// </summary>
        /// <param name="box">城际批次对象</param>
        /// <returns></returns>
        public int UpdateBoxToWaitforDispatch(BoxModel box)
        {
            if (box == null) throw new ArgumentNullException("BoxModel is null");
            String sql = @"
UPDATE TMS_BOX
SET IsPreDispatch = :afterStatus  ,UpdateTime = sysdate
WHERE BoxNo = :BoxNo 
    AND IsPreDispatch = :preStatus
    AND IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = box.BoxNo },
                new OracleParameter() { ParameterName = "preStatus", DbType = DbType.Int16, Value = (int)Enums.BatchPreDispatchedStatus.DispatchedError },
                new OracleParameter() { ParameterName = "afterStatus", DbType = DbType.Int16, Value = (int)Enums.BatchPreDispatchedStatus.NoDispatched }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 批量更新城际批次为待预调度状态
        /// </summary>
        /// <param name="listbox">城际批次对象</param>
        /// <returns></returns>
        public int BatchUpdateUpdateBoxToWaitforDispatch(List<BoxModel> listbox)
        {
            if (listbox == null) throw new ArgumentNullException("listbox is null");
            if (listbox.Count <= 0) return 0;
            String sql = @"
UPDATE TMS_BOX
SET IsPreDispatch = :afterStatus  ,UpdateTime = sysdate
WHERE BoxNo = :BoxNo 
    AND IsPreDispatch = :preStatus
    AND IsDeleted = 0
";
            String[] arrBatchNo = new String[listbox.Count];
            int[] arrpreStatus = new int[listbox.Count];
            int[] arrafterStatus = new int[listbox.Count];
            for (int i = 0; i < listbox.Count; i++)
            {
                arrBatchNo[i] = listbox[i].BoxNo;
                arrpreStatus[i] = (int)Enums.BatchPreDispatchedStatus.DispatchedError;
                arrafterStatus[i] = (int)Enums.BatchPreDispatchedStatus.NoDispatched;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = arrBatchNo },
                new OracleParameter() { ParameterName = "preStatus", DbType = DbType.Int16, Value = arrpreStatus },
                new OracleParameter() { ParameterName = "afterStatus", DbType = DbType.Int16, Value = arrafterStatus }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listbox.Count, parameters);
        }

        #endregion

        public int BatchAddV1(List<PreDispatchModel> model)
        {
            if (model == null || model.Count <= 0)
            {
                return 0;
            }
            String sql = String.Format(@"
INSERT INTO TMS_PreDispatch_V1 (PDID, BoxNo ,TPID ,LPID  ,SeqNo  ,DispatchStatus  ,DepartureID   ,ArrivalID  ,LineGoodsType  ,CreateBy  ,UpdateBy ,IsDeleted)
VALUES({0},  :BoxNo, :TPID  , :LPID , :SeqNo , :DispatchStatus, :DepartureID, :ArrivalID, :LineGoodsType, :CreateBy, :UpdateBy, 0)
",
 model[0].SequenceNextValue()
 );
            String[] arrBoxNo = new String[model.Count];
            int[] arrTPID = new int[model.Count];
            int[] arrLPID = new int[model.Count];
            int[] arrSeqNo = new int[model.Count];
            int[] arrDispatchStatus = new int[model.Count];
            int[] arrDepartureID = new int[model.Count];
            int[] arrArrivalID = new int[model.Count];
            int[] arrLineGoodsType = new int[model.Count];
            int[] arrCreateBy = new int[model.Count];
            int[] arrUpdateBy = new int[model.Count];
            int nPos = 0;
            model.ForEach(p =>
            {
                arrBoxNo[nPos] = p.BoxNo;
                arrTPID[nPos] = p.TPID;
                arrLPID[nPos] = p.LPID;
                arrSeqNo[nPos] = p.SeqNo;
                arrDispatchStatus[nPos] = (int)p.DispatchStatus;
                arrDepartureID[nPos] = p.DepartureID;
                arrArrivalID[nPos] = p.ArrivalID;
                arrLineGoodsType[nPos] = (int)p.LineGoodsType;
                arrCreateBy[nPos] = p.CreateBy;
                arrUpdateBy[nPos] = p.UpdateBy;
                nPos++;
            });
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = arrBoxNo },
                new OracleParameter() { ParameterName = "TPID", DbType = DbType.Int32, Value = arrTPID },
                new OracleParameter() { ParameterName = "LPID", DbType = DbType.Int32, Value = arrLPID },
                new OracleParameter() { ParameterName = "SeqNo", DbType = DbType.Int32, Value = arrSeqNo },
                new OracleParameter() { ParameterName = "DispatchStatus", DbType = DbType.Int32, Value = arrDispatchStatus },
                new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = arrDepartureID },
                new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = arrArrivalID },
                new OracleParameter() { ParameterName = "LineGoodsType", DbType = DbType.Int32, Value = arrLineGoodsType },
                new OracleParameter() { ParameterName = "CreateBy", DbType = DbType.Int32, Value = arrCreateBy },
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = arrUpdateBy }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, model.Count, parameters);
        }
    }
}

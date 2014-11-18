using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Plan;
using Oracle.DataAccess.Client;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Transport.Plan
{
    public class TransportPlanDetailDAL : BaseDAL, ITransportPlanDetailDAL
    {

        #region ITransportPlanDetailDAL 成员

        public int Add(TransportPlanDetailModel model)
        {
            string sql = string.Format(@"
INSERT INTO TMS_TransportPlanDetail(TPDID, TPID, LINEID, SEQNO, CREATEBY, UPDATEBY)
VALUES({0}, :TPID, :LINEID, :SEQNO, :CREATEBY, :UPDATEBY)
", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] { 
            new OracleParameter(){ ParameterName="TPID", DbType= System.Data.DbType.Int32, Value= model.TPID},
            new OracleParameter(){ ParameterName="LINEID", DbType = System.Data.DbType.String, Value= model.LineID},
            new OracleParameter(){ ParameterName="SEQNO", DbType= System.Data.DbType.Int32, Value= model.SeqNo},
            new OracleParameter(){ ParameterName="CREATEBY", DbType = System.Data.DbType.String, Value= UserContext.CurrentUser.ID},
            new OracleParameter(){ ParameterName="UPDATEBY", DbType = System.Data.DbType.String, Value= UserContext.CurrentUser.ID}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Delete(List<int> lstTpid)
        {
            if (lstTpid == null || lstTpid.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_TransportPlanDetail
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE TPID =:TPIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPIDs",DbType= DbType.Int32,Value=lstTpid.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstTpid.Count, arguments);
        }

        public IList<TransportPlanDetailModel> GetByTransportPlanID(int tpid)
        {
            string sql = @"
SELECT TPDID, TPID,LINEID, SEQNO, CREATEBY, CREATETIME , UPDATEBY, UPDATETIME , ISDELETED
FROM TMS_TransportPlanDetail
WHERE  TPID = :TPID
    AND ISDELETED = 0
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="TPID",DbType = System.Data.DbType.Int32, Value=tpid}
            };
            return ExecuteSql_ByReaderReflect<TransportPlanDetailModel>(TMSReadOnlyConnection, sql, arguments);
        }

        #endregion


        #region 线路ID修复

        public List<TransportPlanDetailLineIDRepairModel> GetValidTransportPlanDetail()
        {
            string strSql = string.Format(@"
                SELECT tpd.TPDID,MAX(tpd.LineID) LineID,MAX(lp.LPID) LPID
                FROM TMS_TransportPlanDetail tpd
                JOIN TMS_LinePlan lp
                    ON lp.LineID=tpd.LineID
                        AND lp.IsDeleted=0
                        AND lp.Status={0}
                WHERE tpd.IsDeleted=0
                GROUP BY tpd.TPDID
                UNION ALL
                SELECT tpd.TPDID,tpd.LineID
                    ,(
                        SELECT LPID
                        FROM TMS_LinePlan tlp
                        WHERE tlp.LineID=tpd.LineID
                            AND rownum=1
                    ) LPID
                FROM TMS_TransportPlanDetail tpd
                WHERE tpd.IsDeleted=0
                    AND NOT EXISTS(
                        SELECT 1
                        FROM TMS_LinePlan lp
                        WHERE lp.LineID=tpd.LineID
                            AND lp.IsDeleted=0
                            AND lp.Status={0})"
                , (int)Enums.LineStatus.Effective);
            return (List<TransportPlanDetailLineIDRepairModel>)ExecuteSql_ByReaderReflect<TransportPlanDetailLineIDRepairModel>(TMSReadOnlyConnection, strSql);
        }

        public int RepairTransportPlanDetailLineID(List<TransportPlanDetailLineIDRepairModel> lstModel)
        {
            if (lstModel == null || lstModel.Count == 0)
            {
                return 0;
            }
            string strSql = @"
                UPDATE TMS_TransportPlanDetail
                SET LineID=:LineID
                WHERE TPDID =:TPDID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LineID",DbType= DbType.String,Value=lstModel.Select(m=>m.NewLineID).ToArray()},
                new OracleParameter() { ParameterName="TPDID",DbType= DbType.Int64,Value=lstModel.Select(m=>m.TPDID).ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstModel.Count, arguments);
        }
        #endregion
    }
}

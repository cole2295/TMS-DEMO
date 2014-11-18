using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using System.Data;
using Vancl.TMS.Model.Common;
using System.Data.SqlClient;
using Vancl.TMS.Model.JobMonitor;

namespace Vancl.TMS.DAL.Sql2008.Synchronous
{
    public class InboundLMSDAL : BaseDAL, IInboundLMSDAL
    {
        #region IInboundDAL 成员

        public DataSet GetInboundBox(int mod, int remainder)
        {
            string strSql = string.Format(@"
                SELECT TOP 1 ID,BoxNo,InBoundID,CreateTime
                FROM LMS_SYN_TMS_INBOX(READPAST)
                WHERE SyncFlag={0}
                    AND CAST(BoxNo AS BIGINT) % {1}={2}"
                , (int)Enums.SC2TMSSyncFlag.Notyet
                , mod
                , remainder);
            return ExecuteDataset(LMSWriteConnection, strSql);
        }

        public DataSet GetInboundOrder(int count, int mod, int remainder)
        {
            string strSql = string.Format(@"
                SELECT TOP ({0}) ID,WaybillNo,CustomerOrder,InBoundID,CreateTime
                FROM LMS_SYN_TMS_INOrder(READPAST)
                WHERE SyncFlag={1}
                    AND WaybillNo % {2}={3}"
                , count
                , (int)Enums.SC2TMSSyncFlag.Notyet
                , mod
                , remainder);
            return ExecuteDataset(LMSWriteConnection, strSql);
        }

        public int UpdateBoxSyncFlag(long id, Enums.SC2TMSSyncFlag syncFlag)
        {
            string strSql = @"
                UPDATE LMS_SYN_TMS_INBOX
                SET SyncFlag=@SyncFlag
                    ,SyncTime=GETDATE()
                WHERE ID=@ID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@SyncFlag",SqlDbType.Int,1){Value=(int)syncFlag },
                new SqlParameter("@ID",SqlDbType.BigInt,19){Value=id }
            };
            return ExecuteNonQuery(LMSWriteConnection, strSql, parameters);
        }

        public int UpdateOrderSyncFlag(string ids, Enums.SC2TMSSyncFlag syncFlag)
        {
            string strSql = string.Format(@"
                UPDATE LMS_SYN_TMS_INOrder
                SET SyncFlag=@SyncFlag
                    ,SyncTime=GETDATE()
                WHERE ID IN ({0})", ids);
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@SyncFlag",SqlDbType.Int,1){Value=(int)syncFlag }
            };
            return ExecuteNonQuery(LMSWriteConnection, strSql, parameters);
        }


        public Model.JobMonitor.SyncStatisticInfo GetBoxStatisticInfo()
        {
            string sql = String.Format(@"
SELECT 
 ISNULL(sum(CASE WHEN Syncflag = 0 THEN 1 ELSE 0 END),0) AS [NoSync]
,ISNULL(sum(CASE WHEN Syncflag = 1 THEN 1 ELSE 0 END),0) AS [Syncing]
,ISNULL(sum(CASE WHEN Syncflag = 2 THEN 1 ELSE 0 END),0) AS [Synced]
,getdate() AS [MonitorTime]
FROM LMS_SYN_TMS_INBOX WITH(NOLOCK)
"
                , (int)Enums.SC2TMSSyncFlag.Notyet
                , (int)Enums.SC2TMSSyncFlag.Synchronizing
                , (int)Enums.SC2TMSSyncFlag.Already);
            DataSet ds = ExecuteDataset(LMSWriteConnection, sql);
            if (ds != null
                && ds.Tables.Count > 0)
            {
                DataTable dtResult = ds.Tables[0];
                if (dtResult.Rows.Count > 0)
                {
                    SyncStatisticInfo statisticInfo = new SyncStatisticInfo()
                    {
                        MonitorTime = DateTime.Parse(dtResult.Rows[0]["MonitorTime"].ToString()),
                        NoSync = int.Parse(dtResult.Rows[0]["NoSync"].ToString()),
                        Syncing = int.Parse(dtResult.Rows[0]["Syncing"].ToString()),
                        Synced = int.Parse(dtResult.Rows[0]["Synced"].ToString())
                    };
                    return statisticInfo;
                }
            }
            return null;
        }

        public Model.JobMonitor.SyncStatisticInfo GetOrderStatisticInfo()
        {
            string sql = String.Format(@"
SELECT  
 ISNULL(sum(CASE WHEN Syncflag = 0 THEN 1 ELSE 0 END),0) AS [NoSync]
,ISNULL(sum(CASE WHEN Syncflag = 1 THEN 1 ELSE 0 END),0) AS [Syncing]
,ISNULL(sum(CASE WHEN Syncflag = 2 THEN 1 ELSE 0 END),0) AS [Synced]
,getdate() AS [MonitorTime]
FROM LMS_SYN_TMS_INOrder WITH(NOLOCK)
"
               , (int)Enums.SC2TMSSyncFlag.Notyet
               , (int)Enums.SC2TMSSyncFlag.Synchronizing
               , (int)Enums.SC2TMSSyncFlag.Already);
            DataSet ds = ExecuteDataset(LMSWriteConnection, sql);
            if (ds != null
                && ds.Tables.Count > 0)
            {
                DataTable dtResult = ds.Tables[0];
                if (dtResult.Rows.Count > 0)
                {
                    SyncStatisticInfo statisticInfo = new SyncStatisticInfo()
                    {
                        MonitorTime = DateTime.Parse(dtResult.Rows[0]["MonitorTime"].ToString()),
                        NoSync = int.Parse(dtResult.Rows[0]["NoSync"].ToString()),
                        Syncing = int.Parse(dtResult.Rows[0]["Syncing"].ToString()),
                        Synced = int.Parse(dtResult.Rows[0]["Synced"].ToString())
                    };
                    return statisticInfo;
                }
            }
            return null;
        }

        #endregion
    }
}

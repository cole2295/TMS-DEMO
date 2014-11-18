using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Outbound
{
    /// <summary>
    /// 出库批次数据层实现
    /// </summary>
    public class OutboundBatchDAL : BaseDAL, IOutboundBatchDAL
    {
        #region IOutboundBatchDAL 成员

        public int Add(OutboundBatchEntityModel batchModel)
        {
            if (batchModel == null) throw new ArgumentNullException("batchModel is null");
            String sql = String.Format(@"
INSERT INTO SC_OutboundBatch(OBBID, BatchNo, DepartureID, ArrivalID, OutboundCount, SyncFlag, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted)
VALUES({0}, :BatchNo, :DepartureID, :ArrivalID, :OutboundCount, :SyncFlag, :CreateBy, SYSDATE, :UpdateBy, SYSDATE, 0)
",
   batchModel.KeyCodeNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BatchNo",  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = batchModel.BatchNo},
                new OracleParameter() { ParameterName = "DepartureID",  OracleDbType = OracleDbType.Int32 , Value = batchModel.DepartureID},
                new OracleParameter() { ParameterName = "ArrivalID", OracleDbType = OracleDbType.Int32 , Value = batchModel.ArrivalID},
                new OracleParameter() { ParameterName = "OutboundCount", OracleDbType = OracleDbType.Int32 , Value = batchModel.OutboundCount},
                new OracleParameter() { ParameterName = "SyncFlag", OracleDbType = OracleDbType.Int16 , Value = (int)batchModel.SyncFlag},
                new OracleParameter() { ParameterName = "CreateBy",  OracleDbType = OracleDbType.Int32 , Value = batchModel.CreateBy},
                new OracleParameter() { ParameterName = "UpdateBy",  OracleDbType = OracleDbType.Int32 , Value = batchModel.UpdateBy}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public bool Exists(string batchNo)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IOutboundBatchDAL 成员

        /// <summary>
        /// 是否已经从TMS同步到LMS物流主库
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public bool IsTmsSync2Lms(string batchNo)
        {
            if (String.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException("batchNo is null or empty.");
            String sql = @"
SELECT 1
FROM SC_OutboundBatch
    WHERE BatchNo = :BatchNo
    AND SyncFlag = :SyncFlag
    AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" ,  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "BatchNo" ,  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = batchNo }
            };
            object objValue = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得TMS出库批次记录需要同步到LMS物流主库的出库批次实体对象
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public OutboundBatchEntityModel GetOutboundBatchEntityModel4TmsSync2Lms(string batchNo)
        {
            if (String.IsNullOrWhiteSpace(batchNo)) throw new ArgumentNullException("batchNo is null or empty.");
            String sql = @"
SELECT *
FROM
(
    SELECT  OBBID, BatchNo, DepartureID, ArrivalID, OutboundCount, CreateBy, CreateTime, UpdateBy, UpdateTime
    FROM SC_OutboundBatch
    WHERE BatchNo = :BatchNo
    AND SyncFlag = :SyncFlag
    ORDER BY CreateTime ASC
)
WHERE ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" ,  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "BatchNo" ,  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = batchNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<OutboundBatchEntityModel>(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IOutboundBatchDAL 成员


        public int UpdateSyncedStatus4Tms2Lms(string outboundBatchKey)
        {
            if (String.IsNullOrWhiteSpace(outboundBatchKey)) throw new ArgumentNullException("outboundBatchKey is null or empty.");
            String sql = @"
UPDATE SC_OutboundBatch
SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
WHERE OBBID = :OBBID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" ,  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "OBBID" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = outboundBatchKey }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

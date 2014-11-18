using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Inbound
{
    /// <summary>
    /// 入库队列数据层
    /// </summary>
    public class InboundQueueDAL : BaseDAL, IInboundQueueDAL
    {
        #region IInboundQueueDAL 成员

        /// <summary>
        /// 取得需要待处理的分拣服务队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public List<InboundQueueEntityModel> GetInboundQueueList(InboundQueueJobArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundQueueJobArgModel is null.");
            String sql = String.Format(@"
SELECT *
FROM
(
    SELECT  SC_InboundQueue.IBSID
        , SC_InboundQueue.FormCode
        , SC_InboundQueue.OpCount
        , SC_InboundQueue.DepartureID
        , SC_InboundQueue.ArrivalID
        , SC_InboundQueue.Status
        , SC_InboundQueue.SeqStatus
        , SC_InboundQueue.AgentType
        , SC_InboundQueue.AgentUserID
        , SC_InboundQueue.Createby
        , SC_InboundQueue.CreateDept
        , SC_InboundQueue.Createtime
        , SC_InboundQueue.Updatetime
        , SC_InboundQueue.Isdeleted 
        , SC_InboundQueue.Version
        , SC_InboundQueue.DistributionCode
    FROM SC_InboundQueue 
    WHERE SC_InboundQueue.CreateTime >= :SyncTime
           AND SC_InboundQueue.SeqStatus = {0}
           AND SC_InboundQueue.OpCount <= :OpCount
           AND SC_InboundQueue.IsDeleted = 0
    ORDER BY SC_InboundQueue.CreateTime ASC
)
WHERE ROWNUM <= :TopCount
",
(int)Enums.SeqStatus.NoHand 
 );
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncTime" , OracleDbType = OracleDbType.Date , Value = argument.SyncTime },
                new OracleParameter() { ParameterName="TopCount", OracleDbType = OracleDbType.Int32  , Value = argument.PerBatchCount },
                new OracleParameter() { ParameterName="OpCount",  OracleDbType = OracleDbType.Int32 , Value =  argument.OpCount }
            };
            var result = ExecuteSql_ByDataTableReflect<InboundQueueEntityModel>(TMSWriteConnection, sql, arguments);
            if (result != null)
            {
                var lookupResult =
                                   from p in result
                                   where long.Parse(p.FormCode) % argument.ModValue == argument.Remaider
                                   select p;
                if (lookupResult != null)
                {
                    return lookupResult.ToList();
                }
            }
            return null;
        }

        public int UpdateOpCount(long ID)
        {
            String sql = @"
UPDATE SC_InboundQueue
    SET  OpCount = OpCount+1  , UpdateTime = SYSDATE
WHERE IBSID = :IBSID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IBSID",  OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int UpdateToHandled(long ID)
        {
            String sql = @"
UPDATE SC_InboundQueue
    SET SeqStatus = :SeqStatus  , UpdateTime = SYSDATE
WHERE IBSID = :IBSID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IBSID", OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value =(int)Enums.SeqStatus.Handed }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int UpdateToError(long ID)
        {
            String sql = @"
UPDATE SC_InboundQueue
    SET SeqStatus = :SeqStatus  , UpdateTime = SYSDATE
WHERE IBSID = :IBSID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IBSID",  OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value =(int)Enums.SeqStatus.Error }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 新增入库队列数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(InboundQueueEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundQueueEntityModel is null.");
            String sql = String.Format(@"
INSERT INTO SC_InboundQueue(IBSID,FormCode,OpCount,DepartureID,ArrivalID,Status,SeqStatus, AgentType, AgentUserID, CreateBy,CreateDept,IsDeleted)
VALUES({0} ,:FormCode ,0 ,:DepartureID ,:ArrivalID ,:Status ,:SeqStatus, :AgentType, :AgentUserID, :CreateBy ,:CreateDept ,0)
",
   model.SequenceNextValue()
);
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode},
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32  , Value = model.DepartureID},                
                new OracleParameter() { ParameterName="ArrivalID", OracleDbType = OracleDbType.Int32 , Value = model.ArrivalID},
                new OracleParameter() { ParameterName="Status",  OracleDbType = OracleDbType.Int16 , Value = (int)model.Status},
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)model.SeqStatus},
                new OracleParameter() { ParameterName="CreateBy",  OracleDbType = OracleDbType.Int32 , Value = model.CreateBy},
                new OracleParameter() { ParameterName="CreateDept",  OracleDbType = OracleDbType.Int32 , Value = model.CreateDept},      
                new OracleParameter() { ParameterName="AgentType",  OracleDbType = OracleDbType.Int16 , Value = (int)model.AgentType},
                new OracleParameter() { ParameterName="AgentUserID",  OracleDbType = OracleDbType.Int32 , Value = model.AgentUserID}      
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        #endregion


        public int AddV2(InboundQueueEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundQueueEntityModel is null.");
            String sql = String.Format(@"
INSERT INTO SC_InboundQueue(IBSID,FormCode,OpCount,DepartureID,ArrivalID ,Status,SeqStatus, AgentType, AgentUserID, CreateBy,CreateDept,IsDeleted,Version,DistributionCode)
VALUES({0} ,:FormCode ,0 ,:DepartureID ,:ArrivalID ,:Status,:SeqStatus,:AgentType, :AgentUserID, :CreateBy ,:CreateDept ,0,2,:DistributionCode)
",
   model.SequenceNextValue()
);
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode},
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32  , Value = model.DepartureID},  
                new OracleParameter() { ParameterName="ArrivalID", OracleDbType = OracleDbType.Int32 , Value = model.ArrivalID},
                new OracleParameter() { ParameterName="Status",  OracleDbType = OracleDbType.Int16 , Value = (int)model.Status},
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)model.SeqStatus},
                new OracleParameter() { ParameterName="CreateBy",  OracleDbType = OracleDbType.Int32 , Value = model.CreateBy},
                new OracleParameter() { ParameterName="CreateDept",  OracleDbType = OracleDbType.Int32 , Value = model.CreateDept},      
                new OracleParameter() { ParameterName="AgentType",  OracleDbType = OracleDbType.Int16 , Value = (int)model.AgentType},
                new OracleParameter() { ParameterName="AgentUserID",  OracleDbType = OracleDbType.Int32 , Value = model.AgentUserID},
                new OracleParameter() { ParameterName = "DistributionCode",OracleDbType = OracleDbType.Varchar2,Value = model.DistributionCode} 
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }
    }
}

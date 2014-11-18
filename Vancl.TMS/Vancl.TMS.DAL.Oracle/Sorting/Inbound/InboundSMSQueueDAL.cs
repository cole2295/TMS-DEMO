using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Inbound
{
    public class InboundSMSQueueDAL : BaseDAL, IInboundSMSQueueDAL
    {
        #region IInboundSMSQueueDAL 成员

        /// <summary>
        /// 添加入库短信队列项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(InboundSMSQueueEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundSMSQueueEntityModel is null.");
            String sql = String.Format(@"
INSERT INTO SC_InboundSMSQueue(QUID,FormCode,OpCount,DepartureID,ArrivalID,SendTime,SeqStatus,SendedContent ,ErrorInfo, IsDeleted)
VALUES({0} ,:FormCode ,0 ,:DepartureID ,:ArrivalID ,:SendTime ,:SeqStatus, :SendedContent ,:ErrorInfo ,0)
",
   model.SequenceNextValue()
);
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode},
                new OracleParameter() { ParameterName="DepartureID",  OracleDbType = OracleDbType.Int32 , Value = model.DepartureID},                
                new OracleParameter() { ParameterName="ArrivalID",  OracleDbType = OracleDbType.Int32 , Value = model.ArrivalID},
                new OracleParameter() { ParameterName="SendTime",  OracleDbType = OracleDbType.Date , Value = model.SendTime},
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)model.SeqStatus},
                new OracleParameter() { ParameterName="SendedContent",  OracleDbType = OracleDbType.Varchar2 , Size = 2000, Value = model.SendedContent},
                new OracleParameter() { ParameterName="ErrorInfo",  OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = model.ErrorInfo}      
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 取得需要待发送短信的入库短信队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public List<InboundSMSQueueEntityModel> GetInboundSMSQueue(InboundSMSQueueJobArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundQueueJobArgModel is null.");
            String sql = @"
SELECT *
FROM
(
    SELECT  QUID
        , FormCode
        , OpCount
        , DepartureID
        , ArrivalID
        , SendTime
        , SeqStatus
        , SendedContent
    FROM SC_InboundSMSQueue 
    WHERE CreateTime >=  :SyncTime
           AND SendTime <= SYSDATE
           AND SeqStatus = :SeqStatus
           AND OpCount <= :OpCount
           AND IsDeleted = 0
    ORDER BY CreateTime ASC
)
WHERE ROWNUM <= :TopCount
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncTime" ,  OracleDbType = OracleDbType.Date , Value = argument.SyncTime  },
                new OracleParameter() { ParameterName="TopCount",  OracleDbType = OracleDbType.Int32 , Value = argument.PerBatchCount },
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="OpCount",  OracleDbType = OracleDbType.Int16 , Value =  argument.OpCount}
            };
            var listModel = ExecuteSql_ByDataTableReflect<InboundSMSQueueEntityModel>(TMSWriteConnection, sql, arguments);
            if (listModel != null)
            {
                return listModel.ToList();
            }
            return null;
        }

        /// <summary>
        /// 处理计数自动增1
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateOpCount(long ID, string ErrorInfo)
        {
            String sql = @"
UPDATE SC_InboundSMSQueue
    SET OpCount = OpCount + 1  , ErrorInfo = :ErrorInfo , UpdateTime = SYSDATE
WHERE QUID = :QUID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="QUID",  OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="ErrorInfo",  OracleDbType = OracleDbType.Varchar2 , Size = 4000, Value = ErrorInfo }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 更新为已处理状态
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateToHandled(long ID)
        {
            String sql = @"
UPDATE SC_InboundSMSQueue
    SET SeqStatus = :SeqStatus  , UpdateTime = SYSDATE
WHERE QUID = :QUID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="QUID",  OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value =(int)Enums.SeqStatus.Handed }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 更新为处理失败
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ErrorInfo"></param>
        /// <returns></returns>
        public int UpdateToError(long ID, string ErrorInfo)
        {
            String sql = @"
UPDATE SC_InboundSMSQueue
    SET SeqStatus = :SeqStatus  , ErrorInfo = :ErrorInfo ,  UpdateTime = SYSDATE
WHERE QUID = :QUID AND SeqStatus = :PreSeqStatus
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="QUID",  OracleDbType = OracleDbType.Int64 , Value = ID },
                new OracleParameter() { ParameterName="PreSeqStatus",  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                new OracleParameter() { ParameterName="SeqStatus",  OracleDbType = OracleDbType.Int16 , Value =(int)Enums.SeqStatus.Error },                
                new OracleParameter() { ParameterName="ErrorInfo",  OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = ErrorInfo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

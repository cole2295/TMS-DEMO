using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.Synchronous;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.DAL.Oracle.Synchronous
{
    public class OutboundLMSDAL : BaseDAL, IOutboundLMSDAL
    {

        #region IOutboundLMSDAL 成员

        /// <summary>
        /// 取得正常分拣装箱对象
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private OutBoxModel GetNormalBoxModel(OutboundReadParam argument)
        {
            string sql = String.Format(@"
SELECT *
FROM
(
    SELECT SSTOID, BOXNO , DepartureID,  ArrivalID, SC2TMSFLAG, CREATETIME, NOTYPE
    FROM sc_syn_tms_outbox
    WHERE  CreateTime > :SyncTime
            AND SC2TMSFLAG = :SC2TMSFLAG
            AND  NOTYPE = :NOTYPE
    ORDER BY CreateTime ASC
)
WHERE ROWNUM <= :TopCount
");
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="SyncTime" , OracleDbType = OracleDbType.Date , Value = argument.SyncTime },
                new OracleParameter() { ParameterName="SC2TMSFLAG" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SC2TMSSyncFlag.Notyet   },
                new OracleParameter() { ParameterName="NOTYPE", OracleDbType = OracleDbType.Int16 ,  Value = (int)Enums.SyncNoType.Box },
                new OracleParameter() { ParameterName="TopCount" , OracleDbType = OracleDbType.Int16, Value = 1 }                   // 每次处理一个批次
            };
            var boxModel = ExecuteSqlSingle_ByReaderReflect<OutBoxModel>(TMSWriteConnection, sql, arguments);
            if (boxModel != null)
            {
                String strsql = @"
SELECT BOXNO, WEIGHT, BILLCOUNT AS TotalCount
FROM SC_INBOUNDPACKING
WHERE BOXNO = :BOXNO
AND ROWNUM = 1
";
                OracleParameter[] parameters = new OracleParameter[] 
                { 
                    new OracleParameter() { ParameterName="BOXNO" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxModel.BoxNo }
                };
                var tmpboxModel = ExecuteSqlSingle_ByReaderReflect<OutBoxModel>(TMSWriteConnection, strsql, parameters);
                if (tmpboxModel != null)
                {
                    boxModel.Weight = tmpboxModel.Weight;
                    boxModel.TotalCount = tmpboxModel.TotalCount;
                }
                return boxModel;
            }
            return null;
        }


        /// <summary>
        /// 取得批次号箱的对象
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        private OutBoxModel GetBatchBoxModel(OutboundReadParam argument)
        {
            string sql = String.Format(@"
SELECT *
FROM
(
    SELECT SSTOID, BOXNO , DepartureID,  ArrivalID, SC2TMSFLAG, CREATETIME, NOTYPE
    FROM sc_syn_tms_outbox
    WHERE  CreateTime > :SyncTime
            AND SC2TMSFLAG = :SC2TMSFLAG
            AND  NOTYPE = :NOTYPE
    ORDER BY CreateTime ASC
)
WHERE ROWNUM <= :TopCount
");
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="SyncTime" , OracleDbType = OracleDbType.Date , Value = argument.SyncTime },
                new OracleParameter() { ParameterName="SC2TMSFLAG" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SC2TMSSyncFlag.Notyet   },
                new OracleParameter() { ParameterName="NOTYPE", OracleDbType = OracleDbType.Int16 ,  Value = (int)Enums.SyncNoType.Batch },
                new OracleParameter() { ParameterName="TopCount" , OracleDbType =OracleDbType.Int16, Value = 1 }                   // 每次处理一个批次
            };
            return ExecuteSqlSingle_ByReaderReflect<OutBoxModel>(TMSWriteConnection, sql, arguments);
        }

        public Model.Synchronous.OutBoxModel GetBoxModel(Model.Synchronous.OutboundReadParam argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundReadParam is null");
            if (argument.NoType == Enums.SyncNoType.Box)
            {
                return GetNormalBoxModel(argument);
            }
            return GetBatchBoxModel(argument);
        }

        private List<OrderModel> GetNormalBoxOrderList(OutBoxModel boxModel)
        {
            //注意枚举BillGoodsType到GoodsType的转换
            String sql = @"
SELECT bill.formcode, bill.billtype AS LMSwaybillType , bill.Customerorder, Pack.DEPARTUREID, Pack.ARRIVALID ,
Pack.Boxno AS BOXNO, BILLINFO.WEIGHT, DECODE(BILLINFO.BillGoodsType,0, 1 , 1, 2, 2, 4, 1 )AS GoodsType , BILLINFO.TOTALAMOUNT AS Price
,BILLINFO.INSUREDAMOUNT AS ProtectedPrice
FROM  SC_INBOUNDPACKING Pack
      JOIN SC_INBOUNDPACKINGDETAIL PackDetail ON Pack.Boxno = PackDetail.Boxno
      JOIN SC_BILL BILL ON PackDetail.FORMCODE = BILL.FORMCODE
      JOIN SC_BILLINFO BILLINFO ON BILL.FORMCODE = BILLINFO.FORMCODE
WHERE Pack.Boxno = :BoxNo
AND Pack.IsDeleted = 0 
AND PackDetail.IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxModel.BoxNo }
            };
            var listResult = ExecuteSql_ByReaderReflect<OrderModel>(TMSWriteConnection, sql, parameters);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }


        /// <summary>
        /// 取得按照批次号的箱子订单信息对象
        /// </summary>
        /// <param name="boxModel"></param>
        /// <returns></returns>
        private List<OrderModel> GetBatchBoxOrderList(OutBoxModel boxModel)
        {
            //注意枚举BillGoodsType到GoodsType的转换
            String sql = @"
SELECT bill.formcode, bill.billtype AS LMSwaybillType , bill.Customerorder, OB.DEPARTUREID, OB.ARRIVALID ,
OB.BATCHNO AS BOXNO, BILLINFO.WEIGHT, DECODE(BILLINFO.BillGoodsType,0, 1 , 1, 2, 2, 4, 1 )AS GoodsType , BILLINFO.TOTALAMOUNT AS Price
,BILLINFO.INSUREDAMOUNT AS ProtectedPrice
FROM  SC_OUTBOUND OB
      JOIN SC_BILL BILL ON OB.FORMCODE = BILL.FORMCODE
      JOIN SC_BILLINFO BILLINFO ON BILL.FORMCODE = BILLINFO.FORMCODE
WHERE OB.BATCHNO = :BatchNo
AND OB.IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BatchNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxModel.BoxNo }
            };
            var listResult = ExecuteSql_ByReaderReflect<OrderModel>(TMSWriteConnection, sql, parameters);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        public List<Model.BaseInfo.Order.OrderModel> GetOrderList(Model.Synchronous.OutBoxModel boxModel)
        {
            if (boxModel == null) throw new ArgumentNullException("OutBoxModel is null.");
            if (boxModel.NoType == Enums.SyncNoType.Box)
            {
                return GetNormalBoxOrderList(boxModel);
            }
            return GetBatchBoxOrderList(boxModel);
        }

        public List<Model.BaseInfo.Order.OrderDetailModel> GetOrderDetailList(List<long> listWaybillNo)
        {
            throw new NotImplementedException();
        }

        public void UpdateBoxStatus(Model.Synchronous.OutBoxModel boxModel, Model.Common.Enums.SC2TMSSyncFlag prevSyncFlag)
        {
            if (boxModel == null) throw new ArgumentNullException("OutBoxModel is null.");
            String sql = @"
UPDATE sc_syn_tms_outbox
    SET SC2TMSFLAG = :CurFlag, sc2tmssynctime = SYSDATE
WHERE SSTOID = :SSTOID AND SC2TMSFLAG = :PrvFlag
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SSTOID", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = boxModel.SSTOID } ,
                new OracleParameter() { ParameterName = "PrvFlag", OracleDbType = OracleDbType.Int16, Value =  (int)prevSyncFlag } ,
                new OracleParameter() { ParameterName = "CurFlag", OracleDbType = OracleDbType.Int16, Value = (int)boxModel.SC2TMSFlag } ,
            };
            ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public Model.JobMonitor.SyncStatisticInfo GetStatisticInfo()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

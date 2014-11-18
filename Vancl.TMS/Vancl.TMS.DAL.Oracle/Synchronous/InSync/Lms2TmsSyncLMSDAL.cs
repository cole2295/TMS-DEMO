using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Util.ClsExtender;

namespace Vancl.TMS.DAL.Oracle.Synchronous.InSync
{
    public class Lms2TmsSyncLMSDAL : BaseDAL, ILms2TmsSyncLMSDAL
    {
        #region ILms2TmsSyncLMSDAL 成员

        public List<LmsWaybillStatusChangeLogModel> ReadLmsChangeLogs(Lms2TmsJobArgs args)
        {
            if (args == null) throw new ArgumentNullException("args is null.");
            string strSql = @"
SELECT  
	WaybillStatusChangeLogID AS ID
	,WaybillNO
	,Status
	,MerchantID
	,DistributionCode
	,DeliverStationID
	,CreateTime
	,CreateBy
	,CreateStation
	,NOTE
	,CustomerOrder
	,OperateType
	,SubStatus AS ReturnStatus
FROM LMS_WaybillStatusChangeLog  
WHERE CreateTime > :SyncTime
	AND TmsSyncStatus = :TmsSyncStatus 
	AND OperateType IS NOT NULL
    AND OperateType <> -1 
    AND ROWNUM<=:TopCount
ORDER BY CreateTime
";
            OracleParameter[] arguments = new OracleParameter[] 
			{ 
				new OracleParameter(){ ParameterName="TopCount",  DbType = DbType.Int32, Value = args.TopCount },
				new OracleParameter() { ParameterName="SyncTime", DbType = DbType.DateTime, Value = args.SyncTime},
				new OracleParameter() { ParameterName="TmsSyncStatus", DbType = DbType.Int16, Value = (int)Enums.SyncStatus.NotYet }
			};
            var result = ExecuteSql_ByReaderReflect<LmsWaybillStatusChangeLogModel>(LMSOracleWriteConnection, strSql, arguments);
            if (result != null)
            {
                var lookupResult =
                                   from p in result
                                   where p.WaybillNo % args.Mod == args.Remainder
                                   select p;
                if (lookupResult != null)
                {
                    return lookupResult.ToList();
                }
            }
            return null;
        }

        public System.Data.DataTable GetLmsWayBillData(LmsWaybillStatusChangeLogModel model)
        {
            string strSql = string.Format(@"
				SELECT 
					w.WaybillType
					,w.CreatBy
					,w.CreatStation
					,w.CreatTime
					,w.DeliverCode
					,w.DistributionCode
					,w.BackStatus
					,w.Sources
					,w.WarehouseID
					,w.Tips
					,w.CurrentDistributionCode
					,wi.WaybillProperty
					,wi.WayBillBoxNo
					,wi.MerchantWeight
					,wi.WayBillInfoWeight
					,wi.WaybillBoxModel
					,wsi.ProtectedPrice
					,wsi.PackageCount
					,wsi.AcceptType
					,wsi.NeedAmount
					,wsi.Amount
				FROM Waybill w
				JOIN WaybillInfo wi
					ON wi.WaybillNo=w.WaybillNo
				JOIN WaybillSignInfo wsi
					ON wsi.WaybillNo=w.WaybillNo
				WHERE w.WaybillNo=:WaybillNo AND ROWNUM=1");
            OracleParameter[] parameters = { new OracleParameter(){ ParameterName="WaybillNo",  DbType = DbType.Int64, Value = model.WaybillNo }};
            return ExecuteSqlDataTable(LMSOracleWriteConnection, strSql, parameters);
        }

        public int UpdateSyncStatus(long id, Enums.SyncStatus status)
        {
            string strSql = @"
				UPDATE LMS_WaybillStatusChangeLog
				SET TmsSyncStatus=:TmsSyncStatus
				WHERE WaybillStatusChangeLogID=:WaybillStatusChangeLogID";
            OracleParameter[] parameters = { 
				new OracleParameter() { ParameterName="TmsSyncStatus", DbType=DbType.Int16, Value = Convert.ToInt16(status) },
				new OracleParameter() { ParameterName="WaybillStatusChangeLogID", DbType=DbType.Int64, Value = id }
			};
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, strSql, parameters);
        }

        public string GetCurrentDistributionCode(long waybillNo)
        {
            string strSql = @"
				SELECT CurrentDistributionCode
				FROM Waybill 
				WHERE WaybillNo=:WaybillNo";
            OracleParameter[] parameters = { new OracleParameter() { ParameterName="WaybillNo", DbType=DbType.Int64, Value = waybillNo }
                                           };
            object o = ExecuteSqlScalar(LMSOracleWriteConnection, strSql, parameters);
            if (o == null || o == DBNull.Value)
            {
                return "";
            }
            return Convert.ToString(o);
        }

        public string GetTurnStationKeyCode(long waybillNo)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ILms2TmsSyncLMSDAL 成员


        public Model.LMS.WaybillEntityModel GetWaybillModel4Lms2Tms_Inbound(long waybillNo)
        {
            String sql = @"
SELECT WaybillNO, InboundID, OutboundID
FROM Waybill 
WHERE WaybillNO = :WaybillNO
AND IsDelete = 0 AND ROWNUM=1
";
            OracleParameter[] arguments = new OracleParameter[] 
			{
				new OracleParameter() { ParameterName = "WaybillNO", DbType = DbType.Int64, Value = waybillNo }
			};
            return ExecuteSqlSingle_ByReaderReflect<WaybillEntityModel>(LMSOracleWriteConnection, sql, arguments);
        }

        public Model.LMS.WaybillEntityModel GetWaybillModel4Lms2Tms_Outbound(long waybillNo)
        {
            String sql = @"
SELECT WaybillNO, OutboundID, CurrentDistributionCode
FROM Waybill 
WHERE WaybillNO = :WaybillNO
AND IsDelete = 0 AND ROWNUM=1
";
            OracleParameter[] arguments = new OracleParameter[] 
			{
				new OracleParameter() { ParameterName = "WaybillNO", DbType = DbType.Int64, Value = waybillNo }
			};
            return ExecuteSqlSingle_ByReaderReflect<WaybillEntityModel>(LMSOracleWriteConnection, sql, arguments);
        }

        #endregion

        #region ILms2TmsSyncLMSDAL 成员


        public Model.LMS.WaybillEntityModel GetWaybillModel4Lms2Tms_TurnStation(long waybillNo)
        {
            string strSql = @"
SELECT wb.WaybillNo, wb.Status, wb.TurnStationID, ts.Isfast
FROM Waybill wb 
	JOIN TurnStation ts ON wb.TurnStationID = ts.TurnStationID AND wb.WaybillNo = ts.WaybillNo
WHERE wb.WaybillNo = :WaybillNo AND ROWNUM=1
";
            OracleParameter[] parameters = { new OracleParameter() { ParameterName = "WaybillNo", DbType = DbType.Int64, Value = waybillNo } };

            return ExecuteSqlSingle_ByDataTableReflect<WaybillEntityModel>(LMSOracleWriteConnection, strSql, parameters);
        }

        #endregion
    }
}

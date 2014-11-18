using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.Synchronous.InSync;
using System.Data;
using Vancl.TMS.Util.ClsExtender;
using Vancl.TMS.Model.Common;
using System.Data.SqlClient;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.DAL.Sql2008.Synchronous
{
	public class Lms2TmsSyncLMSDAL : BaseDAL, ILms2TmsSyncLMSDAL
	{
		#region ILms2TmsSyncDAL 成员

		/// <summary>
		/// 读取lms中间表数据
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public List<LmsWaybillStatusChangeLogModel> ReadLmsChangeLogs(Lms2TmsJobArgs args)
		{
			if (args == null) throw new ArgumentNullException("args is null.");
			string strSql = @"
SELECT TOP (@TopCount) 
	t1.[ID]
	,t1.[WaybillNO]
	,t1.[Status]
	,t1.[MerchantID]
	,t1.[DistributionCode]
	,t1.[DeliverStationID]
	,t1.[CreateTime]
	,t1.[CreateBy]
	,t1.[CreateStation]
	,t1.[Description]
	,t1.[CustomerOrder]
	,t1.[OperateType]
	,t1.[SubStatus] AS ReturnStatus
FROM LMS_WaybillStatusChangeLog  t1 WITH (READPAST, FORCESEEK)
WHERE t1.CreateTime > @SyncTime
	AND t1.TmsSyncStatus = @TmsSyncStatus
	AND t1.OperateType IS NOT NULL
ORDER BY t1.CreateTime
";
			SqlParameter[] arguments = new SqlParameter[] 
			{ 
				new SqlParameter(){ ParameterName="@TopCount", SqlDbType = SqlDbType.Int, Value = args.TopCount },
				new SqlParameter() { ParameterName="@SyncTime", SqlDbType = SqlDbType.DateTime, Value = args.SyncTime},
				new SqlParameter() { ParameterName="@TmsSyncStatus", SqlDbType = SqlDbType.Int, Value = (int)Enums.SyncStatus.NotYet }
			};
			var result = ExecuteSql_ByReaderReflect<LmsWaybillStatusChangeLogModel>(LMSWriteConnection, strSql, arguments);
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

		/// <summary>
		/// 取得Lms运单信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public DataTable GetLmsWayBillData(LmsWaybillStatusChangeLogModel model)
		{
			string strSql = string.Format(@"
				SELECT TOP 1
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
				FROM Waybill(NOLOCK) w
				JOIN WaybillInfo(NOLOCK) wi
					ON wi.WaybillNo=w.WaybillNo
				JOIN WaybillSignInfo(NOLOCK) wsi
					ON wsi.WaybillNo=w.WaybillNo
				WHERE w.WaybillNo=@WaybillNo");
			SqlParameter[] parameters = { new SqlParameter("@WaybillNo", SqlDbType.BigInt) { Value = model.WaybillNo } };
			DataSet ds = ExecuteDataset(LMSWriteConnection, strSql, parameters);
			if (ds.IsNull())
			{
				return null;
			}
			return ds.Tables[0];
		}

		/// <summary>
		/// 更新LMS中间表同步状态
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public int UpdateSyncStatus(long id, Enums.SyncStatus status)
		{
			string strSql = @"
				UPDATE LMS_WaybillStatusChangeLog
				SET TmsSyncStatus=@TmsSyncStatus
				WHERE ID=@ID";
			SqlParameter[] parameters = { 
				new SqlParameter("@TmsSyncStatus", SqlDbType.Int,4) { Value = (int)status },
				new SqlParameter("@ID", SqlDbType.BigInt) { Value = id }
			};
			return ExecuteNonQuery(LMSWriteConnection, strSql, parameters);
		}

		/// <summary>
		/// 获取当前配送商编号
		/// </summary>
		/// <param name="waybillNo"></param>
		/// <returns></returns>
		public string GetCurrentDistributionCode(long waybillNo)
		{
			string strSql = @"
				SELECT CurrentDistributionCode
				FROM Waybill WITH(NOLOCK)
				WHERE WaybillNo=@WaybillNo";
			SqlParameter[] parameters = { new SqlParameter("@WaybillNo", SqlDbType.BigInt) { Value = waybillNo } };
			object o = ExecuteScalar(LMSWriteConnection, strSql, parameters);
			if (o == null || o == DBNull.Value)
			{
				return "";
			}
			return Convert.ToString(o);
		}

		/// <summary>
		/// 获取转站key
		/// </summary>
		/// <param name="waybillNo"></param>
		/// <returns></returns>
		public WaybillEntityModel GetWaybillModel4Lms2Tms_TurnStation(long waybillNo)
		{
			string strSql = @"
SELECT TOP 1 wb.WaybillNo, wb.Status, wb.TurnStationID, ts.Isfast
FROM Waybill wb WITH(NOLOCK) 
	JOIN TurnStation ts WITH(NOLOCK)  ON wb.TurnStationID = ts.TurnStationID AND wb.WaybillNo = ts.WaybillNo
WHERE wb.WaybillNo = @WaybillNo
";
			SqlParameter[] parameters = { new SqlParameter("@WaybillNo", SqlDbType.BigInt) { Value = waybillNo } };

			return ExecuteSqlSingle_ByDataTableReflect<WaybillEntityModel>(LMSWriteConnection, strSql, parameters);
		}


		#endregion

	   
		#region ILms2TmsSyncLMSDAL 成员

		/// <summary>
		/// 取得LMS物流主库运单信息【用于LMS到TMS入库的同步】
		/// </summary>
		/// <param name="waybillNo"></param>
		/// <returns></returns>
		public WaybillEntityModel GetWaybillModel4Lms2Tms_Inbound(long waybillNo)
		{
			String sql = @"
SELECT TOP 1 WaybillNO, InboundID, OutboundID
FROM Waybill WITH(NOLOCK)
WHERE WaybillNO = @WaybillNO
AND IsDelete = 0
";
			SqlParameter[] arguments = new SqlParameter[] 
			{
				new SqlParameter() { ParameterName = "@WaybillNO", DbType = DbType.Int64, Value = waybillNo }
			};
			return ExecuteSqlSingle_ByReaderReflect<WaybillEntityModel>(LMSWriteConnection, sql, arguments);
		}

		/// <summary>
		/// 取得LMS物流主库运单信息【用于LMS到TMS出库的同步】
		/// </summary>
		/// <param name="waybillNo"></param>
		/// <returns></returns>
		public WaybillEntityModel GetWaybillModel4Lms2Tms_Outbound(long waybillNo)
		{
			String sql = @"
SELECT TOP 1 WaybillNO, OutboundID, CurrentDistributionCode
FROM Waybill WITH(NOLOCK)
WHERE WaybillNO = @WaybillNO
AND IsDelete = 0
"; 
			SqlParameter[] arguments = new SqlParameter[] 
			{
				new SqlParameter() { ParameterName = "@WaybillNO", DbType = DbType.Int64, Value = waybillNo }
			};
			return ExecuteSqlSingle_ByReaderReflect<WaybillEntityModel>(LMSWriteConnection, sql, arguments);
		}

		#endregion
	}
}

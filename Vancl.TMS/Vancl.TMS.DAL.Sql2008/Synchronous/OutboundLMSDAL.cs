using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using System.Data;
using Vancl.TMS.Model.Synchronous;
using System.Data.SqlClient;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.Model.JobMonitor;


namespace Vancl.TMS.DAL.Sql2008.Synchronous
{
	public class OutboundLMSDAL : BaseDAL, IOutboundLMSDAL
	{

		#region IOutboundReadDAL 成员

		/// <summary>
		/// 取得正常箱号的对象
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		private Model.Synchronous.OutBoxModel GetNormalBoxModel(Model.Synchronous.OutboundReadParam argument)
		{
			string sqlboxInfo = @"
SELECT TOP 1 WaybillCount,BoxWeight
FROM InBoundBox WITH(NOLOCK)
WHERE BoxNo = @BoxNo 
";
			string sql = String.Format(@"
SELECT TOP 1 ID,BoxNo,DepartureID,ArrivalID,SyncFlag,CreateTime
FROM
	(
		SELECT TOP 50 ID,convert(bigint,BoxNo) AS BoxNo ,DepartureID,ArrivalID,SyncFlag,CreateTime
		FROM LMS_SYN_TMS_OUTBOX LSTO WITH(READPAST)
		WHERE SyncFlag = 0	AND NoType = 1      --箱号编码类型
		ORDER BY CreateTime ASC
	) AS INNERTB
WHERE INNERTB.BoxNo % {0} = {1}
ORDER BY CreateTime ASC
", argument.NormalModValue
 , argument.Remaider
 );
			DataSet ds = ExecuteDataset(LMSWriteConnection, sql);
			if (ds != null
				&& ds.Tables.Count > 0)
			{
				DataTable dtResult = ds.Tables[0];
				if (dtResult.Rows.Count > 0)
				{
					OutBoxModel model = new OutBoxModel();
					model.SSTOID = dtResult.Rows[0]["ID"].ToString();
					model.BoxNo = dtResult.Rows[0]["BoxNo"].ToString();
					model.DepartureID = int.Parse(dtResult.Rows[0]["DepartureID"].ToString());
					model.ArrivalID = int.Parse(dtResult.Rows[0]["ArrivalID"].ToString());
					model.SC2TMSFlag = (Model.Common.Enums.SC2TMSSyncFlag)int.Parse(dtResult.Rows[0]["SyncFlag"].ToString());
					model.CreateTime = DateTime.Parse(dtResult.Rows[0]["CreateTime"].ToString());
					model.NoType = Enums.SyncNoType.Box;
					SqlParameter[] sqlargs = new SqlParameter[] {
						new SqlParameter(){ ParameterName="@BoxNo", DbType = DbType.String, Value= model.BoxNo }
					};
					DataSet dsBoxInfo = ExecuteDataset(LMSWriteConnection, sqlboxInfo, sqlargs);
					if (dsBoxInfo != null && dsBoxInfo.Tables.Count > 0)
					{
						if (dsBoxInfo.Tables[0].Rows.Count > 0)
						{
							model.Weight = decimal.Parse(dsBoxInfo.Tables[0].Rows[0]["BoxWeight"].ToString());
							return model;
						}
					}
				}
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
SELECT TOP 1 ID,BoxNo,DepartureID,ArrivalID,SyncFlag,CreateTime
FROM
	(
		SELECT TOP 50 ID,convert(bigint,BoxNo) AS BoxNo ,DepartureID,ArrivalID,SyncFlag,CreateTime
		FROM LMS_SYN_TMS_OUTBOX LSTO WITH(READPAST)
		WHERE SyncFlag = 0	AND  NoType = 2      --批次号编码类型
		ORDER BY CreateTime ASC
	) AS INNERTB
WHERE INNERTB.BoxNo % {0} = {1}
ORDER BY CreateTime ASC
", argument.BatchModValue
 , argument.Remaider
 );
			DataSet ds = ExecuteDataset(LMSWriteConnection, sql);
			if (ds != null
				&& ds.Tables.Count > 0)
			{
				DataTable dtResult = ds.Tables[0];
				if (dtResult.Rows.Count > 0)
				{
					OutBoxModel model = new OutBoxModel();
					model.SSTOID = dtResult.Rows[0]["ID"].ToString();
					model.BoxNo = dtResult.Rows[0]["BoxNo"].ToString();
					model.DepartureID = int.Parse(dtResult.Rows[0]["DepartureID"].ToString());
					model.ArrivalID = int.Parse(dtResult.Rows[0]["ArrivalID"].ToString());
					model.SC2TMSFlag = (Model.Common.Enums.SC2TMSSyncFlag)int.Parse(dtResult.Rows[0]["SyncFlag"].ToString());
					model.CreateTime = DateTime.Parse(dtResult.Rows[0]["CreateTime"].ToString());
					model.NoType = Enums.SyncNoType.Batch;
					model.Weight = 0;
					return model;
				}
			}
			return null;
		}

		/// <summary>
		/// 根据条件取得一条未同步的箱对象
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public Model.Synchronous.OutBoxModel GetBoxModel(Model.Synchronous.OutboundReadParam argument)
		{
			if (null == argument) throw new ArgumentNullException("OutboundReadParam");
			if (argument.NoType == Enums.SyncNoType.Box)
			{
				return GetNormalBoxModel(argument);
			}
			return GetBatchBoxModel(argument);
		}

		/// <summary>
		/// LMS,TMS系统匹配货物类型枚举
		/// </summary>
		/// <param name="orModel"></param>
		/// <param name="LMSGoodsType"></param>
		private void MatchGoodsType(OrderModel orModel, Enums.LMSSyncGoodsType LMSGoodsType)
		{
			switch (LMSGoodsType)
			{
				case Enums.LMSSyncGoodsType.Normal:
					orModel.GoodsType = Enums.GoodsType.Normal;
					break;
				case Enums.LMSSyncGoodsType.Frangible:
					orModel.GoodsType = Enums.GoodsType.Frangible;
					break;
				case Enums.LMSSyncGoodsType.Contraband:
					orModel.GoodsType = Enums.GoodsType.Contraband;
					break;
				default:
					orModel.GoodsType = Enums.GoodsType.Normal;
					break;
			}
		}


		/// <summary>
		/// 取得正常箱子订单信息对象
		/// </summary>
		/// <param name="boxModel"></param>
		/// <returns></returns>
		private List<Model.BaseInfo.Order.OrderModel> GetNormalBoxOrderList(OutBoxModel boxModel)
		{
			String sql = @"
SELECT W.WaybillNO,W.CustomerOrder,W.WaybillType,WI.WaybillProperty AS GoodsType,WSI.ProtectedPrice
FROM InBoundBoxDetail BD WITH(NOLOCK)
JOIN Waybill W WITH(NOLOCK) ON BD.WaybillNo = W.WaybillNO AND W.IsDelete = 0
JOIN WaybillSignInfo WSI WITH(NOLOCK) ON W.WaybillNO = WSI.WaybillNO
JOIN WaybillInfo WI WITH(NOLOCK) ON WI.WaybillNO = W.WaybillNO
WHERE BD.BoxNo = @BoxNo
";
			SqlParameter[] arguments = new SqlParameter[] { 
				new SqlParameter(){ ParameterName="@BoxNo", DbType= DbType.String, Value= boxModel.BoxNo}
			};
			DataSet ds = ExecuteDataset(LMSWriteConnection, sql, arguments);
			if (ds != null
				&& ds.Tables.Count > 0)
			{
				DataTable dtResult = ds.Tables[0];
				if (dtResult.Rows.Count > 0)
				{
					List<OrderModel> listOrder = new List<OrderModel>();
					foreach (DataRow item in dtResult.Rows)
					{
						OrderModel orModel = new OrderModel()
						{
							BoxNo = boxModel.BoxNo,
							Price = 0,
							ProtectedPrice = decimal.Parse(item["ProtectedPrice"].ToString()),
							FormCode = item["WaybillNO"].ToString(),
							CustomerOrder = item["CustomerOrder"].ToString(),
							LMSwaybillNo = null,
							LMSwaybillType = int.Parse(item["WaybillType"].ToString()),
							DepartureID = boxModel.DepartureID,
							ArrivalID = boxModel.ArrivalID
						};
						MatchGoodsType(orModel, (Enums.LMSSyncGoodsType)int.Parse(item["GoodsType"].ToString()));
						listOrder.Add(orModel);
					}
					return listOrder;
				}
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
			String sql = @"
SELECT W.WaybillNO,W.CustomerOrder,W.WaybillType,WI.WaybillProperty AS GoodsType,WSI.ProtectedPrice
FROM OutBound BD WITH(NOLOCK)
JOIN Waybill W WITH(NOLOCK) ON BD.WaybillNo = W.WaybillNO AND W.IsDelete = 0
JOIN WaybillSignInfo WSI WITH(NOLOCK) ON W.WaybillNO = WSI.WaybillNO
JOIN WaybillInfo WI WITH(NOLOCK) ON WI.WaybillNO = W.WaybillNO
WHERE BD.BatchNO = @BatchNO
";
			SqlParameter[] arguments = new SqlParameter[] { 
				new SqlParameter(){ ParameterName="@BatchNO", DbType= DbType.String, Value= boxModel.BoxNo}
			};
			DataSet ds = ExecuteDataset(LMSWriteConnection, sql, arguments);
			if (ds != null
				&& ds.Tables.Count > 0)
			{
				DataTable dtResult = ds.Tables[0];
				if (dtResult.Rows.Count > 0)
				{
					List<OrderModel> listOrder = new List<OrderModel>();
					foreach (DataRow item in dtResult.Rows)
					{
						OrderModel orModel = new OrderModel()
						{
							BoxNo = boxModel.BoxNo,
							Price = 0,
							ProtectedPrice = decimal.Parse(item["ProtectedPrice"].ToString()),
							FormCode = item["WaybillNO"].ToString(),
							CustomerOrder = item["CustomerOrder"].ToString(),
							LMSwaybillNo = null,
							LMSwaybillType = int.Parse(item["WaybillType"].ToString()),
							DepartureID = boxModel.DepartureID,
							ArrivalID = boxModel.ArrivalID
						};
						MatchGoodsType(orModel, (Enums.LMSSyncGoodsType)int.Parse(item["GoodsType"].ToString()));
						listOrder.Add(orModel);
					}
					return listOrder;
				}
			}
			return null;
		}

		/// <summary>
		/// 根据运单取得货物明细列表
		/// </summary>
		/// <param name="listOrderNo"></param>
		/// <returns></returns>
		public List<Model.BaseInfo.Order.OrderModel> GetOrderList(OutBoxModel boxModel)
		{
			if (null == boxModel) throw new ArgumentNullException("OutBoxModel");
			if (boxModel.NoType == Enums.SyncNoType.Box)
			{
				return GetNormalBoxOrderList(boxModel);
			}
			return GetBatchBoxOrderList(boxModel);
		}

		public List<Model.BaseInfo.Order.OrderDetailModel> GetOrderDetailList(List<long> listWaybillNo)
		{
			if (null == listWaybillNo || listWaybillNo.Count < 1) throw new ArgumentNullException("listWaybillNo");
			string sql = String.Format(@"
SELECT W.WaybillNO AS [FormCode]
,ISNULL(WGD.GoodsCode, '  ') AS [ProductCode]
,ISNULL(WGD.GoodsName,'  ') AS [ProductName]
,ISNULL(WGD.Num,0) AS [ProductCount]
,ISNULL(WGD.Unit,'  ') AS [ProductUnit]
,ISNULL(WGD.Price,0) AS [ProductPrice]
,ISNULL(WGD.Size,'  ') AS [ProductSize]
FROM Waybill W WITH(NOLOCK)
JOIN WaybillGoodsDetails WGD WITH(NOLOCK) ON W.WaybillNo = WGD.WaybillNo
WHERE W.WaybillNo IN ({0})
"
				, String.Join<long>(",", listWaybillNo)
				);
			DataSet ds = ExecuteDataset(LMSWriteConnection, sql);
			if (ds != null
				&& ds.Tables.Count > 0)
			{
				DataTable dtResult = ds.Tables[0];
				if (dtResult.Rows.Count > 0)
				{
					List<OrderDetailModel> listOrderDetail = new List<OrderDetailModel>();
					foreach (DataRow item in dtResult.Rows)
					{
						listOrderDetail.Add(new OrderDetailModel()
						{
							FormCode = item["FormCode"].ToString(),
							ProductCode = String.IsNullOrEmpty(item["ProductCode"].ToString()) ? "  " : item["ProductCode"].ToString(),
							ProductName = String.IsNullOrEmpty(item["ProductName"].ToString()) ? "  " : item["ProductName"].ToString(),
							ProductCount = decimal.Parse(item["ProductCount"].ToString()),
							ProductUnit = String.IsNullOrEmpty(item["ProductUnit"].ToString()) ? "  " : item["ProductUnit"].ToString(),
							ProductPrice = decimal.Parse(item["ProductPrice"].ToString()),
							ProductSize = String.IsNullOrEmpty(item["ProductSize"].ToString()) ? "  " : item["ProductSize"].ToString()
						});
					}
					return listOrderDetail;
				}
			}
			return null;
		}

		/// <summary>
		/// 更新箱对象的同步状态
		/// </summary>
		/// <param name="boxModel"></param>
		/// <param name="prevSyncFlag"></param>
		public void UpdateBoxStatus(Model.Synchronous.OutBoxModel boxModel, Enums.SC2TMSSyncFlag prevSyncFlag)
		{
			string sql = String.Format(@"
UPDATE LMS_SYN_TMS_OUTBOX
SET SyncFlag = {0}, SyncTime = GETDATE()
WHERE ID =@ID AND SyncFlag = {1}
"
				, (int)boxModel.SC2TMSFlag
				, (int)prevSyncFlag);
			SqlParameter[] arguments = new SqlParameter[] { 
				new SqlParameter(){ ParameterName="@ID", DbType = DbType.String, Value = boxModel.SSTOID }
			};
			ExecuteNonQuery(LMSWriteConnection, sql, arguments);
		}


		/// <summary>
		/// 取得出库统计信息
		/// </summary>
		/// <returns></returns>
		public Model.JobMonitor.SyncStatisticInfo GetStatisticInfo()
		{
			string sql = String.Format(@"
SELECT  
 ISNULL(sum(CASE WHEN Syncflag = 0 THEN 1 ELSE 0 END),0) AS [NoSync]
,ISNULL(sum(CASE WHEN Syncflag = 1 THEN 1 ELSE 0 END),0) AS [Syncing]
,ISNULL(sum(CASE WHEN Syncflag = 2 THEN 1 ELSE 0 END),0) AS [Synced]
,getdate() AS [MonitorTime]
FROM LMS_SYN_TMS_OUTBOX WITH(NOLOCK)
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

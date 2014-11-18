using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Loading;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.Loading;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Sorting.Loading
{
	public class BillTruckDAL : BaseDAL, IBillTruckDAL
	{
		private List<int> GetOutboundTypeForLoading()
		{
			List<int> outboundTypeList = new List<int>();
			outboundTypeList.Add((int)Enums.SortCenterOperateType.DistributionSorting);
			outboundTypeList.Add((int)Enums.SortCenterOperateType.SimpleSorting);
			outboundTypeList.Add((int)Enums.SortCenterOperateType.TurnSorting);
			return outboundTypeList;
		}

		public PagedList<BillTruckBatchModel> GetBillTruckList(BillTruckSearchModel searchModel)
		{
			List<OracleParameter> parms = new List<OracleParameter>();
			var sbSQL = new StringBuilder();
			sbSQL.AppendFormat(@"
SELECT  ob.BatchNO AS BatchNo,
		MAX(ec.CompanyName) AS ArrivalName,
		MAX(eca.CompanyName) AS DepartureName,
		COUNT(ob.FormCode) AS TotalBillCount,
		COUNT(bt.FormCode) LoadingCount,
		MAX(ob.ArrivalID) AS DeliverStationID,
		MAX(bt.truckno) AS TruckNo,
		MAX(bt.Createtime) AS LoadingTime,
		MAX(wh.WarehouseName) AS Warehouse
FROM   SC_Outbound ob 
JOIN   SC_Bill bill ON  bill.FormCode = ob.FormCode AND ob.OutboundType IN ({0}) 
JOIN   ExpressCompany ec  ON  ec.ExpressCompanyID = ob.ArrivalID
JOIN   ExpressCompany eca ON  eca.ExpressCompanyID = ob.DepartureID 
LEFT JOIN   SC_BillTruck bt ON bt.batchno=ob.batchno AND bt.FormCode = ob.FormCode AND bt.isdeleted=0 
LEFT JOIN   PS_PMS.Warehouse wh ON bill.WarehouseId =wh.WarehouseId    
WHERE   1=1 and ob.createtime>SYSDATE-30 
", string.Join(",", GetOutboundTypeForLoading()));
			if (!string.IsNullOrWhiteSpace(searchModel.BillSource))
			{
				sbSQL.Append(" AND bill.Source = :Source ");
				parms.Add(new OracleParameter() { ParameterName = "Source", DbType = DbType.Int16, Value = Convert.ToInt16(searchModel.BillSource) });
			}
			if (!string.IsNullOrWhiteSpace(searchModel.BatchNO))
			{
				sbSQL.Append(" AND ob.BatchNO = :BatchNO ");
				parms.Add(new OracleParameter() { ParameterName = "BatchNO", DbType = DbType.String, Value = searchModel.BatchNO });
			}
			if (!string.IsNullOrWhiteSpace(searchModel.FromCode))
			{
				sbSQL.Append(" AND ob.FormCode = :FormCode ");
				parms.Add(new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = searchModel.FromCode });
			}
			if (!string.IsNullOrWhiteSpace(searchModel.StationId) && !searchModel.StationId.Equals("-1"))
			{
				sbSQL.Append(" AND ob.ArrivalID = :ArrivalID ");
				parms.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = Convert.ToInt32(searchModel.StationId) });
			}
			//if (!string.IsNullOrEmpty(searchModel.DepartureDistributionCode))
			//{
			//    sbSQL.Append(" and ec.DistributionCode = :DistributionCode ");
			//    parms.Add(new OracleParameter() { ParameterName = "DistributionCode", DbType = DbType.String, Value = searchModel.DepartureDistributionCode });
			//}

			if (!string.IsNullOrWhiteSpace(searchModel.ArrivalDistributionCode))
			{
				sbSQL.Append(@" AND 
								( ec.DistributionCode = :ArrivalDistributionCode 
								  AND eca.DistributionCode = :DepartureDistributionCode 
								) ");
				parms.Add(new OracleParameter() { ParameterName = "ArrivalDistributionCode", DbType = DbType.String, Value = searchModel.ArrivalDistributionCode });
				parms.Add(new OracleParameter() { ParameterName = "DepartureDistributionCode", DbType = DbType.String, Value = searchModel.DepartureDistributionCode });
			}
            else
            {
                sbSQL.Append(@" AND 
								( ec.DistributionCode = :DepartureDistributionCode  
								  OR eca.DistributionCode = :DepartureDistributionCode 
								) ");
                parms.Add(new OracleParameter() { ParameterName = "DepartureDistributionCode", DbType = DbType.String, Value = searchModel.DepartureDistributionCode });
            }

			if (!string.IsNullOrWhiteSpace(searchModel.OutBoundBeginTime))
			{
				sbSQL.Append(" AND ob.CreateTime >= :OutBoundBeginTime ");
				parms.Add(new OracleParameter() { ParameterName = "OutBoundBeginTime", DbType = DbType.Date, Value = DateTime.Parse(searchModel.OutBoundBeginTime) });
			}

			if (!string.IsNullOrWhiteSpace(searchModel.OutBoundEndTime))
			{
				sbSQL.Append(" AND ob.CreateTime <= :OutBoundEndTime ");
				parms.Add(new OracleParameter() { ParameterName = "OutBoundEndTime", DbType = DbType.Date, Value = DateTime.Parse(searchModel.OutBoundEndTime) });
			}

			if (searchModel.OnlyNotLoadingBill)
			{
				sbSQL.Append(" AND bt.BTID is null AND bill.status in (-3,10)");
			}

			sbSQL.Append(" GROUP BY ob.BatchNO ORDER BY BatchNo ASC");

			//if (searchModel.OnlyNotLoadingBill)
			//{
			//    sbSQL.Append(" WHERE TotalBillCount-LoadingCount>0 ");
			//}
            searchModel.OrderByString = "BatchNo";
			return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<BillTruckBatchModel>(TMSWriteConnection, sbSQL.ToString(), searchModel, parms.ToArray());
		}

		/// <summary>
		/// 添加订单装车
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public int addWaybillTruck(BillTruckModel model)
		{
			var strSql = new StringBuilder();
			strSql.Append("insert into SC_BillTruck(");
			strSql.Append("BTID,FormCode,BatchNO,TruckNO,GPSNO,DriverID,CreateBy,CreateTime,UpdateBy,UpdateTime,IsDeleted)");
			strSql.Append(" values (");
			strSql.Append(":BTID,:FormCode,:BatchNO,:TruckNO,:GPSNO,:DriverID,:CreateBy,:CreateTime,:UpdateBy,:UpdateTime,0)");
			OracleParameter[] parameters = new OracleParameter[]{ 
				  new OracleParameter(){ParameterName = "BTID" ,DbType = DbType.String , Value = model.BTID},
				  new OracleParameter(){ParameterName = "FormCode" ,DbType = DbType.String , Value = model.FormCode},
				  new OracleParameter(){ParameterName = "BatchNO" ,DbType = DbType.String , Value = model.BatchNO},
				  new OracleParameter(){ParameterName = "TruckNO" ,DbType = DbType.String , Value = model.TruckNO},
				  new OracleParameter(){ParameterName = "GPSNO" ,DbType = DbType.String , Value = model.GPSNO},
				  new OracleParameter(){ParameterName = "DriverID" ,DbType = DbType.Int32 , Value = model.DriverID},
				  new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = model.CreateBy},
				  new OracleParameter(){ParameterName = "CreateTime" ,DbType = DbType.Date , Value = model.CreateTime},
				  new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = model.UpdateBy},
				  new OracleParameter(){ParameterName = "UpdateTime" ,DbType = DbType.Date , Value = model.UpdateTime}
			};
			return ExecuteSqlNonQuery(TMSWriteConnection, strSql.ToString(), parameters);
		}

		/// <summary>
		/// 根据批次号获取未装车的运单号
		/// </summary>
		/// <param name="batchNO">批次号</param>
		/// <returns></returns>
		public IList<OutBoundLoadingModel> GetNotLoadingBill(string batchNO)
		{
			string sql = string.Format(
								@"SELECT   ob.FormCode,
										   ob.OutboundType,
										   ob.ArrivalID,
										   b.DeliverStationID,
										   b.status BillStatus 
									FROM   SC_Outbound ob  
									JOIN   SC_Bill  b
									  ON   b.FormCode = ob.FormCode 
							   LEFT JOIN   SC_BillTruck bt 
									  ON   bt.batchno=ob.batchno and bt.FormCode=ob.FormCode and bt.isdeleted=0 
								   WHERE   ob.BatchNO = :BatchNO 
									 AND   bt.FormCode is null 
									 AND   ob.OutboundType in ({0})", string.Join(",", GetOutboundTypeForLoading()));
			OracleParameter[] arguments = new OracleParameter[] { 
				new OracleParameter() { ParameterName = "BatchNO", DbType = DbType.String, Value = batchNO } 
			};

			return ExecuteSql_ByDataTableReflect<OutBoundLoadingModel>(TMSWriteConnection, sql, arguments);
		}

		public IList<OutBoundLoadingModel> GetNotLoadingBill(string batchNO, List<string> formCodeList)
		{
			string sql = string.Format(
							  @"SELECT   ob.FormCode,
										 ob.OutboundType,
										 ob.ArrivalID,
										 b.DeliverStationID,
										 b.status BillStatus 
								  FROM   SC_Outbound ob  
								  JOIN   SC_Bill  b
									ON   b.FormCode = ob.FormCode 
							 LEFT JOIN   SC_BillTruck bt 
									ON   bt.batchno=ob.batchno and bt.FormCode=ob.FormCode and bt.isdeleted=0 
								 WHERE   ob.BatchNO=:BatchNO 
								   AND   bt.FormCode is null
								   AND   ob.OutboundType in ({0})
								   AND   ob.FormCode in ({1})",
										 string.Join(",", GetOutboundTypeForLoading()),
										 "'" + string.Join("','", formCodeList) + "'");
			OracleParameter[] parms = new OracleParameter[] {
					new OracleParameter(){ ParameterName = "BatchNO",DbType = DbType.String , Value = batchNO}
			};
			return ExecuteSql_ByDataTableReflect<OutBoundLoadingModel>(TMSWriteConnection, sql, parms);
		}

		public bool IsExistBillTruck(BillTruckModel model)
		{
			string sql = "select BTID from SC_BillTruck  where BatchNO=:BatchNO and FormCode=:FormCode and isdeleted=0";
			OracleParameter[] parms = {
					new OracleParameter() { ParameterName = "BatchNO", DbType = DbType.String, Value = model.BatchNO } 
				   ,new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = model.FormCode }
			};

			var result = ExecuteSqlScalar(TMSWriteConnection, sql, parms);
			if (result == null)
			{
				return false;
			}
			return true;
		}


		public BillTruckModel GetBillTruckModelTmsSync2Lms(string formCode, bool isGetOff)
		{
			string strSQL = @"
SELECT *
FROM
(
	SELECT  BTID, FormCode, BatchNO, TruckNO, GPSNO, DriverID, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted  
	FROM   SC_BillTruck 
	WHERE   FormCode = :FormCode  
		AND   SyncFlag = :SyncFlag
		AND   IsDeleted = :IsDeleted 
	ORDER BY   CreateTime desc
)
WHERE ROWNUM = 1
";
			OracleParameter[] parms = new OracleParameter[]
			{
				   new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = formCode },
				   new OracleParameter() { ParameterName = "SyncFlag" ,  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.NotYet },
				   new OracleParameter() { ParameterName = "IsDeleted" , OracleDbType = OracleDbType.Int16 , Value = Convert.ToInt16(isGetOff) } //可参见OracleType，DBType，Oracle实际类型的映射关系
			};
			return ExecuteSqlSingle_ByDataTableReflect<BillTruckModel>(TMSWriteConnection, strSQL, parms);
		}

		public IList<ViewBillTruckModel> GetLoadingBill(string batchNo, List<string> formCodeList)
		{
			string sql = string.Format(
						  @"SELECT   bt.FormCode,
									 bt.BatchNo,
									 bt.TruckNO,
									 b.DeliverStationID,
									 b.status BillStatus,
									 b.CurrentDistributionCode
							 FROM    SC_BillTruck  bt
						LEFT JOIN    SC_Bill b on bt.FormCode = b.FormCode 
							WHERE    bt.IsDeleted = 0  
							  AND    batchNo=:batchNo 
							  AND    bt.FormCode in ({0})", "'" + string.Join("','", formCodeList) + "'");
			OracleParameter[] parms = new OracleParameter[] {
					new OracleParameter(){ ParameterName="batchNo", DbType=DbType.String, Value =batchNo }
			};
			return ExecuteSql_ByDataTableReflect<ViewBillTruckModel>(TMSWriteConnection, sql, parms);
		}

		/// <summary>
		/// 订单下车
		/// </summary>
		/// <param name="model"></param>
		public int RemoveBillTruck(BillTruckModel model)
		{
			string strSQL = @"UPDATE SC_BillTruck
											  SET   IsDeleted = 1,
													UpdateBy = :UpdateBy,
													UpdateTime = :UpdateTime,
													SyncFlag = :SyncFlag
											WHERE   FormCode = :FormCode 
											  AND   BatchNo = :BatchNo ";
			OracleParameter[] parms = new OracleParameter[] {
					new OracleParameter(){ ParameterName="UpdateBy", DbType = DbType.Int32, Value = model.UpdateBy },
					new OracleParameter(){ ParameterName="UpdateTime", DbType=DbType.Date, Value = model.UpdateTime },
					new OracleParameter(){ ParameterName="FormCode", DbType = DbType.String, Value = model.FormCode },
					new OracleParameter(){ ParameterName="BatchNo", DbType = DbType.String, Value = model.BatchNO },
					new OracleParameter(){ ParameterName="SyncFlag", DbType = DbType.Int16, Value = (int)Enums.SyncStatus.NotYet }
			};
			return ExecuteSqlNonQuery(TMSWriteConnection, strSQL, parms);
		}

		public IList<ViewBillTruckModel> GetOutbondByBatch(BillTruckSearchModel searchModel)
		{
			string strSQL = @"
							SELECT DISTINCT   ob.FormCode as FormCode,
											  wh.WarehouseName as WarehouseName,
											  ec.CompanyName as DeliverStationName,
											  bt.TruckNo,
											  (select StatusName from StatusInfo s where s.statusTypeNO=2 and StatusNO=wb.billType ) as BillTypeName,
											  case wb.Source WHEN 0 THEN 'Vancl订单'
											  WHEN 1 THEN 'V+订单'
											  WHEN 2 THEN '其他订单'
											  END AS SourceName
									   FROM   SC_Outbound ob 
									   JOIN   SC_Bill wb on  ob.formcode=wb.formcode 
								  LEFT JOIN   SC_BillTruck bt 
										 ON   bt.batchno=ob.batchno and bt.FormCode=ob.FormCode and bt.isdeleted=0 
								  LEFT JOIN   Warehouse wh on wh.WarehouseId=wb.WarehouseId
								  LEFT JOIN   ExpressCompany ec on ec.ExpressCompanyID=ob.arrivalid 
									  WHERE   ob.BatchNO=:BatchNO  ";
			OracleParameter[] parms = new OracleParameter[] {
					new OracleParameter(){ ParameterName="BatchNo", DbType = DbType.String, Value = searchModel.BatchNO }
			};
			if (!searchModel.OnlyNotLoadingBill)
			{
				strSQL += " and bt.FormCode is null ";
			}
			return ExecuteSql_ByDataTableReflect<ViewBillTruckModel>(TMSWriteConnection, strSQL, parms);
		}

		public int UpdateSyncedStatus4Tms2Lms(string BillTruckKey)
		{
			if (String.IsNullOrWhiteSpace(BillTruckKey)) throw new ArgumentNullException("BillTruckKey is null or empty.");
			String sql = @"
							UPDATE SC_BillTruck
							SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
							WHERE BTID = :BTID
						  ";
			OracleParameter[] arguments = new OracleParameter[] 
			{
				new OracleParameter() { ParameterName = "SyncFlag" , DbType = DbType.Int16, Value = (int)Enums.SyncStatus.Already },
				new OracleParameter() { ParameterName = "BTID" , DbType = DbType.String, Value = BillTruckKey }
			};
			return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
		}


	}
}

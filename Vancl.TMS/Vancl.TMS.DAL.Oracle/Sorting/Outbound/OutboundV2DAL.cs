using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IDAL.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound.SMS;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.DAL.Oracle.Sorting.Outbound
{
	/// <summary>
	/// 出库数据层实现
	/// </summary>
	public class OutboundV2DAL : BaseDAL, IOutboundV2DAL
	{
		#region IOutboundDAL 成员

		/// <summary>
		/// 取得需要出库的运单列表信息
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public List<ViewOutboundSearchListModel.InnerFormCodeList> GetNeededOutboundFormCodeList(OutboundSearchArgModel argument)
		{
			if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null.");
			if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
			if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
			if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
			String inboundtypesql = null;
			if (argument.ToStation.SortingType == Enums.SortCenterOperateType.SimpleSorting)
			{
				inboundtypesql = String.Format(" AND ib.InboundType IN ( {0} , {1} ) ", (int)Enums.SortCenterOperateType.SimpleSorting, (int)Enums.SortCenterOperateType.TurnSorting);
			}
			else
			{
				inboundtypesql = " AND ib.InboundType = :InboundType ";
			}
			String sql = String.Format(@"
SELECT bill.FormCode, bill.CustomerOrder
FROM SC_Bill bill
    JOIN SC_Inbound ib  ON bill.InboundKey = ib.IBID  AND bill.FormCode = ib.FormCode
WHERE ib.CreateTime BETWEEN  :StartTime AND :EndTime
    AND bill.Status = :Status
    AND bill.IsDeleted = 0
    AND ib.DepartureID = :DepartureID
    AND ib.ArrivalID = :ArrivalID
    {0}
    AND NOT EXISTS      --去掉分拣装箱的订单
    (
        SELECT 1
        FROM SC_InboundPacking box
	        JOIN SC_InboundPackingDetail  boxdetail ON box.BoxNo = boxdetail.BoxNo
        WHERE bill.FormCode = boxdetail.FormCode AND box.DepartureID = :DepartureID 
        AND box.IsDeleted = 0
    )
",
 inboundtypesql
 );
			OracleParameter[] cmmparameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "StartTime", OracleDbType = OracleDbType.Date , Value = argument.InboundStartTime.Value  },
                new OracleParameter() { ParameterName= "EndTime", OracleDbType = OracleDbType.Date , Value = argument.InboundEndTime.Value },
                new OracleParameter() { ParameterName= "Status", OracleDbType = OracleDbType.Int16 , Value = (int)argument.PreCondition.PreStatus },
                new OracleParameter() { ParameterName= "DepartureID", OracleDbType = OracleDbType.Int32 , Value = argument.OpUser.ExpressId.Value },
                new OracleParameter() { ParameterName= "ArrivalID", OracleDbType = OracleDbType.Int32 , Value = argument.ToStation.ExpressCompanyID }
            };
			List<OracleParameter> parameters = new List<OracleParameter>();
			parameters.AddRange(cmmparameters);
			if (argument.ToStation.SortingType != Enums.SortCenterOperateType.SimpleSorting)
			{
				parameters.Add(new OracleParameter() { ParameterName = "InboundType", OracleDbType = OracleDbType.Int16, Value = (int)(argument.ToStation.SortingType) });
			}
			var list = ExecuteSql_ByReaderReflect<ViewOutboundSearchListModel.InnerFormCodeList>(TMSWriteConnection, sql, parameters.ToArray());
			if (list != null)
			{
				return list.ToList();
			}
			return null;
		}

		/// <summary>
		/// 取得处于入库中的运单数量
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public int GetInboundingCount(OutboundSearchArgModel argument)
		{
			if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null.");
			if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
			if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
			if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
			String sql = @"
SELECT COUNT(1) 
FROM SC_InboundQueue siq
    JOIN  SC_Bill bill  ON bill.FormCode = siq.FormCode
WHERE siq.CreateTime BETWEEN :StartTime AND :EndTime
    AND siq.DepartureID = :DepartureID
    AND siq.ArrivalID = :ArrivalID
    AND siq.SeqStatus = :SeqStatus
    AND siq.IsDeleted = 0
    AND bill.IsDeleted = 0
";
			OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "StartTime", DbType = System.Data.DbType.Date , Value = argument.InboundStartTime.Value },
                new OracleParameter() { ParameterName= "EndTime", DbType = System.Data.DbType.Date , Value = argument.InboundEndTime.Value  },
                new OracleParameter() { ParameterName= "DepartureID", OracleDbType = OracleDbType.Int32, Value = argument.OpUser.ExpressId.Value },
                new OracleParameter() { ParameterName= "ArrivalID", OracleDbType = OracleDbType.Int32, Value = argument.ToStation.ExpressCompanyID },
                new OracleParameter() { ParameterName= "SeqStatus", OracleDbType = OracleDbType.Int16, Value = (int)Enums.SeqStatus.NoHand }
            };
			object objValue = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
			if (objValue != null && objValue != DBNull.Value)
			{
				return Convert.ToInt32(objValue);
			}
			return 0;
		}

		public int Add(OutboundEntityModel outboundModel)
		{
			if (outboundModel == null) throw new ArgumentNullException("outboundModel is null.");
			String sql = @"
INSERT INTO SC_Outbound ( OBID , FormCode, BatchNo, DepartureID, ArrivalID, OutboundType, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted, SyncFlag )
VALUES ( :OBID, :FormCode, :BatchNo, :DepartureID, :ArrivalID, :OutboundType, :CreateBy, SYSDATE, :UpdateBy, SYSDATE, 0, :SyncFlag )
";
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="OBID",  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = outboundModel.OBID},
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = outboundModel.FormCode},
                new OracleParameter() { ParameterName="BatchNo",  OracleDbType = OracleDbType.Varchar2 , Size = 20 ,  Value = outboundModel.BatchNo},
                new OracleParameter() { ParameterName="DepartureID",  OracleDbType = OracleDbType.Int32 , Value = outboundModel.DepartureID },
                new OracleParameter() { ParameterName="ArrivalID",  OracleDbType = OracleDbType.Int32 , Value = outboundModel.ArrivalID },
                new OracleParameter() { ParameterName="OutboundType",  OracleDbType = OracleDbType.Int16 , Value = (int)outboundModel.OutboundType },
                new OracleParameter() { ParameterName="CreateBy",  OracleDbType = OracleDbType.Int32 , Value = outboundModel.CreateBy },
                new OracleParameter() { ParameterName="UpdateBy",  OracleDbType = OracleDbType.Int32 , Value = outboundModel.UpdateBy },
                new OracleParameter() { ParameterName="SyncFlag",  OracleDbType = OracleDbType.Int16 , Value = (int)outboundModel.SyncFlag }
            };
			return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
		}

		/// <summary>
		/// 批量新增出库记录
		/// </summary>
		/// <param name="listOutboundModel">出库对象列表</param>
		public void BatchAdd(List<OutboundEntityModel> listOutboundModel)
		{
			if (listOutboundModel == null) throw new ArgumentNullException("listOutboundModel is null.");
			String sql = @"
INSERT INTO SC_Outbound ( OBID , FormCode, BatchNo, DepartureID, ArrivalID, OutboundType, CreateBy, CreateTime, UpdateBy, UpdateTime, IsDeleted, SyncFlag )
VALUES ( :OBID, :FormCode, :BatchNo, :DepartureID, :ArrivalID, :OutboundType, :CreateBy, SYSDATE, :UpdateBy, SYSDATE, 0, :SyncFlag )
";
			int npos = 0;
			String[] arrOBID = new String[listOutboundModel.Count];
			String[] arrFormCode = new String[listOutboundModel.Count];
			String[] arrBatchNo = new String[listOutboundModel.Count];
			int[] arrDepartureID = new int[listOutboundModel.Count];
			int[] arrArrivalID = new int[listOutboundModel.Count];
			int[] arrOutboundType = new int[listOutboundModel.Count];
			int[] arrCreateBy = new int[listOutboundModel.Count];
			int[] arrUpdateBy = new int[listOutboundModel.Count];
			int[] arrSyncFlag = new int[listOutboundModel.Count];
			listOutboundModel.ForEach(p =>
			{
				arrOBID[npos] = p.OBID;
				arrFormCode[npos] = p.FormCode;
				arrBatchNo[npos] = p.BatchNo;
				arrDepartureID[npos] = p.DepartureID;
				arrArrivalID[npos] = p.ArrivalID;
				arrOutboundType[npos] = (int)p.OutboundType;
				arrCreateBy[npos] = p.CreateBy;
				arrUpdateBy[npos] = p.UpdateBy;
				arrSyncFlag[npos] = (int)p.SyncFlag;
				npos++;
			});
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="OBID", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = arrOBID},
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = arrFormCode},
                new OracleParameter() { ParameterName="BatchNo", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = arrBatchNo},
                new OracleParameter() { ParameterName="DepartureID",  OracleDbType = OracleDbType.Int32 , Value = arrDepartureID },
                new OracleParameter() { ParameterName="ArrivalID",  OracleDbType = OracleDbType.Int32 , Value = arrArrivalID },
                new OracleParameter() { ParameterName="OutboundType", OracleDbType = OracleDbType.Int16 , Value = arrOutboundType },
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32 , Value = arrCreateBy },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32 , Value = arrUpdateBy },
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16 , Value = arrSyncFlag }
            };
			ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listOutboundModel.Count, arguments);
		}

		#endregion

		#region IOutboundDAL 成员

		/// <summary>
		/// 取得TMS出库记录需要同步到LMS物流主库的出库实体对象
		/// </summary>
		/// <param name="FormCode"></param>
		/// <returns></returns>
		public OutboundEntityModel GetOutboundEntityModel4TmsSync2Lms(string FormCode)
		{
			if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
			String sql = @"
SELECT *
FROM
(
    SELECT OBID, FormCode, BatchNo, DepartureID, ArrivalID, OutboundType, CreateBy, CreateTime, UpdateBy, UpdateTime
    FROM SC_Outbound
    WHERE FormCode = :FormCode
    AND SyncFlag = :SyncFlag
    ORDER BY CreateTime ASC
)
WHERE ROWNUM = 1
";
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
			return ExecuteSqlSingle_ByDataTableReflect<OutboundEntityModel>(TMSWriteConnection, sql, arguments);
		}

		#endregion

		#region IOutboundDAL 成员


		public int UpdateSyncedStatus4Tms2Lms(string outboundKey)
		{
			if (String.IsNullOrWhiteSpace(outboundKey)) throw new ArgumentNullException("outboundKey is null or empty.");
			String sql = @"
UPDATE SC_Outbound
SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
WHERE OBID = :OBID
";
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "OBID" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = outboundKey }
            };
			return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
		}

		#endregion

		public IList<OutboundOrderCountModel> GetOrderCount(IList<string> batchNoList)
		{
			if (batchNoList == null || batchNoList.Count == 0) throw new ArgumentNullException("batchNoList");
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"

SELECT  Max(ob.BatchNO) BatchNO,
        Max(ec.CompanyName) CompanyName,
        SUM(CASE WHEN TurnstationKey IS NOT NULL
                      AND OutboundType <> 2 THEN 1
                 ELSE 0
            END) turnCount ,
        SUM(CASE WHEN TurnstationKey IS NOT NULL
                      AND OutboundType <> 2
                      AND wb.Source = 0 THEN 1
                 ELSE 0
            END) vanclTurnCount ,
        SUM(CASE WHEN TurnstationKey IS NOT NULL
                      AND OutboundType <> 2
                      AND wb.Source = 1 THEN 1
                 ELSE 0
            END) vjiaTurnCount ,
        SUM(CASE WHEN TurnstationKey IS NOT NULL
                      AND OutboundType <> 2
                      AND wb.Source = 2 THEN 1
                 ELSE 0
            END) otherTurnCount ,   
        SUM(CASE WHEN OutboundType = 2 THEN 1
                 ELSE 0
            END) secondSortingCount ,
        SUM(CASE WHEN OutboundType = 2
                      AND wb.Source = 0 THEN 1
                 ELSE 0
            END) vanclSecondSortingCount ,
        SUM(CASE WHEN OutboundType = 2
                      AND wb.Source = 1 THEN 1
                 ELSE 0
            END) vjiaSecondSortingCount ,
        SUM(CASE WHEN OutboundType = 2
                      AND wb.Source = 2 THEN 1
                 ELSE 0
            END) otherSecondSortingCount ,    
        COUNT(1) AS allCount ,
        SUM(CASE WHEN wb.Source = 0 THEN 1
                 ELSE 0
            END) AS vanclAllCount ,
        SUM(CASE WHEN wb.Source = 1 THEN 1
                 ELSE 0
            END) AS vjiaAllCount,
        SUM(CASE WHEN wb.Source = 2 THEN 1
             ELSE 0
			END) AS otherAllCount,
      sum(bi.InsuredAmount) as InsuredAmount
FROM    sc_OutBound ob 
        inner join SC_OutboundBatch b on b.BatchNO = ob.BatchNO
        INNER JOIN sc_bill wb  ON ob.FormCode = wb.FormCode
        inner join sc_billinfo bi on bi.FormCode = ob.FormCode
        inner join PS_PMS.ExpressCompany ec  on ec.ExpressCompanyID=b.ArrivalID
WHERE 
");

			string batchNos = string.Join(",", batchNoList.Select(x => x));
			sbSql.AppendFormat(GetOracleInParameterWhereSql("ob.BatchNO", "BatchNO", false, false, false));
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2, 4000) { Value = batchNos });
			sbSql.AppendLine(@"group by ob.BatchNO");
			sbSql.AppendLine(@"order by BatchNO");
			return ExecuteSql_ByReaderReflect<OutboundOrderCountModel>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}


		public IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
SELECT rownum as No,
       ob.FormCode as FormCode,
       wh.WarehouseName as WarehouseName,
       ec.CompanyName DeliveryStation,
       si.StatusName as OrderType, 
       ob.CreateTime as OutboundTime,
       bi.ReceivableAmount as ReceivableAmount
FROM sc_OutBound ob
    JOIN sc_bill wb ON ob.FormCode=wb.FormCode AND wb.IsDeleted=0
    JOIN PS_PMS.ExpressCompany ec on ec.ExpressCompanyID=wb.DeliverStationID
    JOIN PS_PMS.StatusInfo si  on wb.billtype=To_number(si.StatusNO) and si.StatusTypeNO=2
    JOIN SC_BillInfo bi on bi.FormCode=wb.FormCode
    LEFT JOIN PS_PMS.Warehouse wh on wb.WarehouseId=wh.WarehouseId
WHERE 
    ob.BatchNO=:BatchNO AND 
    ob.IsDeleted=0
                
                
");
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2) { Value = batchNo });
			return ExecuteSql_ByReaderReflect<PrintBatchDetailModel>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}

		#region IOutboundDAL 成员

		public DepartureArrivalInfo GetDepartureArrivalInfo(int DepartureID, int ArrivalID)
		{
			String sql = @"
 SELECT   CityName as  CityName,
            MnemonicName as DeptName
  FROM  ExpressCompany ec
            JOIN City ct ON ec.CityID = ct.CityID
  WHERE ec.ExpressCompanyID = :DepartureID
  UNION ALL
  SELECT   CityName as CityName,
            MnemonicName as DeptName
  FROM  ExpressCompany ec
            JOIN City ct ON ec.CityID = ct.CityID
  WHERE ec.ExpressCompanyID = :ArrivalID
";
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "DepartureID", OracleDbType = OracleDbType.Int32, Value = DepartureID },
                new OracleParameter() { ParameterName= "ArrivalID", OracleDbType = OracleDbType.Int32, Value = ArrivalID }
            };
			var dtResult = ExecuteSqlDataTable(TMSReadOnlyConnection, sql, arguments);
			if (dtResult != null && dtResult.Rows.Count == 2)
			{
				DepartureArrivalInfo info = new DepartureArrivalInfo();
				if (dtResult.Rows[0]["CityName"] != null && dtResult.Rows[0]["CityName"] != DBNull.Value)
				{
					info.DepartureCityName = dtResult.Rows[0]["CityName"].ToString();
				}
				if (dtResult.Rows[0]["DeptName"] != null && dtResult.Rows[0]["DeptName"] != DBNull.Value)
				{
					info.DepartureDeptName = dtResult.Rows[0]["DeptName"].ToString();
				}
				if (dtResult.Rows[1]["CityName"] != null && dtResult.Rows[1]["CityName"] != DBNull.Value)
				{
					info.ArrivalCityName = dtResult.Rows[1]["CityName"].ToString();
				}
				if (dtResult.Rows[1]["DeptName"] != null && dtResult.Rows[1]["DeptName"] != DBNull.Value)
				{
					info.ArrivalDeptName = dtResult.Rows[1]["DeptName"].ToString();
				}
				return info;
			}
			return null;
		}

		public IList<OutboundPrintExportModel> GetOutboundPrintExportModel(IList<string> batchNoList)
		{
			if (batchNoList == null || batchNoList.Count == 0) throw new ArgumentNullException("batchNoList");
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
select ROW_NUMBER()over(order by sysdate) as No ,
                    ob.batchno as batchno,
                    ob.FormCode as FormCode,
                --    wtsi.ReceiveMobile as ReceiveMobile,
                    wh.WarehouseName as WarehouseName,
                    ec.CompanyName CompanyName,
                    --如果是其它显示商家名称
	                case si.StatusName when '其他' then mer.MerchantName else si.StatusName end as Source, 
                    ob.CreateTime as CreateTime
                From SC_Outbound ob  
                    inner join sc_bill wb  on ob.FormCode=wb.FormCode
                 --   INNER JOIN WaybillTakeSendInfo wtsi  ON wb.WaybillNO=wtsi.WaybillNO
                    left join PS_PMS.Warehouse wh  on wb.WarehouseId=wh.WarehouseId
                    inner join PS_PMS.ExpressCompany ec  on ec.ExpressCompanyID=wb.DeliverStationID
                    inner join PS_PMS.MerchantBaseInfo mer  on mer.id = wb.MerchantID
                    inner join PS_PMS.StatusInfo si  on wb.Source=si.StatusNO
                where 
                si.StatusTypeNO=3
");
			string batchNos = string.Join(",", batchNoList.Select(x => x));
			sbSql.AppendFormat(GetOracleInParameterWhereSql("ob.BatchNO", "BatchNO", false, true, false));
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2, 4000) { Value = batchNos });
			return ExecuteSql_ByReaderReflect<OutboundPrintExportModel>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());

		}

		public IList<OutboundPrintModel> SearchOutboundPrint(int toStation, DateTime beginTime, DateTime endTime, int expressID, string batchNo, string waybillNo, Enums.CompanyFlag companyFlag)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
with t1 as(
    SELECT 
            max(b.ArrivalID) as ArrivalID
            ,b.BatchNO         
            ,COUNT(1) as PackageNum 
         -- ,b.IsEmailSent IsSendEmail
         -- ,SUM(wsi.protectedprice) AS ProtectedpriceSum 
            ,max(b.CreateTime) AS OutboundTime
            ,SUM(bi.InsuredAmount) as InsuredAmount
      FROM SC_OutboundBatch b
      JOIN SC_Outbound ob    ON b.BatchNO = ob.BatchNO      
      JOIN SC_BillInfo bi ON ob.FormCode=bi.FormCode
      WHERE 1=1   

");
			if (!string.IsNullOrEmpty(batchNo))
			{
				sbSql.Append(" AND b.BatchNO=:BatchNO");
				parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2) { Value = batchNo });
			}
			else if (!string.IsNullOrEmpty(waybillNo))
			{
				sbSql.Append(" AND ob.FormCode=:FormCode");
				parameters.Add(new OracleParameter("FormCode", OracleDbType.Long) { Value = waybillNo });
			}
			else
			{
				if (beginTime > DateTime.MinValue)
				{
					sbSql.Append(" AND b.CreateTime > :BeginTime");
					parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = beginTime });
				}
				if (beginTime < DateTime.MaxValue)
				{
					sbSql.Append(" AND b.CreateTime < :EndTime");
					parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = endTime });
				}
				if (toStation > 0)
				{
					sbSql.Append(" AND b.ArrivalID=:ArrivalID");
					parameters.Add(new OracleParameter("ArrivalID", OracleDbType.Int32) { Value = toStation });
				}
			}
			//companyFlag: 部门类型（0行政部门、1分拣中心、2站点、3加盟商）
			if (companyFlag == Enums.CompanyFlag.SortingCenter)
			{
				sbSql.Append(" AND b.DepartureID=:DepartureID");
			}
			else
			{
				sbSql.Append(" AND ob.DepartureID=:DepartureID");
			}
			parameters.Add(new OracleParameter("DepartureID", OracleDbType.Int32) { Value = expressID });
			sbSql.Append(@"
 GROUP BY b.BatchNO
)
select t1.*,ec.ExpressCompanyID,ec.CompanyName,ec.email,ibb.Weight
from t1
JOIN PS_PMS.ExpressCompany ec ON t1.ArrivalID = ec.ExpressCompanyID
LEFT JOIN SC_InboundPacking ibb on t1.BatchNO = ibb.BoxNo
ORDER BY BatchNO
");
			return ExecuteSql_ByReaderReflect<OutboundPrintModel>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());

		}

		/// <summary>
		/// 查询返货目的地
		/// </summary>
		/// <returns></returns>
		public string GetReturnTo(string formCode, int arrivalID)
		{
			StringBuilder sbSql = new StringBuilder();
			sbSql.Append(@"
SELECT COMPANYNAME FROM SC_Outbound ob
LEFT JOIN EXPRESSCOMPANY ec on ob.departureid = ec.expresscompanyid
where ob.formcode=:formcode AND ArrivalID=:ArrivalId
           ");
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "formcode", DbType = DbType.String, Value = formCode },
                new OracleParameter() { ParameterName= "ArrivalId", DbType = DbType.Int32, Value = arrivalID }
            };
			object objStatus = ExecuteSqlScalar(TMSReadOnlyConnection, sbSql.ToString(), arguments);
			if (objStatus != null && objStatus != DBNull.Value)
			{
				return Convert.ToString(objStatus);
			}
			return string.Empty;
		}
		#endregion


		public IList<OutboundEntityModel> GetOutboundEntityByBatchNoList(IList<string> batchNoList)
		{
			if (batchNoList == null || batchNoList.Count == 0) throw new ArgumentNullException("batchNoList");

			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.AppendFormat(@"
SELECT OBID, FormCode, BatchNo, DepartureID, ArrivalID, OutboundType, CreateBy, CreateTime, UpdateBy, UpdateTime
FROM SC_Outbound ob
WHERE 
    {0}    
", GetOracleInParameterWhereSql("ob.BatchNO", "BatchNO", false, false, false));
			string batchNos = string.Join(",", batchNoList.Select(x => x));
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2, 4000) { Value = batchNos });
			return ExecuteSql_ByDataTableReflect<OutboundEntityModel>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}

		public IList<BatchBillInfoForOutBound> GetBatchBillInfoForOutBoundSendMail(IList<string> formCodeList, Enums.SortCenterOperateType outboundType)
		{
			if (formCodeList == null || formCodeList.Count == 0) throw new ArgumentNullException("formCodeList");

			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.AppendFormat(@"
SELECT 
    o.BatchNO,wb.formcode,
    --wbtsi.ReceiveMobile,
    wb.BillType,
    --wbtsi.ReceiveBy,
    --wbtsi.ReceiveAddress,
    CASE WHEN wb.Source=0 THEN 'VANCL' WHEN wb.Source=1 THEN 'Vjia' ELSE mbi.MerchantName END AS Source
    --,ReceivePost
    --,SendTimeType
    --,wbtsi.ReceiveComment
    ,(SELECT CreateTime FROM sc_OutBound where OutboundKey=wb.OutboundKey and rownum=1) OutBoundTime
    --,wbsi.AcceptType,wbsi.Amount,wbsi.PaidAmount,wbsi.NeedAmount,wbsi.NeedBackAmount,wbsi.protectedprice
    ,wbi.Weight,wbi.CustomerBoxNo
    --,wbtsi.ReceiveProvince,wbtsi.ReceiveCity,wbtsi.ReceiveArea
    ,ex.CompanyName 
FROM sc_bill wb
JOIN PS_PMS.ExpressCompany ex ON wb.DeliverStationID = ex.ExpressCompanyID
JOIN sc_billinfo wbi ON wb.formcode=wbi.formcode
LEFT JOIN PS_PMS.MerchantBaseInfo mbi ON wb.MerchantID=mbi.ID
LEFT JOIN sc_outbound o on wb.formcode=o.formcode --and {0} 
WHERE 1=1  {1}
 order by  o.BatchNO,wb.billtype,wb.formcode
",
 outboundType == Enums.SortCenterOperateType.SecondSorting ? " o.OutboundType=2 " : " wb.DeliverStationID=o.ArrivalID ",
GetOracleInParameterWhereSql("wb.formcode", "formcode", false, true, false));
			;
			string formCodes = string.Join(",", formCodeList.Select(x => x));
			parameters.Add(new OracleParameter("formcode", OracleDbType.Varchar2, 4000) { Value = formCodes });
			return ExecuteSql_ByDataTableReflect<BatchBillInfoForOutBound>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}

		#region IOutboundDAL 成员


		public List<ViewOutBoundByBoxModel> GetNeededOutBoundBoxList(OutboundSearchModel argument)
		{
			if (argument == null) throw new ArgumentNullException("Argument Is Null");
			StringBuilder sb = new StringBuilder(@"SELECT BoxNo,Weight,billCount,CreateTime as PackTime
                            FROM SC_INBOUNDPACKING  
                            WHERE ISOUTBOUNDED=0 AND ISDELETED=0 AND CreateTime>=:BeginTime AND CreateTime<=:EndTime AND ArrivalID=:ArrivalID AND departureid=:departureid ");
			List<OracleParameter> param = new List<OracleParameter>();
			param.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = argument.InboundStartTime.Value });
			param.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = argument.InboundEndTime.Value });
			param.Add(new OracleParameter("ArrivalID", OracleDbType.Int32) { Value = argument.ArrivalID });
			param.Add(new OracleParameter("departureid", OracleDbType.Int32) { Value = argument.OpUser.ExpressId.Value });
			if (!String.IsNullOrWhiteSpace(argument.BoxNo))
			{
				sb.Append(" AND BoxNo=:BoxNo ");
				param.Add(new OracleParameter("BoxNo", OracleDbType.Varchar2, 50) { Value = argument.BoxNo });
			}

			var list = ExecuteSql_ByReaderReflect<ViewOutBoundByBoxModel>(TMSWriteConnection, sb.ToString(), param.ToArray());
			if (list != null)
			{
				return list.ToList();
			}
			return null;

		}

		public ViewOutBoundBoxDetailModel GetNeededOutBoundBillListByBoxNo(string boxNo)
		{
			string sql = "SELECT FORMCODE FROM sc_inboundpackingdetail WHERE BOXNO=:BOXNO AND ISDELETED=0";
			OracleParameter[] parameter = new OracleParameter[] { new OracleParameter("BOXNO", OracleDbType.Varchar2, 50) { Value = boxNo } };
			ViewOutBoundBoxDetailModel detail = null;
			DataTable dt = base.ExecuteSqlDataTable(TMSWriteConnection, sql, parameter);
			if (dt != null && dt.Rows.Count > 0)
			{
				detail = new ViewOutBoundBoxDetailModel();
				detail.FormCodes = new List<string>();
				foreach (DataRow item in dt.Rows)
				{
					detail.FormCodes.Add(item["FORMCODE"].ToString());
				}
			}

			return detail;

		}

		#endregion

		#region IOutboundDAL 成员


		public ViewOutBoundByBoxModel GetBoxInfoByBoxNo(string boxNo)
		{
			string sql = "SELECT BOXNO,WEIGHT,BILLCOUNT FROM SC_INBOUNDPACKING WHERE BOXNO=:BOXNO AND ISDELETED=0 AND ISOUTBOUNDED=0";
			OracleParameter[] parameter = new OracleParameter[] { new OracleParameter("BOXNO", OracleDbType.Varchar2, 50) { Value = boxNo } };
			return base.ExecuteSqlSingle_ByReaderReflect<ViewOutBoundByBoxModel>(TMSWriteConnection, sql, parameter);
		}

		#endregion

		#region IOutboundDAL 成员


		public int SetBoxOutBoundStatus(string boxNo, bool isOutBounded)
		{
			string sql = "UPDATE SC_INBOUNDPACKING SET ISOUTBOUNDED=:ISOUTBOUNDED,UPDATETIME=SYSDATE WHERE BOXNO=:BOXNO AND ISDELETED=0";
			OracleParameter[] parameter = new OracleParameter[] { 
                new OracleParameter("BOXNO", OracleDbType.Varchar2, 50) { Value = boxNo } ,
                new OracleParameter("ISOUTBOUNDED", OracleDbType.Int16) { Value = isOutBounded ? 1 : 0 } 
            };
			return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameter);
		}

		#endregion

		#region  V2新增

		public int GetCurOutBoundCount(int departureId)
		{
			StringBuilder strSql = new StringBuilder();
//            strSql.Append(@"select sum(outboundcount) as 出库总量 
//							from sc_outboundbatch t
//							where t.Departureid = :Departureid
//								and t.createtime >= (select trunc(sysdate) from dual)
//								and t.createtime < (select trunc(sysdate)+1 from dual)");

			strSql.Append(@"select count(1) as 出库总量
							from sc_outbound t
							where t.departureid = :Departureid
							and t.createtime >= (select trunc(sysdate) from dual)
							and t.createtime < (select trunc(sysdate)+1 from dual)
							and t.isdeleted = 0");

			OracleParameter[] parameter = new OracleParameter[] { 
                new OracleParameter("Departureid", OracleDbType.Int32, 10) { Value = departureId }
            };

			var dt = ExecuteSqlDataTable(TMSWriteConnection, strSql.ToString(), parameter);
			if (dt.Rows[0]["出库总量"].ToString() == "")
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(dt.Rows[0]["出库总量"]);
			}
		}

		public int GetCurDisOutBoundCount(int departureId, int arrivalid)
		{
			StringBuilder strSql = new StringBuilder();

			strSql.Append(@"select count(1) as 当前目的地出库数量
							from sc_outbound t
							where t.departureid = :Departureid
							and t.arrivalid = :Arrivalid
							and t.createtime >= (select trunc(sysdate) from dual)
							and t.createtime < (select trunc(sysdate)+1 from dual) 
							and t.isdeleted = 0 ");

			OracleParameter[] parameter = new OracleParameter[] { 
                new OracleParameter("Departureid", OracleDbType.Int32, 10) { Value = departureId },
				new OracleParameter("Arrivalid", OracleDbType.Int32, 10) {Value = arrivalid}
            };

			var dt = ExecuteSqlDataTable(TMSWriteConnection, strSql.ToString(), parameter);
			if (dt.Rows[0]["当前目的地出库数量"].ToString() == "")
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(dt.Rows[0]["当前目的地出库数量"]);
			}
		}

		public ViewGetCountAndBatchNo GetCountAndBatchNo(int departureId, int arrivalid)
		{
			var Result = new ViewGetCountAndBatchNo();
			StringBuilder strSql = new StringBuilder();
			strSql.Append(@"select outboundcount, BatchNo from sc_outboundbatch t
							where t.departureid = :Departureid
								and t.arrivalid = :Arrivalid
							   and t.isdeleted = 0");
			OracleParameter[] parameter = new OracleParameter[]
				{
					new OracleParameter("Departureid", OracleDbType.Int32, 10) {Value = departureId},
					new OracleParameter("Arrivalid", OracleDbType.Int32, 10) {Value = arrivalid}
				};

			var dt = ExecuteSqlDataTable(TMSWriteConnection, strSql.ToString(), parameter);
			if (dt == null || dt.Rows.Count == 0)
			{
				Result.CurArrivalBatchNo = "Test";
				Result.CurArrivalCount = 10;
			}
			else
			{
				Result.CurArrivalBatchNo = Convert.ToString(dt.Rows[0]["BatchNo"]);
				if (dt.Rows[0]["outboundcount"].ToString() == "")
				{
					Result.CurArrivalCount = 0;
				}
				else
				{
					Result.CurArrivalCount = Convert.ToInt32(dt.Rows[0]["outboundcount"]);
				}
			}
			Result.IsSuccess = true;
			return Result;
		}

		public OutboundBoxModel GetOutboundBoxModel(string boxNo)
		{
			var Result = new OutboundBoxModel();
			String sql = @"
				select ibpack.boxno,
				   ibpack.billcount,
				   ibpack.inboundtype,
				   ibpack.departureid,
				   ibpack.arrivalid,
				   ibpack.isoutbounded
			  from sc_inboundpacking ibpack
			 where ibpack.isdeleted = 0
			   AND ibpack.boxno = :BoxNo
				";
			OracleParameter[] parameter = new OracleParameter[]
				{
					new OracleParameter("BoxNo", OracleDbType.Varchar2, 50) {Value = boxNo}
				};
			return ExecuteSqlSingle_ByDataTableReflect<OutboundBoxModel>(TMSWriteConnection, sql, parameter);
		}

		#region   出库交接表
		public PagedList<OutboundPrintModelV2> SearchOutboundPrintV2(OutboundPrintSearchModel searchModel)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
					with t1 as(
						select max(ob.CreateTime) AS OutboundTime,
							max(obb.createtime) as BatchPrintTime,
							max(ob.ArrivalID) as OutboundArrivedId,
							max(ob.batchno) as BatchNo,
							count(distinct ipd.boxno) as BoxsCount,
							count(1) as FormCodesCount,
							sum(bi.weight) as Weight							            
					   FROM SC_Outbound ob
					   JOIN SC_BillInfo bi
						 ON ob.FormCode = bi.FormCode
					   Left Join SC_OUTBOUNDBATCH obb
						 on ob.batchno = obb.batchno
					   LEFT join SC_INBOUNDPACKINGDETAIL ipd
						 on ipd.formcode = bi.formcode
					  WHERE 1 = 1 AND (ipd.isdeleted = 0 or ipd.isdeleted is null)    

					");
			//查询时间类型（出库时间、批次打印时间）
			if (searchModel.TypeTime == Convert.ToInt32(Enums.OutboundPrintTimeType.OutboundTime))
			{
				if (searchModel.StartTime > DateTime.MinValue)
				{
					sbSql.Append(" AND ob.CreateTime > :BeginTime");
					parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = searchModel.StartTime });
				}
				if (searchModel.StartTime < DateTime.MaxValue)
				{
					sbSql.Append(" AND ob.CreateTime < :EndTime");
					parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = searchModel.EndTime });
				}
				sbSql.Append(" AND obb.batchno is null  ");
			}
			else
			{
				if (searchModel.StartTime > DateTime.MinValue)
				{
					sbSql.Append(" AND obb.CreateTime > :BeginTime");
					parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = searchModel.StartTime });
				}
				if (searchModel.StartTime < DateTime.MaxValue)
				{
					sbSql.Append(" AND obb.CreateTime < :EndTime");
					parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = searchModel.EndTime });
				}
			}
			if (!string.IsNullOrEmpty(searchModel.FormCode))
			{
				sbSql.Append(" AND ob.FormCode=:FormCode");
				parameters.Add(new OracleParameter("FormCode", OracleDbType.Long) { Value = searchModel.FormCode });
			}
			else if (!string.IsNullOrEmpty(searchModel.BoxNo))
			{
				sbSql.Append(" And ipd.BoxNo=:BoxNo");
				parameters.Add(new OracleParameter("BoxNo", OracleDbType.Long) { Value = searchModel.BoxNo });
			}
			else if (!string.IsNullOrEmpty(searchModel.BatchNo))
			{
				sbSql.Append(" AND obb.BatchNO=:BatchNO");
				parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
			}
			else
			{
				if (searchModel.ArrivalId > 0)
				{
					sbSql.Append(" AND ob.ArrivalID=:ArrivalID");
					parameters.Add(new OracleParameter("ArrivalID", OracleDbType.Int32) { Value = searchModel.ArrivalId });
				}
			}
			sbSql.Append(" AND ob.DepartureID=:DepartureID");
			parameters.Add(new OracleParameter("DepartureID", OracleDbType.Int32) { Value = searchModel.ExpressId });

			sbSql.Append(@"
				 GROUP BY ob.BatchNO,ob.arrivalid
				)
				select t1.*,ec.ExpressCompanyID,ec.CompanyName AS OutboundDesName,ec.email
				from t1
				JOIN PS_PMS.ExpressCompany ec ON t1.OutboundArrivedId = ec.ExpressCompanyID										
				ORDER BY t1.BatchNO
				");

			return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<OutboundPrintModelV2>(TMSReadOnlyConnection,
																							sbSql.ToString(), searchModel,
																							parameters.ToArray());
		}

		public int UpdateBatchNo(OutboundPrintSearchModel searchModel)
		{
			StringBuilder sbSql = new StringBuilder();
			StringBuilder sbSqlWhere = new StringBuilder();
			sbSql.Append(@"Update Sc_Outbound ob
						   SET ob.batchno = :BathchNo,
							   ob.updatetime = sysdate,
							   ob.updateby = :UpdateBy							   
						 where ob.formcode in 
						 (select ob.formcode from
						 Sc_Outbound ob
						 JOIN SC_BillInfo bi ON ob.FormCode = bi.FormCode 
						 Left Join SC_OUTBOUNDBATCH obb on ob.batchno = obb.batchno
						 LEFT join SC_INBOUNDPACKINGDETAIL ipd on ipd.formcode = bi.formcode
					     Where 1=1 AND obb.batchno is null And (ipd.isdeleted = 0 or ipd.isdeleted is null)");

			List<OracleParameter> parameters = new List<OracleParameter>();
			parameters.Add(new OracleParameter("BathchNo", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
			parameters.Add(new OracleParameter("UpdateBy", OracleDbType.Varchar2) { Value = UserContext.CurrentUser.ID });
			if (!string.IsNullOrEmpty(searchModel.FormCode))
			{
				sbSql.Append(" AND ob.FormCode = :FormCode");
				parameters.Add(new OracleParameter("FormCode", OracleDbType.Long) { Value = searchModel.FormCode });
			}
			else if (!string.IsNullOrEmpty(searchModel.BoxNo))
			{
				sbSql.Append(" AND ipd.BoxNo = :BoxNo");
				parameters.Add(new OracleParameter("BoxNo", OracleDbType.Long) { Value = searchModel.BoxNo });
			}
			else
			{
				if (searchModel.StartTime > DateTime.MinValue)
				{
					sbSqlWhere.Append(" AND ob.CreateTime > :BeginTime");
					parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = searchModel.StartTime });
				}
				if (searchModel.StartTime < DateTime.MaxValue)
				{
					sbSqlWhere.Append(" AND ob.CreateTime < :EndTime");
					parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = searchModel.EndTime });
				}
			}
			sbSqlWhere.Append(" AND ob.departureid = :DepartureId");
			sbSqlWhere.Append(" AND ob.arrivalid = :Arrivalid");
			parameters.Add(new OracleParameter("DepartureId", OracleDbType.Int32) { Value = searchModel.ExpressId });
			parameters.Add(new OracleParameter("Arrivalid", OracleDbType.Int32) { Value = searchModel.ArrivalId });
			sbSql.Append(sbSqlWhere);
			sbSql.Append(")");
			sbSql.Append(" And ob.batchno = 0 ");
			sbSql.Append(sbSqlWhere);
			return ExecuteSqlNonQuery(TMSWriteConnection, sbSql.ToString(), parameters.ToArray());
		}
		/// <summary>
		/// 获取出库打印交接单V2
		/// </summary>
		/// <param name="searchModel"></param>
		/// <returns></returns>
		public IList<OutboundPrintModelV2> GetOutboundPrintReceipt(OutboundPrintSearchModel searchModel)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
					with t1 as(
						select max(ob.CreateTime) AS OutboundTime,
							max(obb.createtime) as BatchPrintTime,
							max(ob.ArrivalID) as OutboundArrivedId,
							max(ob.batchno) as BatchNo,
							count(distinct ipd.boxno) as BoxsCount,
							count(1) as FormCodesCount,
							sum(bi.weight) as Weight,
							max(ob.DepartureID) as DepartureID             
					   FROM SC_Outbound ob
					   JOIN SC_BillInfo bi
						 ON ob.FormCode = bi.FormCode
					   Left Join SC_OUTBOUNDBATCH obb
						 on ob.batchno = obb.batchno
					   LEFT join SC_INBOUNDPACKINGDETAIL ipd
						 on ipd.formcode = bi.formcode
					  WHERE 1 = 1 AND (ipd.isdeleted = 0 or ipd.isdeleted is null)	    

					");
			if (searchModel.StartTime > DateTime.MinValue)
			{
				sbSql.Append(" AND ob.CreateTime > :BeginTime");
				parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = searchModel.StartTime });
			}
			if (searchModel.StartTime < DateTime.MaxValue)
			{
				sbSql.Append(" AND ob.CreateTime < :EndTime");
				parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = searchModel.EndTime });
			}
			if (!string.IsNullOrEmpty(searchModel.FormCode))
			{
				sbSql.Append(" AND ob.FormCode=:FormCode");
				parameters.Add(new OracleParameter("FormCode", OracleDbType.Long) { Value = searchModel.FormCode });
			}
			else if (!string.IsNullOrEmpty(searchModel.BoxNo))
			{
				sbSql.Append(" And ipd.BoxNo=:BoxNo");
				parameters.Add(new OracleParameter("BoxNo", OracleDbType.Long) { Value = searchModel.BoxNo });
			}
			else if (!string.IsNullOrEmpty(searchModel.BatchNo))
			{
				sbSql.Append(" AND obb.BatchNO=:BatchNO");
				parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
			}
			else
			{
				if (!string.IsNullOrEmpty(searchModel.ArrivalIdList))
				{
					sbSql.Append(@" And ob.ArrivalID in 
					(SELECT REGEXP_SUBSTR(:ArrivalIdList, '[^,]+', 1, LEVEL) AS ArrivalIdList
					FROM DUAL
					CONNECT BY LEVEL <=
					LENGTH(TRIM(TRANSLATE(:ArrivalIdList,TRANSLATE(:ArrivalIdList, ',', ' '), ' '))) + 1)");
					parameters.Add(new OracleParameter(string.Format(":{0}", "ArrivalIdList"), String.Join(",", searchModel.ArrivalIdList)));
				}
			}

			sbSql.Append(" AND ob.DepartureID=:DepartureID");
			parameters.Add(new OracleParameter("DepartureID", OracleDbType.Int32) { Value = searchModel.ExpressId });

			sbSql.Append(@"
				 GROUP BY ob.BatchNO,ob.arrivalid
				)
				select t1.*,ec.ExpressCompanyID,ec.CompanyName AS OutboundDesName,ec1.CompanyName AS DepartureName,ec.email
				from t1
				JOIN PS_PMS.ExpressCompany ec ON t1.OutboundArrivedId = ec.ExpressCompanyID	
				Join PS_PMS.ExpressCompany ec1 ON t1.DepartureID = ec1.ExpressCompanyID				
				ORDER BY t1.BatchNO
				");
			return ExecuteSql_ByReaderReflect<OutboundPrintModelV2>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}

		public IList<OutboundPrintModelV2> GetOutboundPrintReceiptByBatchNos(IList<string> batchNoList)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"
					with t1 as(
						select max(ob.CreateTime) AS OutboundTime,
							max(obb.createtime) as BatchPrintTime,
							max(ob.ArrivalID) as OutboundArrivedId,
							max(ob.batchno) as BatchNo,
							count(distinct ipd.boxno) as BoxsCount,
							count(1) as FormCodesCount,
							sum(bi.weight) as Weight,
							max(ob.DepartureID) as DepartureID             
					   FROM SC_Outbound ob
					   JOIN SC_BillInfo bi
						 ON ob.FormCode = bi.FormCode
					   Left Join SC_OUTBOUNDBATCH obb
						 on ob.batchno = obb.batchno
					   LEFT join SC_INBOUNDPACKINGDETAIL ipd
						 on ipd.formcode = bi.formcode
					  WHERE 1 = 1 AND (ipd.isdeleted = 0 or ipd.isdeleted is null) 		      

					");
			string batchNos = string.Join(",", batchNoList.Select(x => x));
			sbSql.AppendFormat(GetOracleInParameterWhereSql("ob.BatchNO", "BatchNO", false, true, false));
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2, 4000) { Value = batchNos });
			sbSql.AppendLine(@"group by ob.BatchNO");
			sbSql.AppendLine(@")
				select t1.*,ec.ExpressCompanyID,ec.CompanyName AS OutboundDesName,ec1.CompanyName AS DepartureName,ec.email
				from t1
				JOIN PS_PMS.ExpressCompany ec ON t1.OutboundArrivedId = ec.ExpressCompanyID	
				Join PS_PMS.ExpressCompany ec1 ON t1.DepartureID = ec1.ExpressCompanyID				
				ORDER BY t1.BatchNO");
			return ExecuteSql_ByReaderReflect<OutboundPrintModelV2>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
		}

		/// <summary>
		/// 导出出库明细
		/// </summary>
		/// <param name="batchNoList"></param>
		/// <returns></returns>
		public IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(IList<string> batchNoList)
		{
			if (batchNoList == null || batchNoList.Count == 0) throw new ArgumentNullException("batchNoList");
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"select ROW_NUMBER()over(order by sysdate) as No,
							  ob.Formcode as FormCode,
							  b.customerorder as CustomerCode,
							  mbi.merchantname as MerchantName,
							  bi.receivableamount as NeedFund,
							  --'应退金额' as BackFund,
							  ob.createtime as OutboundTime,
							  ec.companyname as ArrivedDes          
					   From SC_Outbound ob  
					   inner join sc_bill b  on ob.FormCode=b.FormCode
					   inner join sc_billinfo bi on ob.Formcode=bi.formcode
					   inner join PS_PMS.MerchantBaseInfo mbi  on mbi.id = b.MerchantID
					   inner join PS_PMS.ExpressCompany ec  on ec.ExpressCompanyID=ob.arrivalid
					   where 1=1 ");
			string batchNos = string.Join(",", batchNoList.Select(x => x));
			sbSql.AppendFormat(GetOracleInParameterWhereSql("ob.BatchNO", "BatchNO", false, true, false));
			parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2, 4000) { Value = batchNos });
			var reportList = ExecuteSql_ByReaderReflect<OutboundPrintExportDetailsModelV2>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
			var totalModel = new OutboundPrintExportDetailsModelV2();
			totalModel.No = "合计";
			totalModel.FormCode = Convert.ToString(reportList.Count);
			totalModel.NeedFund = reportList.ToList<OutboundPrintExportDetailsModelV2>().Sum(x => x.NeedFund);
			totalModel.OutboundTime = null;
			reportList.Add(totalModel);
			return reportList;
		}
		public IList<OutboundPrintExportDetailsModelV2> GetOutboundPrintExportModelV2(OutboundPrintSearchModel searchModel)
		{
			StringBuilder sbSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			sbSql.Append(@"select ROW_NUMBER()over(order by sysdate) as No,
							  ob.Formcode as FormCode,
							  b.customerorder as CustomerCode,
							  mbi.merchantname as MerchantName,
							  bi.receivableamount as NeedFund,
							  --'应退金额' as BackFund,
							  ob.createtime as OutboundTime,
							  ec.companyname as ArrivedDes          
					   From SC_Outbound ob  
					   inner join sc_bill b  on ob.FormCode=b.FormCode
					   inner join sc_billinfo bi on ob.Formcode=bi.formcode
					   inner join PS_PMS.MerchantBaseInfo mbi  on mbi.id = b.MerchantID
					   inner join PS_PMS.ExpressCompany ec  on ec.ExpressCompanyID=ob.arrivalid
					   left join sc_outboundbatch obb on ob.batchno = obb.batchno
					   LEFT join SC_INBOUNDPACKINGDETAIL ipd on ipd.formcode = bi.formcode
					   where 1=1 
					  AND (ipd.isdeleted = 0 or ipd.isdeleted is null) ");
			if (searchModel.StartTime > DateTime.MinValue)
			{
				sbSql.Append(" AND ob.CreateTime > :BeginTime");
				parameters.Add(new OracleParameter("BeginTime", OracleDbType.Date) { Value = searchModel.StartTime });
			}
			if (searchModel.StartTime < DateTime.MaxValue)
			{
				sbSql.Append(" AND ob.CreateTime < :EndTime");
				parameters.Add(new OracleParameter("EndTime", OracleDbType.Date) { Value = searchModel.EndTime });
			}
			sbSql.Append(" AND obb.batchno is null  ");
			if (!string.IsNullOrEmpty(searchModel.FormCode))
			{
				sbSql.Append(" AND ob.FormCode=:FormCode");
				parameters.Add(new OracleParameter("FormCode", OracleDbType.Long) { Value = searchModel.FormCode });
			}
			else if (!string.IsNullOrEmpty(searchModel.BoxNo))
			{
				sbSql.Append(" And ipd.BoxNo=:BoxNo");
				parameters.Add(new OracleParameter("BoxNo", OracleDbType.Long) { Value = searchModel.BoxNo });
			}
			else if (!string.IsNullOrEmpty(searchModel.BatchNo))
			{
				sbSql.Append(" AND obb.BatchNO=:BatchNO");
				parameters.Add(new OracleParameter("BatchNO", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
			}
			else
			{
				if (!string.IsNullOrEmpty(searchModel.ArrivalIdList))
				{
					sbSql.Append(@" And ob.ArrivalID in 
					(SELECT REGEXP_SUBSTR(:ArrivalIdList, '[^,]+', 1, LEVEL) AS ArrivalIdList
					FROM DUAL
					CONNECT BY LEVEL <=
					LENGTH(TRIM(TRANSLATE(:ArrivalIdList,TRANSLATE(:ArrivalIdList, ',', ' '), ' '))) + 1)");
					parameters.Add(new OracleParameter(string.Format(":{0}", "ArrivalIdList"), String.Join(",", searchModel.ArrivalIdList)));
				}
			}

			sbSql.Append(" AND ob.DepartureID=:DepartureID");
			parameters.Add(new OracleParameter("DepartureID", OracleDbType.Int32) { Value = searchModel.ExpressId });

			var reportList = ExecuteSql_ByReaderReflect<OutboundPrintExportDetailsModelV2>(TMSReadOnlyConnection, sbSql.ToString(), parameters.ToArray());
			var totalModel = new OutboundPrintExportDetailsModelV2();
			totalModel.No = "合计";
			totalModel.FormCode = Convert.ToString(reportList.Count);
			totalModel.NeedFund = reportList.ToList<OutboundPrintExportDetailsModelV2>().Sum(x => x.NeedFund);
			totalModel.OutboundTime = null;
			reportList.Add(totalModel);
			return reportList;
		}
		#endregion

		#endregion
		
		
	}
}

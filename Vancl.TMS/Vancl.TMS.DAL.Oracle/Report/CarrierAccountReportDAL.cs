using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Report;
using Vancl.TMS.Model.Report.CarrierAccountReport;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Report
{
    public class CarrierAccountReportDAL : BaseDAL, ICarrierAccountReportDAL
    {

        #region ICarrierAccountReportDAL 成员

        /// <summary>
        /// 构建通用参数对象
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private OracleParameter[] CreateParameter(CarrierAccountReportSearchModel searchModel)
        {
            List<OracleParameter> listParameter = new List<OracleParameter>();
            if (searchModel.DepartureStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureStartTime", DbType = DbType.DateTime, Value = searchModel.DepartureStartTime.Value });
            }
            if (searchModel.DepartureEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureEndTime", DbType = DbType.DateTime, Value = searchModel.DepartureEndTime.Value });
            }
            if (searchModel.TransportType.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int16, Value = (int)searchModel.TransportType });
            }
            if (searchModel.CarrierID.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.String, Value = searchModel.CarrierID });
            }
            if (searchModel.DepartureID.HasValue && searchModel.DepartureID > 0)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            }
            if (searchModel.ArrivalID.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DeliveryStatus", DbType = DbType.Int32, Value = Convert.ToInt32(searchModel.DeliveryStatus.Value) });
            }
            return listParameter.ToArray();
        }

        /// <summary>
        /// 构建统计信息的SQL
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateStatisticsPLSQL(CarrierAccountReportSearchModel searchModel)
        {
            String sql = @"
SELECT 
SUM(custWaybill.TotalCount) AS OrderCount
,SUM(custWaybill.Weight) AS TotalWeight
,SUM(dispAss.Baseamount) AS BaseAmount
,SUM(disp.protectedprice) AS ProtectedPrice
,SUM(dispAss.Insuranceamount) AS InsuranceAmount
,SUM(dispAss.Complementamount) AS ComplementAmount
,SUM(dispAss.Longdeliveryamount) AS LongDeliveryAmount
,SUM(dispAss.Longtransferamount) AS LongTransferAmount
,SUM(dispAss.Longpickprice) AS LongPickPrice
,SUM(dispAss.Needamount) AS NeedAmount
,SUM(CASE WHEN dispAss.Kpidelaytype = 0 
THEN  round(dispAss.Needamount * NVL(dispAss.Discount,1),2) - dispAss.Needamount
ELSE NVL(dispAss.Delayamount, 0) END)  AS KPIAmount
,SUM(dispAss.Lostdeduction) AS LostDeduction
,SUM(dispAss.Otheramount) AS OtherAmount
,SUM(dispAss.Confirmedamount) AS ConfirmedAmount
FROM TMS_DISPATCH disp
     JOIN TMS_DELIVERYASSESSMENT dispAss ON disp.deliveryno = dispAss.Deliveryno
     JOIN TMS_SITEASSESSMENT siteAss ON disp.deliveryno = siteAss.Deliveryno
     JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
     JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
     JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
     JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID
     JOIN TMS_LINEPLAN lp ON disp.lpid  = lp.lpid
WHERE disp.Isdeleted = 0
";
            StringBuilder resultSql = new StringBuilder(sql);
            resultSql.Append(CreateCommonConditionSQLstring(searchModel));
            return resultSql.ToString();
        }

        /// <summary>
        /// 构建统计信息的SQL(动态)
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateDynamicStatisticsPLSQL(CarrierAccountReportSearchModel searchModel)
        {
            String sql = @"
SELECT 
SUM(custWaybill.TotalCount) AS OrderCount
,SUM(custWaybill.Weight) AS TotalWeight
,SUM(disp.protectedprice) AS ProtectedPrice
FROM TMS_DISPATCH disp
     JOIN TMS_SITEASSESSMENT siteAss ON disp.deliveryno = siteAss.Deliveryno
     JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
     JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
     JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
     JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID
     JOIN TMS_LINEPLAN lp ON disp.lpid  = lp.lpid
WHERE disp.Isdeleted = 0
";
            StringBuilder resultSql = new StringBuilder(sql);
            resultSql.Append(CreateCommonConditionSQLstring(searchModel));
            return resultSql.ToString();
        }

        /// <summary>
        /// 创建通用条件SQL字符串
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateCommonConditionSQLstring(CarrierAccountReportSearchModel searchModel)
        {
            StringBuilder resultSql = new StringBuilder();
            if (searchModel.DepartureStartTime.HasValue)
            {
                resultSql.Append(@"
AND siteass.leavetime >= :DepartureStartTime
                ");
            }
            if (searchModel.DepartureEndTime.HasValue)
            {
                resultSql.Append(@"
AND siteass.leavetime <= :DepartureEndTime
                ");
            }
            if (searchModel.TransportType.HasValue)
            {
                resultSql.Append(@"
AND lp.transporttype = :TransportType
                ");
            }
            if (searchModel.CarrierID.HasValue)
            {
                resultSql.Append(@"
AND disp.carrierid = :CarrierID
");
            }
            if (searchModel.DepartureID.HasValue && searchModel.DepartureID > 0)
            {
                resultSql.Append(@"
AND disp.departureid = :DepartureID
");
            }
            if (searchModel.ArrivalID.HasValue)
            {
                resultSql.Append(@"
AND disp.arrivalid = :ArrivalID
");
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                resultSql.Append(@"
AND disp.DeliveryStatus = :DeliveryStatus
");
            }
            return resultSql.ToString();
        }

        /// <summary>
        /// 创建取得列表信息通用SQL
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateGetListCommonPLSQL(CarrierAccountReportSearchModel searchModel)
        {
            String sql = @"
SELECT 
siteass.leavetime AS DepartureTime
,lp.transporttype AS TransportType
,carr.carriername  AS CarrierName
,custWaybill.Waybillno AS CustWaybillNo
,ecd.companyname AS DepartureName
,eca.companyname AS ArrivalName
,custWaybill.TotalCount AS OrderCount
,custWaybill.Weight AS TotalWeight
,dispAss.Baseamount AS BaseAmount
,disp.protectedprice AS ProtectedPrice
,dispAss.Insuranceamount AS InsuranceAmount
,dispAss.Complementamount AS ComplementAmount
,dispAss.Longdeliveryamount AS LongDeliveryAmount
,dispAss.Longtransferamount AS LongTransferAmount
,dispAss.Longpickprice AS LongPickPrice
,dispAss.Needamount AS NeedAmount
,CASE WHEN dispAss.Kpidelaytype = 0 
THEN round(dispAss.Needamount * NVL(dispAss.Discount,1),2) - dispAss.Needamount
ELSE NVL(dispAss.Delayamount, 0) END  AS KPIAmount
,dispAss.Lostdeduction AS LostDeduction
,dispAss.Otheramount AS OtherAmount
,dispAss.Confirmedamount AS ConfirmedAmount
,disp.deliveryno AS DeliveryNo
FROM TMS_DISPATCH disp
     JOIN TMS_DELIVERYASSESSMENT dispAss ON disp.deliveryno = dispAss.Deliveryno
     JOIN TMS_SITEASSESSMENT siteAss ON disp.deliveryno = siteAss.Deliveryno
     JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
     JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
     JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
     JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID
     JOIN TMS_LINEPLAN lp ON disp.lpid  = lp.lpid
WHERE disp.Isdeleted = 0
";
            StringBuilder resultSql = new StringBuilder(sql);
            resultSql.Append(CreateCommonConditionSQLstring(searchModel));
            return resultSql.ToString();
        }

        /// <summary>
        /// 创建取得列表信息通用SQL (动态)
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateGetDynamicListCommonPLSQL(CarrierAccountReportSearchModel searchModel)
        {
            String sql = @"
SELECT 
siteass.leavetime AS DepartureTime
,lp.transporttype AS TransportType
,carr.carriername  AS CarrierName
,custWaybill.Waybillno AS CustWaybillNo
,ecd.companyname AS DepartureName
,eca.companyname AS ArrivalName
,custWaybill.TotalCount AS OrderCount
,custWaybill.Weight AS TotalWeight
,disp.Deliveryno AS DeliveryNo
,disp.protectedprice AS ProtectedPrice
,disp.DeliveryStatus AS DeliveryStatus
FROM TMS_DISPATCH disp
     JOIN TMS_SITEASSESSMENT siteAss ON disp.deliveryno = siteAss.Deliveryno
     JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
     JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
     JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
     JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID
     JOIN TMS_LINEPLAN lp ON disp.lpid  = lp.lpid
WHERE disp.Isdeleted = 0
";
            StringBuilder resultSql = new StringBuilder(sql);
            resultSql.Append(CreateCommonConditionSQLstring(searchModel));
            resultSql.Append(" ORDER BY DepartureTime");
            return resultSql.ToString();
        }


        public Util.Pager.PagedList<ViewCarrierAccountReport> Search(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            searchModel.OrderByString = "DepartureTime";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewCarrierAccountReport>(TMSReadOnlyConnection, CreateGetListCommonPLSQL(searchModel), searchModel, CreateParameter(searchModel));
        }

        public ViewCarrierAccountReportStatisticsModel GetStatisticsInfo(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            return ExecuteSqlSingle_ByReaderReflect<ViewCarrierAccountReportStatisticsModel>(TMSReadOnlyConnection, CreateStatisticsPLSQL(searchModel), CreateParameter(searchModel));
        }

        public List<ViewCarrierAccountReport> Export(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            var listResult = ExecuteSql_ByReaderReflect<ViewCarrierAccountReport>(TMSReadOnlyConnection, CreateGetListCommonPLSQL(searchModel), CreateParameter(searchModel));
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion

        #region ICarrierAccountReportDAL 成员

        public List<ViewCarrierAccountReport> SearchDynamicReport(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            var listResult = ExecuteSql_ByReaderReflect<ViewCarrierAccountReport>(TMSReadOnlyConnection, CreateGetDynamicListCommonPLSQL(searchModel), CreateParameter(searchModel));
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion

        #region ICarrierAccountReportDAL 成员


        public ViewCarrierAccountReportStatisticsModel GetDynamicStatisticsInfo(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            return ExecuteSqlSingle_ByReaderReflect<ViewCarrierAccountReportStatisticsModel>(TMSReadOnlyConnection, CreateDynamicStatisticsPLSQL(searchModel), CreateParameter(searchModel));
        }

        #endregion
    }
}

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.Spot;
using Vancl.TMS.IDAL.Delivery.Spot;
using Oracle.DataAccess.Client;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Delivery.Spot
{
    public class SiteAssDAL : BaseDAL, ISiteAssDAL
    {
        public int Add(SiteAssessmentModel model)
        {
            string sql = @"
                    INSERT INTO TMS_SITEASSESSMENT
                            (
                            DELIVERYNO,
                            ARRIVALTIME,
                            LEAVETIME,
                            ASSESSMENTSTATUS,
                            REASON,
                            CREATEBY,
                            UPDATEBY,
                            ISDELETED）
                        VALUES(
                            :DELIVERYNO,
                            :ARRIVALTIME,
                            :LEAVETIME,
                            :ASSESSMENTSTATUS,
                            :REASON,
                            :CREATEBY,
                            :UPDATEBY,
                            0）";
            OracleParameter[] parameters = new OracleParameter[]{
                new OracleParameter(){ParameterName = "DELIVERYNO" ,DbType = DbType.String , Value = model.DeliveryNO} 
                ,new OracleParameter(){ParameterName = "ARRIVALTIME" ,DbType = DbType.DateTime , Value = model.ArrivalTime} 
                ,new OracleParameter(){ParameterName = "LEAVETIME" ,DbType = DbType.DateTime , Value = model.LeaveTime} 
                ,new OracleParameter(){ParameterName = "ASSESSMENTSTATUS" ,DbType = DbType.Byte , Value = model.AssessmentStatus} 
                ,new OracleParameter(){ParameterName = "REASON" ,DbType = DbType.String , Value = model.Reason} 
                ,new OracleParameter(){ParameterName = "CREATEBY" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID} 
                ,new OracleParameter(){ParameterName = "UPDATEBY" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public IList<ViewSiteAssModel> Search(SiteAssSearchModel searchModel)
        {
            var paramList = new List<OracleParameter>();
            string sql = string.Format(@"
                SELECT  A.DELIVERYNO, A.BOXCOUNT, A.TOTALAMOUNT, E1.COMPANYNAME DEPARTURENAME,A.DELIVERYSTATUS,
                    E2.COMPANYNAME ARRIVALNAME,C.CARRIERNAME CARRIERname, A.TRANSPORTTYPE,A.LINEGOODSTYPE, 
                    A.ARRIVALTIMING,W.WAYBILLNO CARRIERWAYBILLNO,CITY.CITYNAME ArrivalCity,
                    L.ARRIVALASSESSMENTTIME,L.LEAVEASSESSMENTTIME,A.DispatchTime DispatchCreateTime
                FROM TMS_DISPATCH A
                    JOIN EXPRESSCOMPANY E1 ON A.DEPARTUREID=E1.EXPRESSCOMPANYID
                    JOIN EXPRESSCOMPANY E2 ON A.ARRIVALID=E2.EXPRESSCOMPANYID
                    JOIN CITY ON CITY.CITYID = E2.CITYID
                    JOIN PROVINCE P ON P.PROVINCEID = E2.PROVINCEID
                    JOIN TMS_CARRIER C ON A.CARRIERID=C.CARRIERID 
                    JOIN TMS_CARRIERWAYBILL W ON W.CWID = A.CARRIERWAYBILLID
                    JOIN TMS_LINEPLAN L ON L.LPID = A.LPID
                WHERE A.IsDeleted=0 AND  A.DELIVERYSTATUS={0}", (int)Enums.DeliveryStatus.Dispatched);
            
            if (searchModel.DepartureID != 0)
            {
                sql += " AND A.DEPARTUREID = :DEPARTUREID";
                paramList.Add(new OracleParameter() { ParameterName = "DEPARTUREID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            }
            if (searchModel.ArrivalID != 0)
            {
                sql += " AND A.ARRIVALID = :ARRIVALID";
                paramList.Add(new OracleParameter() { ParameterName = "ARRIVALID", DbType = DbType.Int32, Value = searchModel.ArrivalID });
            }
            if (searchModel.ArrivalProvince != 0)
            {
                sql += " AND P.PROVINCEID = :PROVINCEID";
                paramList.Add(new OracleParameter() { ParameterName = "PROVINCEID", DbType = DbType.Int32, Value = searchModel.ArrivalProvince });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                sql += " AND A.CARRIERID = :CARRIERID";
                paramList.Add(new OracleParameter() { ParameterName = "CARRIERID", DbType = DbType.String, Value = searchModel.CarrierID });
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                sql += " AND A.DELIVERYSTATUS = :DELIVERYSTATUS";
                paramList.Add(new OracleParameter() { ParameterName = "DELIVERYSTATUS", DbType = DbType.Int32, Value = (int)searchModel.DeliveryStatus });
            }
            //if (searchModel.LineType.HasValue)
            //{
            //    sql += " AND A.DELIVERYSTATUS = :DELIVERYSTATUS";
            //    paramList.Add(new OracleParameter() { ParameterName = "DELIVERYSTATUS", DbType = DbType.Int32, Value = (int)searchModel.DeliveryStatus });
            //}
            if (searchModel.CreateDateBegin.HasValue)
            {
                sql += " AND A.DispatchTime >= :CreateDateBegin";
                paramList.Add(new OracleParameter() { ParameterName = "CreateDateBegin", DbType = DbType.DateTime, Value = searchModel.CreateDateBegin });
            }
            if (searchModel.CreateDateEnd.HasValue)
            {
                sql += " AND A.DispatchTime <= :CreateDateEnd";
                paramList.Add(new OracleParameter() { ParameterName = "CreateDateEnd", DbType = DbType.DateTime, Value = searchModel.CreateDateEnd });
            }
            searchModel.OrderByString = "DELIVERYNO";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewSiteAssModel>(TMSReadOnlyConnection, sql, searchModel, paramList.ToArray());
        }
    }
}

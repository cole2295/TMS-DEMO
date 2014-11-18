using System.Data;
using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Line;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using System;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Util.Converter;
using System.Text;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Core.Security;
using System.Linq;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Line
{
    public class LinePlanDAL : BaseDAL, ILinePlanDAL
    {
        #region ILinePlanDAL 成员

        /// <summary>
        /// 新增线路计划
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(LinePlanModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("LinePlanModel");
            }
            string strSql = @"
                INSERT INTO TMS_LinePlan
                    (LPID
                    ,LineID
                    ,DepartureID
                    ,ArrivalID
                    ,CarrierID
                    ,TransportType
                    ,ArrivalAssessmentTime
                    ,LeaveAssessmentTime
                    ,ArrivalTiming
                    ,InsuranceRate
                    ,LowestPrice
                    ,LongDeliveryPrice
                    ,LongPickPrice
                    ,LongTransferRate
                    ,Priority
                    ,IsEnabled
                    ,Status
                    ,LineType
                    ,BusinessType
                    ,LineGoodsType
                    ,ExpressionType
                    ,EffectiveTime
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted)
                VALUES
                    (:LPID
                    ,:LineID
                    ,:DepartureID
                    ,:ArrivalID
                    ,:CarrierID
                    ,:TransportType
                    ,:ArrivalAssessmentTime
                    ,:LeaveAssessmentTime
                    ,:ArrivalTiming
                    ,:InsuranceRate
                    ,:LowestPrice
                    ,:LongDeliveryPrice
                    ,:LongPickPrice
                    ,:LongTransferRate
                    ,:Priority
                    ,:IsEnabled
                    ,:Status
                    ,:LineType
                    ,:BusinessType
                    ,:LineGoodsType
                    ,:ExpressionType
                    ,:EffectiveTime
                    ,:CreateBy
                    ,:UpdateBy
                    ,0)";
            OracleParameter[] parameters = new OracleParameter[]{
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID} 
                ,new OracleParameter(){ParameterName = "LineID" ,DbType = DbType.String , Value = model.LineID} 
                ,new OracleParameter(){ParameterName = "DepartureID" ,DbType = DbType.Int32 , Value = model.DepartureID} 
                ,new OracleParameter(){ParameterName = "ArrivalID" ,DbType = DbType.Int32 , Value = model.ArrivalID} 
                ,new OracleParameter(){ParameterName = "CarrierID" ,DbType = DbType.Int32 , Value = model.CarrierID} 
                ,new OracleParameter(){ParameterName = "TransportType" ,DbType = DbType.Int32 , Value = (int)model.TransportType} 
                ,new OracleParameter(){ParameterName = "ArrivalAssessmentTime" ,DbType = DbType.DateTime , Value = model.ArrivalAssessmentTime} 
                ,new OracleParameter(){ParameterName = "LeaveAssessmentTime" ,DbType = DbType.DateTime , Value = model.LeaveAssessmentTime} 
                ,new OracleParameter(){ParameterName = "ArrivalTiming" ,DbType = DbType.Decimal , Value = model.ArrivalTiming}
                ,new OracleParameter(){ParameterName = "InsuranceRate" ,DbType = DbType.Decimal , Value = model.InsuranceRate} 
                ,new OracleParameter(){ParameterName = "LowestPrice" ,DbType = DbType.Decimal , Value = model.LowestPrice} 
                ,new OracleParameter(){ParameterName = "LongDeliveryPrice" ,DbType = DbType.Decimal , Value = model.LongDeliveryPrice} 
                ,new OracleParameter(){ParameterName = "LongPickPrice" ,DbType = DbType.Decimal , Value = model.LongPickPrice} 
                ,new OracleParameter(){ParameterName = "LongTransferRate" ,DbType = DbType.Decimal , Value = model.LongTransferRate} 
                ,new OracleParameter(){ParameterName = "Priority" ,DbType = DbType.Int32 , Value = (int)model.Priority} 
                ,new OracleParameter(){ParameterName = "Status" ,DbType = DbType.Int32 , Value =(int)model.Status}
                ,new OracleParameter(){ParameterName = "IsEnabled" ,DbType = DbType.Byte , Value =model.IsEnabled} 
                ,new OracleParameter(){ParameterName = "LineType" ,DbType = DbType.Int32 , Value = (int)model.LineType} 
                ,new OracleParameter(){ParameterName = "BusinessType" ,DbType = DbType.Int32 , Value = (int)model.BusinessType} 
                ,new OracleParameter(){ParameterName = "LineGoodsType" ,DbType = DbType.Int32 , Value = (int)model.LineGoodsType} 
                ,new OracleParameter(){ParameterName = "ExpressionType" ,DbType = DbType.Int32 , Value = (int)model.ExpressionType} 
                ,new OracleParameter(){ParameterName = "EffectiveTime" ,DbType = DbType.DateTime , Value = model.EffectiveTime} 
                ,new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID} 
                ,new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public int Update(LinePlanModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("LinePlanModel");
            }
            string strSql = @"
                UPDATE TMS_LinePlan SET
                     DepartureID = :DepartureID
                    ,ArrivalID = :ArrivalID
                    ,CarrierID = :CarrierID
                    ,TransportType = :TransportType
                    ,ArrivalAssessmentTime = :ArrivalAssessmentTime
                    ,LeaveAssessmentTime = :LeaveAssessmentTime
                    ,ArrivalTiming = :ArrivalTiming
                    ,InsuranceRate = :InsuranceRate
                    ,LowestPrice = :LowestPrice
                    ,LongDeliveryPrice = :LongDeliveryPrice
                    ,LongPickPrice = :LongPickPrice
                    ,LongTransferRate = :LongTransferRate
                    ,Priority = :Priority
                    ,Status = :Status
                    ,LineType = :LineType
                    ,BusinessType = :BusinessType
                    ,LineGoodsType = :LineGoodsType
                    ,ExpressionType = :ExpressionType
                    ,EFFECTIVETIME = :EFFECTIVETIME
                    ,UpdateBy = :UpdateBy
                    ,UpdateTime=sysdate
               WHERE LPID = :LPID";
            OracleParameter[] parameters = new OracleParameter[]{
                new OracleParameter(){ParameterName = "DepartureID" ,DbType = DbType.Int32 , Value = model.DepartureID} 
                ,new OracleParameter(){ParameterName = "ArrivalID" ,DbType = DbType.Int32 , Value = model.ArrivalID} 
                ,new OracleParameter(){ParameterName = "CarrierID" ,DbType = DbType.Int32 , Value = model.CarrierID} 
                ,new OracleParameter(){ParameterName = "TransportType" ,DbType = DbType.Int32 , Value = (int)model.TransportType} 
                ,new OracleParameter(){ParameterName = "ArrivalAssessmentTime" ,DbType = DbType.DateTime , Value = model.ArrivalAssessmentTime} 
                ,new OracleParameter(){ParameterName = "LeaveAssessmentTime" ,DbType = DbType.DateTime , Value = model.LeaveAssessmentTime} 
                ,new OracleParameter(){ParameterName = "ArrivalTiming" ,DbType = DbType.Decimal , Value = model.ArrivalTiming}
                ,new OracleParameter(){ParameterName = "InsuranceRate" ,DbType = DbType.Decimal , Value = model.InsuranceRate} 
                ,new OracleParameter(){ParameterName = "LowestPrice" ,DbType = DbType.Decimal , Value = model.LowestPrice} 
                ,new OracleParameter(){ParameterName = "LongDeliveryPrice" ,DbType = DbType.Decimal , Value = model.LongDeliveryPrice} 
                ,new OracleParameter(){ParameterName = "LongPickPrice" ,DbType = DbType.Decimal , Value = model.LongPickPrice} 
                ,new OracleParameter(){ParameterName = "LongTransferRate" ,DbType = DbType.Decimal , Value = model.LongTransferRate} 
                ,new OracleParameter(){ParameterName = "Priority" ,DbType = DbType.Int32 , Value = (int)model.Priority} 
                ,new OracleParameter(){ParameterName = "Status" ,DbType = DbType.Int32 , Value =(int)model.Status} 
                ,new OracleParameter(){ParameterName = "LineType" ,DbType = DbType.Int32 , Value = (int)model.LineType} 
                ,new OracleParameter(){ParameterName = "BusinessType" ,DbType = DbType.Int32 , Value = (int)model.BusinessType} 
                ,new OracleParameter(){ParameterName = "LineGoodsType" ,DbType = DbType.Int32 , Value = (int)model.LineGoodsType} 
                ,new OracleParameter(){ParameterName = "ExpressionType" ,DbType = DbType.Int32 , Value = (int)model.ExpressionType} 
                ,new OracleParameter(){ParameterName = "EFFECTIVETIME" ,DbType = DbType.DateTime , Value = model.EffectiveTime}
                ,new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
                ,new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID} };

            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 线路计划审核
        /// </summary>
        /// <param name="lpid"></param>
        /// <param name="lineStatus"></param>
        /// <param name="effectiveTime"></param>
        /// <param name="isBatch"></param>
        /// <returns></returns>
        public int AuditLinePlan(int lpid, Enums.LineStatus lineStatus, DateTime? effectiveTime, bool isBatch)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            string strSql = "";
            parameters.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)lineStatus });
            parameters.Add(new OracleParameter() { ParameterName = "LPID", DbType = DbType.Int32, Value = lpid });
            parameters.Add(new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID });
            parameters.Add(new OracleParameter() { ParameterName = "APPRVEBY", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID });

            if (lineStatus == Enums.LineStatus.Approved && !isBatch)
            {
                strSql = @"
                    UPDATE TMS_LinePlan  
                    SET Status = :Status
                        ,UpdateTime=sysdate
                        ,UpdateBy=:UpdateBy
                        ,EFFECTIVETIME=:EFFECTIVETIME 
                        ,APPRVETIME = sysdate
                        ,APPRVEBY = :APPRVEBY 
                    WHERE LPID = :LPID 
                        AND IsDeleted=0";
                parameters.Add(new OracleParameter() { ParameterName = "EFFECTIVETIME", DbType = DbType.Date, Value = effectiveTime.Value });
            }
            else
            {
                strSql = @"
                    UPDATE TMS_LinePlan  
                    SET Status = :Status
                        ,UpdateTime=sysdate
                        ,UpdateBy=:UpdateBy
                        ,APPRVETIME = sysdate
                        ,APPRVEBY = :APPRVEBY 
                    WHERE LPID = :LPID 
                        AND IsDeleted=0";
            }

            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters.ToArray());
        }

        /// <summary>
        /// 根据主键ID查询线路计划
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        public LinePlanModel GetLinePlan(int lpid)
        {
            string strSql = @"select LPID,LineID,DepartureID,ArrivalID,L.CarrierID,TransportType
                              ,ArrivalAssessmentTime,LeaveAssessmentTime,ArrivalTiming,InsuranceRate
                              ,LowestPrice,LongDeliveryPrice,LongPickPrice,LongTransferRate,Priority
                              ,L.Status,LineType,LineGoodsType,ExpressionType,EffectiveTime,BusinessType,
                              E1.COMPANYNAME DEPARTURENAME,E2.COMPANYNAME ARRIVALNAME,C.CARRIERNAME 
                        FROM TMS_LinePlan  L
                             JOIN EXPRESSCOMPANY E1 ON E1.EXPRESSCOMPANYID = L.DepartureID 
                             JOIN EXPRESSCOMPANY E2 ON E2.EXPRESSCOMPANYID = L.ArrivalID 
                             JOIN TMS_CARRIER C ON C.CARRIERID = L.CarrierID
                        WHERE LPID=:LPID AND L.IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid }
            };
            return ExecuteSqlSingle_ByDataTableReflect<LinePlanModel>(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 根据主键ID查询线路计划
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        public ViewLinePlanModel GetViewLinePlan(int lpid)
        {
            string strSql = @"
                SELECT LPID,LineID,L.DepartureID,E1.COMPANYNAME DEPARTURENAME,L.ArrivalID,E2.COMPANYNAME ARRIVALNAME
                ,CITY.CITYNAME ARRIVALCITYNAME,C.CARRIERNAME,TransportType,BusinessType
                ,ArrivalAssessmentTime,LeaveAssessmentTime,ArrivalTiming,InsuranceRate
                ,LowestPrice,LongDeliveryPrice,LongPickPrice,LongTransferRate,Priority
                ,L.Status,LineType,LineGoodsType,ExpressionType,EffectiveTime,L.IsEnabled
                from TMS_LinePlan L
                    JOIN EXPRESSCOMPANY E1 ON E1.EXPRESSCOMPANYID = L.DepartureID 
                    JOIN EXPRESSCOMPANY E2 ON E2.EXPRESSCOMPANYID = L.ArrivalID 
                    JOIN CITY ON CITY.CITYID = E2.CITYID  
                    JOIN TMS_CARRIER C ON C.CARRIERID = L.CarrierID AND C.IsDeleted=0
                WHERE LPID=:LPID AND L.IsDeleted=0
            ";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid }
            };
            return ExecuteSqlSingle_ByReaderReflect<ViewLinePlanModel>(TMSReadOnlyConnection, strSql, parameters);
        }

        /// <summary>
        /// 根据查询条件查询线路计划
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public PagedList<ViewLinePlanModel> GetLinePlan(LinePlanSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException("LinePlanSearchModel");
            }
            List<OracleParameter> parameters = new List<OracleParameter>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                SELECT LPID,LineID,L.DepartureID,E1.COMPANYNAME DEPARTURENAME,L.ArrivalID,E2.COMPANYNAME ARRIVALNAME
                    ,CITY.CITYNAME ARRIVALCITYNAME,C.CARRIERNAME,TransportType
                    ,ArrivalAssessmentTime,LeaveAssessmentTime,ArrivalTiming,InsuranceRate
                    ,LowestPrice,LongDeliveryPrice,LongPickPrice,LongTransferRate,Priority
                    ,L.Status,LineType,LineGoodsType,ExpressionType,EffectiveTime
                    ,CASE WHEN L.IsEnabled=1 AND C.Status=1 THEN 1 ELSE 0 END IsEnabled
                FROM TMS_LinePlan L
                    JOIN EXPRESSCOMPANY E1 ON E1.EXPRESSCOMPANYID = L.DepartureID 
                    JOIN EXPRESSCOMPANY E2 ON E2.EXPRESSCOMPANYID = L.ArrivalID 
                    JOIN CITY ON CITY.CITYID = E2.CITYID  
                    JOIN TMS_CARRIER C ON C.CARRIERID = L.CarrierID AND C.IsDeleted=0
                WHERE L.IsDeleted=0 ");
            if (searchModel.CarrierID.HasValue)
            {
                sb.Append(" and L.CarrierID =:CarrierID");
                parameters.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.DepartureID != 0)
            {
                sb.Append(" and  L.DepartureID =:DepartureID");
                parameters.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            }
            if (searchModel.ArrivalID != 0)
            {
                sb.Append(" and  L.ArrivalID =:ArrivalID");
                parameters.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID });
            }
            if (searchModel.Status.HasValue)
            {
                sb.Append(" and  L.Status =:Status");
                parameters.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)searchModel.Status });
            }
            if (searchModel.TransportType.HasValue)
            {
                sb.Append(" and  L.TransportType =:TransportType");
                parameters.Add(new OracleParameter() { ParameterName = "TransportType", DbType = DbType.Int32, Value = (int)searchModel.TransportType });
            }
            if (searchModel.LineType.HasValue)
            {
                sb.Append(" and  L.LineType =:LineType");
                parameters.Add(new OracleParameter() { ParameterName = "LineType", DbType = DbType.Int32, Value = (int)searchModel.LineType });
            }
            if (searchModel.LineGoodsType.HasValue)
            {
                sb.Append(" and (L.LineGoodsType+:LineGoodsType)-BITAND(L.LineGoodsType,:LineGoodsType)=L.LineGoodsType");
                parameters.Add(new OracleParameter() { ParameterName = "LineGoodsType", DbType = DbType.Int32, Value = (int)searchModel.LineGoodsType.Value });
            }
            searchModel.OrderByString = "LineID,EffectiveTime";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewLinePlanModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, parameters.ToArray());
        }

        /// <summary>
        /// 删除线路计划
        /// </summary>
        /// <param name="lpidList"></param>
        /// <returns></returns>
        public int Delete(List<int> lpidList)
        {
            if (lpidList == null)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_LinePlan
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE LPID =:LPIDs
                    AND IsDeleted=0
                    AND Status IN ({1},{2})"
                , UserContext.CurrentUser.ID
                , (int)Enums.LineStatus.Dismissed
                , (int)Enums.LineStatus.NotApprove);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LPIDs",DbType= DbType.Int32,Value=lpidList.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lpidList.Count, arguments);
        }

        /// <summary>
        /// 取得起点所能到达的所有点集合
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <returns></returns>
        public IList<int> GetEffectiveNextStation(int DepartureID)
        {
            if (DepartureID <= 0) throw new ArgumentNullException("DepartureID");
            string sql = String.Format(@"
SELECT DISTINCT ARRIVALID
FROM TMS_LINEPLAN lp
JOIN TMS_Carrier c
    ON c.IsDeleted=0
        AND c.CarrierID=lp.CarrierID
        AND c.Status={0}
WHERE lp.IsDeleted = 0 
    AND lp.Status = {1} 
    AND lp.IsEnabled = 1
    AND lp.DEPARTUREID = :DEPARTUREID
"
                , (int)Enums.CarrierStatus.Valid
                , (int)Enums.LineStatus.Effective);
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName="DEPARTUREID", DbType= DbType.Int32, Value= DepartureID}};
            DataTable dt = ExecuteSqlDataTable(TMSReadOnlyConnection, sql, arguments);
            if (dt != null)
            {
                List<int> listResult = new List<int>();
                foreach (DataRow item in dt.Rows)
                {
                    listResult.Add(int.Parse(item[0].ToString()));
                }
                return listResult;
            }
            return null;
        }

        public bool IsExsitLinePlan(LinePlanModel linePlanModel)
        {
            if (linePlanModel == null)
            {
                throw new CodeNotValidException();
            }
            List<OracleParameter> parameters = new List<OracleParameter>();
            string strSql = @"
                SELECT COUNT(*)
                FROM TMS_LinePlan
                WHERE DepartureID =:DepartureID
                    AND ArrivalID=:ArrivalID
                    AND IsDeleted=0
                    AND CARRIERID=:CARRIERID
                    AND LineGoodsType=:LineGoodsType";

            parameters.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = linePlanModel.DepartureID });
            parameters.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = linePlanModel.ArrivalID });
            parameters.Add(new OracleParameter() { ParameterName = "CARRIERID", DbType = DbType.Int32, Value = linePlanModel.CarrierID });
            parameters.Add(new OracleParameter() { ParameterName = "LineGoodsType", DbType = DbType.Int32, Value = (int)linePlanModel.LineGoodsType });

            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, parameters.ToArray());
            if (o == null || Convert.ToInt32(o) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 服务更新线路生效状态：
        /// 先找出需要更新的线路计划的line_id；
        /// 然后找出该line_id下的所有线路计划；
        /// 然后把这些计划中应该生效的一条计划更新为生效，把其他已生效状态的更新为过期，其他不变
        /// </summary>
        /// <returns></returns>
        public int UpdateDeadLineStatus()
        {
            string sqlStr = @"
                UPDATE tms_lineplan lp
                SET lp.status =
                (
                    CASE WHEN lp.lpid=(
                                    SELECT t.lpid
                                    FROM (
                                        SELECT b.lpid,b.lineid
                                        FROM tms_lineplan b
                                        WHERE b.IsDeleted=0
                                            AND b.status=:Approved
                                            AND b.EffectiveTime<=sysdate
                                        ORDER BY b.EffectiveTime
                                    ) t
                                    WHERE t.lineid=lp.lineid
                                        AND rownum=1
                                )
                        THEN :Effective
                    WHEN lp.status=:Effective 
                        THEN :Expired
                    ELSE lp.status END
                )
                WHERE EXISTS 
                (
                    SELECT 1
                    FROM 
                    (
                        SELECT tlp.lineid
                        FROM tms_lineplan tlp
                        WHERE tlp.isdeleted=0
                            AND tlp.status=:Approved
                            AND tlp.EffectiveTime<=sysdate
                    ) a
                    WHERE a.lineid=lp.lineid 
                )
                    AND lp.isdeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="Effective",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Effective},
                new OracleParameter() { ParameterName="Expired",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Expired},
                new OracleParameter() { ParameterName="Approved",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Approved}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sqlStr, arguments);
        }

        public int SetIsEnabled(List<string> lineID, bool isEnabled)
        {
            if (lineID == null || lineID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            string strSql = string.Format(@"
                UPDATE TMS_LinePlan 
                SET 
                    IsEnabled={0}
                WHERE LineID=:LineID
                    AND IsDeleted=0
                    AND IsEnabled={1}"
                , isEnabled ? 1 : 0
                , isEnabled ? 0 : 1);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LineID",DbType= DbType.String,Value=lineID.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lineID.Count, arguments);
        }

        public bool GetLineIsEnabled(string lineID)
        {
            string strSql = @"
                SELECT IsEnabled
                FROM TMS_LinePlan
                WHERE IsDeleted=0
                    AND LineID=:LineID
                    AND rownum=1
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LineID",DbType= DbType.String,Value=lineID}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToInt16(o) == 1 ? true : false;
            }
            return true;
        }
        #endregion

        #region 修复线路编号

        public List<LinePlanLineIDRepairModel> GetAllValidLinePlan()
        {
            string strSql = @"
                SELECT L.LPID,L.DepartureID,L.ArrivalID,L.CarrierID,TransportType
                        ,L.LineGoodsType,L.BusinessType,L.LineID
                FROM TMS_LinePlan  L
                        JOIN TMS_CARRIER C ON C.CARRIERID = L.CarrierID
                WHERE L.IsDeleted=0";
            return (List<LinePlanLineIDRepairModel>)ExecuteSql_ByReaderReflect<LinePlanLineIDRepairModel>(TMSReadOnlyConnection, strSql);
        }

        public int RepairLineID(List<LinePlanLineIDRepairModel> lstModel)
        {
            if (lstModel == null || lstModel.Count == 0)
            {
                return 0;
            }
            string strSql = @"
                UPDATE TMS_LinePlan
                SET LineID=:LineID
                WHERE LPID =:LPID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LineID",DbType= DbType.String,Value=lstModel.Select(m=>m.LineID).ToArray()},
                new OracleParameter() { ParameterName="LPID",DbType= DbType.Int32,Value=lstModel.Select(m=>m.LPID).ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstModel.Count, arguments);
        }

        #endregion

        #region ILinePlanDAL 成员

        /// <summary>
        /// 取得需要生效的线路计划
        /// </summary>
        /// <returns></returns>
        public int? GetNeedEffectivedLinePlan()
        {
            String sql = @"
SELECT TP.LPID
FROM
(
          SELECT LPID
          FROM TMS_LINEPLAN
          WHERE STATUS = :STATUS
                AND EFFectiveTime <=  SYSDATE
                AND IsDeleted = 0
          ORDER BY EFFectiveTime ASC
)  TP
WHERE ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="STATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Approved}
            };
            object LPID = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (LPID != null)
            {
                return Convert.ToInt32(LPID);
            }
            return null;
        }

        /// <summary>
        /// 取得处于生效状态的线路计划列表
        /// </summary>
        /// <param name="LineID">线路ID</param>
        /// <returns></returns>
        public List<int> GetEffectivedLinePlan(string LineID)
        {
            String sql = @"
SELECT LPID
FROM TMS_LINEPLAN
WHERE STATUS = :STATUS
    AND LineID = :LineID
    AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="STATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Effective},
                new OracleParameter() { ParameterName="LineID", DbType = DbType.String, Value = LineID}
            };
            DataTable dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, arguments);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<int> listLPID = new List<int>();
                foreach (DataRow item in dtResult.Rows)
                {
                    listLPID.Add(Convert.ToInt32(item[0]));
                }
                return listLPID;
            }
            return null;
        }

        /// <summary>
        /// 设置为生效
        /// </summary>
        /// <param name="TPID">线路计划LPID</param>
        /// <returns></returns>
        public int UpdateToEffective(int LPID)
        {
            String sql = @"
UPDATE TMS_LINEPLAN
    SET STATUS = :STATUS
WHERE LPID = :LPID
    AND STATUS = :PRESTATUS
    AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="STATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Effective},
                new OracleParameter() { ParameterName="LPID", DbType = DbType.Int32, Value = LPID},
                 new OracleParameter() { ParameterName="PRESTATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Approved}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 设置为作废
        /// </summary>
        /// <param name="LPID"></param>
        /// <returns></returns>
        public int UpdateToExpired(int LPID)
        {
            String sql = @"
UPDATE TMS_LINEPLAN
    SET STATUS = :STATUS
WHERE LPID = :LPID
    AND STATUS = :PRESTATUS
    AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="STATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Expired},
                new OracleParameter() { ParameterName="LPID", DbType = DbType.Int32, Value = LPID},
                 new OracleParameter() { ParameterName="PRESTATUS",DbType= DbType.Int16,Value=(int)Enums.LineStatus.Effective}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region ILinePlanDAL 成员

        /// <summary>
        /// 取得该线路生效并且可用的线路计划列表
        /// </summary>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public List<LinePlanModel> GetEnabledUsefulLinePlan(string LineID)
        {
            if (String.IsNullOrWhiteSpace(LineID)) throw new ArgumentNullException("LineID IS NULL OR Empty");
            String sql = String.Format(@"
SELECT lp.LPID, lp.LineID ,lp.DepartureID, lp.ArrivalID, lp.TransportType ,lp.LineType, lp.LineGoodsType, lp.ExpressionType, lp.BusinessType, lp.Priority
FROM TMS_LinePlan lp
    JOIN TMS_Carrier carr ON carr.CarrierID = lp.CarrierID
WHERE lp.LineID = :LineID
AND lp.Status = {0}
AND lp.IsEnabled = 1
AND lp.IsDeleted = 0
AND carr.IsDeleted = 0
AND carr.Status = {1}
", (int)Enums.LineStatus.Effective
 , (int)Enums.CarrierStatus.Valid
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "LineID", DbType = DbType.String, Value = LineID }
            };
            var listResult = ExecuteSql_ByDataTableReflect<LinePlanModel>(TMSWriteConnection, sql, arguments);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion
    }
}

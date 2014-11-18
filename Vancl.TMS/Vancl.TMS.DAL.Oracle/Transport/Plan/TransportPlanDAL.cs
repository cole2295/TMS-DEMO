using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Plan;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.DAL.Oracle.Transport.Plan
{
    public class TransportPlanDAL : BaseDAL, ITransportPlanDAL
    {
        #region ITransportPlanDAL 成员

        public int Add(TransportPlanModel model)
        {
            string sql = @"
INSERT INTO TMS_TransportPlan(TPID, DEPARTUREID, ARRIVALID , LINEGOODSTYPE , ISTRANSIT, TRANSFERSTATIONMULTI, DEADLINE , CREATEBY, UPDATEBY, ISENABLED, EFFECTIVETIME, STATUS)
VALUES(:TPID, :DEPARTUREID, :ARRIVALID , :LINEGOODSTYPE , :ISTRANSIT, :TRANSFERSTATIONMULTI, :DEADLINE , :CREATEBY, :UPDATEBY, :ISENABLED, :EFFECTIVETIME, :STATUS)
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="TPID", DbType= DbType.Int32, Value=model.TPID},
                new OracleParameter(){ ParameterName="DEPARTUREID", DbType= DbType.Int32, Value= model.DepartureID},
                new OracleParameter(){ ParameterName="ARRIVALID", DbType= DbType.Int32,Value= model.ArrivalID},
                new OracleParameter(){ ParameterName="LINEGOODSTYPE", DbType= DbType.Int32, Value= (int)model.LineGoodsType},
                new OracleParameter(){ ParameterName="ISTRANSIT", DbType= DbType.Byte, Value= model.IsTransit},
                //new OracleParameter(){ ParameterName="TRANSFERSTATION", DbType= DbType.Int32, Value= model.IsTransit? model.TransferStation.Value : (int?)null},
                new OracleParameter(){ ParameterName="TRANSFERSTATIONMULTI", DbType= DbType.String, Value= model.IsTransit? model.TransferStationMulti : null},
                new OracleParameter(){ ParameterName="DEADLINE",DbType= DbType.DateTime, Value= model.Deadline},
                new OracleParameter(){ ParameterName="CREATEBY", DbType= DbType.Int32, Value=UserContext.CurrentUser.ID},
                new OracleParameter(){ ParameterName="UPDATEBY",DbType= DbType.Int32, Value=UserContext.CurrentUser.ID},
                new OracleParameter(){ ParameterName="ISENABLED", DbType = DbType.Byte, Value = model.IsEnabled},
                new OracleParameter(){ ParameterName="EFFECTIVETIME", DbType = DbType.DateTime, Value = model.EffectiveTime},
                new OracleParameter(){ ParameterName="STATUS", DbType= DbType.Int32, Value=(int)model.Status}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int Update(TransportPlanModel model)
        {
            string strSql = @"
                UPDATE TMS_TransportPlan
                SET DepartureID=:DepartureID
                    ,ArrivalID=:ArrivalID
                    ,LineGoodsType=:LineGoodsType
                    ,IsTransit=:IsTransit
                    ,TransferStation=:TransferStation
                    ,TransferStationMulti=:TransferStationMulti
                    ,Deadline=:Deadline
                    ,EffectiveTime = :EffectiveTime
                    ,Status = :Status
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                WHERE TPID=:TPID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=model.DepartureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=model.ArrivalID},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=(int)model.LineGoodsType},
                new OracleParameter() { ParameterName="IsTransit",DbType= DbType.Byte,Value=model.IsTransit},
                new OracleParameter() { ParameterName="TransferStation",DbType= DbType.Int32,Value=model.IsTransit? model.TransferStation : (int?)null},
                new OracleParameter() { ParameterName="TransferStationMulti",DbType= DbType.String,Value=model.IsTransit? model.TransferStationMulti : null},
                new OracleParameter() { ParameterName="Deadline",DbType= DbType.DateTime,Value=model.Deadline},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.String,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value=model.TPID},
                new OracleParameter() { ParameterName="EffectiveTime",DbType= DbType.DateTime,Value=model.EffectiveTime},
                new OracleParameter() { ParameterName="Status",DbType= DbType.Int32,Value=(int)model.Status}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public bool IsExistsTransportPlan(TransportPlanModel model)
        {
            string sql = @"
SELECT COUNT(*) AS CT
FROM TMS_TransportPlan
WHERE IsDeleted = 0
    AND DepartureID=:DepartureID
    AND ArrivalID=:ArrivalID
    AND (LineGoodsType+:LineGoodsType)-BITAND(LineGoodsType,:LineGoodsType)=LineGoodsType
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=(int)model.LineGoodsType},
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=model.DepartureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=model.ArrivalID}
            };
            int Intval = Convert.ToInt32(ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments));
            return Intval > 0;
        }

        public int Delete(List<int> lstTpid)
        {
            if (lstTpid == null || lstTpid.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_TransportPlan
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE TPID =:TPIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPIDs",DbType= DbType.Int32,Value=lstTpid.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstTpid.Count, arguments);
        }

        public PagedList<ViewTransportPlanModel> Search(TransportPlanSearchModel searchModel)
        {
            StringBuilder sb = new StringBuilder();
            List<OracleParameter> listArguments = new List<OracleParameter>();
            sb.Append(@"
                SELECT tp.TPID,ecd.CompanyName AS Departure,eca.CompanyName AS Arrival,City.CityName as ArrivalCity
                    , tp.LineGoodsType, tp.IsTransit, tp.Deadline,ect.CompanyName AS TransferStation
                    ,CASE WHEN tp.Deadline<sysdate THEN 2 ELSE tp.Status END AS Status
                    ,   CASE WHEN (1,1)=
                        (
                            SELECT MIN(lp.IsEnabled),MIN(c.Status)
                            FROM TMS_TransportPlanDetail tpd
                            JOIN TMS_LinePlan lp
                                ON lp.IsDeleted=0
                                    AND lp.LineID=tpd.LineID
                            JOIN TMS_Carrier c
                                ON c.IsDeleted=0
                                    AND c.CarrierID=lp.CarrierID
                            WHERE tpd.IsDeleted=0
                                AND tpd.TPID=tp.TPID
                        )
                            AND tp.IsEnabled=1 THEN 1
                        ELSE 0 END IsEnabled
                    ,tp.EffectiveTime,tp.CreateTime,tp.ArrivalID,tp.DepartureID,nvl(tp.TransferStationMulti,tp.TransferStation) TransferStationMulti
                FROM TMS_TransportPlan tp
                JOIN ExpressCompany ecd
                    ON tp.DepartureID=ecd.ExpressCompanyID
                JOIN ExpressCompany eca
                    ON tp.ArrivalID=eca.ExpressCompanyID
                JOIN City
                    ON City.CityID=eca.CityID
                LEFT JOIN ExpressCompany ect
                    ON tp.TransferStation=ect.ExpressCompanyID
                WHERE tp.IsDeleted=0");

            if (searchModel.Status.HasValue)
            {
                if (searchModel.Status.Value == Enums.TransportStatus.Expired)
                {
                    sb.Append(" AND tp.Deadline<sysdate");
                }
                else
                {
                    sb.Append(" AND tp.Status=:Status");
                    listArguments.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)searchModel.Status.Value });
                }
                if (searchModel.Status.Value == Enums.TransportStatus.Effective)
                {
                    sb.Append(" AND tp.Deadline>=sysdate");
                }
            }
            if (searchModel.DepartureID.HasValue)
            {
                sb.AppendFormat(" AND tp.DepartureID=:DepartureID");
                listArguments.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.ArrivalID.HasValue)
            {
                sb.AppendFormat(" AND tp.ArrivalID=:ArrivalID");
                listArguments.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (searchModel.Deadline.HasValue)
            {
                sb.AppendFormat(" AND tp.Deadline<=:Deadline");
                listArguments.Add(new OracleParameter() { ParameterName = "Deadline", DbType = DbType.DateTime, Value = searchModel.Deadline.Value });
            }
            searchModel.OrderByString = "DepartureID,ArrivalID,LineGoodsType,CreateTime";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewTransportPlanModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, listArguments.ToArray());
        }

        public TransportPlanModel Get(int tpid)
        {
            string strSql = @"
                SELECT TPID,DepartureID,ArrivalID,LineGoodsType,Deadline,TransferStation
                    ,IsTransit,UpdateBy,UpdateTime,CreateBy,CreateTime,IsDeleted,IsEnabled,EffectiveTime,Status
                FROM TMS_TransportPlan
                WHERE TPID=:TPID 
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value=tpid}
            };
            return ExecuteSqlSingle_ByDataTableReflect<TransportPlanModel>(TMSWriteConnection, strSql, arguments);
        }

        public List<int> GetTpids(int departureID, int arrivalID, Enums.GoodsType lineGoodsType)
        {
            string strSql = @"
                SELECT TPID 
                FROM TMS_TransportPlan
                WHERE DepartureID =:DepartureID
                    AND ArrivalID=:ArrivalID
                    AND (LineGoodsType+:LineGoodsType)-BITAND(LineGoodsType,:LineGoodsType)=:LineGoodsType
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=departureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=(int)lineGoodsType}
            };
            DataTable dt = ExecuteSqlDataTable(TMSReadOnlyConnection, strSql, arguments);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> lst = new List<int>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(Convert.ToInt32(dr["TPID"]));
                }
                return lst;
            }
            return null;
        }

        public ViewTansportEditorModel GetViewData(int tpid)
        {
            string strSql = @"SELECT tp.TPID,tp.DepartureID AS DepartureID,ecd.CompanyName AS DepartureName,tp.ArrivalID AS ArrivalID,eca.CompanyName AS ArrivalName,tp.TRANSFERSTATION AS TransitStationID,ect.CompanyName AS TransitStationName
                    , tp.LineGoodsType AS GoodsType,tp.Deadline AS DeadLine, tp.IsTransit AS IsTransifer,tp.EffectiveTime AS EffectiveTime,nvl(tp.TRANSFERSTATIONMUlTI,tp.TRANSFERSTATION) TransitStationMulti
                FROM TMS_TransportPlan tp
                LEFT JOIN ExpressCompany ecd
                    ON tp.DepartureID=ecd.ExpressCompanyID
                LEFT JOIN ExpressCompany eca
                    ON tp.ArrivalID=eca.ExpressCompanyID
                LEFT JOIN ExpressCompany ect
                    ON tp.TransferStation=ect.ExpressCompanyID
                WHERE TP.TPID=:TPID 
                    AND TP.IsDeleted=0";

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value=tpid}
            };

            return ExecuteSqlSingle_ByReaderReflect<ViewTansportEditorModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public IList<ViewLinePlanModel> GetLinePlanByTpid(int tpid)
        {
            string strSql = string.Format(@"
                SELECT lp.LPID,lp.LineID,lp.DepartureID,ecd.CompanyName DepartureName
                    ,lp.ArrivalID,eca.CompanyName ArrivalName,c.CarrierName,lp.TransportType
                    ,cityd.CityName DepartureCityName,citya.CityName ArrivalCityName,lp.ArrivalAssessmentTime
                    ,lp.LeaveAssessmentTime,lp.ArrivalTiming,lp.InsuranceRate
                    ,lp.LowestPrice,lp.LongDeliveryPrice,lp.LongPickPrice
                    ,lp.LongTransferRate,lp.Priority,lp.Status,lp.LineType
                    ,lp.LineGoodsType,lp.ExpressionType,lp.EffectiveTime
                FROM TMS_TransportPlanDetail tpd
                JOIN TMS_LinePlan lp
                    ON lp.IsDeleted=0
                        AND lp.Status={0}
                        AND lp.LineID=tpd.LineID
                JOIN ExpressCompany ecd
                    ON ecd.ExpressCompanyID=lp.DepartureID
                JOIN City cityd
                    ON cityd.CityID=ecd.CityID
                JOIN ExpressCompany eca
                    ON eca.ExpressCompanyID=lp.ArrivalID
                JOIN City citya
                    ON citya.CityID=eca.CityID
                JOIN TMS_Carrier c
                    ON c.CarrierID=lp.CarrierID
                WHERE tpd.IsDeleted=0
                    AND tpd.TPID=:TPID
            ", (int)Enums.LineStatus.Effective);

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value=tpid}
            };

            return ExecuteSql_ByReaderReflect<ViewLinePlanModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        /// <summary>
        /// 批量更新为停用状态
        /// </summary>
        /// <param name="listTPID">TPID列表</param>
        /// <returns></returns>
        public int BatchUpdateToDisabled(List<int> listTPID)
        {
            if (null == listTPID || listTPID.Count < 1) throw new ArgumentNullException("listTPID");
            string sql = @"
UPDATE TMS_TransportPlan
SET IsEnabled = 0
WHERE TPID=:TPID AND IsDeleted = 0 AND IsEnabled = 1
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value = listTPID.ToArray()}
            };

            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listTPID.Count, arguments);
        }

        /// <summary>
        /// 更新为停用状态
        /// </summary>
        /// <param name="TPID">TPID</param>
        /// <returns></returns>
        public int UpdateToDisabled(int TPID)
        {
            string sql = @"
UPDATE TMS_TransportPlan
SET IsEnabled = 0
WHERE TPID=:TPID AND IsDeleted = 0 AND IsEnabled = 1
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value = TPID}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 更新为启用状态
        /// </summary>
        /// <param name="TPID"></param>
        /// <returns></returns>
        public int UpdateToEnabled(int TPID)
        {
            string sql = @"
UPDATE TMS_TransportPlan
SET IsEnabled = 1
WHERE TPID=:TPID AND IsDeleted = 0 AND IsEnabled = 0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TPID",DbType= DbType.Int32,Value = TPID}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region ITransportPlanDAL 成员

        /// <summary>
        ///  根据出发地目的地和货物类型取得可以使用的运输计划主键TPID
        /// </summary>
        /// <param name="departureID">出发地</param>
        /// <param name="arrivalID">目的地</param>
        /// <param name="lineGoodsType">货物类型</param>
        /// <returns></returns>
        public List<int> GetUsefullyTPIDs(int departureID, int arrivalID, Enums.GoodsType lineGoodsType)
        {
            string strSql = String.Format(@"
                SELECT TPID 
                FROM TMS_TransportPlan
                WHERE DepartureID =:DepartureID
                    AND ArrivalID=:ArrivalID
                    --AND (LineGoodsType+:LineGoodsType)-BITAND(LineGoodsType,:LineGoodsType)=:LineGoodsType
                    AND LineGoodsType = :LineGoodsType
                    AND IsEnabled = 1 AND Status = {0} AND DeadLine > sysdate
                    AND IsDeleted=0",
                                    (int)Enums.TransportStatus.Effective);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=departureID},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=arrivalID},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=(int)lineGoodsType}
            };
            DataTable dt = ExecuteSqlDataTable(TMSWriteConnection, strSql, arguments);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> lst = new List<int>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(Convert.ToInt32(dr["TPID"]));
                }
                return lst;
            }
            return null;
        }

        /// <summary>
        /// 设置为生效
        /// </summary>
        /// <param name="TPID">运输计划TPID</param>
        /// <returns></returns>
        public int UpdateToEffective(int TPID)
        {
            string sql = String.Format(@"
UPDATE TMS_TRANSPORTPLAN
SET STATUS = {0}
WHERE TPID = :TPID
AND STATUS = {1} 
AND IsDeleted = 0 
",
(int)Enums.TransportStatus.Effective,
(int)Enums.TransportStatus.NotEffective
);
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="TPID", DbType = DbType.Int32, Value= TPID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 取得需要做生效处理的运输计划
        /// </summary>
        /// <returns></returns>
        public TransportPlanModel GetNeedEffectivedTPID()
        {
            string sql = String.Format(@"
SELECT  TPID,DepartureID,ArrivalID,LineGoodsType,Deadline,TransferStation
    ,IsTransit,UpdateBy,UpdateTime,CreateBy,CreateTime,IsDeleted,IsEnabled,EffectiveTime,Status
FROM TMS_TRANSPORTPLAN
WHERE STATUS = {0} AND EffectiveTime <= sysdate AND DeadLine > sysdate
AND IsDeleted = 0 
ORDER BY EffectiveTime ASC
",
(int)Enums.TransportStatus.NotEffective
);
            return ExecuteSqlSingle_ByDataTableReflect<TransportPlanModel>(TMSWriteConnection, sql);
        }

        #endregion

        #region ITransportPlanDAL 成员


        public LinePlanModel GetLinePlan(TransportPlanSearchModel condition)
        {
            if (!condition.ArrivalID.HasValue || !condition.CarrierID.HasValue || !condition.DepartureID.HasValue || !condition.GoodsType.HasValue)
            {
                throw new Exception("获取线路计划条件不足");
            }
            string sb = @"SELECT 
                                lp.lpid,
                                lp.lineid,
                                lp.departureid,
                                lp.arrivalid,
                                lp.carrierid,
                                c.carriername,
                                lp.priority,
                                lp.status,
                                lp.linegoodstype,
                                lp.ArrivalTiming,
                                lp.TransportType,
                                lp.InsuranceRate
                                FROM tms_transportplan TP
                                JOIN tms_transportplandetail TPD
                                     ON TP.TPID=TPD.TPID
                                JOIN TMS_LINEPLAN LP
                                     ON LP.LINEID=TPD.LINEID
                                JOIN tms_carrier C
                                     ON c.carrierid=lP.Carrierid
                                WHERE DeadLine>=sysdate 
                                    AND TP.ArrivalID=:ArrivalID
                                    AND TP.DepartureID=:DepartureID 
                                    AND TP.LineGoodsType=:LineGoodsType 
                                    AND TP.IsDeleted=0 
                                    AND TP.IsEnabled=1 
                                    AND TP.Status=:TransPlanStatus
                                    AND LP.Carrierid=:Carrierid
                                    AND LP.Priority=:Priority
                                    AND LP.ISENABLED=1
                                    AND LP.ISDELETED=0
                                    AND LP.STATUS=:LinePlanStatus
                                    AND c.Isdeleted=0
                                    AND LP.TransportType=:TransportType
                                    AND ROWNUM=1
                                    ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DepartureID",DbType= DbType.Int32,Value=condition.DepartureID.Value},
                new OracleParameter() { ParameterName="ArrivalID",DbType= DbType.Int32,Value=condition.ArrivalID.Value},
                new OracleParameter() { ParameterName="LineGoodsType",DbType= DbType.Int32,Value=Convert.ToInt32(condition.GoodsType.Value)},
                new OracleParameter() { ParameterName="Carrierid",DbType= DbType.Int32,Value=Convert.ToInt32(condition.CarrierID.Value)},
                new OracleParameter() { ParameterName="TransPlanStatus",DbType= DbType.Int32,Value=Convert.ToInt32(Enums.TransportStatus.Effective)},
                new OracleParameter() { ParameterName="LinePlanStatus",DbType= DbType.Int32,Value=Convert.ToInt32(Enums.LineStatus.Effective)},
                new OracleParameter() { ParameterName="Priority",DbType= DbType.Int32,Value=Convert.ToInt32(Enums.LinePriority.Priority)},
                new OracleParameter() { ParameterName="TransportType",DbType= DbType.Int32,Value=Convert.ToInt32(condition.TransportType.Value)}
            };

            return ExecuteSqlSingle_ByDataTableReflect<LinePlanModel>(TMSWriteConnection, sb, arguments);
        }

        #endregion

        #region ITransportPlanDAL 成员

        /// <summary>
        /// 根据出发地目的地取得生效并且可用的运输计划
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        public List<TransportPlanModel> GetEnabledUsefulTransportPlan(int DepartureID, int ArrivalID)
        {
            String sql = String.Format(@"
SELECT TPID, DepartureID, ArrivalID, LineGoodsType, TransferStation ,IsTransit
FROM TMS_TransportPlan
WHERE DepartureID = :DepartureID
    AND  ArrivalID = :ArrivalID
    AND Deadline >= SYSDATE
    AND IsEnabled = 1
    AND Status = {0}
    AND IsDeleted=0
",
 (int)Enums.TransportStatus.Effective
 );
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = DepartureID },
                 new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = ArrivalID }
            };
            var listResult = ExecuteSql_ByDataTableReflect<TransportPlanModel>(TMSWriteConnection, sql, parameters);
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion
    }
}

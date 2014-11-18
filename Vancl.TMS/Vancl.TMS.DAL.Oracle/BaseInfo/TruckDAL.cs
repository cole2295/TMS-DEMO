using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Truck;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    public class TruckDAL : BaseDAL, ITruckDAL
    {
        public PagedList<ViewTruckModel> GetTruckList(TruckSearchModel searchModel)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                          SELECT   tb.TBID,
                                   tb.TruckNo,
                                   tb.GPSNO,
                                   P.ProvinceName,
                                   C.CityName,
                                   d.DistributionName,
                                   tb.IsDeleted 
                            FROM   TruckBaseInfo tb 
                            join   Province p ON  p.ProvinceID = tb.ProvinceID
                            JOIN   City c  ON  c.CityID = tb.CityID
                            JOIN   Distribution d  ON  d.DistributionCode = tb.DistributionCode 
                            WHERE  c.ProvinceID = p.ProvinceID
                                   AND tb.CityID = c.CityID ");
            if (!string.IsNullOrEmpty(searchModel.TruckNO))
            {
                sb.Append(" and TruckNo=:TruckNo ");
                parameters.Add(new OracleParameter() { ParameterName = "TruckNo", DbType = DbType.String, Value = searchModel.TruckNO });
            }

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewTruckModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, parameters.ToArray());
        }

        public int Add(TruckBaseInfoModel model)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(@"
                          INSERT INTO TruckBaseInfo
                                 (TBID
                                 ,TruckNO
                                 ,GPSNO
                                 ,ProvinceID
                                 ,CityID
                                 ,DistributionCode
                                 ,CreateBy
                                 ,CreateTime
                                 ,UpdateBy
                                 ,UpdateTime
                                 ,IsDeleted)
                          VALUES (:TBID
                                 ,:TruckNO
                                 ,:GPSNO
                                 ,:ProvinceID
                                 ,:CityID
                                 ,:DistributionCode
                                 ,:CreateBy
                                 ,:CreateTime
                                 ,:UpdateBy
                                 ,:UpdateTime
                                 ,:IsDeleted)");

             OracleParameter[] parameters = new OracleParameter[]{ 
                  new OracleParameter(){ParameterName = "TBID" ,DbType = DbType.String , Value = model.TBID}
                  ,new OracleParameter(){ParameterName = "TruckNO" ,DbType = DbType.String , Value = model.TruckNO} 
                  ,new OracleParameter(){ParameterName = "GPSNO" ,DbType = DbType.String , Value = model.GPSNO}  
                  ,new OracleParameter(){ParameterName = "ProvinceID" ,DbType = DbType.String , Value = model.ProvinceID}
                  ,new OracleParameter(){ParameterName = "CityID" ,DbType = DbType.String , Value = model.CityID}
                  ,new OracleParameter(){ParameterName = "DistributionCode" ,DbType = DbType.String , Value = model.DistributionCode}
                  ,new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = model.CreateBy}
                  ,new OracleParameter(){ParameterName = "CreateTime" ,DbType = DbType.Date , Value = model.CreateTime}
                  ,new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = model.UpdateBy}
                  ,new OracleParameter(){ParameterName = "UpdateTime" ,DbType = DbType.Date , Value = model.UpdateTime}
                  ,new OracleParameter(){ParameterName = "IsDeleted" ,DbType = DbType.Int16 , Value = model.IsDeleted}
            };

             return ExecuteSqlNonQuery(TMSWriteConnection, sbSQL.ToString(), parameters);
        }

        public int Update(TruckBaseInfoModel model)
        {
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(@"
                          UPDATE TruckBaseInfo 
                             SET 
                                 TruckNO=:TruckNO,
                                 GPSNO=:GPSNO,
                                 ProvinceID=:ProvinceID,
                                 CityID=：CityID,
                                 DistributionCode=:DistributionCode,
                                 UpdateBy=:UpdateBy,
                                 UpdateTime=:UpdateTime,
                                 IsDeleted=:IsDeleted 
                          WHERE  TBID=:TBID");

            OracleParameter[] parameters = new OracleParameter[]{ 
                  new OracleParameter(){ParameterName = "TBID" ,DbType = DbType.String , Value = model.TBID}
                  ,new OracleParameter(){ParameterName = "TruckNO" ,DbType = DbType.String , Value = model.TruckNO} 
                  ,new OracleParameter(){ParameterName = "GPSNO" ,DbType = DbType.String , Value = model.GPSNO}  
                  ,new OracleParameter(){ParameterName = "ProvinceID" ,DbType = DbType.String , Value = model.ProvinceID}
                  ,new OracleParameter(){ParameterName = "CityID" ,DbType = DbType.String , Value = model.CityID}
                  ,new OracleParameter(){ParameterName = "DistributionCode" ,DbType = DbType.String , Value = model.DistributionCode}
                  ,new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = model.UpdateBy}
                  ,new OracleParameter(){ParameterName = "UpdateTime" ,DbType = DbType.Date , Value = model.UpdateTime}
                  ,new OracleParameter(){ParameterName = "IsDeleted" ,DbType = DbType.Int16 , Value = model.IsDeleted}
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sbSQL.ToString(), parameters);
        }

        public int SetDisabled(List<string> tbidList)
        {
            if (tbidList == null)
            {
                return 0;
            }
            string strSql = string.Format(@"
                                            UPDATE   TruckBaseInfo
                                               SET   
                                                     IsDeleted=1,
                                                     UpdateBy={0},
                                                     UpdateTime=sysdate 
                                             WHERE   TBID =:TBIDs"
                                            , UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="TBIDs",DbType= DbType.Int32,Value=tbidList.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, tbidList.Count, arguments);
        }


        public TruckBaseInfoModel GetTruckBaseInfo(string tbid)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                          SELECT   TBID,
                                   TruckNo,
                                   GPSNO,
                                   ProvinceID,
                                   CityID,
                                   DistributionCode,
                                   CreateBy,
                                   CreateTime,
                                   UpdateBy,
                                   UpdateTime,
                                   IsDeleted 
                            FROM   TruckBaseInfo 
                            WHERE  TBID=:TBID");

            OracleParameter[] parameters = new OracleParameter[]{ 
                  new OracleParameter(){ParameterName = "TBID" ,DbType = DbType.String , Value = tbid}
            };

            return ExecuteSqlSingle_ByDataTableReflect<TruckBaseInfoModel>(TMSReadOnlyConnection, sb.ToString(), parameters);
        }

        public IList<TruckBaseInfoModel> GetAll()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                          SELECT   TBID,
                                   TruckNo,
                                   GPSNO,
                                   ProvinceID,
                                   CityID,
                                   DistributionCode,
                                   CreateBy,
                                   CreateTime,
                                   UpdateBy,
                                   UpdateTime,
                                   IsDeleted 
                            FROM   TruckBaseInfo 
                            WHERE  IsDeleted = 0");

            return ExecuteSql_ByDataTableReflect<TruckBaseInfoModel>(TMSReadOnlyConnection, sb.ToString());
        }



    }
}

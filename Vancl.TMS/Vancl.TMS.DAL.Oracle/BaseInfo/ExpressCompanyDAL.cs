using System.Data;
using System.Linq;
using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.BaseInfo;
using System;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Model.Sorting.RangeDefined;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    class ExpressCompanyDAL : BaseDAL, IExpressCompanyDAL
    {
        #region IExpressCompanyDAL 成员

        /// <summary>
        /// 根据父节点获取子站点的所有数据
        /// </summary>
        /// <param name="parentId">父节点编号</param>
        /// <returns></returns>
        public IList<ExpressCompanyModel> GetChildExpressCompany(int parentId)
        {
            var sql = @"
                   SELECT   a.ExpressCompanyID as ID, 
                            a.CompanyName as Name,
                            a.SimpleSpell,
                            a.ParentID,
                            CASE WHEN NVL(b.c,0) > 0 THEN 1 ELSE 0 END AS HasChild   
                     FROM   ExpressCompany a
                LEFT JOIN  
                (
                   SELECT   COUNT(1) as c,
                            ParentID 
                     FROM   ExpressCompany ec
                 GROUP BY   ParentID	
                ) b
                ON  a.ExpressCompanyID = b.ParentID 
                WHERE a.ParentID = :ParentID AND a.IsDeleted = 0
                ORDER BY HasChild DESC";
            var parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "ParentID" , DbType = DbType.Int32 ,Value = parentId }
            };
            return ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, sql, parameters);
        }

        public ExpressCompanyModel Get(int expressCompanyId)
        {
            var sql = @"
                   SELECT   a.ExpressCompanyID as ID, 
                            a.CompanyName as Name,
                            a.SimpleSpell,
                            a.ParentID,
                            a.Email,
                            a.SiteNo,
							a.CompanyFlag,
                            CASE WHEN NVL(b.c,0) > 0 THEN 1 ELSE 0 END AS HasChild   
                     FROM   ExpressCompany a
                LEFT JOIN  
                (
                   SELECT   COUNT(1) as c,
                            ParentID 
                     FROM   ExpressCompany ec
                 GROUP BY   ParentID	
                ) b
                ON  a.ExpressCompanyID = b.ParentID 
                WHERE a.ExpressCompanyID = :ExpressCompanyID AND a.IsDeleted = 0
                ORDER BY HasChild DESC";
            var parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "ExpressCompanyID" , DbType = DbType.Int32 ,Value = expressCompanyId }
            };
            return ExecuteSqlSingle_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, sql, parameters);
        }
        /// <summary>
        /// 获取所有的部门名称(供自动完成使用)
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllCompanyNames()
        {
            var sql = @"
                   SELECT  DISTINCT (CompanyName) Name
                     FROM  ExpressCompany 
                    WHERE  IsDeleted = 0";
            var models = ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, sql);
            return (from m in models select m.Name).ToList();
        }

        /// <summary>
        /// 根据ID列表检索对象
        /// </summary>
        /// <param name="listExpressCompanyID"></param>
        /// <returns></returns>
        public IList<ExpressCompanyModel> Search(List<int> listExpressCompanyID)
        {
            if (null == listExpressCompanyID) throw new ArgumentNullException("listExpressCompanyID");
            if (listExpressCompanyID.Count <= 0) throw new Exception("ID参数列表为空");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < listExpressCompanyID.Count - 1; i++)
            {
                sb.Append(listExpressCompanyID[i].ToString()).Append(",");
            }
            sb.Append(listExpressCompanyID[listExpressCompanyID.Count - 1]);
            string sql = string.Format(@"
                    SELECT EXPRESSCOMPANYID AS ID
                    ,EXPRESSCOMPANYCODE AS CompanyCode
                    ,COMPANYNAME AS NAME
                    ,COMPANYALLNAME AS CompanyAllName
                    ,CompanyFlag
                    ,SimpleSpell
                    FROM ExpressCompany 
                    WHERE IsDeleted = 0 AND EXPRESSCOMPANYID IN ({0})
            ", sb.ToString());
            return ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, sql);
        }

        public int GetExpressCompanyIDByName(string expressCompanyName)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            var sql = @"SELECT EXPRESSCOMPANYID AS ID FROM ExpressCompany WHERE 1=1";
            if (!string.IsNullOrEmpty(expressCompanyName))
            {
                sql += " AND COMPANYNAME=:COMPANYNAME";
                parameters.Add(new OracleParameter() { ParameterName = "COMPANYNAME", DbType = DbType.String, Value = expressCompanyName });
            }
            return Convert.ToInt32(ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters.ToArray()));
        }

        /// <summary>
        /// 根据配送商编码取得分拣中心列表不包含登录人所在分拣中心
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetSortingCentersByDistributionCodeWithoutSelf()
        {
            string strSql = @"
                SELECT ec1.EXPRESSCOMPANYID AS ID
                    ,ec1.COMPANYNAME AS Name
                    ,ec1.COMPANYFLAG
                    ,ec1.DistributionCode
                FROM ExpressCompany ec1
                WHERE ec1.IsDeleted = 0
                    AND ec1.COMPANYFLAG=:COMPANYFLAG
                    AND ec1.DISTRIBUTIONCODE=:DISTRIBUTIONCODE
                    AND ec1.EXPRESSCOMPANYID<>:EXPRESSCOMPANYID
                    AND ec1.EXPRESSCOMPANYID<>(
                        SELECT ec2.PARENTID
                        FROM ExpressCompany ec2
                        WHERE ec2.EXPRESSCOMPANYID=:EXPRESSCOMPANYID
                    )
                ORDER BY ec1.COMPANYNAME";
            OracleParameter[] parameters = { 
                new OracleParameter(){ParameterName = "COMPANYFLAG" , DbType = DbType.Int32 ,Value = (int)Enums.CompanyFlag.SortingCenter },
                new OracleParameter(){ParameterName = "DISTRIBUTIONCODE" , DbType = DbType.String ,Value = UserContext.CurrentUser.DistributionCode },
                new OracleParameter(){ParameterName = "EXPRESSCOMPANYID" , DbType = DbType.Int32 ,Value = UserContext.CurrentUser.DeptID }
            };
            return (List<ExpressCompanyModel>)ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql, parameters);
        }

        /// <summary>
        /// 根据配送商编码取得分拣中心列表不包含登录人所在分拣中心
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetSortingCentersByDistributionCode()
        {
            string strSql = @"
                SELECT ec1.EXPRESSCOMPANYID AS ID
                    ,ec1.COMPANYNAME AS Name
                    ,ec1.COMPANYFLAG
                    ,ec1.DistributionCode
                FROM ExpressCompany ec1
                WHERE ec1.IsDeleted = 0
                    AND ec1.COMPANYFLAG=:COMPANYFLAG
                    AND ec1.DISTRIBUTIONCODE=:DISTRIBUTIONCODE                    
                ORDER BY ec1.COMPANYNAME";
            OracleParameter[] parameters = { 
                new OracleParameter(){ParameterName = "COMPANYFLAG" , DbType = DbType.Int32 ,Value = (int)Enums.CompanyFlag.SortingCenter },
                new OracleParameter(){ParameterName = "DISTRIBUTIONCODE" , DbType = DbType.String ,Value = UserContext.CurrentUser.DistributionCode }
            };
            return (List<ExpressCompanyModel>)ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql, parameters);
        }


	    public List<ExpressCompanyModel> GetDistributionCooprelation(string distributionCode)
	    {
			string strSql = @"select ex.expresscompanyid as ID,
							   ex.companyname as Name,
							   ex.companyflag as COMPANYFLAG,
							   dcr.coopdistributioncode as DistributionCode
						  from DISTRIBUTIONCOOPRELATION dcr
						  join Expresscompany ex
							on ex.distributioncode = dcr.coopdistributioncode
						 where dcr.IsDeleted = 0
						   and dcr.distributioncode = :DISTRIBUTIONCODE
						   and ex.companyflag = :COMPANYFLAG
						 order by ex.companyname";
			OracleParameter[] parameters = { 
                new OracleParameter(){ParameterName = "COMPANYFLAG" , DbType = DbType.Int32 ,Value = (int)Enums.CompanyFlag.Distributor },
                new OracleParameter(){ParameterName = "DISTRIBUTIONCODE" , DbType = DbType.String ,Value = distributionCode }
            };
			return (List<ExpressCompanyModel>)ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql, parameters);

	    }

	    /// <summary>
        /// 获得所有配送商
        /// </summary>
        /// <returns></returns>
        public List<ExpressCompanyModel> GetAllDistributors()
        {
            string strSql = @"
                SELECT EXPRESSCOMPANYID AS ID
                    ,COMPANYNAME AS Name
                    ,COMPANYFLAG
                    ,DistributionCode
                    ,Expresscompanycode
					,CompanyAllName
                FROM ExpressCompany
                WHERE COMPANYFLAG=:COMPANYFLAG
                    AND PARENTID=0
                ORDER BY COMPANYNAME";
            OracleParameter[] parameters = { 
                new OracleParameter(){ParameterName = "COMPANYFLAG" , DbType = DbType.Int32 ,Value = (int)Enums.CompanyFlag.Distributor }
            };
            return (List<ExpressCompanyModel>)ExecuteSql_ByReaderReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql, parameters);
        }

		public ExpressCompanyModel GetDistributor(string code)
		{
			string strSql = @"
                SELECT EXPRESSCOMPANYID AS ID
                    ,COMPANYNAME AS Name
                    ,COMPANYFLAG
                    ,DistributionCode
                    ,Expresscompanycode
					,CompanyAllName
                FROM ExpressCompany
                WHERE COMPANYFLAG=:COMPANYFLAG
					AND Distributioncode = :Distributioncode
                    AND PARENTID=0					 
                ORDER BY COMPANYNAME";
			OracleParameter[] parameters = { 
                new OracleParameter(){ParameterName = "COMPANYFLAG" , DbType = DbType.Int32 ,Value = (int)Enums.CompanyFlag.Distributor },
				new OracleParameter(){ParameterName = "Distributioncode" , DbType = DbType.String ,Value = code }
            };
			return ExecuteSqlSingle_ByDataTableReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql, parameters);
		}

        /// <summary>
        /// 依据部门ID获取部门Model
        /// </summary>
        /// <param name="expressCompanyID"></param>
        /// <returns></returns>
        public ExpressCompanyModel GetModel(int expressCompanyID)
        {
            var strSql = new StringBuilder();
            strSql.Append(
               @"SELECT	  ExpressCompanyCode CompanyCode,
                          CompanyAllName ,
                          SimpleSpell ,
                          ParentID ,
                          CompanyFlag ,
                          DistributionCode ,
                          MnemonicCode ,
                          MnemonicName ,
                          ParentSortingCenterId 
                   FROM   PS_PMS.ExpressCompany    ");
            strSql.Append(" where ROWNUM=1 and  ExpressCompanyID=:ExpressCompanyID  ");

            OracleParameter[] parameters = {
                 new OracleParameter(){ParameterName = "ExpressCompanyID" , DbType = DbType.Int32 ,Value = expressCompanyID }
             };
            return ExecuteSqlSingle_ByDataTableReflect<ExpressCompanyModel>(TMSReadOnlyConnection, strSql.ToString(), parameters);
        }
        #endregion

        #region IExpressCompanyDAL 成员

        /// <summary>
        /// 取得分拣中心下属站点
        /// </summary>
        /// <param name="SortCenterID">分拣中心ID</param>
        /// <returns></returns>
        public List<int> GetSortCenterStationList(int SortCenterID)
        {
            string sql = @"
SELECT ExpressCompanyID
FROM ExpressCompany
WHERE ParentID = :ParentID AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="ParentID",DbType = DbType.Int32, Value = SortCenterID}
            };
            DataTable dt = ExecuteSqlDataTable(TMSReadOnlyConnection, sql, arguments);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<int> listExpressCompanyID = new List<int>(dt.Rows.Count);
                foreach (DataRow item in dt.Rows)
                {
                    listExpressCompanyID.Add(int.Parse(item["ExpressCompanyID"].ToString()));
                }
                return listExpressCompanyID;
            }
            return null;
        }

        /// <summary>
        /// 取得联系电话
        /// </summary>
        /// <param name="ExpresscompanyID"></param>
        /// <returns></returns>
        public string GetContacterPhone(int ExpresscompanyID)
        {
            string sql = @"
SELECT ContacterPhone
FROM ExpressCompany
WHERE ExpressCompanyID = :ExpressCompanyID AND IsDeleted = 0
AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="ExpressCompanyID",DbType = DbType.Int32, Value = ExpresscompanyID}
            };
            object obj = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString();
            }
            return null;
        }

        #endregion



        #region IExpressCompanyDAL 成员
        /// <summary>
        /// 根据仓库编码取得物流系统出发地
        /// </summary>
        /// <param name="wmsCode"></param>
        /// <returns></returns>
        public int? GetDepartureByWMSCode(string wmsCode)
        {
            if (String.IsNullOrWhiteSpace(wmsCode)) throw new ArgumentNullException("wmsCode is null or empty");
            String sql = @"
SELECT SORTATIONID
FROM WAREHOUSESORTRELATION
WHERE WAREHOUSEID = :WAREHOUSEID
AND ISDELETED = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "WAREHOUSEID", DbType = DbType.String, Value = wmsCode }
            };
            var objValue = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return Convert.ToInt32(objValue);
            }
            return null;
        }

        /// <summary>
        /// 根据VJIA WMS提供的SCM目的地ID取得物流系统的目的地
        /// </summary>
        /// <param name="scmArrival"></param>
        /// <returns></returns>
        public int? GetArrivalByVJIASCMArrival(String scmArrival)
        {
            if (String.IsNullOrWhiteSpace(scmArrival)) throw new ArgumentNullException("scmArrival is null or empty");
            String sql = @"
SELECT ExpressCompanyID
FROM expresscompany
WHERE EXPRESSCOMPANYVJIAOLDID = :EXPRESSCOMPANYVJIAOLDID
AND ISDELETED = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "EXPRESSCOMPANYVJIAOLDID", DbType = DbType.Int32, Value = int.Parse(scmArrival) }
            };
            var objValue = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return Convert.ToInt32(objValue);
            }
            return null;
        }

        /// <summary>
        /// 根据WMS提供的SCM目的地ID取得物流系统的目的地
        /// </summary>
        /// <param name="scmArrival"></param>
        /// <returns></returns>
        public int? GetArrivalByVanclSCMArrival(string scmArrival)
        {
            if (String.IsNullOrWhiteSpace(scmArrival)) throw new ArgumentNullException("scmArrival is null or empty");
            String sql = @"
SELECT ExpressCompanyID
FROM expresscompany
WHERE EXPRESSCOMPANYOLDID = :EXPRESSCOMPANYOLDID
AND ISDELETED = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "EXPRESSCOMPANYOLDID", DbType = DbType.Int32, Value = int.Parse(scmArrival) }
            };
            var objValue = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return Convert.ToInt32(objValue);
            }
            return null;
        }

        #endregion

        #region 分拣范围定义

        public PagedList<ViewRangeDefinedSearchListModel> GetRangeDefineInfoList(RangeDefinedSearchModel searchModel)
        {
            StringBuilder strSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();
            //分拣范围是配送商是需要join Distribution表
            strSql.Append(@"select r.rangedefinedid, ex.companyname AS SortingCenter,ex1.companyname as RangeDefined
							from TMS_RangeDefined r							
							join Expresscompany ex on ex.expresscompanyid = r.baseexpresscompanyid
							join Expresscompany ex1 on ex1.expresscompanyid = r.rangedexpresscompanyid
							where r.isdeleted = 0 
							");

            //没有选择具体的分拣中心(根据配送商编码查询)
            if (searchModel.BaseExpressCompanyId == -1)
            {
                strSql.Append(@" AND r.BaseDistributioncode = :BaseDistributioncode");
                parameters.Add(new OracleParameter(":BaseDistributioncode", OracleDbType.NVarchar2, 20)
                {
                    Value = UserContext.CurrentUser.DistributionCode
                });
            }
            else
            {
                strSql.Append(@" AND r.baseexpresscompanyid = :baseexpresscompanyid");
                parameters.Add(new OracleParameter(":baseexpresscompanyid", OracleDbType.Int32, 20)
                {
                    Value = searchModel.BaseExpressCompanyId
                });

            }
            //没有选择具体的配送公司
            if (searchModel.RangedExpressCompanyIds == "-1" || searchModel.RangedExpressCompanyIds.Length == 0)
            {
                strSql.Append(@" AND r.rangedcompanyflag = :rangedcompanyflag ");
                parameters.Add(new OracleParameter(":rangedcompanyflag", OracleDbType.Int32, 1)
                {
                    Value = searchModel.CompanyFlag
                });
            }
            else
            {
                
               strSql.Append(@" And r.rangedexpresscompanyid in 
				(SELECT REGEXP_SUBSTR(:rangedexpresscompanyid, '[^,]+', 1, LEVEL) AS RangedExpressEompanyids
				FROM DUAL
				CONNECT BY LEVEL <=
				LENGTH(TRIM(TRANSLATE(:rangedexpresscompanyid,TRANSLATE(:rangedexpresscompanyid, ',', ' '), ' '))) + 1)");
                
                parameters.Add(new OracleParameter(string.Format(":{0}", "rangedexpresscompanyid"),
                                                       String.Join(",", searchModel.RangedExpressCompanyIds)));
            }
            strSql.Append(@" order by r.rangedefinedid DESC");
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewRangeDefinedSearchListModel>(TMSReadOnlyConnection, strSql.ToString(), searchModel, parameters.ToArray());

        }

	    /// <summary>
	    /// 获取已存在分拣范围定义的对象
	    /// </summary>
	    /// <param name="companyFlag"></param>
	    /// <param name="rangedExpressCompanyId"></param>
	    /// <returns></returns>
	    public ViewRangeDefinedSearchListModel GetExistInfoByStationId(int companyFlag, int rangedExpressCompanyId)
	    {
			StringBuilder strSql = new StringBuilder();
			List<OracleParameter> parameters = new List<OracleParameter>();
			//分拣范围是配送商是需要join Distribution表
			strSql.Append(@"select r.rangedefinedid, ex.companyname AS SortingCenter,ex1.companyname as RangeDefined
							from TMS_RangeDefined r							
							join Expresscompany ex on ex.expresscompanyid = r.baseexpresscompanyid
							join Expresscompany ex1 on ex1.expresscompanyid = r.rangedexpresscompanyid
							where r.isdeleted = 0 
							");
		    
			strSql.Append(@" And r.rangedexpresscompanyid = :RangedexprEsscompanyid");
		    
			parameters.Add(new OracleParameter(":RangedexprEsscompanyid", OracleDbType.Int32, 20)
			{
				Value = rangedExpressCompanyId
			});
			return ExecuteSqlSingle_ByReaderReflect<ViewRangeDefinedSearchListModel>(TMSReadOnlyConnection, strSql.ToString(), parameters.ToArray());
	    }

	    public bool IsExistsOfRangeDefined(RangeDefinedModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select count(1) from tms_rangedefined r 
								where r.isdeleted = 0 
									  --AND r.baseexpresscompanyid = :baseexpresscompanyid 
									  AND r.rangedexpresscompanyid = :rangedexpresscompanyid									  
							");
            List<OracleParameter> parameters = new List<OracleParameter>();
			//parameters.Add(new OracleParameter(":baseexpresscompanyid", OracleDbType.Int32, 20)
			//    {
			//        Value = model.BaseExpressCompanyId
			//    });
            parameters.Add(new OracleParameter(":rangedexpresscompanyid", OracleDbType.Int32, 20)
                {
                    Value = model.RangedExpressCompanyId
                });
            object value = ExecuteSqlScalar(TMSReadOnlyConnection, strSql.ToString(), parameters.ToArray());
            int nCount = Convert.ToInt32(value);
            return nCount > 0;
        }

        public int AddRangeDefined(RangeDefinedModel model)
        {

            string strSql = string.Format(@"INSERT INTO tms_rangedefined(
							   rangedefinedid,
							   baseexpresscompanyid,
							   basedistributioncode,
							   rangedexpresscompanyid,
							   rangedcompanyflag,
							   createby,							   
							   updateby
							   
						)VALUES(
							   {0},
							   :baseexpresscompanyid,
							   :basedistributioncode,
							   :rangedexpresscompanyid,
							   :rangedcompanyflag,
							   :createby,							   
							   :updateby							   
						)", model.SequenceNextValue());
            List<OracleParameter> parameters = new List<OracleParameter>();
            //model.SequenceNextValue()
            parameters.Add(new OracleParameter(":baseexpresscompanyid", OracleDbType.Int32, 20)
            {
                Value = model.BaseExpressCompanyId
            });
            parameters.Add(new OracleParameter(":basedistributioncode", OracleDbType.Varchar2, 100)
            {
                Value = UserContext.CurrentUser.DistributionCode
            });
            parameters.Add(new OracleParameter(":rangedexpresscompanyid", OracleDbType.Int32, 20)
            {
                Value = model.RangedExpressCompanyId
            });
            parameters.Add(new OracleParameter(":rangedcompanyflag", OracleDbType.Int32, 1)
            {
                Value = model.RangedCompanyFlag
            });
            parameters.Add(new OracleParameter(":createby", OracleDbType.Int32, 10)
            {
                Value = UserContext.CurrentUser.ID
            });
            parameters.Add(new OracleParameter(":updateby", OracleDbType.Int32, 10)
            {
                Value = UserContext.CurrentUser.ID
            });
            var result = ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters.ToArray());
            return result;
        }

        public int DeleteRangeDefined(List<int> rangeDefinedIds)
        {
            if (rangeDefinedIds.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"UPDATE tms_rangedefined
							SET IsDeleted=1
								,UpdateBy={0}
								,UpdateTime=sysdate
							WHERE rangedefinedid =:rangedefinedids
								AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] parameters = new OracleParameter[] {
                new OracleParameter() { ParameterName="rangedefinedids",DbType= DbType.Int32,Value=rangeDefinedIds.ToArray()}
            };

            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql.ToString(), rangeDefinedIds.Count, parameters);
        }

        public IList<RangeDefinedPrintExportModel> GetRangeDefinedPrintExportModel(IList<string> rangeIdList, int chooseType)
        {
            if (rangeIdList == null || rangeIdList.Count == 0) throw new ArgumentNullException("rangeIdList");
            StringBuilder sbSql = new StringBuilder();
            List<OracleParameter> parameters = new List<OracleParameter>();

            sbSql.Append(@"
						select  ROW_NUMBER()over(order by r.rangedefinedid DESC) as No ,
								ex.companyname AS SortingCenter,
								ex1.companyname as RangeDefined
						from TMS_RangeDefined r
						join Expresscompany ex on ex.expresscompanyid = r.baseexpresscompanyid
						join Expresscompany ex1 on ex1.expresscompanyid = r.rangedexpresscompanyid		
					");
           
            string rangeIds = string.Join(",", rangeIdList.Select(x => x));
            sbSql.AppendFormat(GetOracleInParameterWhereSql("r.RangeDefinedId", "RangeDefinedId", false, true, false));
            parameters.Add(new OracleParameter("RangeDefinedId", OracleDbType.Varchar2, 4000) { Value = rangeIds });
            return ExecuteSql_ByReaderReflect<RangeDefinedPrintExportModel>(TMSReadOnlyConnection, sbSql.ToString(),
                                                                            parameters.ToArray());
        }

        public RangeDefinedModel GetSortingCenterByStation(int station)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select BASEEXPRESSCOMPANYID,RANGEDCOMPANYFLAG 
                              from tms_rangedefined 
                             where isdeleted =0 
                               and RANGEDEXPRESSCOMPANYID = :station
							");
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":station", OracleDbType.Int32)
                {
                    Value = station
                });
            return ExecuteSqlSingle_ByReaderReflect<RangeDefinedModel>(TMSReadOnlyConnection, strSql.ToString(), parameters.ToArray());

        }

	    #endregion
    }
}

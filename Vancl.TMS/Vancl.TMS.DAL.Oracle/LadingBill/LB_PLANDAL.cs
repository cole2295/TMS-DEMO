using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.DbHelper;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.DAL.Oracle.LadingBill
{
    /// <summary>
    /// 数据访问类:LB_PLAN
    /// </summary>
    public partial class LB_PLANDAL : BaseDAL, ILB_PLANDAL
    {

        /// <summary>
        /// 把计划生成状态 改为0
        /// </summary>
        /// <returns></returns>
        public int UpPlanIsCreate()
        {
            const string strsql = "update lb_plan set iscreated=0";

            return ExecuteSqlNonQuery(TMSWriteConnection, strsql);
        }

        public IList<LB_PLAN> GetList()
        {
            string strsql = @"select id,
                                merchantid,
                                warehouseid,
                                fromdistributioncode,
                                department,
                                orderquantity,
                                predictweight,
                                mileage,
                                pricetype,
                                amount,
                                timetype,
                                week,
                                specifictime,
                                isdeleted,
                                isenabled,
                                receivemail,
                                createtime,
                                updatetime,
                                updateby,
                                createby,
                                creatstation,
                                todistributioncode,
                                iscreated
                              from lb_plan  where iscreated=0 and ISDELETED=0 and ISENABLED=0";

            return ExecuteSql_ByDataTableReflect<LB_PLAN>(TMSReadOnlyConnection, strsql);
        }

        public int Add(Vancl.TMS.Model.LadingBill.LB_PLAN lbPlan)
        {
            string strSql = string.Format(@"
            Insert Into PS_TMS.LB_PLAN (
            ID	,
            MERCHANTID	,
            WAREHOUSEID	,
            FROMDISTRIBUTIONCODE	,
            DEPARTMENT	,
            ORDERQUANTITY	,
            PREDICTWEIGHT	,
            MILEAGE	,
            PRICETYPE	,
            AMOUNT	,
            TIMETYPE	,
            WEEK	,
            SPECIFICTIME	,
            ISDELETED	,
            ISENABLED	,
            RECEIVEMAIL	,
            CREATETIME	,
            UPDATETIME	,
            UPDATEBY	,
            CREATEBY	,
            CREATSTATION	,
            TODISTRIBUTIONCODE	,
            ISCREATED)
            Values (
            {0},
            :MERCHANTID	,
            :WAREHOUSEID	,
            :FROMDISTRIBUTIONCODE	,
            :DEPARTMENT	,
            :ORDERQUANTITY	,
            :PREDICTWEIGHT	,
            :MILEAGE	,
            :PRICETYPE	,
            :AMOUNT	,
            :TIMETYPE	,
            :WEEK	,
            :SPECIFICTIME	,
            :ISDELETED	,
            :ISENABLED	,
            :RECEIVEMAIL	,
            :CREATETIME	,
            :UPDATETIME	,
            :UPDATEBY	,
            :CREATEBY	,
            :CREATSTATION	,
            :TODISTRIBUTIONCODE	,
            :ISCREATED )", lbPlan.SequenceNextValue());
            OracleParameter[] parameters =
                {
                    new OracleParameter(":MERCHANTID",  OracleDbType.Decimal, 22){Value =  lbPlan.MERCHANTID},
                    new OracleParameter(":WAREHOUSEID",  OracleDbType.Varchar2, 100){Value =  lbPlan.WAREHOUSEID},
                    new OracleParameter(":FROMDISTRIBUTIONCODE",  OracleDbType.Varchar2, 100){Value =  lbPlan.FROMDISTRIBUTIONCODE},
                    new OracleParameter(":DEPARTMENT",  OracleDbType.Varchar2, 100){Value =  lbPlan.DEPARTMENT},
                    new OracleParameter(":ORDERQUANTITY",  OracleDbType.Decimal, 22){Value =  lbPlan.ORDERQUANTITY},
                    new OracleParameter(":PREDICTWEIGHT",  OracleDbType.Decimal, 22){Value =  lbPlan.PREDICTWEIGHT},
                    new OracleParameter(":MILEAGE",  OracleDbType.Decimal, 22){Value =  lbPlan.MILEAGE},
                    new OracleParameter(":PRICETYPE",  OracleDbType.Decimal, 22){Value =  lbPlan.PRICETYPE},
                    new OracleParameter(":AMOUNT",  OracleDbType.Decimal, 22){Value =  lbPlan.AMOUNT},
                    new OracleParameter(":TIMETYPE",  OracleDbType.Decimal, 22){Value =  lbPlan.TIMETYPE},
                    new OracleParameter(":WEEK",  OracleDbType.Varchar2, 50){Value =  lbPlan.WEEK},
                    new OracleParameter(":SPECIFICTIME",  OracleDbType.Varchar2, 50){Value =  lbPlan.SPECIFICTIME},
                    new OracleParameter(":ISDELETED",  OracleDbType.Decimal, 22){Value =  lbPlan.IsDeleted},
                    new OracleParameter(":ISENABLED",  OracleDbType.Decimal, 22){Value =  lbPlan.ISENABLED},
                    new OracleParameter(":RECEIVEMAIL",  OracleDbType.Varchar2, 50){Value =  lbPlan.RECEIVEMAIL},
                    new OracleParameter(":CREATETIME",  OracleDbType.Date, 7){Value =  lbPlan.CreateTime},
                    new OracleParameter(":UPDATETIME",  OracleDbType.Date, 7){Value =  lbPlan.UpdateTime},
                    new OracleParameter(":UPDATEBY",  OracleDbType.Decimal, 22){Value =  lbPlan.UpdateBy},
                    new OracleParameter(":CREATEBY",  OracleDbType.Decimal, 22){Value =  lbPlan.CreateBy},
                    new OracleParameter(":CREATSTATION",  OracleDbType.Decimal, 22){Value =  lbPlan.CREATSTATION},
                    new OracleParameter(":TODISTRIBUTIONCODE",  OracleDbType.Varchar2, 100){Value =  lbPlan.TODISTRIBUTIONCODE},
                    new OracleParameter(":ISCREATED",  OracleDbType.Decimal, 22){Value =  lbPlan.ISCREATED},
                };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 查询提货计划明细
        /// </summary>
        /// <param name="planID">提货计划ID</param>
        /// <returns></returns>
        public LB_PLAN GetPlanDetails(decimal planID)
        {
            //sql
            #region
            const string strSql = @"SELECT 
	                                    ID,
	                                    MERCHANTID,
	                                    WAREHOUSEID,
	                                    FROMDISTRIBUTIONCODE,
	                                    DEPARTMENT,
	                                    ORDERQUANTITY,
	                                    PREDICTWEIGHT,
	                                    MILEAGE,
	                                    PRICETYPE,
	                                    AMOUNT,
	                                    TIMETYPE,
	                                    WEEK,
	                                    SPECIFICTIME,
	                                    ISDELETED,
	                                    ISENABLED,
	                                    RECEIVEMAIL,
	                                    CREATETIME,
	                                    UPDATETIME,
	                                    UPDATEBY,
	                                    CREATEBY,
	                                    CREATSTATION,
	                                    TODISTRIBUTIONCODE,
	                                    ISCREATED
                                    FROM PS_TMS.LB_PLAN
                                     where ID = :ID AND IsDeleted=0";
            #endregion

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ID", DbType = DbType.Int32, Value = planID } 
            };
            return ExecuteSqlSingle_ByReaderReflect<LB_PLAN>(TMSReadOnlyConnection, strSql, arguments);
        }

        public int Update(Vancl.TMS.Model.LadingBill.LB_PLAN lbPlan)
        {
            const string strSql = @"UPDATE PS_TMS.LB_PLAN
                                    SET MERCHANTID = :MERCHANTID,
	                                    WAREHOUSEID = :WAREHOUSEID,
	                                    FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE,
	                                    DEPARTMENT = :DEPARTMENT,
	                                    ORDERQUANTITY = :ORDERQUANTITY,
	                                    PREDICTWEIGHT = :PREDICTWEIGHT,
	                                    MILEAGE = :MILEAGE,
	                                    PRICETYPE = :PRICETYPE,
	                                    AMOUNT = :AMOUNT,
	                                    TIMETYPE = :TIMETYPE,
	                                    WEEK = :WEEK,
	                                    SPECIFICTIME = :SPECIFICTIME,
	                                    RECEIVEMAIL = :RECEIVEMAIL,
	                                    UPDATETIME = :UPDATETIME,
	                                    UPDATEBY = :UPDATEBY,
	                                    CREATSTATION = :CREATSTATION,
	                                    TODISTRIBUTIONCODE = :TODISTRIBUTIONCODE WHERE
                                    ID = :ID";
            OracleParameter[] parameters =
                {
                    new OracleParameter(":ID",  OracleDbType.Decimal, 22){Value =  lbPlan.ID},
                    new OracleParameter(":MERCHANTID",  OracleDbType.Decimal, 22){Value =  lbPlan.MERCHANTID},
                    new OracleParameter(":WAREHOUSEID",  OracleDbType.Varchar2, 100){Value =  lbPlan.WAREHOUSEID},
                    new OracleParameter(":FROMDISTRIBUTIONCODE",  OracleDbType.Varchar2, 100){Value =  lbPlan.FROMDISTRIBUTIONCODE},
                    new OracleParameter(":DEPARTMENT",  OracleDbType.Varchar2, 100){Value =  lbPlan.DEPARTMENT},
                    new OracleParameter(":ORDERQUANTITY",  OracleDbType.Decimal, 22){Value =  lbPlan.ORDERQUANTITY},
                    new OracleParameter(":PREDICTWEIGHT",  OracleDbType.Decimal, 22){Value =  lbPlan.PREDICTWEIGHT},
                    new OracleParameter(":MILEAGE",  OracleDbType.Decimal, 22){Value =  lbPlan.MILEAGE},
                    new OracleParameter(":PRICETYPE",  OracleDbType.Decimal, 22){Value =  lbPlan.PRICETYPE},
                    new OracleParameter(":AMOUNT",  OracleDbType.Decimal, 22){Value =  lbPlan.AMOUNT},
                    new OracleParameter(":TIMETYPE",  OracleDbType.Decimal, 22){Value =  lbPlan.TIMETYPE},
                    new OracleParameter(":WEEK",  OracleDbType.Varchar2, 50){Value =  lbPlan.WEEK},
                    new OracleParameter(":SPECIFICTIME",  OracleDbType.Varchar2, 50){Value =  lbPlan.SPECIFICTIME},
                    new OracleParameter(":RECEIVEMAIL",  OracleDbType.Varchar2, 50){Value =  lbPlan.RECEIVEMAIL},
                    new OracleParameter(":UPDATETIME",  OracleDbType.Date, 7){Value =  lbPlan.UpdateTime},
                    new OracleParameter(":UPDATEBY",  OracleDbType.Decimal, 22){Value =  lbPlan.UpdateBy},
                    new OracleParameter(":CREATSTATION",  OracleDbType.Decimal, 22){Value =  lbPlan.CREATSTATION},
                    new OracleParameter(":TODISTRIBUTIONCODE",  OracleDbType.Varchar2, 100){Value =  lbPlan.TODISTRIBUTIONCODE},
                };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 更新计划表是否已生成
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="iscreateCode">0没有生成，1已生成</param>
        /// <returns></returns>
        public int UpCreate(Decimal id, int iscreateCode)
        {
            string strSql = @"UPDATE PS_TMS.LB_PLAN
                                    SET ISCREATED=:ISCREATED WHERE
                                    ID = :ID";
            OracleParameter[] parameters =
                {
                    new OracleParameter(":ID", OracleDbType.Decimal, 22) {Value = id},
                    new OracleParameter(":ISCREATED", OracleDbType.Varchar2, 100) {Value = iscreateCode}
                };

            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public PagedList<LB_PLANDTO> GetPlanList(LB_PLANDTO searchModel)
        {
            List<OracleParameter> parms = new List<OracleParameter>();
            var sbSQL = new StringBuilder();
            sbSQL.AppendFormat(@"select 
                                   t.id,
                                   t.MERCHANTID,
                                   t.CREATETIME,
                                   m.MERCHANTNAME,
                                   t.WAREHOUSEID,
                                   w.WAREHOUSENAME,
                                   w.warehouseaddress              as WAREHOUSEADDRESS,
                                   t.fromdistributioncode,
                                   d2.distributionname    as FROMDISTRIBUTIONNAME,
                                   t.DEPARTMENT,
                                   t.ORDERQUANTITY,
                                   t.PREDICTWEIGHT,
                                   t.MILEAGE,
                                   t.PRICETYPE,
                                   t.AMOUNT,
                                   t.week                 as WEEKS,
                                   t.ORDERQUANTITY        as AMOUNTDESC,
                                   t.TIMETYPE,
                                   t.WEEK,
                                   t.SPECIFICTIME,
                                   t.isdeleted            as ISDELETEDNAME,
                                   t.ISENABLED,
                                   t.RECEIVEMAIL,
                                   t.CREATSTATION,
                                   t.todistributioncode,
                                   d1.distributionname    as TODISTRIBUTIONNAME,
                                   t.ISCREATED
                              from LB_PLAN t
                              join ps_pms.merchantwarehouse w
                                on w.warehousecode = t.warehouseid
                              join ps_pms.distribution d1
                                on d1.distributioncode = t.todistributioncode
                              join ps_pms.distribution d2
                                on d2.distributioncode = t.fromdistributioncode
                              join ps_pms.merchantbaseinfo m
                                on m.id = t.merchantid
                             where t.IsDeleted = 0");
            if (!string.IsNullOrEmpty(searchModel.FROMDISTRIBUTIONCODE))
            {
                sbSQL.Append(" AND t.FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE ");
                parms.Add(new OracleParameter() { ParameterName = "FROMDISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.FROMDISTRIBUTIONCODE });
            }
            if (searchModel.MERCHANTID > 0)
            {
                sbSQL.Append(" AND t.merchantid = :merchantid ");
                parms.Add(new OracleParameter() { ParameterName = "merchantid", DbType = DbType.Decimal, Value = Convert.ToDecimal(searchModel.MERCHANTID) });
            }
            if (!string.IsNullOrEmpty(searchModel.TODISTRIBUTIONCODE))
            {
                sbSQL.Append(" AND t.TODISTRIBUTIONCODE = :TODISTRIBUTIONCODE ");
                parms.Add(new OracleParameter() { ParameterName = "TODISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.TODISTRIBUTIONCODE });
            }
            if (searchModel.ISENABLED >= 0)
            {
                sbSQL.Append(" AND t.ISENABLED = :ISENABLED ");
                parms.Add(new OracleParameter() { ParameterName = "ISENABLED", DbType = DbType.Decimal, Value = Convert.ToDecimal(searchModel.ISENABLED) });
            }
            if (!string.IsNullOrEmpty(searchModel.WAREHOUSEID))
            {
                sbSQL.Append(" AND t.WAREHOUSEID = :WAREHOUSEID ");
                parms.Add(new OracleParameter() { ParameterName = "WAREHOUSEID", DbType = DbType.String, Value = searchModel.WAREHOUSEID });
            }
            if (!string.IsNullOrEmpty(searchModel.DEPARTMENT))
            {
                sbSQL.Append(" AND t.DEPARTMENT = :DEPARTMENT ");
                parms.Add(new OracleParameter() { ParameterName = "DEPARTMENT", DbType = DbType.String, Value = searchModel.DEPARTMENT });
            }
            //searchModel.OrderByString = "ID";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<LB_PLANDTO>(TMSWriteConnection, sbSQL.ToString(), searchModel, parms.ToArray());
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public int Delete(List<string> PlanID)
        {
            if (PlanID == null || PlanID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            string strSql = string.Format(@"UPDATE LB_PLAN SET IsDeleted=1 WHERE ID=:ID");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ID",DbType= DbType.String,Value=PlanID.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, PlanID.Count, arguments);
        }

        public Vancl.TMS.Model.LadingBill.LB_PLAN GetModel()
        {
            throw new NotImplementedException();
        }

        public Vancl.TMS.Model.LadingBill.LB_PLAN DataRowToModel(DataRow row)
        {
            throw new NotImplementedException();
        }

        public DataSet GetList(string strWhere)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 置为可用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public int SetIsEnabled(List<string> PlanID)
        {
            if (PlanID == null || PlanID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            string strSql = string.Format(@"UPDATE LB_PLAN SET ISENABLED=0 WHERE ID=:ID");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ID",DbType= DbType.String,Value=PlanID.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, PlanID.Count, arguments);
        }


        /// <summary>
        /// 置为不可用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public int SetIsDisabled(List<string> PlanID)
        {
            if (PlanID == null || PlanID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            string strSql = string.Format(@"UPDATE LB_PLAN SET ISENABLED=1 WHERE ID=:ID");
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ID",DbType= DbType.String,Value=PlanID.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, PlanID.Count, arguments);
        }
    }
}


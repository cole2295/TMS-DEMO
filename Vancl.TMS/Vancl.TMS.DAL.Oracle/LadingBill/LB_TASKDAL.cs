using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Oracle.DataAccess.Client;
using Vancl.TMS.DAL.Oracle;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Exceptions;

namespace Vancl.TMS.Model.LadingBill.OracleDAL
{
    /// <summary>
    /// 数据访问类:LB_TASK
    /// </summary>
    public class LB_TASKDAL : BaseDAL, ILB_TASKDAL
    {
        public LB_TASKDAL()
        { }

        public DateTime GetDBTime()
        {
            string sql = "select sysdate from dual";
            return Convert.ToDateTime(ExecuteSqlScalar(TMSReadOnlyConnection, sql));
        }

        #region 查询任务

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public Util.Pager.PagedList<TaskViewModel> GetTaskPage(TaskSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException("LinePlanSearchModel");
            }
            StringBuilder sb = new StringBuilder();
            #region SQL语句
            sb.Append(@"select
                                    t.id, 
                                    t.mileage,
                                    t.taskcode,
                                    t.pickgoodsamount,
                                    t.receiveemail,
                                    t.todistributioncode,
                                    t.orderquantity,
                                    t.PICKPRICETYPE,
                                    t.ONCEAMOUNT,
                                    t.ORDERAMOUNT,
                                    t.weight,
                                    t.PICKMAN,
                                    t.TASKTIME,
                                    t.FINISHTIME,
                                    t.CREATETIME,
                                    t.taskstatus,
                                    t.department,
                                    t.predictorderquantity,
                                    t.predictweight,
                                    t.PREDICTTIME,
                                    mer.merchantname,
                                    m.warehouseaddress,
                                    m.warehousename,
                                    fromdis.distributionname fromdistributionname,
                                    dis.distributionname
                                from ps_tms.lb_task t
                                join ps_pms.MERCHANTWAREHOUSE m
                                on m.warehousecode = t.warehouseid
                                join ps_pms.merchantbaseinfo mer
                                on t.merchantid = mer.id
                                join ps_pms.DISTRIBUTION dis
                                on dis.distributioncode = t.todistributioncode
                                join ps_pms.DISTRIBUTION fromdis
                                on fromdis.distributioncode=t.fromdistributioncode
                                where t.isdeleted = 0");
            #endregion

            List<OracleParameter> parameters = new List<OracleParameter>();


            #region 条件
            if (!string.IsNullOrEmpty(searchModel.FROMDISTRIBUTIONCODE))
            {
                sb.Append(" AND t.FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE");
                parameters.Add(new OracleParameter() { ParameterName = "FROMDISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.FROMDISTRIBUTIONCODE });
            }
            if (!string.IsNullOrEmpty(searchModel.DEPARTMENT))
            {
                sb.Append(" and t.DEPARTMENT like '%" + searchModel.DEPARTMENT + "%'");
                //parameters.Add(new OracleParameter() { ParameterName = "DEPARTMENT", DbType = DbType.String, Value = searchModel.DEPARTMENT });
            }
            if (searchModel.timeType == 0)
            {
                if (searchModel.endTime != null && searchModel.endTime.Year != 1)
                {
                    sb.Append(" AND t.predicttime <= :EndTime");
                    parameters.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.endTime });
                }

                if (searchModel.starTime != null && searchModel.starTime.Year != 1)
                {
                    sb.Append(" AND t.predicttime >= :StartTime");
                    parameters.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.starTime });
                }
            }
            else
            {
                if (searchModel.endTime != null && searchModel.endTime.Year != 1)
                {
                    sb.Append(" AND t.FINISHTIME <= :EndTime");
                    parameters.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.endTime });
                }

                if (searchModel.starTime != null && searchModel.starTime.Year != 1)
                {
                    sb.Append(" AND t.FINISHTIME >= :StartTime");
                    parameters.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.starTime });
                }
            }


            if (searchModel.merchantid != null)
            {
                sb.Append(" AND t.merchantid = :merchantid");
                parameters.Add(new OracleParameter() { ParameterName = "merchantid", DbType = DbType.Int32, Value = searchModel.merchantid });
            }
            if (searchModel.taskCode != null)
            {
                sb.Append(" AND t.taskCode = :taskCode");
                parameters.Add(new OracleParameter() { ParameterName = "taskCode", DbType = DbType.String, Value = searchModel.taskCode });
            }
            if (searchModel.taskStatus > 0)
            {
                sb.Append(" AND t.taskStatus = :taskStatus");
                parameters.Add(new OracleParameter() { ParameterName = "taskStatus", DbType = DbType.Int32, Value = searchModel.taskStatus });
            }
            if (searchModel.todisribution != null)
            {
                sb.Append(" AND t.todistributioncode = :todisribution");
                parameters.Add(new OracleParameter() { ParameterName = "todisribution", DbType = DbType.String, Value = searchModel.todisribution });
            }
            if (searchModel.warehouseid != null && searchModel.warehouseid != "0")
            {
                sb.Append(" AND t.warehouseid = :warehouseid");
                parameters.Add(new OracleParameter() { ParameterName = "warehouseid", DbType = DbType.String, Value = searchModel.warehouseid });
            }
            #endregion

            searchModel.OrderByString = "CREATETIME  desc";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<TaskViewModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, parameters.ToArray()); ;
        }

        /// <summary>
        /// 获得TaskViewModel 实体
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public TaskViewModel GetTaskModel(TaskSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException("LinePlanSearchModel");
            }
            StringBuilder sb = new StringBuilder();
            #region SQL语句
            sb.Append(@"select
                                    t.id, 
                                    t.mileage,
                                    t.taskcode,
                                    t.pickgoodsamount,
                                    t.receiveemail,
                                    t.todistributioncode,
                                    t.orderquantity,
                                    t.PICKPRICETYPE,
                                    t.ONCEAMOUNT,
                                    t.ORDERAMOUNT,
                                    t.weight,
                                    t.PICKMAN,
                                    t.TASKTIME,
                                    t.FINISHTIME,
                                    t.CREATETIME,
                                    t.taskstatus,
                                    t.department,
                                    t.predictorderquantity,
                                    t.predictweight,
                                    t.PREDICTTIME,
                                    mer.merchantname,
                                    m.warehouseaddress,
                                    m.warehousename,
                                    fromdis.distributionname fromdistributionname,
                                    dis.distributionname
                                from ps_tms.lb_task t
                                join ps_pms.MERCHANTWAREHOUSE m
                                on m.warehousecode = t.warehouseid
                                join ps_pms.merchantbaseinfo mer
                                on t.merchantid = mer.id
                                join ps_pms.DISTRIBUTION dis
                                on dis.distributioncode = t.todistributioncode
                                join ps_pms.DISTRIBUTION fromdis
                                on fromdis.distributioncode=t.fromdistributioncode
                                where t.isdeleted = 0");
            #endregion

            List<OracleParameter> parameters = new List<OracleParameter>();


            #region 条件

            if (searchModel.todisribution != null)
            {
                sb.Append(" AND t.FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE");
                parameters.Add(new OracleParameter() { ParameterName = "FROMDISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.FROMDISTRIBUTIONCODE });
            }
            if (searchModel.FROMDISTRIBUTIONCODE != null)
            {
                sb.Append(" AND t.FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE");
                parameters.Add(new OracleParameter() { ParameterName = "FROMDISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.FROMDISTRIBUTIONCODE });
            }

            #endregion

            searchModel.OrderByString = "CREATETIME  desc";
            var data = ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<TaskViewModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, parameters.ToArray());
            if (data.Count > 0)
            {
                return data[0];
            }
            else
            {
                TaskViewModel taskView = new TaskViewModel();
                return taskView;
            }

        }

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public IList<TaskViewModel> GetTaskPage(string taskIDS)
        {
            string strsql = string.Format(@"select
                                    t.id, 
                                    t.mileage,
                                    t.taskcode,
                                    t.pickgoodsamount,
                                    t.receiveemail,
                                    t.todistributioncode,
                                    t.orderquantity,
                                    t.weight,
                                    t.PICKMAN,
                                    t.TASKTIME,
                                    t.FINISHTIME,
                                    t.CREATETIME,
                                    t.taskstatus,
                                    t.department,
                                    t.predictorderquantity,
                                    t.predictweight,
                                    t.PREDICTTIME,
                                    mer.merchantname,
                                    m.warehouseaddress,
                                    m.warehousename,
                                    fromdis.distributionname fromdistributionname,
                                    dis.distributionname
                                from ps_tms.lb_task t
                                join ps_pms.MERCHANTWAREHOUSE m
                                on m.warehousecode = t.warehouseid
                                join ps_pms.merchantbaseinfo mer
                                on t.merchantid = mer.id
                                join ps_pms.DISTRIBUTION dis
                                on dis.distributioncode = t.todistributioncode
                                join ps_pms.DISTRIBUTION fromdis
                                on fromdis.distributioncode=t.fromdistributioncode
                                where t.isdeleted = 0 and t.id in({0})", taskIDS);

            List<OracleParameter> parameters = new List<OracleParameter>();


            var result = ExecuteSql_ByReaderReflect<TaskViewModel>(TMSReadOnlyConnection, strsql, parameters.ToArray());
            return result;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public IList<TaskViewModel> GetTaskExport(TaskSearchModel searchModel)
        { 
            if (searchModel == null)
            {
                throw new ArgumentNullException("LinePlanSearchModel");
            }
            StringBuilder sb = new StringBuilder();
            #region SQL语句
            sb.Append(@"select
                                    t.id, 
                                    t.mileage,
                                    t.taskcode,
                                    t.pickgoodsamount,
                                    t.receiveemail,
                                    t.todistributioncode,
                                    t.orderquantity,
                                    t.PICKPRICETYPE,
                                    t.ONCEAMOUNT,
                                    t.ORDERAMOUNT,
                                    t.weight,
                                    t.PICKMAN,
                                    t.TASKTIME,
                                    t.FINISHTIME,
                                    t.CREATETIME,
                                    t.taskstatus,
                                    t.department,
                                    t.predictorderquantity,
                                    t.predictweight,
                                    t.PREDICTTIME,
                                    mer.merchantname,
                                    m.warehouseaddress,
                                    m.warehousename,
                                    fromdis.distributionname fromdistributionname,
                                    dis.distributionname
                                from ps_tms.lb_task t
                                join ps_pms.MERCHANTWAREHOUSE m
                                on m.warehousecode = t.warehouseid
                                join ps_pms.merchantbaseinfo mer
                                on t.merchantid = mer.id
                                join ps_pms.DISTRIBUTION dis
                                on dis.distributioncode = t.todistributioncode
                                join ps_pms.DISTRIBUTION fromdis
                                on fromdis.distributioncode=t.fromdistributioncode
                                where t.isdeleted = 0");
            #endregion

            List<OracleParameter> parameters = new List<OracleParameter>();


            #region 条件
            if (!string.IsNullOrEmpty(searchModel.FROMDISTRIBUTIONCODE))
            {
                sb.Append(" AND t.FROMDISTRIBUTIONCODE = :FROMDISTRIBUTIONCODE");
                parameters.Add(new OracleParameter() { ParameterName = "FROMDISTRIBUTIONCODE", DbType = DbType.String, Value = searchModel.FROMDISTRIBUTIONCODE });
            }
            if (!string.IsNullOrEmpty(searchModel.DEPARTMENT))
            {
                sb.Append(" and t.DEPARTMENT like '%" + searchModel.DEPARTMENT + "%'");
                //parameters.Add(new OracleParameter() { ParameterName = "DEPARTMENT", DbType = DbType.String, Value = searchModel.DEPARTMENT });
            }
            if (searchModel.timeType == 0)
            {
                if (searchModel.endTime != null && searchModel.endTime.Year != 1)
                {
                    sb.Append(" AND t.predicttime <= :EndTime");
                    parameters.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.endTime });
                }

                if (searchModel.starTime != null && searchModel.starTime.Year != 1)
                {
                    sb.Append(" AND t.predicttime >= :StartTime");
                    parameters.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.starTime });
                }
            }
            else
            {
                if (searchModel.endTime != null && searchModel.endTime.Year != 1)
                {
                    sb.Append(" AND t.FINISHTIME <= :EndTime");
                    parameters.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.endTime });
                }

                if (searchModel.starTime != null && searchModel.starTime.Year != 1)
                {
                    sb.Append(" AND t.FINISHTIME >= :StartTime");
                    parameters.Add(new OracleParameter() { ParameterName = "StartTime", DbType = DbType.DateTime, Value = searchModel.starTime });
                }
            }


            if (searchModel.merchantid != null)
            {
                sb.Append(" AND t.merchantid = :merchantid");
                parameters.Add(new OracleParameter() { ParameterName = "merchantid", DbType = DbType.Int32, Value = searchModel.merchantid });
            }
            if (searchModel.taskCode != null)
            {
                sb.Append(" AND t.taskCode = :taskCode");
                parameters.Add(new OracleParameter() { ParameterName = "taskCode", DbType = DbType.String, Value = searchModel.taskCode });
            }
            if (searchModel.taskStatus > 0)
            {
                sb.Append(" AND t.taskStatus = :taskStatus");
                parameters.Add(new OracleParameter() { ParameterName = "taskStatus", DbType = DbType.Int32, Value = searchModel.taskStatus });
            }
            if (searchModel.todisribution != null)
            {
                sb.Append(" AND t.todistributioncode = :todisribution");
                parameters.Add(new OracleParameter() { ParameterName = "todisribution", DbType = DbType.String, Value = searchModel.todisribution });
            }
            if (searchModel.warehouseid != null && searchModel.warehouseid != "0")
            {
                sb.Append(" AND t.warehouseid = :warehouseid");
                parameters.Add(new OracleParameter() { ParameterName = "warehouseid", DbType = DbType.String, Value = searchModel.warehouseid });
            }
            #endregion

            searchModel.OrderByString = "CREATETIME  desc";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<TaskViewModel>(TMSReadOnlyConnection, sb.ToString(), searchModel, parameters.ToArray()); ;
        } 

        #endregion

        public bool Exists(decimal ID)
        {
            throw new NotImplementedException();
        }

        public int ToDayTaskCount()
        {
            return Convert.ToInt32(ExecuteSqlScalar(TMSWriteConnection, "select COUNT(*) from PS_TMS.lb_task  where to_char(createtime,'dd')=to_char(sysdate,'dd')")) + 1;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="model">任务实体</param>
        /// <returns></returns>
        public bool Add(LadingBill.LB_TASK model)
        {

            if (model != null)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into LB_TASK(");
                strSql.Append(
                    "ID,TASKCODE,MERCHANTID,WAREHOUSEID,FROMDISTRIBUTIONCODE,TODISTRIBUTIONCODE,DEPARTMENT,PREDICTTIME,MILEAGE,PICKPRICETYPE,ONCEAMOUNT,ORDERAMOUNT,TASKSTATUS,CREATETIME,UPDATETIME,RECEIVEEMAIL,RECEIVEEMAILTIME,PICKGOODSAMOUNT,FINISHTIME,PREDICTORDERQUANTITY,PREDICTWEIGHT,KPIAMOUNT,REMARK)");
                strSql.Append(" values (");
                strSql.Append("" + model.SequenceNextValue() + ",:TASKCODE,:MERCHANTID,:WAREHOUSEID,:FROMDISTRIBUTIONCODE,:TODISTRIBUTIONCODE,:DEPARTMENT,:PREDICTTIME,:MILEAGE,:PICKPRICETYPE,:ONCEAMOUNT,:ORDERAMOUNT,:TASKSTATUS,:CREATETIME,:UPDATETIME,:RECEIVEEMAIL,:RECEIVEEMAILTIME,:PICKGOODSAMOUNT,:FINISHTIME,:PREDICTORDERQUANTITY,:PREDICTWEIGHT,:KPIAMOUNT,:REMARK)");
                #region OracleParameter 参数
                OracleParameter[] parameters =
                    {
                        new OracleParameter(":TASKCODE", OracleDbType.Varchar2, 100),
                        new OracleParameter(":MERCHANTID", OracleDbType.Int32, 100),
                        new OracleParameter(":WAREHOUSEID", OracleDbType.Varchar2, 100),
                        new OracleParameter(":FROMDISTRIBUTIONCODE", OracleDbType.Varchar2, 100),
                        new OracleParameter(":TODISTRIBUTIONCODE", OracleDbType.Varchar2, 100),
                        new OracleParameter(":DEPARTMENT", OracleDbType.Varchar2, 50),
                        new OracleParameter(":PREDICTTIME", OracleDbType.Date),
                        new OracleParameter(":MILEAGE", OracleDbType.Decimal, 50),
                        new OracleParameter(":PICKPRICETYPE", OracleDbType.Int32, 9),
                        new OracleParameter(":ONCEAMOUNT", OracleDbType.Decimal, 22),
                        new OracleParameter(":ORDERAMOUNT", OracleDbType.Decimal, 22),
                        new OracleParameter(":TASKSTATUS", OracleDbType.Int32, 22),
                        new OracleParameter(":CREATETIME", OracleDbType.Date),
                        new OracleParameter(":UPDATETIME", OracleDbType.Date),
                        new OracleParameter(":RECEIVEEMAIL", OracleDbType.Varchar2, 50),
                        new OracleParameter(":RECEIVEEMAILTIME", OracleDbType.Date),
                        new OracleParameter(":PICKGOODSAMOUNT", OracleDbType.Decimal, 22),
                        new OracleParameter(":FINISHTIME", OracleDbType.Date),
                        new OracleParameter(":PREDICTORDERQUANTITY", OracleDbType.Int32, 22),
                        new OracleParameter(":PREDICTWEIGHT", OracleDbType.Decimal, 22),
                        new OracleParameter(":KPIAMOUNT", OracleDbType.Varchar2, 50),
                        new OracleParameter(":REMARK", OracleDbType.Varchar2, 400)
                    };

                parameters[0].Value = model.TASKCODE;
                parameters[1].Value = model.MERCHANTID;
                parameters[2].Value = model.WAREHOUSEID;
                parameters[3].Value = model.FROMDISTRIBUTIONCODE;
                parameters[4].Value = model.TODISTRIBUTIONCODE;
                parameters[5].Value = model.DEPARTMENT;
                parameters[6].Value = model.PREDICTTIME;
                parameters[7].Value = model.MILEAGE;
                parameters[8].Value = model.PICKPRICETYPE;
                parameters[9].Value = model.ONCEAMOUNT;
                parameters[10].Value = model.ORDERAMOUNT;
                parameters[11].Value = model.TASKSTATUS;
                parameters[12].Value = model.CREATETIME;
                parameters[13].Value = model.UPDATETIME;
                parameters[14].Value = model.RECEIVEEMAIL;
                parameters[15].Value = model.RECEIVEEMAILTIME;
                parameters[16].Value = model.PICKGOODSAMOUNT;
                parameters[17].Value = model.FINISHTIME;
                parameters[18].Value = model.PREDICTORDERQUANTITY;
                parameters[19].Value = model.PREDICTWEIGHT;
                parameters[20].Value = model.KPIAMOUNT;
                parameters[21].Value = model.REMARK;
                #endregion
                int result = ExecuteSqlNonQuery(TMSWriteConnection, strSql.ToString(), parameters);

                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 取消，恢复
        /// </summary>
        /// <param name="taskid">任务ID</param>
        /// <param name="taskStatus">状态id</param>
        /// <returns></returns>
        public bool SetIsEnabled(List<string> taskid, int taskStatus)
        {
            string strsql = string.Format("update ps_tms.LB_TASK set TASKSTATUS ={0} WHERE ID=:ID", taskStatus);

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ID",DbType= DbType.String,Value=taskid.ToArray()}
            };

            if (ExecuteSqlArrayNonQuery(TMSWriteConnection, strsql, taskid.Count, arguments) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IList<LB_TASK> GeTasks(List<string> taskid)
        {
            string idlist = "";
            foreach (var VARIABLE in taskid)
            {
                idlist += VARIABLE + ",";
            }
            idlist = idlist.Remove(idlist.LastIndexOf(","), 1);

            string strsql =
                string.Format(
                    @"select t.*, m.warehousename, mer.merchantname, dis.distributionname,m.WAREHOUSEADDRESS     
                                 from LB_TASK t
                                 join ps_pms.MERCHANTWAREHOUSE m
                                   on t.warehouseid = m.warehousecode
                                 join ps_pms.merchantbaseinfo mer
                                   on t.merchantid = mer.id
                                 join ps_pms.DISTRIBUTION dis
                                   on t.todistributioncode = dis.distributioncode  where t.ID in ({0})", idlist);




            return ExecuteSql_ByDataTableReflect<LB_TASK>(TMSReadOnlyConnection, strsql);
        }


        /// <summary>
        /// 更新 LB_TASK  根据ID （主键）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(LadingBill.LB_TASK model)
        {
            string strsql = string.Format(@"update ps_tms.LB_TASK set 
                                               TODISTRIBUTIONCODE=:TODISTRIBUTIONCODE,
                                               DEPARTMENT=:DEPARTMENT,
                                               RECEIVEEMAIL=:RECEIVEEMAIL
                                                    where ID={0}                                                            
                                         ", model.ID);

            OracleParameter[] parameters = {
					new OracleParameter(":TODISTRIBUTIONCODE", OracleDbType.Varchar2,100),
                    	new OracleParameter(":DEPARTMENT", OracleDbType.Varchar2,50),
                        new OracleParameter(":RECEIVEEMAIL", OracleDbType.Varchar2,50)
					};
            parameters[0].Value = model.TODISTRIBUTIONCODE;
            parameters[1].Value = model.DEPARTMENT;
            parameters[2].Value = model.RECEIVEEMAIL;


            if (ExecuteSqlNonQuery(TMSWriteConnection, strsql, parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 考核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAudit(LB_TASK model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ps_tms.LB_TASK set 
                        ORDERQUANTITY=:ORDERQUANTITY,
                        WEIGHT=:WEIGHT,
                        ONCEAMOUNT=:ONCEAMOUNT,
                        ORDERAMOUNT=:ORDERAMOUNT,
                        MILEAGE=:MILEAGE,
                        FINISHTIME=:FINISHTIME,
                        KPIAMOUNT=:KPIAMOUNT,
                        REMARK=:REMARK,
                        TASKSTATUS=:TASKSTATUS,
                        PICKGOODSAMOUNT=:PICKGOODSAMOUNT
                        where ID=:ID");

            //string strsq = string.Format("update ps_tms.LB_TASK set ORDERQUANTITY=:ORDERQUANTITY,WEIGHT=:WEIGHT,ONCEAMOUNT=:ONCEAMOUNT,ORDERAMOUNT=:ORDERAMOUNT,MILEAGE=:MILEAGE,FINISHTIME=:FINISHTIME,KPIAMOUNT=:KPIAMOUNT where ID={0}", model.ID);
            OracleParameter[] parameters = {
					new OracleParameter(":ORDERQUANTITY", OracleDbType.Decimal,9),
                    	new OracleParameter(":WEIGHT", OracleDbType.Decimal,9),
                        new OracleParameter(":ONCEAMOUNT", OracleDbType.Decimal,9),
                        new OracleParameter(":ORDERAMOUNT", OracleDbType.Decimal,9),
                        new OracleParameter(":MILEAGE", OracleDbType.Decimal,9),
                        new OracleParameter(":FINISHTIME", OracleDbType.Date,9),
                        new OracleParameter(":KPIAMOUNT", OracleDbType.Varchar2,50),
                        new OracleParameter(":ID", OracleDbType.Int32,9),
                        new OracleParameter(":REMARK", OracleDbType.Varchar2,400),
                        new OracleParameter(":TASKSTATUS", OracleDbType.Varchar2,400),
                        new OracleParameter(":PICKGOODSAMOUNT", OracleDbType.Decimal,400)

					};
            parameters[0].Value = model.ORDERQUANTITY;
            parameters[1].Value = model.WEIGHT;
            parameters[2].Value = model.ONCEAMOUNT;
            parameters[3].Value = model.ORDERAMOUNT;
            parameters[4].Value = model.MILEAGE;
            parameters[5].Value = model.FINISHTIME;
            parameters[6].Value = model.KPIAMOUNT;
            parameters[7].Value = model.ID;
            parameters[8].Value = model.REMARK;
            parameters[9].Value = model.TASKSTATUS;
            parameters[10].Value = model.PICKGOODSAMOUNT;
            if (ExecuteSqlNonQuery(TMSWriteConnection, strSql.ToString(), parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 接受任务配送商反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateTaskEdit(LB_TASK model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ps_tms.LB_TASK set 
                        ORDERQUANTITY=:ORDERQUANTITY,
                        WEIGHT=:WEIGHT,
                        MILEAGE=:MILEAGE,
                        FINISHTIME=:FINISHTIME,
                        TASKSTATUS=:TASKSTATUS,
                        PICKMAN=:PICKMAN,
                        PICKGOODSAMOUNT=:PICKGOODSAMOUNT
                        where ID=:ID");

            //string strsq = string.Format("update ps_tms.LB_TASK set ORDERQUANTITY=:ORDERQUANTITY,WEIGHT=:WEIGHT,ONCEAMOUNT=:ONCEAMOUNT,ORDERAMOUNT=:ORDERAMOUNT,MILEAGE=:MILEAGE,FINISHTIME=:FINISHTIME,KPIAMOUNT=:KPIAMOUNT where ID={0}", model.ID);
            OracleParameter[] parameters = {
					new OracleParameter(":ORDERQUANTITY", OracleDbType.Decimal,9),
                    	new OracleParameter(":WEIGHT", OracleDbType.Decimal,9),
                        new OracleParameter(":MILEAGE", OracleDbType.Decimal,9),
                        new OracleParameter(":FINISHTIME", OracleDbType.Date,9),
                        new OracleParameter(":ID", OracleDbType.Int32,9),
                        new OracleParameter(":TASKSTATUS", OracleDbType.Varchar2,400),
                        new OracleParameter(":PICKMAN", OracleDbType.Varchar2,50),
                        new OracleParameter(":PICKGOODSAMOUNT", OracleDbType.Decimal,9)
					};
            parameters[0].Value = model.ORDERQUANTITY;
            parameters[1].Value = model.WEIGHT;
            parameters[2].Value = model.MILEAGE;
            parameters[3].Value = model.FINISHTIME;
            parameters[4].Value = model.ID;
            parameters[5].Value = model.TASKSTATUS;
            parameters[6].Value = model.PICKMAN;
            parameters[7].Value = model.PICKGOODSAMOUNT;
            if (ExecuteSqlNonQuery(TMSWriteConnection, strSql.ToString(), parameters) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(decimal ID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteList(string IDlist)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据任务id获得任务实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public LadingBill.LB_TASK GetModel(decimal ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,TASKCODE,MERCHANTID,WAREHOUSEID,FROMDISTRIBUTIONCODE,TODISTRIBUTIONCODE,DEPARTMENT,PLANTIME,TASKTIME,MILEAGE,PICKPRICETYPE,ONCEAMOUNT,ORDERAMOUNT,TASKSTATUS,CREATETIME,UPDATETIME,RECEIVEEMAIL,RECEIVEEMAILTIME,PICKGOODSAMOUNT,FINISHTIME,ORDERQUANTITY,WEIGHT,KPIAMOUNT,REMARK,ISDELETED,ISENABLED,ISPRINT from LB_TASK ");
            strSql.Append(" where ID=:ID ");
            OracleParameter[] parameters = {
					new OracleParameter(":ID", OracleDbType.Int32,10)			};
            parameters[0].Value = ID;

            LB_TASK model = new LB_TASK();
            model = ExecuteSqlSingle_ByDataTableReflect<LB_TASK>(TMSReadOnlyConnection, strSql.ToString(), parameters);

            return model;
        }

        public LB_TASK GetModelAll(decimal ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select t.*, m.warehousename, mer.merchantname, dis.distributionname,m.WAREHOUSEADDRESS
                                 from LB_TASK t
                                 join ps_pms.MERCHANTWAREHOUSE m
                                   on t.warehouseid = m.warehousecode
                                 join ps_pms.merchantbaseinfo mer
                                   on t.merchantid = mer.id
                                 join ps_pms.DISTRIBUTION dis
                                   on t.todistributioncode = dis.distributioncode ");
            strSql.Append(" where t.ID=:ID ");
            OracleParameter[] parameters = {
					new OracleParameter(":ID", OracleDbType.Int32,10)			};
            parameters[0].Value = ID;

            LB_TASK model = new LB_TASK();
            model = ExecuteSqlSingle_ByDataTableReflect<LB_TASK>(TMSReadOnlyConnection, strSql.ToString(), parameters);

            return model;
        }


        public LadingBill.LB_TASK DataRowToModel(DataRow row)
        {
            throw new NotImplementedException();
        }

        public DataSet GetList(string strWhere)
        {
            throw new NotImplementedException();
        }
    }
}


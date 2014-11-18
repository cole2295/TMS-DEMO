using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Log
{
    /// <summary>
    /// 拣运状态改变日志表数据层
    /// </summary>
    public class BillChangeLogDAL : BaseDAL, IBillChangeLogDAL
    {
        #region IBillChangeLogDAL 成员

        /// <summary>
        /// 新增状态改变日志
        /// </summary>
        /// <param name="logModel"></param>
        /// <returns></returns>
        public int Add(BillChangeLogModel logModel)
        {
            if (logModel == null) throw new ArgumentNullException("BillChangeLogModel is null");
            string strSql = string.Format(@"
                INSERT INTO SC_BillChangeLog(
                    BCID
                    ,FormCode
                    ,CurrentStatus
                    ,PreStatus
                    ,ReturnStatus
                    ,OperateType
                    ,CurrentDistributionCode
                    ,TODISTRIBUTIONCODE
                    ,DeliverStationID
                    ,TOEXPRESSCOMPANYID
                    ,Note
                    ,CreateBy
                    ,CreateDept
                    ,Syncflag
                    ,IpAddress
                    ,ClientInfo)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:CurrentStatus
                    ,:PreStatus
                    ,:ReturnStatus
                    ,:OperateType
                    ,:CurrentDistributionCode
                    ,:TODISTRIBUTIONCODE
                    ,:DeliverStationID
                    ,:TOEXPRESSCOMPANYID
                    ,:Note
                    ,:CreateBy
                    ,:CreateDept
                    ,:Syncflag
                    ,:IpAddress
                    ,:ClientInfo)
", logModel.KeyCodeNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value = logModel.FormCode },
                new OracleParameter() { ParameterName="CurrentStatus",DbType= DbType.Int32,Value = (int)logModel.CurrentStatus },
                new OracleParameter() { ParameterName="PreStatus",DbType= DbType.Int32,Value = (int)logModel.PreStatus },
                new OracleParameter() { ParameterName="ReturnStatus",DbType= DbType.Int32,Value =  logModel.ReturnStatus.HasValue?(int)logModel.ReturnStatus:(int?)null },
                new OracleParameter() { ParameterName="OperateType",DbType= DbType.Int16,Value = (int)logModel.OperateType },
                new OracleParameter() { ParameterName="CurrentDistributionCode",DbType= DbType.String,Value = logModel.CurrentDistributionCode },
                new OracleParameter() { ParameterName="ToDistributionCode",DbType= DbType.String,Value = logModel.ToDistributionCode },
                new OracleParameter() { ParameterName="DeliverStationID", DbType = System.Data.DbType.Int32 , Value = logModel.DeliverStationID },
                new OracleParameter() { ParameterName="TOEXPRESSCOMPANYID", DbType = System.Data.DbType.Int32 , Value = logModel.ToExpressCompanyID },
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value = logModel.Note },
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value = logModel.CreateBy },
                new OracleParameter() { ParameterName="CreateDept",DbType= DbType.Int32,Value = logModel.CreateDept },
                new OracleParameter() { ParameterName="Syncflag",DbType= DbType.Int32,Value=(int)logModel.SyncFlag },
                new OracleParameter() { ParameterName="IpAddress",DbType= DbType.String,Value = logModel.IPAddress },
                new OracleParameter() { ParameterName="ClientInfo",DbType= DbType.String,Value = logModel.ClientInfo }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        /// <summary>
        /// 批量新增状态改变日志
        /// </summary>
        /// <param name="listLogModel">改变日志列表</param>
        /// <returns></returns>
        public int BatchAdd(List<BillChangeLogModel> listLogModel)
        {
            if (listLogModel == null) throw new ArgumentNullException("BillChangeLogModel is null");
            String strSql = String.Format(@"
                INSERT INTO SC_BillChangeLog(
                    BCID
                    ,FormCode
                    ,CurrentStatus
                    ,PreStatus
                    ,ReturnStatus
                    ,OperateType
                    ,CurrentDistributionCode
					,ToDistributionCode
					,ToExpressCompanyid
                    ,DeliverStationID
                    ,Note
                    ,CreateBy
                    ,CreateDept
                    ,Syncflag
                    ,IpAddress
                    ,ClientInfo)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:CurrentStatus
                    ,:PreStatus
                    ,:ReturnStatus
                    ,:OperateType
                    ,:CurrentDistributionCode
					,:ToDistributionCode
					,:ToExpressCompanyid
                    ,:DeliverStationID
                    ,:Note
                    ,:CreateBy
                    ,:CreateDept
                    ,:Syncflag
                    ,:IpAddress
                    ,:ClientInfo)
", listLogModel[0].KeyCodeNextValue());
            String[] arrFormCode = new String[listLogModel.Count];
            int[] arrCurrentStatus = new int[listLogModel.Count];
            int[] arrPreStatus = new int[listLogModel.Count];
            int?[] arrReturnStatus = new int?[listLogModel.Count];
            int[] arrOperateType = new int[listLogModel.Count];
            String[] arrCurrentDistributionCode = new String[listLogModel.Count];
	        String[] arrToDistributionCode = new String[listLogModel.Count];
	        int[] arrToExpressCompanyid = new int[listLogModel.Count];
            int[] arrDeliverStationID = new int[listLogModel.Count];
            String[] arrNote = new String[listLogModel.Count];
            int[] arrCreateBy = new int[listLogModel.Count];
            int[] arrCreateDept = new int[listLogModel.Count];
            int[] arrSyncFlag = new int[listLogModel.Count];
            String[] arrIPAddress = new String[listLogModel.Count];
            String[] arrClientInfo = new String[listLogModel.Count];
            int pos = 0;                        //index
            listLogModel.ForEach(p =>
            {
                arrFormCode[pos] = p.FormCode;
                arrCurrentStatus[pos] = (int)p.CurrentStatus;
                arrPreStatus[pos] = (int)p.PreStatus;
                arrReturnStatus[pos] = p.ReturnStatus.HasValue ? (int)p.ReturnStatus.Value : (int?)null;
                arrOperateType[pos] = (int)p.OperateType;
                arrCurrentDistributionCode[pos] = p.CurrentDistributionCode;
				arrToDistributionCode[pos] = p.ToDistributionCode;
	            arrToExpressCompanyid[pos] = p.ToExpressCompanyID;
                arrDeliverStationID[pos] = p.DeliverStationID;
                arrNote[pos] = p.Note;
                arrCreateBy[pos] = p.CreateBy;
                arrCreateDept[pos] = p.CreateDept;
                arrSyncFlag[pos] = (int)p.SyncFlag;
                arrIPAddress[pos] = p.IPAddress;
                arrClientInfo[pos] = p.ClientInfo;
                pos++;
            });
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value =  arrFormCode},
                new OracleParameter() { ParameterName="CurrentStatus",DbType= DbType.Int32,Value = arrCurrentStatus },
                new OracleParameter() { ParameterName="PreStatus",DbType= DbType.Int32,Value = arrPreStatus },
                new OracleParameter() { ParameterName="ReturnStatus",DbType= DbType.Int32,Value =  arrReturnStatus },
                new OracleParameter() { ParameterName="OperateType",DbType= DbType.Int16,Value = arrOperateType },
                new OracleParameter() { ParameterName="CurrentDistributionCode",DbType= DbType.String,Value =  arrCurrentDistributionCode },
				new OracleParameter() { ParameterName="ToDistributionCode",DbType= DbType.String,Value =  arrToDistributionCode },
				new OracleParameter() { ParameterName="ToExpressCompanyid",DbType= DbType.Int32,Value =  arrToExpressCompanyid },
                new OracleParameter() { ParameterName="DeliverStationID", DbType = System.Data.DbType.Int32 , Value = arrDeliverStationID },
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value = arrNote },
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value =  arrCreateBy },
                new OracleParameter() { ParameterName="CreateDept",DbType= DbType.Int32,Value = arrCreateDept},
                new OracleParameter() { ParameterName="Syncflag",DbType= DbType.Int32,Value= arrSyncFlag},
                new OracleParameter() { ParameterName="IpAddress",DbType= DbType.String,Value = arrIPAddress},
                new OracleParameter() { ParameterName="ClientInfo",DbType= DbType.String,Value = arrClientInfo}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listLogModel.Count, arguments);
        }

        public List<BillChangeLogModel> GetNotices(int count)
        {
            string sql = @" select lg.currentdistributioncode,
                                   lg.TODISTRIBUTIONCODE,
                                   lg.deliverstationid,
                                   lg.TOEXPRESSCOMPANYID,
                                   lg.currentstatus,
                                   lg.formcode,
                                   lg.bcid,
                                   lg.RETURNSTATUS
                              from sc_billchangelog lg
                             where lg.currentstatus in (20, 30, 11)
                               and lg.syncflag = -1
                               and rownum <= :ct";

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="ct",DbType= DbType.Int32,Value=count}
            };

            var notices = ExecuteSql_ByDataTableReflect<BillChangeLogModel>(TMSWriteConnection, sql, arguments.ToArray());
            if (notices == null)
            {
                return new List<BillChangeLogModel>();
            }
            return notices.ToList();
        }

        public bool UpdateSynStatus(string bcid)
        {
            string sql = @"update Sc_Billchangelog set syncflag = -2 where bcid = :bcid";

            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName=":bcid",DbType= DbType.String,Value=bcid}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments) > 0;
        }

        #endregion
    }
}

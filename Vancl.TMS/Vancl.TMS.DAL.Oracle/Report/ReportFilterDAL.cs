using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Report;
using Vancl.TMS.Model.Report;
using Oracle.DataAccess.Client;
using Vancl.TMS.Util.Converter;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Report
{
    public class ReportFilterDAL : BaseDAL, IReportFilterDAL
    {
        #region IReportFilterDAL 成员
        /// <summary>
        /// 检索报表筛选对象
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        public List<ReportFilterModel> Search(ReportFilterSearchModel searchModel)
        {
            List<OracleParameter> lstParams = new List<OracleParameter>();
            string strSql = @"
                SELECT rfid, reporttype, viewobjpropertyname, isshow
                    , custshowname, createby, createtime, updateby, updatetime, isdeleted 
                FROM tms_reportfilter 
                WHERE IsDeleted=0
                    AND reporttype=:reporttype
                    AND createby=:createby";
            if (searchModel.IsShow.HasValue)
            {
                strSql += @" AND isshow=:isshow";
                lstParams.Add(new OracleParameter() { ParameterName = "isshow", DbType = DbType.Byte, Value = searchModel.IsShow });
            }
            lstParams.Add(new OracleParameter() { ParameterName = "reporttype", DbType = DbType.Int32, Value = (int)searchModel.ReportType });
            lstParams.Add(new OracleParameter() { ParameterName = "createby", DbType = DbType.Int32, Value = searchModel.CreateBy });
            return (List<ReportFilterModel>)ExecuteSql_ByReaderReflect<ReportFilterModel>(TMSReadOnlyConnection, strSql, lstParams.ToArray());
        }

        /// <summary>
        /// 新增报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(ReportFilterModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO tms_reportfilter
                (rfid, reporttype, viewobjpropertyname, isshow, custshowname, createby, updateby)
                VALUES
                ({0} ,:reporttype, :viewobjpropertyname, :isshow, :custshowname, :createby, :createby)"
                , model.SequenceNextValue());
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "reporttype", DbType = DbType.Int32, Value = (int)model.ReportType},
                new OracleParameter(){ParameterName = "viewobjpropertyname", DbType = DbType.String, Value = model.ViewObjPropertyName},
                new OracleParameter(){ParameterName = "isshow", DbType = DbType.Byte, Value = model.IsShow},
                new OracleParameter(){ParameterName = "custshowname", DbType = DbType.String, Value = model.CustShowName},
                new OracleParameter(){ParameterName = "createby", DbType = DbType.Int32, Value =UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 修改报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(ReportFilterModel model)
        {
            if (model == null)
            {
                return 0;
            }
            string strSql = @"
                UPDATE tms_reportfilter
                SET reporttype = :reporttype,
                    viewobjpropertyname = :viewobjpropertyname,
                    isshow = :isshow,
                    custshowname = :custshowname,
                    updateby = :updateby,
                    updatetime = sysdate
                WHERE rfid = :rfid
                    AND isdeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "reporttype", DbType = DbType.Int32, Value = (int)model.ReportType},
                new OracleParameter(){ParameterName = "viewobjpropertyname", DbType = DbType.String, Value = model.ViewObjPropertyName},
                new OracleParameter(){ParameterName = "isshow", DbType = DbType.Byte, Value = model.IsShow},
                new OracleParameter(){ParameterName = "custshowname", DbType = DbType.String, Value = model.CustShowName},
                new OracleParameter(){ParameterName = "updateby", DbType = DbType.Int32, Value =UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName = "rfid", DbType = DbType.Int64, Value = model.RFDI }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete(long RFDI)
        {
            string strSql = @"
                UPDATE tms_reportfilter
                SET isdeleted=1
                    ,updatetime=sysdate
                    ,updateby=:updateby
                WHERE rfid=:rfid
                    AND isdeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "rfid", DbType = DbType.Int64, Value = RFDI } ,
                new OracleParameter() { ParameterName = "updateby", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        /// <summary>
        /// 批量新增报表筛选对象
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public int BatchAdd(List<ReportFilterModel> listModel)
        {
            if (listModel == null || listModel.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                INSERT INTO tms_reportfilter
                (rfid, reporttype, viewobjpropertyname, isshow, custshowname, createby, updateby)
                VALUES
                ({0}, :reporttype, :viewobjpropertyname, :isshow, :custshowname, {1}, {1})"
                , listModel[0].SequenceNextValue()
                , UserContext.CurrentUser.ID);
            int[] reporttype = new int[listModel.Count];
            string[] viewobjpropertyname = new string[listModel.Count];
            bool[] isshow = new bool[listModel.Count];
            string[] custshowname = new string[listModel.Count];
            int index = 0;
            listModel.ForEach(m =>
                {
                    reporttype[index] = (int)m.ReportType;
                    viewobjpropertyname[index] = m.ViewObjPropertyName;
                    isshow[index] = m.IsShow;
                    custshowname[index] = m.CustShowName;
                    index++;
                }
            );
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "reporttype", DbType = DbType.Int32, Value = reporttype},
                new OracleParameter(){ParameterName = "viewobjpropertyname", DbType = DbType.String, Value = viewobjpropertyname},
                new OracleParameter(){ParameterName = "isshow", DbType = DbType.Byte, Value = isshow},
                new OracleParameter(){ParameterName = "custshowname", DbType = DbType.String, Value = custshowname}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listModel.Count, parameters);
        }

        /// <summary>
        /// 批量删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int BatchDelete(List<long> listRFDI)
        {
            if (listRFDI == null || listRFDI.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE tms_reportfilter
                SET isdeleted=1
                    ,updatetime=sysdate
                    ,updateby={0}
                WHERE rfid=:rfids
                    AND isdeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "rfids", DbType = DbType.Int64, Value = listRFDI.ToArray() }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listRFDI.Count, parameters);
        }

        #endregion
    }
}

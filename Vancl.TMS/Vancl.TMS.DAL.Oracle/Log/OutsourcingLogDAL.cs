using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Log
{
    /// <summary>
    /// 外包日志数据层
    /// </summary>
    public class OutsourcingLogDAL : BaseDAL, IOutsourcingLogDAL
    {

        #region IOutsourcingLogDAL 成员
        /// <summary>
        /// 新增外包日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(OutsourcingLogModel model)
        {
            if (model == null) throw new ArgumentNullException("OutsourcingLogModel is null");
            string strSql = string.Format(@"
                INSERT INTO SC_OutsourcingLog(
                    OLID
                    ,FormCode
                    ,PrincipalUserID
                    ,PrincipalDeptID
                    ,PrincipalDistributionCode
                    ,AgentUserID
                    ,AgentDeptID
                    ,AgentDistributionCode
                    ,OperateType
                    ,UpdateBy
                    ,Note)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:PrincipalUserID
                    ,:PrincipalDeptID
                    ,:PrincipalDistributionCode
                    ,:AgentUserID
                    ,:AgentDeptID
                    ,:AgentDistributionCode
                    ,:OperateType
                    ,:UpdateBy
                    ,:Note)", model.KeyCodeNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value = model.FormCode },
                new OracleParameter() { ParameterName="PrincipalUserID",DbType= DbType.Int32,Value = model.PrincipalUserID },
                new OracleParameter() { ParameterName="PrincipalDeptID",DbType= DbType.Int32,Value = model.PrincipalDeptID },
                new OracleParameter() { ParameterName="PrincipalDistributionCode",DbType= DbType.String,Value = model.PrincipalDistributionCode },
                new OracleParameter() { ParameterName="AgentUserID",DbType= DbType.Int32,Value =  model.AgentUserID },
                new OracleParameter() { ParameterName="AgentDeptID",DbType= DbType.Int32,Value = model.AgentDeptID },
                new OracleParameter() { ParameterName="AgentDistributionCode",DbType= DbType.String,Value = model.AgentDistributionCode },
                new OracleParameter() { ParameterName="OperateType", DbType = DbType.Int32 , Value = (int)model.OperateType },
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value = model.Note },
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value = model.UpdateBy }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        /// <summary>
        /// 批量新增外包日志
        /// </summary>
        /// <param name="listModel"></param>
        /// <returns></returns>
        public int BatchAdd(List<OutsourcingLogModel> listModel)
        {
            if (listModel == null) throw new ArgumentNullException("OutsourcingLogModel List is null");
            String strSql = String.Format(@"
                INSERT INTO SC_OutsourcingLog(
                    OLID
                    ,FormCode
                    ,PrincipalUserID
                    ,PrincipalDeptID
                    ,PrincipalDistributionCode
                    ,AgentUserID
                    ,AgentDeptID
                    ,AgentDistributionCode
                    ,OperateType
                    ,UpdateBy
                    ,Note)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:PrincipalUserID
                    ,:PrincipalDeptID
                    ,:PrincipalDistributionCode
                    ,:AgentUserID
                    ,:AgentDeptID
                    ,:AgentDistributionCode
                    ,:OperateType
                    ,:UpdateBy
                    ,:Note)", listModel[0].KeyCodeNextValue());
            string[] arrFormCode = new string[listModel.Count];
            int[] arrPrincipalUserID = new int[listModel.Count];
            int[] arrPrincipalDeptID = new int[listModel.Count];
            string[] arrPrincipalDistributionCode = new string[listModel.Count];
            int[] arrAgentUserID = new int[listModel.Count];
            int[] arrAgentDeptID = new int[listModel.Count];
            string[] arrAgentDistributionCode = new string[listModel.Count];
            int[] arrOperateType = new int[listModel.Count];
            int[] arrUpdateBy = new int[listModel.Count];
            string[] arrNote = new string[listModel.Count];
            int pos = 0;                        //index
            listModel.ForEach(p =>
            {
                arrFormCode[pos] = p.FormCode;
                arrPrincipalUserID[pos] = p.PrincipalUserID;
                arrPrincipalDeptID[pos] = p.PrincipalDeptID;
                arrPrincipalDistributionCode[pos] = p.PrincipalDistributionCode;
                arrAgentUserID[pos] = p.AgentUserID;
                arrAgentDeptID[pos] = p.AgentDeptID;
                arrAgentDistributionCode[pos] = p.AgentDistributionCode;
                arrOperateType[pos] = (int)p.OperateType;
                arrUpdateBy[pos] = p.UpdateBy;
                arrNote[pos] = p.Note;
                pos++;
            });
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value = arrFormCode },
                new OracleParameter() { ParameterName="PrincipalUserID",DbType= DbType.Int32,Value = arrPrincipalUserID },
                new OracleParameter() { ParameterName="PrincipalDeptID",DbType= DbType.Int32,Value = arrPrincipalDeptID},
                new OracleParameter() { ParameterName="PrincipalDistributionCode",DbType= DbType.String,Value =arrPrincipalDistributionCode },
                new OracleParameter() { ParameterName="AgentUserID",DbType= DbType.Int32,Value =  arrAgentUserID },
                new OracleParameter() { ParameterName="AgentDeptID",DbType= DbType.Int32,Value = arrAgentDeptID},
                new OracleParameter() { ParameterName="AgentDistributionCode",DbType= DbType.String,Value =arrAgentDistributionCode },
                new OracleParameter() { ParameterName="OperateType", DbType = DbType.Int32 , Value = arrOperateType },
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value =arrNote },
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value = arrUpdateBy }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listModel.Count, arguments);
        }

        #endregion
    }
}

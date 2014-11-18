using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WaybillStatusChangeLogDAL : LMSBaseDAL, IWaybillStatusChangeLogDAL
    {
        #region IWaybillStatusChangeLogDAL 成员

        /// <summary>
        /// 添加状态变更日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(WaybillStatusChangeLogEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("WaybillStatusChangeLogEntityModel is null");
            String sql = String.Format(@"
INSERT INTO LMS_WaybillStatusChangeLog(WAYBILLSTATUSCHANGELOGID, WaybillNO, CurNode, Status, SubStatus, MerchantID, DistributionCode, DeliverStationID, CreateTime,CreateBy ,CreateStation, IsSyn, Note
           ,CustomerOrder ,LMS_WaybillStatusChangeLogKid ,IsM2sSyn, OperateType ,TmsSyncStatus )
VALUES
           ({0} , :WaybillNO ,:CurNode ,:Status ,:SubStatus ,:MerchantID ,:DistributionCode ,:DeliverStationID ,SYSDATE ,:CreateBy ,:CreateDept ,:IsSyn ,:Note
           ,:CustomerOrder ,:LMS_WaybillStatusChangeLogKid ,:IsM2sSyn, :OperateType ,:TmsSyncStatus )
",
    model.SequenceNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNO", OracleDbType = OracleDbType.Int64, Value = model.WaybillNO },
                new OracleParameter() { ParameterName="CurNode", OracleDbType = OracleDbType.Int32, Value = (int)model.CurNode },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = ((int)model.Status).ToString() },
                new OracleParameter() { ParameterName="SubStatus", OracleDbType = OracleDbType.Int32, Value = model.SubStatus },
                new OracleParameter() { ParameterName="MerchantID", OracleDbType = OracleDbType.Int32, Value = model.MerchantID },
                new OracleParameter() { ParameterName="DistributionCode", OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.DistributionCode },
                new OracleParameter() { ParameterName="DeliverStationID", OracleDbType = OracleDbType.Int32, Value = model.DeliverStationID },
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32, Value = model.CreateBy },
                new OracleParameter() { ParameterName="CreateDept", OracleDbType = OracleDbType.Int32, Value = model.CreateDept },
                new OracleParameter() { ParameterName="IsSyn", OracleDbType = OracleDbType.Int32, Value = Convert.ToInt32(model.IsSyn) },
                new OracleParameter() { ParameterName="Note", OracleDbType = OracleDbType.Varchar2, Size = 800, Value = model.Note },
                new OracleParameter() { ParameterName="CustomerOrder", OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.CustomerOrder },
                new OracleParameter() { ParameterName="LMS_WaybillStatusChangeLogKid", OracleDbType = OracleDbType.Varchar2, Size = 40, Value = model.LMS_WaybillStatusChangeLogKid },
                new OracleParameter() { ParameterName="IsM2sSyn", OracleDbType = OracleDbType.Varchar2, Size = 1, Value = model.IsM2sSyn },
                new OracleParameter() { ParameterName="OperateType", OracleDbType = OracleDbType.Int32, Value = (int)model.OperateType },
                new OracleParameter() { ParameterName="TmsSyncStatus", OracleDbType = OracleDbType.Int32, Value = (int)model.TmsSyncStatus }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }

        #endregion
    }
}

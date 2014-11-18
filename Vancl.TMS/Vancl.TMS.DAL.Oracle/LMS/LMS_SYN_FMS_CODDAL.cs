using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    /// <summary>
    /// 
    /// </summary>
    public class LMS_SYN_FMS_CODDAL : BaseDAL, ILMS_SYN_FMS_CODDAL
    {
        #region ILMS_SYN_FMS_CODDAL 成员

        public int Add(LMS_SYN_FMS_CODEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = String.Format(@"
INSERT INTO LMS_SYN_FMS_COD (LMS_SYN_FMS_CODIDID, WaybillNo, OperateType, OperateTime, StationID, IsSyn , Createby, LMSSYNFMSCODKid )
VALUES({0}, :WaybillNo , :OperateType, :OperateTime, :StationID, 0 , :Createby, :LMSSYNFMSCODKid  )
",model.SequenceNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "WaybillNo" , OracleDbType = OracleDbType.Int64 , Value = model.WaybillNo },
                new OracleParameter() { ParameterName = "OperateType" , OracleDbType = OracleDbType.Int32 , Value = model.OperateType },
                new OracleParameter() { ParameterName = "OperateTime" ,  OracleDbType = OracleDbType.Date , Value = model.OperateTime },
                new OracleParameter() { ParameterName = "StationID" ,  OracleDbType = OracleDbType.Int32 , Value = model.StationID },
                new OracleParameter() { ParameterName = "Createby" ,  OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = model.CODCreateBy },
                new OracleParameter() { ParameterName = "LMSSYNFMSCODKid" ,  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.LmsSynFMSCodKid }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }

        #endregion
    }


}

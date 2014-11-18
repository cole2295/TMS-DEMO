using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;


namespace Vancl.TMS.DAL.Sql2008.LMS
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
            String sql = @"
INSERT INTO LMS_SYN_FMS_COD (WaybillNo, OperateType, OperateTime, StationID, IsSyn , Createby, LMSSYNFMSCODKid )
VALUES(@WaybillNo , @OperateType, @OperateTime, @StationID, 0 , @Createby, @LMSSYNFMSCODKid  )
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@WaybillNo" ,  SqlDbType = System.Data.SqlDbType.BigInt , Value = model.WaybillNo },
                new SqlParameter() { ParameterName = "@OperateType" ,  SqlDbType = System.Data.SqlDbType.Int , Value = model.OperateType },
                new SqlParameter() { ParameterName = "@OperateTime" ,  SqlDbType = System.Data.SqlDbType.DateTime , Value = model.OperateTime },
                new SqlParameter() { ParameterName = "@StationID" ,  SqlDbType = System.Data.SqlDbType.Int , Value = model.StationID },
                new SqlParameter() { ParameterName = "@Createby" ,  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50,  Value = model.CODCreateBy },
                new SqlParameter() { ParameterName = "@LMSSYNFMSCODKid" , SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = model.LmsSynFMSCodKid }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

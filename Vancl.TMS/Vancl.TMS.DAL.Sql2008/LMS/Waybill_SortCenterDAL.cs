using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;
using System.Data;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class Waybill_SortCenterDAL : BaseDAL, IWaybill_SortCenterDAL
    {
        #region IWaybill_SortCenterDAL 成员

        public int Merge(Waybill_SortCenterEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("Waybill_SortCenterEntityModel is null");
            StringBuilder sqlcol = new StringBuilder();
            sqlcol.Append(@"
WHEN MATCHED THEN 
    UPDATE SET des.UpdateBy = src.UpdateBy , des.UpdateTime = getdate()
");
            if (model.InBoundKid != null)
            {
                sqlcol.Append(" , des.InBoundKid = src.InBoundKid ");
            }
            if (model.OutBoundKid != null)
            {
                sqlcol.Append(" , des.OutBoundKid = src.OutBoundKid ");
            }
            String sql = String.Format(@"
MERGE INTO Waybill_SortCenter AS des
USING 
(
    SELECT @WaybillNO AS WaybillNO, @InBoundKid AS InBoundKid,  @OutBoundKid AS OutBoundKid, @UpdateBy AS UpdateBy 
) AS src
 ON des.WaybillNO = src.WaybillNO
{0}
WHEN NOT MATCHED THEN
INSERT (WaybillNO,InBoundKid, OutBoundKid, CreatBy, CreatTime, UpdateBy, UpdateTime, IsDelete )
VALUES (src.WaybillNO, src.InBoundKid, src.OutBoundKid, src.UpdateBy, getdate(),src.UpdateBy, getdate() , 0) ;
",
    sqlcol.ToString()
 );
            SqlParameter[] parameters = new SqlParameter[]  
            {
                new SqlParameter("@WaybillNO", SqlDbType.BigInt ){ Value = model.WaybillNO},
                new SqlParameter("@InBoundKid", SqlDbType.VarChar, 20){Value= model.InBoundKid},
                new SqlParameter("@OutBoundKid", SqlDbType.VarChar, 20){Value= model.OutBoundKid},
                new SqlParameter("@UpdateBy", SqlDbType.Int){Value= model.UpdateBy}
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, parameters);
        }


        #endregion
    }
}

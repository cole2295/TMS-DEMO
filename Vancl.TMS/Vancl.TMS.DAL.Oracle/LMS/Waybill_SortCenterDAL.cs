using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class Waybill_SortCenterDAL : LMSBaseDAL, IWaybill_SortCenterDAL
    {
        #region IWaybill_SortCenterDAL 成员

        public int Merge(Waybill_SortCenterEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("Waybill_SortCenterEntityModel is null");
            StringBuilder sqlcol = new StringBuilder();
            sqlcol.Append(@"
WHEN MATCHED THEN 
    UPDATE SET des.UpdateBy = src.UpdateBy  , des.UpdateTime = SYSDATE 
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
MERGE INTO Waybill_SortCenter  des
USING 
(
    SELECT :WaybillNO AS WaybillNO, :InBoundKid AS InBoundKid,  :OutBoundKid AS OutBoundKid, :UpdateBy AS UpdateBy  FROM dual
)  src
ON ( des.WaybillNO = src.WaybillNO )
{0}
WHEN NOT MATCHED THEN
INSERT (WaybillNO,InBoundKid, OutBoundKid, CreatBy, CreatTime, UpdateBy, UpdateTime)
VALUES (src.WaybillNO, src.InBoundKid, src.OutBoundKid, src.UpdateBy, SYSDATE ,src.UpdateBy, SYSDATE )
",
    sqlcol.ToString()
 );
            OracleParameter[] parameters = new OracleParameter[]  
            {
                new OracleParameter(){ ParameterName="WaybillNO",  OracleDbType = OracleDbType.Int64 , Value = model.WaybillNO },
                new OracleParameter(){ ParameterName="InBoundKid", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value= model.InBoundKid },
                new OracleParameter(){ ParameterName="OutBoundKid",  OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value= model.OutBoundKid },
                new OracleParameter(){ ParameterName="UpdateBy" , OracleDbType = OracleDbType.Int32 , Value= model.UpdateBy }
            };
            //ORACLE MERGE 返回 -1
            if (-1 == ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, parameters)) 
            {
                return 1;
            }
            return 0;
        }

        #endregion
    }
}

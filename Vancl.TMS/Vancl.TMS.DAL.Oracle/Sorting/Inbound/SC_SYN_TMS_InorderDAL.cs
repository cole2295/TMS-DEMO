using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.Sorting.Inbound
{
    public class SC_SYN_TMS_InorderDAL : BaseDAL, ISC_SYN_TMS_InorderDAL
    {

        #region ISC_SYN_TMS_InorderDAL 成员

        public int Add(SC_SYN_TMS_InorderEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("SC_SYN_TMS_InorderEntityModel is null");
            String sql = String.Format(@"
INSERT INTO SC_SYN_TMS_Inorder(SSTID, FormCode, CustomerOrder, InboundID, SC2TMSFlag, CreateTime, IsDeleted)
VALUES({0}, :FormCode, :CustomerOrder, :InboundID, :SC2TMSFlag, SYSDATE , 0 )
", model.KeyCodeNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "FormCode",  OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode },
                new OracleParameter() { ParameterName = "CustomerOrder", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.CustomerOrder },
                new OracleParameter() { ParameterName = "InboundID",  OracleDbType = OracleDbType.Int32 , Value = model.InboundID },
                new OracleParameter() { ParameterName = "SC2TMSFlag",  OracleDbType = OracleDbType.Int16 , Value = (int)model.SC2TMSFlag }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

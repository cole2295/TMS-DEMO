using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Sorting.Outbound
{
    public class SC_SYN_TMS_OutboxDAL : BaseDAL, ISC_SYN_TMS_OutboxDAL
    {
        #region ISC_SYN_TMS_OutboxDAL 成员
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(SC_SYN_TMS_OutboxEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("SC_SYN_TMS_OutboxEntityModel is null");
            String sql = String.Format(@"
INSERT INTO SC_SYN_TMS_Outbox(SSTOID, BoxNo, DepartureID, ArrivalID, SC2TMSFlag, NoType, IsDeleted)
VALUES({0}, :BoxNo, :DepartureID, :ArrivalID, :SC2TMSFlag, :NoType, 0)
",
    model.KeyCodeNextValue()
 );
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", DbType = System.Data.DbType.String, Value = model.BoxNo },
                new OracleParameter() { ParameterName = "DepartureID", DbType = System.Data.DbType.Int32, Value = model.DepartureID },
                new OracleParameter() { ParameterName = "ArrivalID", DbType = System.Data.DbType.Int32, Value = model.ArrivalID },
                new OracleParameter() { ParameterName = "SC2TMSFlag", DbType = System.Data.DbType.Int16, Value = (int)model.SC2TMSFlag },
                new OracleParameter() { ParameterName = "NoType", DbType = System.Data.DbType.Int16, Value = (int)model.NoType }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}

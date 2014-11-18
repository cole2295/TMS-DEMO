using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.Threading;
using Vancl.TMS.Core.Pool;
using System.Data.Common;
using Oracle.DataAccess.Client;
using System.Data.SqlClient;

namespace Vancl.TMS.Core.Base
{
    public class VanclDbConnection
    {
        /// <summary>
        /// TMS只读连接
        /// </summary>
        public static VanclObjectInPool<OracleConnection> TMSReadOnlyConnection
        {
            get { return GetConnection<OracleConnection>(Enums.ConnectionType.TMSReadOnly); }
        }
        /// <summary>
        /// TMS写连接
        /// </summary>
        public static VanclObjectInPool<OracleConnection> TMSWriteConnection
        {
            get { return GetConnection<OracleConnection>(Enums.ConnectionType.TMSWrite); }
        }

        public static VanclObjectInPool<T> GetConnection<T>(Enums.ConnectionType connectionType) where T : DbConnection
        {
            return ConnectionPool<T>.Get(Thread.CurrentThread.ManagedThreadId, connectionType);
        }

        /// <summary>
        /// LMSsql只读连接
        /// </summary>
        public static VanclObjectInPool<SqlConnection> LMSSqlReadOnlyConnection
        {
            get { return GetConnection<SqlConnection>(Enums.ConnectionType.LMSSqlReadOnly); }
        }

        /// <summary>
        /// LMSsql写连接
        /// </summary>
        public static VanclObjectInPool<SqlConnection> LMSSqlWriteConnection
        {
            get { return GetConnection<SqlConnection>(Enums.ConnectionType.LMSSqlWrite); }
        }

        /// <summary>
        /// LMSoracle只读连接
        /// </summary>
        public static VanclObjectInPool<OracleConnection> LMSOracleReadOnlyConnection
        {
            get { return GetConnection<OracleConnection>(Enums.ConnectionType.LMSOracleReadOnly); }
        }

        /// <summary>
        /// LMSoracle写连接
        /// </summary>
        public static VanclObjectInPool<OracleConnection> LMSOracleWriteConnection
        {
            get { return GetConnection<OracleConnection>(Enums.ConnectionType.LMSOracleWrite); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Base;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.Core.ACIDManager
{
    public class ACIDScopeFactory
    {
        /// <summary>
        /// 获得tms用事务对象
        /// </summary>
        /// <returns></returns>
        public static IACID GetTmsACID()
        {
            return new OracleDbACIDScope(System.Data.IsolationLevel.ReadCommitted, VanclDbConnection.TMSWriteConnection);
        }

        public static IACID GetACID(System.Data.IsolationLevel level, Enums.ConnectionType connectionType)
        {
            return new OracleDbACIDScope(level, VanclDbConnection.GetConnection<OracleConnection>(connectionType));
        }

        /// <summary>
        /// 获得lms用事务对象
        /// </summary>
        /// <returns></returns>
        //public static IACID GetLmsACID()
        //{
        //    return GetLmsSqlACID();
        //}

        /// <summary>
        /// 获取LMS物流主库【Oracle】事务对象
        /// </summary>
        /// <returns></returns>
        public static IACID GetLmsOracleACID()
        {
            return new OracleDbACIDScope(System.Data.IsolationLevel.ReadCommitted, VanclDbConnection.LMSOracleWriteConnection);
        }

        /// <summary>
        /// 获取LMS物流主库【SQL】事务对象
        /// </summary>
        /// <returns></returns>
        public static IACID GetLmsSqlACID()
        {
            return new SqlServerDbACIDScope(System.Data.IsolationLevel.ReadCommitted, VanclDbConnection.LMSSqlWriteConnection);
        }


    }
}

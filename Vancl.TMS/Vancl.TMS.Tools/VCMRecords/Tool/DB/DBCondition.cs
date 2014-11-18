using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Vancl.TMS.Tools.VCMRecords.Tool.DB
{
    /// <summary>
    ///  数据库查询条件项
    /// </summary>
    public class DBCondition
    {
        /// <summary>
        /// sql表达式 如(id = 123456)
        /// </summary>
        public string sqlExp;

        /// <summary>
        /// sql参数
        /// </summary>
        public SqlParameter sqlparmeter;

        /// <summary>
        /// 参数名
        /// </summary>
        public String paramName;

        public DBCondition()
        {
        }

        /// <summary>
        /// 查询条件项
        /// </summary>
        /// <param name="sqlExp">sql表达式</param>
        /// <param name="paramName">参数名</param>
        /// <param name="value">参数值</param>
        public DBCondition(String sqlExp, String paramName, object value)
        {
            this.sqlExp = sqlExp;
            sqlparmeter = new SqlParameter(paramName, value);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Vancl.TMS.Tools.VCMRecords.Tool.DataGrid;

namespace Vancl.TMS.Tools.VCMRecords.Tool.DB
{
    /// <summary>
    /// 数据库相关静态方法
    /// </summary>
    public static class DBFunc
    {
        /// <summary>
        /// 把一个字符串的数组转化为字符串（以interval隔开）
        /// </summary>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static string ArrayToString(String[] fileds, String interval = ",")
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < fileds.Length - 1; i++)
            {
                str.Append(String.Format(" {0} {1} ", fileds[i], interval));
            }
            str.Append(fileds[fileds.Length - 1]);
            return str.ToString();
        }

        /// <summary>
        /// 得到PageModel的查询Sql
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static string GetQuerySQLByPageModel(this PageModel pageModel)
        {
            IList<String> fields = new List<String>();
            foreach (Column column in pageModel.columns)
            {
                if (column.isSql)
                    fields.Add(column.name);
            }
            String rownumSql = String.Format("select ROW_NUMBER() over(order by {0} {3})  as rownum,{1} from {2}  ",
                ArrayToString(pageModel.orderFields), ArrayToString(fields.ToArray()), pageModel.tableName,pageModel.ordertype.ToString());
            if (String.IsNullOrEmpty(pageModel.whereClause) == false)
            {
                rownumSql += " where " + pageModel.whereClause;
            }

            return String.Format("select {0} from ({1}) t where t.rownum>{2} and t.rownum<{3}",
                ArrayToString(fields.ToArray()), rownumSql, pageModel.currentPage * pageModel.pageSize, pageModel.currentPage * pageModel.pageSize + pageModel.pageSize);
        }

        /// <summary>
        /// 得到PageModel的总记录数的sql
        /// </summary>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public static String GetQueryCountSqlByPageModel(this PageModel pageModel)
        {
            String sql = string.Format("select count(*) from {0} ", pageModel.tableName);
            if (String.IsNullOrEmpty(pageModel.whereClause) == false)
            {
                sql += " where " + pageModel.whereClause;
            }
            return sql;
        }

        /// <summary>
        /// 根据DBConnection数组获得sql表达式和sql参数数组
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="parameters"></param>
        public static String GetSqlAndParametersByConditions(DBCondition[] conditions, out SqlParameter[] parameters)
        {
            List<SqlParameter> parametersList = new List<SqlParameter>();
            List<String> whereItems = new List<string>();
            foreach (DBCondition condition in conditions)
            {
                whereItems.Add(condition.sqlExp);
                parametersList.Add(condition.sqlparmeter);
            }
            parameters = parametersList.ToArray();
            return ArrayToString(whereItems.ToArray(), "and");
        }

    }
}
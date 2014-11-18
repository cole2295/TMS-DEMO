using System.Data.SqlClient;
using Vancl.TMS.Model.Common;
using System.Threading;
using Vancl.TMS.Core.Pool;
using Vancl.TMS.Util.DbHelper;
using Vancl.TMS.Util.Pager;
using System.Collections.Generic;
using Vancl.TMS.Model;
using System.Data;
using Vancl.TMS.Core.Base;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.DAL.Sql2008
{
    public class BaseDAL
    {
        /// <summary>
        /// 只读连接
        /// </summary>
        public VanclObjectInPool<SqlConnection> LMSReadOnlyConnection
        {
            get { return VanclDbConnection.LMSSqlReadOnlyConnection; }
        }

        /// <summary>
        /// 写连接
        /// </summary>
        public VanclObjectInPool<SqlConnection> LMSWriteConnection
        {
            get { return VanclDbConnection.LMSSqlWriteConnection; }
        }

        public int ExecuteNonQuery(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return SqlHelper.ExecuteNonQuery((SqlTransaction)dataContext.Transaction, System.Data.CommandType.Text, commandText, commandParameters);
                }
                else
                {
                    return SqlHelper.ExecuteNonQuery(connection.PoolObject, System.Data.CommandType.Text, commandText, commandParameters);
                }
            }
        }

        public IList<T> ExecuteReaderPagedByTopGrammar<T>(VanclObjectInPool<SqlConnection> connection, string commandText, PageInfo pageInfo, params SqlParameter[] commandParameters) where T : BaseModel, new()
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReaderPagedByTopGrammar((SqlTransaction)dataContext.Transaction, commandText, pageInfo, commandParameters));
                }
                else
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReaderPagedByTopGrammar(connection.PoolObject, commandText, pageInfo, commandParameters));
                }
            }
        }

        /// <summary>
        /// 返回分页后的结果对象列表(reader反射)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="condition"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public PagedList<T> ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<T>(VanclObjectInPool<SqlConnection> connection, string commandText, BaseSearchModel condition, params SqlParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                PageInfo info = new PageInfo() { CurrentPageIndex = condition.PageIndex, PageSize = condition.PageSize, SortString = condition.OrderByString };
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                IList<T> list = null;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    list = new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReaderPagedByRowNumberGrammar((SqlTransaction)dataContext.Transaction, commandText, info, commandParameters));
                }
                else
                {
                    list = new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReaderPagedByRowNumberGrammar(connection.PoolObject, commandText, info, commandParameters));
                }
                return PagedList.Create(list, info.CurrentPageIndex, info.PageSize, info.ItemCount);
            }
        }

        public object ExecuteScalar(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return SqlHelper.ExecuteScalar((SqlTransaction)dataContext.Transaction, System.Data.CommandType.Text, commandText, commandParameters);
                }
                else
                {
                    return SqlHelper.ExecuteScalar(connection.PoolObject, System.Data.CommandType.Text, commandText, commandParameters);
                }
            }
        }


        public DataSet ExecuteDataset(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return SqlHelper.ExecuteDataset((SqlTransaction)dataContext.Transaction, System.Data.CommandType.Text, commandText, commandParameters);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(connection.PoolObject, System.Data.CommandType.Text, commandText, commandParameters);
                }
            }
        }

        /// <summary>
        /// 返回结果对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public IList<T> ExecuteSql_ByDataTableReflect<T>(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteDataTable((SqlTransaction)dataContext.Transaction, commandText, commandParameters));
                }
                else
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteDataTable(connection.PoolObject, commandText, commandParameters));

                }
            }
        }

        /// <summary>
        /// 返回单个结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public T ExecuteSqlSingle_ByReaderReflect<T>(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return new T().SetValueFromDB(SqlHelper.ExecuteReader((SqlTransaction)dataContext.Transaction, CommandType.Text, commandText, commandParameters));
                }
                else
                {
                    return new T().SetValueFromDB(SqlHelper.ExecuteReader(connection.PoolObject, CommandType.Text, commandText, commandParameters));
                }
            }
        }

        /// <summary>
        /// 返回单个结果对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public T ExecuteSqlSingle_ByDataTableReflect<T>(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return new T().SetValueFromDB(SqlHelper.ExecuteDataTable((SqlTransaction)dataContext.Transaction, commandText, commandParameters));
                }
                else
                {
                    return new T().SetValueFromDB(SqlHelper.ExecuteDataTable(connection.PoolObject, commandText, commandParameters));
                }
            }
        }

        /// <summary>
        /// 返回结果对象列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public IList<T> ExecuteSql_ByReaderReflect<T>(VanclObjectInPool<SqlConnection> connection, string commandText, params SqlParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                ACIDDataContext dataContext = connection.DataContext as ACIDDataContext;
                if (dataContext != null && dataContext.Transaction != null)
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReader((SqlTransaction)dataContext.Transaction, CommandType.Text, commandText, commandParameters));
                }
                else
                {
                    return new List<T>().SetValueFromDB<T>(SqlHelper.ExecuteReader(connection.PoolObject, CommandType.Text, commandText, commandParameters));
                }
            }
        }
    }
}

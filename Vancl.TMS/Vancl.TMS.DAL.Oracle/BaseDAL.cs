using Vancl.TMS.Model.Common;
using System.Threading;
using Vancl.TMS.Core.Pool;
using Vancl.TMS.Model;
using System.Collections.Generic;
using Vancl.TMS.Util.DbHelper;
using Vancl.TMS.Util.Pager;
using Oracle.DataAccess.Client;
using Vancl.TMS.Core.Base;
using System.Data;
using System;
using Vancl.TMS.IDAL;


namespace Vancl.TMS.DAL.Oracle
{
    public abstract class BaseDAL : ISequenceDAL
    {
        /// <summary>
        /// TMS只读连接
        /// </summary>
        public VanclObjectInPool<OracleConnection> TMSReadOnlyConnection
        {
            get { return VanclDbConnection.TMSReadOnlyConnection; }
        }

        /// <summary>
        /// TMS写连接
        /// </summary>
        public VanclObjectInPool<OracleConnection> TMSWriteConnection
        {
            get { return VanclDbConnection.TMSWriteConnection; }
        }

        /// <summary>
        /// LMS物流主库只读连接
        /// </summary>
        public VanclObjectInPool<OracleConnection> LMSOracleReadOnlyConnection
        {
            get { return VanclDbConnection.LMSOracleReadOnlyConnection; }
        }

        /// <summary>
        /// LMS物流主库写连接
        /// </summary>
        public VanclObjectInPool<OracleConnection> LMSOracleWriteConnection
        {
            get { return VanclDbConnection.LMSOracleWriteConnection; }
        }


        /// <summary>
        /// 传入数组参数批量执行
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="commandText"></param>
        /// <param name="arrayCount"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public int ExecuteSqlArrayNonQuery(VanclObjectInPool<OracleConnection> connection, string commandText, int arrayCount, params OracleParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                return OracleHelper.ExecuteSqlArrayNonQuery(connection.PoolObject, commandText, arrayCount, commandParameters);
            }
        }

        public int ExecuteSqlNonQuery(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                return OracleHelper.ExecuteSqlNonQuery(connection.PoolObject, commandText, commandParameters);
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
        public PagedList<T> ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<T>(VanclObjectInPool<OracleConnection> connection, string commandText, BaseSearchModel condition, params OracleParameter[] commandParameters) where T :  new()
        {
            using (connection)
            {
                connection.Open();
                PageInfo info = new PageInfo() { CurrentPageIndex = condition.PageIndex, PageSize = condition.PageSize, SortString = condition.OrderByString };
                IList<T> list = new List<T>().SetValueFromDB<T>(OraclePagingHelper.ExecuteSqlDataReader(connection.PoolObject, commandText, info, commandParameters));
                return PagedList.Create(list, info.CurrentPageIndex, info.PageSize, info.ItemCount);
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
        public IList<T> ExecuteSql_ByReaderReflect<T>(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters) where T :  new()
        {
            using (connection)
            {
                connection.Open();
                return new List<T>().SetValueFromDB<T>(OracleHelper.ExecuteSqlReader(connection.PoolObject, commandText, commandParameters));
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
        public IList<T> ExecuteSql_ByDataTableReflect<T>(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters) where T :  new()
        {
            using (connection)
            {
                connection.Open();
                return new List<T>().SetValueFromDB<T>(OracleHelper.ExecuteSqlDataTable(connection.PoolObject, commandText, commandParameters));
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
        public T ExecuteSqlSingle_ByDataTableReflect<T>(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters) where T : new()
        {
            using (connection)
            {
                connection.Open();
                return new T().SetValueFromDB(OracleHelper.ExecuteSqlDataTable(connection.PoolObject, commandText, commandParameters));
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
        public T ExecuteSqlSingle_ByReaderReflect<T>(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters) where T :  new()
        {
            using (connection)
            {
                connection.Open();
                return new T().SetValueFromDB(OracleHelper.ExecuteSqlReader(connection.PoolObject, commandText, commandParameters));
            }
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable ExecuteSqlDataTable(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                return OracleHelper.ExecuteSqlDataTable(connection.PoolObject, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 返回Scalar
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public object ExecuteSqlScalar(VanclObjectInPool<OracleConnection> connection, string commandText, params OracleParameter[] commandParameters)
        {
            using (connection)
            {
                connection.Open();
                return OracleHelper.ExecuteSqlScalar(connection.PoolObject, commandText, commandParameters);
            }
        }

        /// <summary>
        /// 取得序列值
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        /// <returns></returns>
        public virtual long GetNextSequence(string sequenceName)
        {
            string strSql = string.Format(@"
                SELECT {0}.NEXTVAL FROM dual
            ", sequenceName);
            object o = ExecuteSqlScalar(TMSWriteConnection, strSql);
            return o == null ? 0 : Convert.ToInt64(o);
        }

        /// <summary>
        /// 取得提货单完成状态
        /// </summary>
        /// <returns></returns>
        public List<int> GetDeliveryFinishedStatus()
        {
            List<int> listStatus = new List<int>();
            listStatus.Add((int)Enums.DeliveryStatus.AllLost);
            listStatus.Add((int)Enums.DeliveryStatus.ArrivedDelay);
            listStatus.Add((int)Enums.DeliveryStatus.ArrivedOnTime);
            listStatus.Add((int)Enums.DeliveryStatus.KPIApproved);
            return listStatus;
        }

        /// <summary>
        /// 取得提货单未完成状态
        /// </summary>
        /// <returns></returns>
        public List<int> GetDeliveryNotFinishStatus()
        {
            List<int> listStatus = new List<int>();
            listStatus.Add((int)Enums.DeliveryStatus.Dispatched);
            listStatus.Add((int)Enums.DeliveryStatus.InTransit);
            listStatus.Add((int)Enums.DeliveryStatus.WaitForDispatch);
            return listStatus;
        }

        /// <summary>
        /// oracle in 绑定变量
        /// </summary>
        /// <param name="columnName">查询字段名称</param>
        /// <param name="parameterName">变量名称 不需要：</param>
        /// <param name="isNotIn">true为not in，false为in</param>
        /// <param name="isOr">true为or，false为and</param>
        /// <returns></returns>
        public string GetOracleInParameterWhereSql(string columnName, string parameterName, bool isNotIn,bool haveOrOrAnd, bool isOr)
        {
            var inOrNotinStr = isNotIn ? " NOT IN " : " IN ";
            var orOrAndStr = "";
            if(haveOrOrAnd)
            {
                orOrAndStr = isOr ? " OR " : " AND ";
            }
            string inWhereSql = " {0} {1} {2} (select regexp_substr(:{3},'[^,]+',1,level) as value_id from dual connect by level<=length(trim(translate(:{3},translate(:{3},',',' '),' ')))+1) ";
            return string.Format(inWhereSql, orOrAndStr, columnName, inOrNotinStr, parameterName);
        }


    }
}

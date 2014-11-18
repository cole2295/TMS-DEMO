using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;

namespace Vancl.TMS.Tools.VCMRecords.DAL
{
    /// <summary>
    /// DAL的基类
    /// </summary>
    public class BaseDAL
    {
        /// <summary>
        /// 显示列
        /// </summary>
        protected string displayColumns = "";

        /// <summary>
        /// 数据表名
        /// </summary>
        protected string tablename = "";
        
        /// <summary>
        /// 主键名
        /// </summary>
        protected String primaryKey = "ID";

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string connectionString = DBConn.VCMRecordsConnctionString;

        /// <summary>
        /// 按参数查找
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public DataTable FindByCondition(DBCondition[] conditions)
        {
            SqlParameter[] parameters;
            String whereSql = DBFunc.GetSqlAndParametersByConditions(conditions, out parameters);
            String sql = String.Format("select {0} from {2} where {1}", displayColumns, whereSql,tablename);
            DataTable resultTab = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, parameters).Tables[0];
            return resultTab;           
        }

        /// <summary>
        /// 按参数查找
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public DataTable FindByCondition(DBCondition[] conditions, String displayColumns)
        {
            SqlParameter[] parameters;
            String whereSql = DBFunc.GetSqlAndParametersByConditions(conditions, out parameters);
            String sql = String.Format("select {0} from {2} where {1}", displayColumns, whereSql, tablename);
            DataTable resultTab = SqlHelper.ExecuteDataset(connectionString, CommandType.Text, sql, parameters).Tables[0];
            return resultTab;
        }

        /// <summary>
        /// 根据主键获得对象实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable find<T>(T entity)
        {
            return null;
        }

        /// <summary>
        /// 创建一个新对象(默认ID不传入值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnResult Create<T>(T entity)
        {
            Type type = entity.GetType();
            List<String> columns = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<String> values = new List<string>();
            String idSql = "";
            SqlParameter idParameter = null;
            PropertyInfo idPro = null;
            foreach (PropertyInfo pro in type.GetProperties())
            {
                if (pro.Name == primaryKey)
                {
                    idSql = String.Format("SELECT  @{0}= @@identity",pro.Name);
                    idParameter = new SqlParameter("@" + pro.Name,pro.GetValue(entity,null));
                    idParameter.Direction = ParameterDirection.Output;
                    parameters.Add(idParameter);
                    idPro = pro;
                    continue;
                }
                columns.Add(pro.Name);
                values.Add("@" + pro.Name);                
                parameters.Add(new SqlParameter("@" + pro.Name, pro.GetValue(entity,null)));                
            }
            String columnSql = DBFunc.ArrayToString(columns.ToArray(), ",");
            String valuesSql = DBFunc.ArrayToString(values.ToArray());
            String createSql = String.Format(
                "INSERT INTO {0}" +
                    "({1})" +
                    "VALUES({2})",
                    tablename, columnSql, valuesSql);


            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, createSql+" ; "+idSql, parameters.ToArray());
                if (idPro != null)
                {                    
                    idPro.SetValue(entity,idParameter.Value ,null);//给ID赋值
                }
                return new ReturnResult(true);
            }
            catch (Exception ex)
            {
                Fault fault = new Fault("添加失败",ex);
                ReturnResult falutResult = new ReturnResult(false, fault);
                return falutResult;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnResult Update<T>(T entity)
        {
            Type type = entity.GetType();
            List<String> updateItems = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            String where = "";
            foreach (PropertyInfo pro in type.GetProperties())
            {
                if (pro.Name == "ID")
                {
                    where = String.Format("{0}=@{0}", pro.Name);
                    parameters.Add(new SqlParameter("@" + pro.Name, pro.GetValue(entity,null)));
                    continue;
                }
                updateItems.Add(String.Format("{0}=@{0}", pro.Name));
                parameters.Add(new SqlParameter("@" + pro.Name, pro.GetValue(entity, null)));
            }
            String updateSql = String.Format(
                "UPDATE {0} SET {1}  WHERE {2}",
                tablename,DBFunc.ArrayToString( updateItems.ToArray()),where
                );
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, updateSql, parameters.ToArray());
                return new ReturnResult(true);
            }
            catch (Exception ex)
            {
                Fault fault = new Fault(ex.Message, ex);
                ReturnResult falutResult = new ReturnResult(false, fault);
                return falutResult;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ReturnResult Remove<T>(T entity)
        {
            Object keyValue = getPrimaryKeyValue<T>(entity);
            if (keyValue == null)
            {
                return new ReturnResult(false, new Fault("没有找到主键值", null));
            }
            string delSql = String.Format(
                "DELETE FROM {0} WHERE {1} = @{1}",tablename,primaryKey
                );
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@"+primaryKey,keyValue) };
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, delSql, parameters);
                return new ReturnResult(true);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, new Fault(ex.Message, ex));
            }
        }

        /// <summary>
        /// 得到主键字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Object getPrimaryKeyValue<T>(T entity)
        {
            foreach (FieldInfo field in  entity.GetType() .GetFields())
            {
                if (field.Name == primaryKey)
                    return field.GetValue(entity);
            }
            foreach (PropertyInfo pro in entity.GetType().GetProperties())
            {
                if (pro.Name == primaryKey)
                    return pro.GetValue(entity, null);
            }
            return null;
        }
        
    }
}

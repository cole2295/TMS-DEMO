using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMS.FMS.Model;
using RFD.FMS.MODEL.MetaData;
using System.Reflection;
using LMS.Util;
using System.Data;

namespace RFD.FMS.DAL.Core
{
    public class DaoHelper
    {
        private static IDictionary<string, string> dicExistsSql = new Dictionary<string, string>();
        private static IDictionary<string, string> dicAddSql = new Dictionary<string, string>();
        private static IDictionary<string, string> dicUpdateSql = new Dictionary<string, string>();
        private static IDictionary<string, string> dicGetModelSql = new Dictionary<string, string>();
        private static IDictionary<string, string> dicGetListSql = new Dictionary<string, string>();

        public static string Exists(string className, string idValue)
        {
            string sql = "";

            if (dicExistsSql.ContainsKey(className) == true)
            {
                sql = dicExistsSql[className];
            }
            else
            {
                MetaClass metaClass = MetaManager.GetMetaClass(className);

                MetaProperty primaryProperty = metaClass.PrimaryProperty;

                StringBuilder builder = new StringBuilder();

                builder.Append("select count(*) from ");
                builder.Append(metaClass.TableName);
                builder.Append(" where ");
                builder.Append(primaryProperty.DbName);
                builder.Append(" = ");

                if (primaryProperty.DbType == System.Data.SqlDbType.Text)
                {
                    builder.Append("'{0}'");
                }
                else
                {
                    builder.Append("{0}");
                }

                dicExistsSql.Add(className, sql);
            }

            sql = String.Format(sql, idValue);

            return sql;
        }

        public static string Add(string className, object model)
        {
            string sql = "";

            int index = -1;

            MetaProperty tempProperty = null;
            MetaClass metaClass = null;

            if (dicAddSql.ContainsKey(className) == true)
            {
                sql = dicAddSql[className];
            }
            else
            {
                metaClass = MetaManager.GetMetaClass(className);

                StringBuilder fieldBuilder = new StringBuilder();
                StringBuilder valueBuilder = new StringBuilder();

                for (int i = 0; i < metaClass.Propertys.Count; i++)
                {
                    tempProperty = metaClass.Propertys[i];

                    if (metaClass.IdAutoCreate == true && tempProperty.IsPrimary == true) continue;

                    index++;

                    fieldBuilder.Append(tempProperty.DbName);
                    fieldBuilder.Append(",");

                    if (tempProperty.DbType == System.Data.SqlDbType.Text)
                    {
                        valueBuilder.Append("'{");
                        valueBuilder.Append(index);
                        valueBuilder.Append("}'");
                    }
                    else
                    {
                        valueBuilder.Append("{");
                        valueBuilder.Append(index);
                        valueBuilder.Append("}");
                    }

                    valueBuilder.Append(",");
                }

                fieldBuilder = fieldBuilder.Remove(fieldBuilder.Length - 1, 1);
                valueBuilder = valueBuilder.Remove(valueBuilder.Length - 1, 1);

                StringBuilder builder = new StringBuilder();

                builder.Append("insert into ");
                builder.Append(metaClass.TableName);
                builder.Append("(");
                builder.Append(fieldBuilder.ToString());
                builder.Append(")");
                builder.Append(" values( ");
                builder.Append(valueBuilder.ToString());
                builder.Append(") ");

                if (metaClass.IdAutoCreate == true)
                {
                    builder.Append(";select @@IDENTITY");
                }

                sql = builder.ToString();

                dicAddSql.Add(className, sql);
            }

            Type modelType = model.GetType();
            PropertyInfo propertyInfo = null;

            index = -1;
            object value = "";

            for (int i = 0; i < metaClass.Propertys.Count; i++)
            {
                tempProperty = metaClass.Propertys[i];

                if (metaClass.IdAutoCreate == true && tempProperty.IsPrimary == true) continue;

                index++;

                propertyInfo = modelType.GetProperty(tempProperty.PropertyName);

                value = propertyInfo.GetValue(model, null);

                sql = sql.Replace("{"+index+"}",DataConvert.ToString(value));
            }

            return sql;
        }

        public static string Update(string className, object model)
        {
            string sql = "";

            int index = -1;

            MetaProperty tempProperty = null;
            MetaClass metaClass = null;
            MetaProperty primaryProperty = metaClass.PrimaryProperty;

            if (dicUpdateSql.ContainsKey(className) == true)
            {
                sql = dicUpdateSql[className];
            }
            else
            {
                metaClass = MetaManager.GetMetaClass(className);

                StringBuilder builder = new StringBuilder();

                builder.Append("update ");
                builder.Append(metaClass.TableName);
                builder.Append(" set ");

                for (int i = 0; i < metaClass.Propertys.Count; i++)
                {
                    tempProperty = metaClass.Propertys[i];

                    if (tempProperty.IsPrimary == true) continue;

                    index++;

                    builder.Append(tempProperty.DbName);
                    builder.Append("=");

                    if (tempProperty.DbType == System.Data.SqlDbType.Text)
                    {
                        builder.Append("'{");
                        builder.Append(index);
                        builder.Append("}'");
                    }
                    else
                    {
                        builder.Append("{");
                        builder.Append(index);
                        builder.Append("}");
                    }

                    builder.Append(",");
                }

                builder = builder.Remove(builder.Length - 1, 1);

                builder.Append(" where ");
                builder.Append(primaryProperty.DbName);
                builder.Append("={1000}");

                sql = builder.ToString();

                dicUpdateSql.Add(className, sql);
            }

            Type modelType = model.GetType();
            PropertyInfo propertyInfo = null;

            index = -1;
            object value = "";

            for (int i = 0; i < metaClass.Propertys.Count; i++)
            {
                tempProperty = metaClass.Propertys[i];

                if (tempProperty.IsPrimary == true) continue;

                index++;

                propertyInfo = modelType.GetProperty(tempProperty.PropertyName);

                value = propertyInfo.GetValue(model, null);

                sql = sql.Replace("{" + index + "}", DataConvert.ToString(value));
            }

            propertyInfo = modelType.GetProperty(primaryProperty.PropertyName);

            value = propertyInfo.GetValue(model, null);

            sql = sql.Replace("{1000}", DataConvert.ToString(value));

            return sql;
        }

        public static string Delete(string className, string id)
        {
            MetaClass metaClass = MetaManager.GetMetaClass(className);

            MetaProperty primaryProperty = metaClass.PrimaryProperty;

            StringBuilder builder = new StringBuilder();

            builder.Append("delete from ");
            builder.Append(metaClass.TableName);
            builder.Append(" where ");
            builder.Append(primaryProperty.PropertyName);
            builder.Append("=");

            if (primaryProperty.DbType == System.Data.SqlDbType.Text)
            {
                builder.Append("'");
                builder.Append(id);
                builder.Append("'");
            }
            else
            {
                builder.Append(id);
            }

            return builder.ToString();
        }

        public static string DeleteCondition(string className, string condition)
        {
            MetaClass metaClass = MetaManager.GetMetaClass(className);

            StringBuilder builder = new StringBuilder();

            builder.Append("delete from ");
            builder.Append(metaClass.TableName);
            builder.Append(" where ");
            builder.Append(condition);

            return builder.ToString();
        }

        public static string DeleteList(string className, string ids)
        {
            MetaClass metaClass = MetaManager.GetMetaClass(className);

            MetaProperty primaryProperty = metaClass.PrimaryProperty;

            StringBuilder builder = new StringBuilder();

            builder.Append("delete from ");
            builder.Append(metaClass.TableName);
            builder.Append(" where ");
            builder.Append(primaryProperty.PropertyName);
            builder.Append(" in(");
            builder.Append(ids);
            builder.Append(")");

            return builder.ToString();
        }

        public static string GetModel(string className, string id)
        {
            string sql = "";

            MetaClass metaClass = MetaManager.GetMetaClass(className);

            MetaProperty primaryProperty = metaClass.PrimaryProperty;

            if (dicGetModelSql.ContainsKey(className) == true)
            {
                sql = dicGetModelSql[className];
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("select ");

                MetaProperty tempProperty = null;

                for (int i = 0; i < metaClass.Propertys.Count; i++)
                {
                    tempProperty = metaClass.Propertys[i];

                    builder.Append(tempProperty.DbName);

                    if (i != metaClass.Propertys.Count - 1)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append(" from ");
                builder.Append(metaClass.TableName);
                builder.Append(" where ");
                builder.Append(primaryProperty.DbName);
                
                if (primaryProperty.DbType == System.Data.SqlDbType.Text)
                {
                    builder.Append("='{0}'");
                }
                else
                {
                    builder.Append("={0}");
                }

                sql = builder.ToString();

                dicGetModelSql.Add(className,sql);
            }

            sql = String.Format(sql, id);

            return sql;
        }

        public static string GetList(string className, string condition)
        {
            string sql = "";

            MetaClass metaClass = MetaManager.GetMetaClass(className);

            MetaProperty primaryProperty = metaClass.PrimaryProperty;

            if (dicGetListSql.ContainsKey(className) == true)
            {
                sql = dicGetListSql[className];
            }
            else
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("select ");

                MetaProperty tempProperty = null;

                for (int i = 0; i < metaClass.Propertys.Count; i++)
                {
                    tempProperty = metaClass.Propertys[i];

                    builder.Append(tempProperty.DbName);

                    if (i != metaClass.Propertys.Count - 1)
                    {
                        builder.Append(",");
                    }
                }

                builder.Append(" from ");
                builder.Append(metaClass.TableName);
                builder.Append(" where ");

                sql = builder.ToString();

                dicGetListSql.Add(className, sql);
            }

            sql += condition;

            return sql;
        }

        public static T DataRowToObject<T>(DataRow dataRow)
        {
            MetaClass metaClass = MetaManager.GetMetaClass(typeof(T).FullName);

            MetaProperty tempProperty = null;

            T model = Activator.CreateInstance<T>();

            Type modelType = model.GetType();

            PropertyInfo propertyInfo = null;

            for (int i = 0; i < metaClass.Propertys.Count; i++)
            {
                tempProperty = metaClass.Propertys[i];

                propertyInfo = modelType.GetProperty(tempProperty.PropertyName);

                propertyInfo.SetValue(model, dataRow[tempProperty.DbName], null);
            }

            return model;
        }
    }
}

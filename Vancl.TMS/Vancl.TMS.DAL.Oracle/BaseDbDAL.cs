using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL;
using Vancl.TMS.Model;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.DbAttributes;
using System.Data;
using System.Reflection;

namespace Vancl.TMS.DAL.Oracle
{
    /// <summary>
    /// 基于数据库单表的数据CRUD访问封装
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseDbDAL<TModel, TKey> : BaseDAL, IDbModelDAL<TModel, TKey> where TModel : BaseDbModel<TKey>, new()
    {
        /// <summary>
        /// 缓存dbmodel信息，提高效率
        /// </summary>
        private static List<DbModelInfo> CacheDbModelInfo = new List<DbModelInfo>();
        private static object lockObj = new object();
        private DbModelInfo DbModelInfo
        {
            get
            {
                if (CacheDbModelInfo.Any(x => x.DbModelType == typeof(TModel)))
                {
                    return CacheDbModelInfo.FirstOrDefault(x => x.DbModelType == typeof(TModel));
                }
                else
                {
                    lock (lockObj)
                    {
                        ///确保线程安全，先检查一次
                        if (CacheDbModelInfo.Any(x => x.DbModelType == typeof(TModel)))
                        {
                            return CacheDbModelInfo.FirstOrDefault(x => x.DbModelType == typeof(TModel));
                        }
                        var modelInfo = new DbModelInfo();
                        MemberInfo info = typeof(TModel);
                        var tableAttr = (TableAttribute)info.GetCustomAttributes(true).FirstOrDefault(x => x is TableAttribute);

                        modelInfo.DbModelType = typeof(TModel);
                        modelInfo.TableName = tableAttr == null ? typeof(TModel).Name : tableAttr.Name;
                        typeof(TModel).GetProperties().ToList().ForEach(property =>
                        {
                            var columAttr = (ColumnAttribute)property.GetCustomAttributes(true).FirstOrDefault(x => x is ColumnAttribute);
                            if (columAttr == null) return;
                            var columnName = String.IsNullOrWhiteSpace(columAttr.Name) ? property.Name : columAttr.Name;
                            DbType dbType = columAttr.DbType.HasValue ? columAttr.DbType.Value : ConvertToDbType(property.PropertyType);
                            modelInfo.Columns.Add(new DbModelInfo.ColumnInfo()
                            {
                                ColumnName = columnName,
                                DbType = dbType,
                                IsKey = columAttr.IsKey,
                                PropertyInfo = property,
                            });
                        });
                        return modelInfo;
                    }
                }
            }
        }

        private DbType ConvertToDbType(Type type)
        {
            if (type == typeof(string)) return DbType.String;
            else if (type.IsEnum) return DbType.Int32;
            else if (type == typeof(Int64)) return DbType.Int64;
            else if (type == typeof(Int32)) return DbType.Int32;
            else if (type == typeof(bool)) return DbType.Int32; //oracle中没有bool
            else if (type == typeof(DateTime)) return DbType.DateTime;
            else if (type == typeof(decimal)) return DbType.Decimal;
            else if (type == typeof(float)) return DbType.Double;
            else return DbType.Object;
        }

        public virtual TModel Get(TKey id)
        {
            var keyColumn = DbModelInfo.Columns.FirstOrDefault(x => x.IsKey);
            StringBuilder SbSql = new StringBuilder();
            SbSql.AppendLine("SELECT");
            for (int i = 0; i < DbModelInfo.Columns.Count; i++)
            {
                var c = DbModelInfo.Columns[i];
                SbSql.AppendLine(c.ColumnName
                    + (i == DbModelInfo.Columns.Count - 1 ? string.Empty : ",")
                 );
            }
            SbSql.AppendLine("FROM " + DbModelInfo.TableName);
            SbSql.AppendLine("WHERE " + keyColumn.ColumnName + "=:p0");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "p0", DbType = keyColumn.DbType, Value = id } 
            };
            return ExecuteSqlSingle_ByDataTableReflect<TModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }

        public virtual int Add(TModel model)
        {
            var keyColumn = DbModelInfo.Columns.FirstOrDefault(x => x.IsKey);
            var OtherColumn = DbModelInfo.Columns.Where(x => !x.IsKey).ToList();
            var useSequence = model is ISequenceable && model.Id.Equals(default(TKey));
            StringBuilder SbSql = new StringBuilder();
            SbSql.AppendLine("INSERT INTO " + DbModelInfo.TableName);
            SbSql.AppendLine("(");
            SbSql.Append(keyColumn.ColumnName + ",");
            for (int i = 0; i < OtherColumn.Count; i++)
            {
                var c = OtherColumn[i];
                SbSql.Append(c.ColumnName
                    + (i == OtherColumn.Count - 1 ? "\r\n" : ",")
                 );
            }
            SbSql.AppendLine(")VALUES(");
            if (useSequence) SbSql.Append((model as ISequenceable).SequenceName + ".NEXTVAL,");
            else SbSql.Append(":p0,");
            for (int i = 0; i < OtherColumn.Count; i++)
            {
                var c = OtherColumn[i];
                SbSql.Append(":p" + (i + 1)
                    + (i == OtherColumn.Count - 1 ? "\r\n" : ","));
            }
            SbSql.AppendLine(")");

            Console.WriteLine(SbSql.ToString());

            List<OracleParameter> arguments = new List<OracleParameter>();
            if (!useSequence)
                arguments.Add(new OracleParameter() { ParameterName = "p0", DbType = keyColumn.DbType, Value = keyColumn.PropertyInfo.GetValue(model, null) });
            for (int i = 0; i < OtherColumn.Count; i++)
            {
                var c = OtherColumn[i];
                arguments.Add(new OracleParameter() { ParameterName = "p" + (i + 1), DbType = c.DbType, Value = c.PropertyInfo.GetValue(model, null) });
            }
            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments.ToArray());
        }

        public virtual int Update(TModel model)
        {
            var oldModel = this.Get(model.Id);
            if (oldModel == null) return 0;//throw Exception
            var keyColumn = DbModelInfo.Columns.FirstOrDefault(x => x.IsKey);
            var OtherColumn = DbModelInfo.Columns.Where(x => !x.IsKey).ToList();
            var isSequenceModel = model is ISequenceable;
            StringBuilder SbSql = new StringBuilder();
            SbSql.AppendLine("UPDATE " + DbModelInfo.TableName);
            SbSql.AppendLine("SET");
            List<OracleParameter> arguments = new List<OracleParameter>();
            StringBuilder subColumn = new StringBuilder();
            for (int i = 0; i < OtherColumn.Count; i++)
            {
                var c = OtherColumn[i];
                var newValue = c.PropertyInfo.GetValue(model, null);
                var oldValue= c.PropertyInfo.GetValue(oldModel, null);
                if (newValue == null && oldValue == null) continue;
                if (newValue.Equals(oldValue)) continue;

                subColumn.Append(c.ColumnName + "=:p" + (i + 1) + ",");
                arguments.Add(new OracleParameter() { ParameterName = "p" + (i + 1), DbType = c.DbType, Value = newValue });
            }
            if (arguments.Count == 0) return 1;//没有需要更新的列

            SbSql.Append(subColumn.ToString().TrimEnd(','));
            SbSql.AppendLine("\r\nWHERE " + keyColumn.ColumnName + "=:p0");
            arguments.Add(new OracleParameter() { ParameterName = "p0", DbType = keyColumn.DbType, Value = keyColumn.PropertyInfo.GetValue(model, null) });

            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments.ToArray());
        }

        public virtual int Delete(TKey id)
        {
            var keyColumn = DbModelInfo.Columns.FirstOrDefault(x => x.IsKey);
            StringBuilder SbSql = new StringBuilder();
            List<OracleParameter> arguments = new List<OracleParameter>();

            SbSql.AppendLine("UPDATE " + DbModelInfo.TableName);
            SbSql.AppendLine("SET Isdeleted=1");

            SbSql.AppendLine("WHERE " + keyColumn.ColumnName + "=:p0");
            arguments.Add(new OracleParameter() { ParameterName = "p0", DbType = keyColumn.DbType, Value = id });

            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments.ToArray());
        }
    }

    class DbModelInfo
    {
        public DbModelInfo()
        {
            Columns = new List<ColumnInfo>();
        }
        public Type DbModelType { get; set; }
        public string TableName { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public struct ColumnInfo
        {
            public string ColumnName { get; set; }
            public DbType DbType { get; set; }
            public bool IsKey { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
        }
    }
}

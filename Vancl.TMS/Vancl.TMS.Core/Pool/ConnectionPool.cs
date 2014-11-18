using System;
using System.Collections;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.ConfigUtil;
using System.Data.Common;

namespace Vancl.TMS.Core.Pool
{
    /// <summary>
    /// 数据库连接池对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionPool<T> where T : DbConnection
    {
        /// <summary>
        /// 缓存事务中的连接
        /// </summary>
        static readonly Hashtable context = new Hashtable();
        
        public static VanclObjectInPool<T> Get(int key, Enums.ConnectionType connectionType)
        {
            if (context.Contains(key))
            {
                return context[key] as VanclObjectInPool<T>;
            }

            //没有缓存,则创建新连接
            VanclObjectInPool<T> con = GetNewConnection(connectionType);
            return con;
        }

        public static void AddConnectionCache(int key, VanclObjectInPool<T> vo)
        {
            if (!context.Contains(key))
            {
                context.Add(key, vo);
            }
        }

        public static void ReMoveConnectionCache(int key)
        {
            if (context.Contains(key))
            {
                context.Remove(key);
            }
        }

        private static VanclObjectInPool<T> GetNewConnection(Enums.ConnectionType connectionType)
        {
            string conKey = "";
            switch (connectionType)
            {
                case Enums.ConnectionType.TMSReadOnly:
                    conKey = Consts.TMS_READONLY_CONNECTION_KEY;
                    break;
                case Enums.ConnectionType.TMSWrite:
                    conKey = Consts.TMS_WRITE_CONNECTION_KEY;
                    break;
                case Enums.ConnectionType.LMSSqlReadOnly:
                    conKey = Consts.LMS_RFD_READONLY_CONNECTION_KEY;
                    break;
                case Enums.ConnectionType.LMSSqlWrite:
                    conKey = Consts.LMS_RFD_WRITE_CONNECTION_KEY;
                    break;
                case Enums.ConnectionType.LMSOracleReadOnly:
                    conKey = Consts.PS_LMS_Oracle_READONLY_CONNECTION_KEY;
                    break;
                case Enums.ConnectionType.LMSOracleWrite:
                    conKey = Consts.PS_LMS_Oracle_WRITE_CONNECTION_KEY;
                    break;
                default:
                    break;
            }
            if (String.IsNullOrWhiteSpace(conKey))
            {
                throw new Exception("木有配置数据库连接字符串");
            }
            T t = (T)Activator.CreateInstance(typeof(T));
            t.ConnectionString = ConfigurationHelper.GetConnectionString(conKey);
            VanclObjectInPool<T> vo = new VanclObjectInPool<T>();
            vo.PoolObject = t;
            return vo;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Tools.VCMRecords.Tool
{
    /// <summary>
    /// 各类数据转换
    /// </summary>
    public class Transfer
    {
        /// <summary>
        /// 把table 转为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable table)
        {
            Type type = typeof(T);
            List<T> result = new List<T>();
            foreach (DataRow row in table.Rows)
            {                
                T entity = (T)Activator.CreateInstance(type,row);
                result.Add(entity);
            }
            return result;
        }

        /// <summary>
        /// table转List
        /// </summary>
        /// <param name="table"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Object> TableToList(DataTable table, Type type)
        {
            List<Object> result = new List<Object>();
            foreach (DataRow row in table.Rows)
            {
                Object entity =Activator.CreateInstance(type, row);
                result.Add(entity);
            }
            return result;
        }

        /// <summary>
        /// 把obj转成对应类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type">转换后的目标数据类型</param>
        public static Object ConvertToType(object obj,Type type)
        {
            if(type==typeof(int))
            {
                return Convert.ToInt32(obj);
            }
            if(type==typeof(double))
            {
                return Convert.ToDouble(obj);
            }
            if (type == typeof(string))
            {
                return obj.ToString();
            }
            if (type == typeof(short))
            {
                return Convert.ToInt16(obj);
            }
            if (type == typeof(long))
            {
                return Convert.ToInt64(obj);
            }
            if (type == typeof(byte))
            {
                return Convert.ToByte(obj);
            }
            return obj;
        }

    }
}

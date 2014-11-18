using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Base;

namespace Vancl.TMS.Util.Extensions
{
    /// <summary>
    /// 实体对象扩展方法
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 设置对象的属性值
        /// </summary>
        /// <param name="entityList"></param>
        /// <param name="relation">数组index和对象属性的对应关系</param>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public static T SetValue<T>(this BaseEntity entity, Dictionary<int, string> relation, string[] data) where T : new()
        {
            if (relation == null || relation.Count == 0)
                throw new ArgumentNullException("relation", "relation参数不能为空");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("relation", "data参数不能为空");
            if (relation.Count != data.Length)
                throw new Exception("对应关系与数据源不匹配");
            T obj = new T();
            Type type = obj.GetType();
            for (int i = 0; i < data.Length; i++)
            {
                type.GetProperty(relation[i]).SetValue(obj, data[i], null);
            }

            return obj;
        }
    }
}

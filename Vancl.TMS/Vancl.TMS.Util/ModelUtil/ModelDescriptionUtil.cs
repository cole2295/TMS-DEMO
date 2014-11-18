using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Vancl.TMS.Util.ModelUtil
{
    /// <summary>
    /// 实体对象描述相关工具类
    /// </summary>
    public class ModelDescriptionUtil
    {
        /// <summary>
        /// 根据属性的描述获取属性名称
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string GetFieldNameByDescription<M>(string description) where M : class
        {
            string fieldName = string.Empty;
            Type type = typeof(M);
            foreach (var item in type.GetProperties())
            {
                fieldName = LookUpFieldByDescription(description, item);
                if (!string.IsNullOrWhiteSpace(fieldName))
                    break;
            }
            return fieldName;
        }

        private static string LookUpFieldByDescription(string description, PropertyInfo item)
        {
            string fieldName = string.Empty;
            object[] attrs = item.GetCustomAttributes(false);
            foreach (var attr in attrs)
            {
                if (attr is DescriptionAttribute)
                {
                    if (((DescriptionAttribute)attr).Description == description)
                    {
                        fieldName = item.Name;
                        break;
                    }
                }
            }
            return fieldName;
        }
    }
}

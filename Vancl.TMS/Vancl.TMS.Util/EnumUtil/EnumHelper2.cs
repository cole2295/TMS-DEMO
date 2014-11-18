using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Vancl.TMS.Util.EnumUtil
{

    /// <summary>
    /// 枚举相关辅助类
    /// </summary>
    public static partial class EnumHelper2
    {
        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的枚举对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Parse<T>(this Enum e, string value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T只能接受枚举类型。");
            }
            return (T)Enum.Parse(typeof(T), value);
        }


        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<EnumItemInfo> GetEnumItemInfoList(Type enumType)
        {
            //if (StaticCache.Cache.ContainsKey(enumType))
            //{
            //    var obj = StaticCache.Cache[enumType];
            //    return obj as List<EnumItemInfo>;
            //}

            if (!enumType.IsEnum)
            {
                throw new Exception("参数只能接受枚举类型。");
            }


            var list = new List<EnumItemInfo>();

            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (!field.FieldType.IsEnum) continue;
                string key = field.Name;

                //获取枚举对应的值
                int value = Convert.ToInt32(enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null));

                var arr = (DescriptionAttribute[])(field.GetCustomAttributes(typeof(DescriptionAttribute), true));
                string text = string.Empty;
                string description = string.Empty;
                if (arr.Length > 0)
                {
                    var attr = arr[0] as DescriptionAttribute;
                    text = attr.Description;
                    description = attr.Description;
                }

                EnumItemInfo itemInfo = new EnumItemInfo
                {
                    Name = field.Name,
                    Text = text,
                    Value = value,
                };
                list.Add(itemInfo);
            }
            //     StaticCache.Cache.Add(enumType, list);
            return list;
        }

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumItemInfo> GetEnumItemInfoList<T>()
        {
            return GetEnumItemInfoList(typeof(T));
        }

        public static List<EnumItemInfo> GetEnumItemInfoList<T>(T value)
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("参数只能接受枚举类型。");
            }

            int intval = Convert.ToInt32(value);

            var list = new List<EnumItemInfo>();

            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (!field.FieldType.IsEnum) continue;
                string key = field.Name;

                //获取枚举对应的值
                int v = Convert.ToInt32(enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null));

                if ((v & intval) == v)
                {
                    var arr = (DescriptionAttribute[])(field.GetCustomAttributes(typeof(DescriptionAttribute), true));
                    string text = string.Empty;
                    string description = string.Empty;
                    if (arr.Length > 0)
                    {
                        var attr = arr[0] as DescriptionAttribute;
                        text = attr.Description;
                        description = attr.Description;
                    }

                    EnumItemInfo itemInfo = new EnumItemInfo
                    {
                        Name = field.Name,
                        Text = text,
                        Value = v,
                    };
                    list.Add(itemInfo);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取枚举信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumItemInfo GetEnumItemInfo<T>(T value)
        {
            int v = Convert.ToInt32(value);
            var list = GetEnumItemInfoList<T>();
            foreach (var info in list)
            {
                if (info.Value == v)
                {
                    return info;
                }
            }
            return null;
        }



        public static IList<T> GetEnumValueList<T>(T t)
        {
            int value = Convert.ToInt32(t);

            IList<T> list = new List<T>();
            var infoList = GetEnumItemInfoList<T>();
            foreach (var info in infoList)
            {
                int v = info.Value;
                if ((v & value) == v)
                {
                    list.Add((T)Enum.Parse(typeof(T), v.ToString()));
                }
            }
            return list;
        }
    }

    /// <summary>
    /// 枚举项信息
    /// </summary>
    public class EnumItemInfo
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return String.Format("{0}[{1}={2}]", Text, Name, Value);
        }
    }

}

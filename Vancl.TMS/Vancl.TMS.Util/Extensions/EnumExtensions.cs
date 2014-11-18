using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Vancl.TMS.Util.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            string enstr = en.ToString();

            bool isFlagsEnum = false;
            object[] flagsAttr = type.GetCustomAttributes(typeof(FlagsAttribute), false);
            if (flagsAttr != null && flagsAttr.Length > 0)
            {
                isFlagsEnum = true;
            }
            if (!isFlagsEnum)
            {
                MemberInfo[] memInfo = type.GetMember(enstr);
                if (memInfo == null || memInfo.Length == 0) return enstr;
                object[] descAttr = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descAttr != null && descAttr.Length > 0)
                {
                    return ((DescriptionAttribute)descAttr[0]).Description;
                }
                else
                {
                    return enstr;
                }
            }
            else
            {
                string desc = string.Empty;
                enstr.Split(',').ToList().ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x))
                    {
                        MemberInfo[] memInfo = type.GetMember(x.Trim());
                        object[] descAttr = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descAttr != null && descAttr.Length > 0)
                        {
                            desc += ((DescriptionAttribute)descAttr[0]).Description + ", ";
                        }
                        else
                        {
                            desc += x + ", ";
                        }
                    }
                });
                return desc.TrimEnd(", ".ToCharArray());
            }
        }

        /// <summary>
        /// 获取枚举名称
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetName(this Enum en)
        {
            return en.ToString();
        }

        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static int GetInt32Value(this Enum en)
        {
            return Convert.ToInt32(en);
        }

        /// <summary>
        /// 获取枚举值数组
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static int[] GetEnumValueList(this Enum en)
        {
            int value = Convert.ToInt32(en);
            var type = en.GetType();
            
            object[] flagsAttr = type.GetCustomAttributes(typeof(FlagsAttribute), false);
            //不为flag类型
            if (flagsAttr == null || flagsAttr.Length == 0)
            {
                return new int[] { value };
            }

            var list = new List<int>();
            var vl = Enum.GetValues(type) as int[];
            foreach (var v in vl)
            {
                if ((v & value) == v)
                {
                    list.Add(v);
                }
            }
            return list.ToArray();
        }
    }
}

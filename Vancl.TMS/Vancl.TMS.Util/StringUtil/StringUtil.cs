using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vancl.TMS.Util
{
    public static class StringUtil
    {
        /// <summary>
        /// 特殊字符
        /// </summary>
        public const string SpecialChar = @"[~!@#$%&*':?/.\\|}{)(=]";

        /// <summary>
        /// 判断是否为正确的固定电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhone(string phone)
        {
            //string pattern = @"^(\d{7,8}|(\(|\（)\d{3,4}(\)|\）)\d{7,8}|\d{3,4}(-?)\d{7,8})(((-|转)\d{1,9})?)$ ";
            if (string.IsNullOrEmpty(phone)) return false;
            const string pattern = @"\b\d{7,16}";
            return Regex.Match(phone, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;
            const string pattern = @"\b1(3|5|8)\d{9}\b";
            return Regex.Match(mobile, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的邮政编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsValidPostalCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            const string pattern = @"\b\d{6}\b";
            return Regex.Match(code, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的Email地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            const string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return Regex.Match(email, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 是否中文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCnString(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            const string pattern = @"[\u4e00-\uf900]";
            return Regex.Match(str, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 是否是中文字符
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsCnString(char ch)
        {
            return IsCnString(ch.ToString());
        }

        /// <summary>
        /// 是否包含特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsContainSpecialChar(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return Regex.Match(str, SpecialChar, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutString(string str, int length)
        {
            string newString = string.Empty;
            if (str != string.Empty)
            {
                if (str.Length > length)
                {
                    newString = str.Substring(0, length);
                }
                else
                {
                    newString = str;
                }
            }
            return newString;
        }
        /// <summary>
        /// 判断是否为空数据
        /// </summary>
        /// <typeparam name="T">当前类型</typeparam>
        /// <param name="current">当前对象</param>
        /// <returns></returns>
        public static bool IsNullData<T>(this T current)
        {
            if (current == null || (object)current == DBNull.Value) return true;
            return String.IsNullOrEmpty(current.ToString().Trim());
        }

        public static string GetSequeceString(string original, int length, string separator)
        {
            string s = original;
            for (int i = 0; i < length; i++)
            {
                s = separator + s;
                if (s.Length >= length)
                    break;
            }

            return s;
        }

        /// <summary>
        /// 获取真实类名(泛型)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetRealName(string name, Type[] types)
        {
            string[] ss = name.Split(';');
            if (ss.Length == 0)
                throw new Exception("配置文件配置出错,请检查配置文件!");
            if (ss.Length == 1 || types == null)
                return name;
            else
            {
                string realName = "{0}`{1}[{2}]";
                string genericModelCount = (ss.Length - 1).ToString();

                string g = string.Empty;
                for (int i = 1; i < ss.Length; i++)
                {
                    string[] generic = ss[i].Split(',');
                    if (generic.Length != 2)
                        throw new Exception("泛型配置错误,请检查配置文件!");
                    else
                    {
                        if (g != string.Empty)
                            g += ",";
                        g += string.Format("[" + types[i - 1].FullName + ", " + types[i - 1].Assembly.FullName + "]", generic[0], generic[1]);
                    }
                }

                if (g != string.Empty)
                {
                    return string.Format(realName, ss[0], genericModelCount, g);
                }
                else
                    throw new Exception("泛型配置错误,请检查配置文件!");
            }
        }
    }
}

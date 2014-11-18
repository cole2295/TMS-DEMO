﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.China
{
    public class ChsSpell
    {
        /**/
        /// <summary>
        /// 取中文汉字的第一个拼音字母
        /// </summary>
        /// <param name="strText">中字汉字</param>
        /// <returns>大写拼音首字母</returns>
        public static string GetChsSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += getSpell(strText.Substring(i, 1));
            }
            return myStr;
        }


        private static string getSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = arrCN[0];

                int pos = arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }
    }
}

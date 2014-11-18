using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Tools.VCMRecords.Tool
{
    /// <summary>
    /// 序列化类
    /// </summary>
    public static  class Serialize
    {
        /// <summary>
        /// 字节数组转为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String BytesToString(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64;
        }

        /// <summary>
        /// 字符串转为字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return bytes;
        }
    }
}

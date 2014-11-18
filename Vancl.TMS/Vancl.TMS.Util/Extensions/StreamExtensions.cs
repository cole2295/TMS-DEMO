using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vancl.TMS.Util.Extensions
{
    public static class StreamExtensions
    {
        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] ToBytes(this Stream stream)
        {
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            return bytes;
        }

    }
}

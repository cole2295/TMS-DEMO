using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vancl.TMS.Util.Extensions
{
    public static class ByteExtensions
    {
        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Vancl.TMS.Tools.VCMRecords.Tool.File
{
    /// <summary>
    /// 自定义文件处理类
    /// </summary>
    public static class CustomFile
    {
        /// <summary>
        /// 根据文件名，把文件序列化成字符串
        /// </summary>
        /// <param name="fullname">文件名</param>
        /// <returns></returns>
        public static string serializeFileToString(string fullname)
        {
            FileInfo fileInfo = new FileInfo(fullname);
            if (fileInfo.Length > 10 * 1024 * 1024)
                throw new Exception("上传文件大小不能超过10M");
            FileStream readStream = new FileStream(fullname, FileMode.Open, FileAccess.Read);
            byte[] contens = new byte[fileInfo.Length];
            readStream.Read(contens, 0, Convert.ToInt32(fileInfo.Length));
            readStream.Close();
            return Serialize.BytesToString(contens);
        }

        /// <summary>
        /// 把序列化的字符串保存为文件
        /// </summary>
        /// <param name="fileContent">序列化的字符串</param>
        /// <param name="fullname">待存文件名</param>
        public static void serializeStringToFile(String fileContent, String fullname)
        {
            byte[] bytes = Serialize.StringToByte(fileContent);
            if (System.IO.File.Exists(fullname))
            {
                System.IO.File.Delete(fullname);
            }
            FileInfo info = new FileInfo(fullname);
            FileStream writeStream = new FileStream(fullname, FileMode.CreateNew, FileAccess.Write);
            writeStream.Write(bytes, 0, bytes.Length);
            writeStream.Close();
        }

        /// <summary>
        /// 得到文件大小的文字描述（10K,10M等）
        /// </summary>
        /// <param name="length">字节大小</param>
        /// <returns></returns>
        public static String getFileSizelabel(long length)
        {
            if (length < 1024 * 1024)
            {
                return String.Format("{0}K", (length/(1024)).ToString("0.00"));
            }
            if (length < 1024 * 1024 * 1024)
            {
                return String.Format("{0}M", (length / (1024 * 1024)).ToString(".##"));
            }
            return String.Format("{0}M", (length / (1024 * 1024*1024)).ToString(".##"));
        }

        /// <summary>
        /// 得到文件大小的文字描述
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static String getFileSizelabel(String filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            long length = fileInfo.Length;
            if (length < 1024 * 1024)
            {
                return String.Format("{0}K", (length / (1024 * 1024)).ToString(".##"));
            }
            if (length < 1024 * 1024 * 1024)
            {
                return String.Format("{0}M", (length / (1024 * 1024 * 1024)).ToString(".##"));
            }
            return "";
        }

    }
}

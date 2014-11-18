using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.Transfer
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：将用户数据分解为发送的通信数据包
    * 说明：将用户数据分解为发送的通信数据包，用户接收端解决粘包问题
    * 作者：任 钰
    * 创建日期：2012-01-17 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class PackageSpliter
    {
        public PackageSpliter()
        {

        }

        public byte PackageIdent
        {
            get;
            set;
        }

        public byte PackageType
        {
            get;
            set;
        }

        /// <summary>
        /// 分隔字符串类型数据包
        /// </summary>
        /// <param name="OrigData"></param>
        /// <returns></returns>
        public byte[] SplitPackage(string OrigData)
        {
            if (string.IsNullOrEmpty(OrigData))
            {
                throw new Exception("不能发送空的数据");
            }
            byte[] OrigDataBuffer = Encoding.Unicode.GetBytes(OrigData);
            byte[] FinallyDataBuffer = new byte[TransferProtocol.PackageHeadLength + OrigDataBuffer.Length];
            FinallyDataBuffer[0] = PackageIdent;
            FinallyDataBuffer[1] = PackageType;
            BitConverter.GetBytes(OrigDataBuffer.Length).CopyTo(FinallyDataBuffer, TransferProtocol.PackageIdentLength+ TransferProtocol.PackageTypeLength);
            OrigDataBuffer.CopyTo(FinallyDataBuffer, TransferProtocol.PackageHeadLength);
            return FinallyDataBuffer;
        }




    }
}

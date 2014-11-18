using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Transfer.Listener;

namespace Vancl.TMS.Util.Transfer
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：组合接收缓冲区的数据为具体传输数据
    * 说明：组合接收缓冲区的数据为具体传输数据，用户接收端解决粘包问题
    * 作者：任 钰
    * 创建日期：2012-01-13 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    internal  class PackageBuilder
    {
        private readonly ReceivedPackageDetail PackageDetail;

        /// <summary>
        /// 当前实际接收缓冲区剩余字节长度
        /// </summary>
        private int realDataBufferRemainLength
        {
            get
            {
                return realDataBuffer.Count - curOffset;
            }
        }

        /// <summary>
        /// 当前实际接收缓冲区的偏移量
        /// </summary>
        private int curOffset;

        /// <summary>
        /// 当前实际接收缓冲区
        /// </summary>
        private ArraySegment<byte> realDataBuffer;

        public PackageBuilder(ReceivedPackageDetail packagedetail)
        {
            PackageDetail = packagedetail;
        }


        /// <summary>
        /// Copy缓冲区数据到Context的header buffer中
        /// </summary>
        /// <param name="copyLength"></param>
        private void CopyHeaderToContext(int copyLength)
        {
            Array.Copy(realDataBuffer.Array, realDataBuffer.Offset + curOffset, PackageDetail.PackageHeaderBuffer, PackageDetail.PackageHeaderOffset, copyLength);
            PackageDetail.PackageHeaderOffset += copyLength;
            curOffset += copyLength;
        }

        /// <summary>
        /// 组合包头
        /// </summary>
        private void BuildingContextPackageHeader()
        {
            if (realDataBufferRemainLength >= PackageDetail.PackageHeaderRemainLength)                                
            {
                CopyHeaderToContext(PackageDetail.PackageHeaderRemainLength);
                PackageDetail.PackageFullyType = TransferProtocol.emPackageFullyType.PackageHeadFully;
            }
            else
            {
                CopyHeaderToContext(realDataBufferRemainLength);
                PackageDetail.PackageFullyType = TransferProtocol.emPackageFullyType.PackageHeadPartly;
            }
        }

        /// <summary>
        /// Copy缓冲区数据到Context的data buffer中
        /// </summary>
        /// <param name="copyLength"></param>
        private void CopyDataToContext(int copyLength)
        {
            Array.Copy(realDataBuffer.Array, realDataBuffer.Offset + curOffset, PackageDetail.PackageDataBuffer, PackageDetail.PackageDataOffset, copyLength);
            PackageDetail.PackageDataOffset += copyLength;
            curOffset += copyLength;
        }

        /// <summary>
        /// 组合包实际数据
        /// </summary>
        private void BuildingContextPackageData()
        {
            if (realDataBufferRemainLength >= PackageDetail.PackageDataRemainLength)                              
            {
                CopyDataToContext(PackageDetail.PackageDataRemainLength);
                PackageDetail.PackageFullyType = TransferProtocol.emPackageFullyType.PackageDataFully;
            }
            else
            {
                CopyDataToContext(realDataBufferRemainLength);
                PackageDetail.PackageFullyType = TransferProtocol.emPackageFullyType.PackageDataPartly;
            }
        }

        /// <summary>
        /// 组合接收到的
        /// </summary>
        public void BuildingDataPackage(int realDataBufferLength)
        {
            curOffset = 0;
            realDataBuffer = new ArraySegment<byte>(PackageDetail.buffer, 0, realDataBufferLength);
            while (curOffset < realDataBuffer.Count)
            {
                switch (PackageDetail.PackageFullyType)
                {
                    case TransferProtocol.emPackageFullyType.PackageHeadFully:
                    case TransferProtocol.emPackageFullyType.PackageDataPartly:
                        BuildingContextPackageData();
                        break;
                    case TransferProtocol.emPackageFullyType.PackageDataFully:
                    case TransferProtocol.emPackageFullyType.PackageHeadPartly:
                          BuildingContextPackageHeader();
                        break;
                    default:
                        throw new Exception("传输协议包完整类型不对") ;
                }
            }
        }


    }
}

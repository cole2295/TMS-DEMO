using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Transfer.Listener;
using System.Net.Sockets;

namespace Vancl.TMS.Util.Transfer
{

    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：一个完整的数据包接收详情类
    * 说明：一个完整的数据包接收详情类,包含包头,数据，等等
    * 作者：任 钰
    * 创建日期：2012-01-13 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class ReceivedPackageDetail
    {
        private const int Buffer_Default_Length = 1024 * 4;                     //4 KB
        private TransferProtocol.emPackageFullyType _packagefullytype;
        private PackageBuilder builder;
        private SocketContext Context;

        /// <summary>
        /// 连接的socket
        /// </summary>
        public Socket WorkSocket
        {
            get;
            private set;
        }

        public ReceivedPackageDetail(SocketContext context, Socket socket)
            : this(context, Buffer_Default_Length, socket)
        {

        }

        public ReceivedPackageDetail(SocketContext context, int bufferlength, Socket socket)
        {
            Context = context;
            BufferSize = bufferlength;
            WorkSocket = socket;
            Init();
        }

        /// <summary>
        ///  接收或者发送自定义缓冲区的长度
        /// </summary>
        public readonly int BufferSize;

        /// <summary>
        /// 接收或者发送自定义缓冲区
        /// </summary>
        public byte[] buffer
        {
            get;
            private set;
        }

        /// <summary>
        /// 包头buffer
        /// </summary>
        public byte[] PackageHeaderBuffer
        {
            get;
            private set;
        }

        /// <summary>
        /// 包头偏移量
        /// </summary>
        public int PackageHeaderOffset
        {
            get;
            set;
        }

        /// <summary>
        /// 包头剩余长度
        /// </summary>
        public int PackageHeaderRemainLength
        {
            get
            {
                return TransferProtocol.PackageHeadLength - PackageHeaderOffset;
            }
        }

        /// <summary>
        /// 数据包头是否完整
        /// </summary>
        public bool ReceivedHeaderFully
        {
            get
            {
                if (0 == PackageHeaderOffset)
                {
                    return false;
                }
                return PackageHeaderOffset == TransferProtocol.PackageHeadLength;
            }
        }

        /// <summary>
        /// 包总数据长度
        /// </summary>
        public int PackageDataTotalLength
        {
            get
            {
                if (PackageHeaderOffset == TransferProtocol.PackageHeadLength)
                {
                    return BitConverter.ToInt32(PackageHeaderBuffer, TransferProtocol.PackageHeadLength - TransferProtocol.PackageTotalLength);
                }
                throw new Exception("数据包头不全，缺少包总长度的标记");
            }
        }

        /// <summary>
        /// 包数据剩余长度
        /// </summary>
        public int PackageDataRemainLength
        {
            get
            {
                return PackageDataTotalLength - PackageDataOffset;
            }
        }


        /// <summary>
        /// 是否数据包接收完毕
        /// </summary>
        public bool ReceivedDataFully
        {
            get
            {
                if (PackageDataOffset == 0)
                {
                    return false;
                }
                return PackageDataOffset == PackageDataTotalLength;
            }
        }

        /// <summary>
        /// 包身份标记
        /// </summary>
        public byte PackageIdent
        {
            get
            {
                if (PackageHeaderOffset >= TransferProtocol.PackageIdentLength)
                {
                    return PackageHeaderBuffer[0];
                }
                throw new Exception("数据包头不全，缺少包身份标记");
            }
        }


        /// <summary>
        /// 数据包完整类型
        /// </summary>
        public TransferProtocol.emPackageFullyType PackageFullyType
        {
            get
            {
                return _packagefullytype;
            }
            set
            {
                _packagefullytype = value;
                switch (_packagefullytype)
                {
                    case TransferProtocol.emPackageFullyType.PackageHeadFully:
                        PackageDataBuffer = new byte[PackageDataTotalLength];
                        break;
                    case TransferProtocol.emPackageFullyType.PackageHeadPartly:
                        break;
                    case TransferProtocol.emPackageFullyType.PackageDataFully:
                        NotifyTransferWasReceived();
                        NotifyTranferToBeReceived();
                        break;
                    case TransferProtocol.emPackageFullyType.PackageDataPartly:
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 包字节数组
        /// </summary>
        public byte[] PackageDataBuffer
        {
            get;
            set;
        }
        /// <summary>
        /// 包数据偏移量
        /// </summary>
        public int PackageDataOffset
        {
            get;
            set;
        }

        /// <summary>
        /// 包实体数据对象
        /// </summary>
        public object PackageDataModel
        {
            get
            {
                return Encoding.Unicode.GetString(PackageDataBuffer, 0, PackageDataBuffer.Length);
            }
        }


        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            builder = new PackageBuilder(this);
            buffer = new byte[BufferSize];
            PackageHeaderBuffer = new byte[TransferProtocol.PackageHeadLength];
            ///初始化为数据包数据完整型
            _packagefullytype = TransferProtocol.emPackageFullyType.PackageDataFully;
        }

        /// <summary>
        /// 清空相应数据以及状态
        /// </summary>
        protected virtual void ClearUp()
        {
            for (int i = 0; i < PackageHeaderBuffer.Length; i++)
            {
                PackageHeaderBuffer[i] = default(byte);
            }
            PackageDataOffset = 0;
            PackageHeaderOffset = 0;
            PackageDataBuffer = null;
        }

        /// <summary>
        /// 当前包组合完整,通知接收完成
        /// </summary>
        /// <param name="context"></param>
        protected virtual void NotifyTransferWasReceived()
        {
            Context.Application.NotifyTransferWasReceived(Context);
            //清空接收数据的buffer状态，准备组合下一个Package
            ClearUp();
        }

        /// <summary>
        /// 准备开始组合下一个数据包
        /// </summary>
        protected virtual void NotifyTranferToBeReceived()
        {
            Context.Application.NotifyTranferToBeReceived(Context);
        }

        /// <summary>
        /// 组合接收缓冲区的包
        /// </summary>
        /// <param name="realDataBufferLength"></param>
        public virtual void HandleDataPackage(int realDataBufferLength)
        {
            builder.BuildingDataPackage(realDataBufferLength);
        }

    }
}

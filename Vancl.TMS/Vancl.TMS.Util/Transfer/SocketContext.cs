using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Vancl.TMS.Util.Transfer
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：通信上下文
    * 说明：通信上下文
    * 作者：任 钰
    * 创建日期：2012-01-16 11:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public sealed class SocketContext
    {
        public SocketApplication Application
        {
            get;
            private set;
        }

        public SendPackageDetail SePackageDetail
        {
            get;
            private set;
        }

        /// <summary>
        /// 接收包详细对象
        /// </summary>
        public ReceivedPackageDetail RecPackageDetail
        {
            get;
            private set;
        }

        public SocketContext(Socket socket)
        {
            RecPackageDetail = new ReceivedPackageDetail(this, socket);
            SePackageDetail = new SendPackageDetail(socket);
            Application = new SocketApplication();
        }

        public SocketContext(Socket recSocket, Socket SendSocket)
        {
            RecPackageDetail = new ReceivedPackageDetail(this, recSocket);
            SePackageDetail = new SendPackageDetail(SendSocket);
            Application = new SocketApplication();
        }

        public SocketContext(int bufferlength, Socket socket)
        {
            RecPackageDetail = new ReceivedPackageDetail(this, bufferlength, socket);
            SePackageDetail = new SendPackageDetail(socket);
            Application = new SocketApplication();
        }

        public SocketContext(int bufferlength, Socket recSocket, Socket SendSocket)
        {
            RecPackageDetail = new ReceivedPackageDetail(this, bufferlength, recSocket);
            SePackageDetail = new SendPackageDetail(SendSocket);
            Application = new SocketApplication();
        }

    }
}

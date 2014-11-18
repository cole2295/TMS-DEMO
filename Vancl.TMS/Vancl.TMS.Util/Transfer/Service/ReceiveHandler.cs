using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common.Logging;
using Common.Logging.Log4Net;
using Vancl.TMS.Util.Transfer.Listener;

namespace Vancl.TMS.Util.Transfer.Service
{
    public class ReceiveHandler : ITransfer, IDisposable
    {
        /// <summary>
        /// log
        /// </summary>
        protected ILog logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 默认监听队列大小
        /// </summary>
        private const int Default_ListenerQueueSize = 100;
        /// <summary>
        /// 非托管资源释放标记
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// 服务端机器名
        /// </summary>
        public string MachineName
        {
            get
            {
                return Dns.GetHostName();
            }
        }

        /// <summary>
        /// 服务端提供服务的IP地址
        /// 如果服务器为多个网卡，尽量使用IP进行网络通信
        /// </summary>
        public string IpAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// 提供接收服务的端口
        /// </summary>
        public int RecPort
        {
            get;
            private set;
        }

        /// <summary>
        /// 提供发送数据服务的端口
        /// </summary>
        public int SendPort
        {
            get;
            private set;
        }

        /// <summary>
        /// 监听队列大小
        /// </summary>
        public int ListenerQueueSize
        {
            get;
            set;
        }

        public ReceiveHandler(string IpAddress, int Port)
        {
            this.IpAddress = IpAddress;
            this.RecPort = Port;
            ListenerQueueSize = Default_ListenerQueueSize;
        }

        public ReceiveHandler(string IpAddress,int Port, int sendport)
        {
            this.IpAddress = IpAddress;
            this.RecPort = Port;
            this.SendPort = sendport;
            ListenerQueueSize = Default_ListenerQueueSize;
        }

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public void StartListening()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(IpAddress), RecPort);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(ListenerQueueSize);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            if (handler.Connected)
            {
                SocketContext context = new SocketContext(handler);
                context.Application.AddTransferListener(new StdTransferListener(this));
                Receive(context);
            }
        }

        public void Send(SocketContext context)
        {
            try
            {
                byte[] byteData = context.SePackageDetail.SplitPackage(context.RecPackageDetail.PackageDataModel.ToString());
                context.Application.NotifyTransferToBeSend(context);
                context.SePackageDetail.WorkSocket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Application.NotifyTransferException(context, new TransferException("接收异常", e));
                }
            }
        }

        public void Receive(SocketContext context)
        {
            try
            {
                context.RecPackageDetail.WorkSocket.BeginReceive(
               context.RecPackageDetail.buffer
               , 0
               , context.RecPackageDetail.BufferSize
               , 0
               , new AsyncCallback(ReceivedCallback)
               , context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Application.NotifyTransferException(context, new TransferException("接收异常", e));
                }
            }
        }

        private void ReceivedCallback(IAsyncResult ar)
        {
            SocketContext context = (SocketContext)ar.AsyncState;
            try
            {
                Socket handler = context.RecPackageDetail.WorkSocket;
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    context.RecPackageDetail.HandleDataPackage(bytesRead);
                }
                else
                {
                    logger.ErrorFormat("客服端IP:{0}\t端口:{1},发送空数据", handler.LocalEndPoint.ToString());
                }
                handler.BeginReceive(context.RecPackageDetail.buffer, 0, context.RecPackageDetail.BufferSize, 0, new AsyncCallback(ReceivedCallback), context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Application.NotifyTransferException(context, new TransferException("接收异常", e));
                }
            }
        }


        private void SendCallback(IAsyncResult ar)
        {
            SocketContext context = (SocketContext)ar.AsyncState;
            try
            {
                int bytesSent = context.SePackageDetail.WorkSocket.EndSend(ar);
                context.Application.NotifyTransferWasSend(context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Application.NotifyTransferException(context, new TransferException("发送异常", e));
                }
            }
        }
        /// <summary>
        /// 销毁并释放资源
        /// </summary>
        public void Destroy()
        {
            (this as IDisposable).Dispose();
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    //释放托管资源
                }
                //始终释放非托管


                isDisposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 由GC觉得 析构函数的调用
        /// </summary>
        ~ReceiveHandler()
        {
            Dispose(false);
        }
    }
}

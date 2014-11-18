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

namespace Vancl.TMS.Util.Transfer.Endpoint
{
    public class SendHandler : ITransfer, IDisposable
    {
        /// <summary>
        /// log
        /// </summary>
        protected ILog logger = LogManager.GetCurrentClassLogger();
        protected SocketContext Context;
        /// <summary>
        /// 释放非托管资源的标记
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
        /// 提供服务的端口
        /// </summary>
        public int Port
        {
            get;
            private set;
        }

        public SendHandler(string IpAddress, int Port)
        {
            this.IpAddress = IpAddress;
            this.Port = Port;
        }

        public void StartClient()
        {
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
                Socket WorkSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Context = new SocketContext(WorkSocket);
                Context.Application.AddTransferListener(new CltStdTransferListner());
                Context.SePackageDetail.WorkSocket.Connect(remoteEP);
                Receive(Context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            SocketContext context = (SocketContext)ar.AsyncState;
            try
            {
                Socket client = context.RecPackageDetail.WorkSocket;
                int bytesRead = client.EndReceive(ar);
                if (bytesRead > 0)
                {
                    context.RecPackageDetail.HandleDataPackage(bytesRead);
                }
                client.BeginReceive(context.RecPackageDetail.buffer, 0, context.RecPackageDetail.BufferSize, 0,
                      new AsyncCallback(ReceiveCallback), context);
            }
            catch (Exception e)
            {
                if (context != null)
                {
                    context.Application.NotifyTransferException(context, new TransferException("接收异常", e));
                }
            }
        }

        public void Send(SocketContext context)
        {
            //throw new NotImplementedException();
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
                ,new AsyncCallback(ReceiveCallback)
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

        public void Send(String data)
        {
            Context.Application.NotifyTransferToBeSend(Context);
            Context.SePackageDetail.WorkSocket.Send(Context.SePackageDetail.SplitPackage(data));
            Context.Application.NotifyTransferWasSend(Context);
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
                    if (Context != null)
                    {
                        using (Socket soc = Context.RecPackageDetail.WorkSocket)
                        {
                            if (soc!= null && soc.Connected)
                            {
                                soc.Shutdown(SocketShutdown.Both);
                            }
                        }
                        using (Socket soc = Context.SePackageDetail.WorkSocket)
                        {
                            if (soc != null && soc.Connected)
                            {
                                soc.Shutdown(SocketShutdown.Both);
                            }
                        }
                    }
                    this.Context = null;
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
        ~SendHandler()
        {
            Dispose(false);
        }

    }
}

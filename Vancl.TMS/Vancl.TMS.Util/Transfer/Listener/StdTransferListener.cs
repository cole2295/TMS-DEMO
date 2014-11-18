using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Transfer.Service;

namespace Vancl.TMS.Util.Transfer.Listener
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：标准监听类
    * 说明：标准监听类
    * 作者：任 钰
    * 创建日期：2012-01-13 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class StdTransferListener : ITransferListener
    {
        private readonly ITransfer transfer;

        public StdTransferListener(ITransfer hander)
        {
            this.transfer = hander;
        }

        public void TransferToBeSend(SocketContext context)
        {
            //Console.WriteLine("准备发送");
        }

        public void TransferWasSend(SocketContext context)
        {
            Console.WriteLine("发送的数据结束");
        }

        public void TransferToBeReceive(SocketContext context)
        {
            Console.WriteLine("准备接收");
        }

        public void TransferWasReceived(SocketContext context)
        {
            transfer.Send(context);
        }

        public void TransferException(SocketContext context, TransferException exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine(exception.InnerException.StackTrace);
        }

    }
}

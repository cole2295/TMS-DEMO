using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Transfer.Listener;

namespace Vancl.TMS.Util.Transfer.Listener
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：标准监听接口
    * 说明：标准监听接口
    * 作者：任 钰
    * 创建日期：2012-01-13 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public interface ITransferListener
    {
        /// <summary>
        /// 即将发送数据
        /// </summary>
        /// <param name="context"></param>
        void TransferToBeSend(SocketContext context);
        /// <summary>
        /// 已经完全发送数据
        /// </summary>
        /// <param name="context"></param>
        void TransferWasSend(SocketContext context);

        /// <summary>
        /// 即将接收数据
        /// </summary>
        /// <param name="context"></param>
        void TransferToBeReceive(SocketContext context);

        /// <summary>
        /// 已经完全接收数据
        /// </summary>
        /// <param name="context"></param>
        void TransferWasReceived(SocketContext context);

        /// <summary>
        /// 传输异常
        /// </summary>
        /// <param name="context"></param>
        void TransferException(SocketContext context, TransferException exception);

    }
}

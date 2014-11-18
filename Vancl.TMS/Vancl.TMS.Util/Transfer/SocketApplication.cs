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
    * 模块名称：处理传输通信
    * 说明：处理传输通信
    * 作者：任 钰
    * 创建日期：2012-01-16 11:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class SocketApplication
    {
        /// <summary>
        /// 监听列表
        /// </summary>
        public IList<ITransferListener> ListenerLists
        {
            get;
            private set;
        }

        public SocketApplication()
        {
            ListenerLists = new List<ITransferListener>();
        }

        /// <summary>
        /// 添加传输监听对象
        /// </summary>
        /// <param name="listener"></param>
        public virtual void AddTransferListener(ITransferListener listener)
        {
            if (!ListenerLists.Contains(listener))
            {
                ListenerLists.Add(listener);
            }
        }

        /// <summary>
        /// 移除传输监听对象
        /// </summary>
        /// <param name="listener"></param>
        public virtual void RemoveTransferListener(ITransferListener listener)
        {
            if (ListenerLists.Contains(listener))
            {
                ListenerLists.Remove(listener);
            }
        }

        /// <summary>
        /// 移除所有的传输监听对象
        /// </summary>
        public virtual void ClearUpTransferListener()
        {
            ListenerLists.Clear();
        }

        /// <summary>
        /// 通知准备接收
        /// </summary>
        /// <param name="context"></param>
        internal virtual void NotifyTranferToBeReceived(SocketContext context)
        {
            foreach (var item in ListenerLists)
            {
                item.TransferToBeReceive(context);
            }
        }

        /// <summary>
        /// 通知接收完成
        /// </summary>
        /// <param name="context"></param>
        internal virtual void NotifyTransferWasReceived(SocketContext context)
        {
            foreach (var item in ListenerLists)
            {
                item.TransferWasReceived(context);
            }
        }

        /// <summary>
        /// 准备发送数据
        /// </summary>
        /// <param name="context"></param>
        internal virtual void NotifyTransferToBeSend(SocketContext context)
        {
            foreach (var item in ListenerLists)
            {
                item.TransferToBeSend(context);
            }
        }

        /// <summary>
        /// 发送数据完成
        /// </summary>
        /// <param name="context"></param>
        internal virtual void NotifyTransferWasSend(SocketContext context)
        {
            foreach (var item in ListenerLists)
            {
                item.TransferWasSend(context);
            }
        }

        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        internal virtual void NotifyTransferException(SocketContext context, TransferException exception)
        {
            foreach (var item in ListenerLists)
            {
                item.TransferException(context, exception);
            }
        }

    }
}

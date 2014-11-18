using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.Transfer
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：通用传输接口
    * 说明：通用传输接口
    * 作者：任 钰
    * 创建日期：2012-01-13 14:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public interface ITransfer
    {
        void Send(SocketContext context);
        void Receive(SocketContext context);
    }
}

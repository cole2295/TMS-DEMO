using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Vancl.TMS.Util.Transfer
{
    /*
    * (C)Copyright 2011-2012 TMS
    * 
    * 模块名称：一个完整的数据包发送详情类
    * 说明：一个完整的数据包发送详情类,包含包头,数据，等等
    * 作者：任 钰
    * 创建日期：2012-01-16 11:34:00
    * 修改人：
    * 修改时间：
    * 修改记录：记录以便查阅
    */
    public class SendPackageDetail
    {
        private PackageSpliter Spliter;
        /// <summary>
        /// 连接的socket
        /// </summary>
        public Socket WorkSocket
        {
            get;
            private set;
        }

        public SendPackageDetail(Socket socket)
        {
            Spliter = new PackageSpliter();
            WorkSocket = socket;
        }

        public byte[] SplitPackage(string Data)
        {
            return Spliter.SplitPackage(Data);
        }


    }
}

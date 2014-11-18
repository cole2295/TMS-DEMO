using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.Transfer
{
    /*
     * (C)Copyright 2011-2012 TMS
     * 
     * 模块名称：通信异常
     * 说明：通信异常
     * 作者：任 钰
     * 创建日期：2012-01-16 11:34:00
     * 修改人：
     * 修改时间：
     * 修改记录：记录以便查阅
     */
    public class TransferException : Exception
    {
        public TransferException()
        {

        }

        public TransferException(string msg)
            : base(msg)
        {

        }

        public TransferException(string msg, Exception exception)
            : base(msg, exception)
        {
            InnerException = exception;
        }

        public Exception InnerException
        {
            get;
            private set;
        }

    }



}

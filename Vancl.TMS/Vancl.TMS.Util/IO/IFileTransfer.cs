using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vancl.TMS.Util.IO
{
    public interface IFileTransfer
    {
        /// <summary>
        /// 执行前的事件
        /// </summary>
        event EventHandler BeForeTransfer;

        /// <summary>
        /// 执行后的事件
        /// </summary>
        event EventHandler AfterTransfer;

        /// <summary>
        /// 本地上下文
        /// </summary>
        FtpTransferLocalContext LocalContext { get; set; }

        /// <summary>
        /// 服务器上下文
        /// </summary>
        FtpTransferServerContext ServerContext { get; set; }

        /// <summary>
        /// IO流
        /// </summary>
        Stream ActionFileStream { get; set; }

        /// <summary>
        /// 执行文件操作
        /// </summary>
        void DoAction();
    }

    public enum FtpAction
    { 
        /// <summary>
        /// 下载文件
        /// </summary>
        DownLoad,

        /// <summary>
        /// 上传文件
        /// </summary>
        UpLoad,

        /// <summary>
        /// 删除文件
        /// </summary>
        Delete
    }
}

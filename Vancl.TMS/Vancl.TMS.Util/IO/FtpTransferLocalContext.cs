using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Vancl.TMS.Util.IO
{
    public class FtpTransferLocalContext
    {
        /// <summary>
        /// 本地上传文件路径/本地下载保存路径
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// 数据流
        /// </summary>
        public Stream DataStream { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;
using System.IO;
using System.ComponentModel;

namespace Vancl.TMS.Util.IO
{
    public class FtpTransferServerContext
    {
        /// <summary>
        /// 服务器IP
        /// </summary>
        [Obsolete]
        internal string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        [Obsolete]
        internal int Port { get; set; }

        /// <summary>
        /// 登录用户名
        /// </summary>
        internal string UserName { get { return ConfigurationHelper.GetAppSetting("FileServerFtpUserName"); } }

        /// <summary>
        /// 登录密码
        /// </summary>
        internal string PassWord { get { return ConfigurationHelper.GetAppSetting("FileServerFtpUserPwd"); } }

        /// <summary>
        /// 请求文件夹路径
        /// </summary>
        public string DefaultPath { get { return ConfigurationHelper.GetAppSetting("FileServerDefaultFtpAddress"); } }

        /// <summary>
        /// 服务端文件夹(用户远程创建不存在的文件夹)
        /// </summary>
        public string ServerPath { get; set; }

        /// <summary>
        /// 请求的文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 请求文件路径
        /// </summary>
        internal string FilePath { get { return Path.Combine(this.DefaultPath, FileName); } }
    }
}

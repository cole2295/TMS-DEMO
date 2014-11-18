using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Core.Schedule
{
    /// <summary>
    /// 调度服务通用常量
    /// </summary>
    public sealed class CommonConst
    {
        /// <summary>
        /// 线程数
        /// </summary>
        public static readonly int ThreadCount = int.Parse(ConfigurationHelper.GetAppSetting("ThreadCount"));

        /// <summary>
        /// 工作配置文件目录
        /// </summary>
        public static readonly string JobConfigPath = ConfigurationHelper.GetAppSetting("JobConfigPath");

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public static readonly string SmtpServer = ConfigurationHelper.GetAppSetting("SmtpServer");

        /// <summary>
        /// 发件人
        /// </summary>
        public static readonly string MailFrom = ConfigurationHelper.GetAppSetting("MailFrom");

        /// <summary>
        /// 邮件服务器用户名
        /// </summary>
        public static readonly string SmtpUID = ConfigurationHelper.GetAppSetting("SmtpUID");

        /// <summary>
        /// 邮件服务用户密码
        /// </summary>
        public static readonly string SmtpPWD = ConfigurationHelper.GetAppSetting("SmtpPWD");

        /// <summary>
        /// 是否使用默认邮件验证
        /// </summary>
        public static readonly bool SmtpUseDefaultCredentials = bool.Parse(ConfigurationHelper.GetAppSetting("SmtpUseDefaultCredentials"));

        /// <summary>
        /// 邮件服务端口
        /// </summary>
        public static readonly int SmtpMailPort = int.Parse(ConfigurationHelper.GetAppSetting("SmtpMailPort"));

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        public static readonly bool SmtpEnableSsl = bool.Parse(ConfigurationHelper.GetAppSetting("SmtpEnableSsl"));

        /// <summary>
        /// 发件人显示名称
        /// </summary>
        public static readonly string FromDisplayName = ConfigurationHelper.GetAppSetting("FromDisplayName");

        /// <summary>
        /// 主题
        /// </summary>
        public static readonly string Subject =ConfigurationHelper.GetAppSetting("Subject");

        /// <summary>
        /// 收件人
        /// </summary>
        public static readonly string To = ConfigurationHelper.GetAppSetting("To");

        /// <summary>
        /// 抄送
        /// </summary>
        public static readonly string CC = ConfigurationHelper.GetAppSetting("CC");

        /// <summary>
        /// 服务启用停用邮件主题
        /// </summary>
        public static readonly string ServiceEnabledEmailSubject = ConfigurationHelper.GetAppSetting("ServiceEnabledEmailSubject");

    }
}

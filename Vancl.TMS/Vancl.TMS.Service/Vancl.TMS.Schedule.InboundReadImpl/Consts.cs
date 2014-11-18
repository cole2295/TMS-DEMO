using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.InboundReadImpl
{
    public static class Consts
    {
        /// <summary>
        /// 文件名过滤模型
        /// </summary>
        public static readonly string SEARCH_PATTERN = ConfigurationHelper.GetAppSetting("SearchPattern");

        /// <summary>
        /// 缓冲文件存储路径
        /// </summary>
        public static readonly string FILE_DIR = ConfigurationHelper.GetAppSetting("FileDir");

        /// <summary>
        /// 每批次最大执行条数
        /// </summary>
        public static readonly int BATCH_MAX_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("BatchCount"));

        /// <summary>
        /// 读取箱的线程数
        /// </summary>
        public static readonly int BOX_THREAD_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("BoxThreadCount"));

        /// <summary>
        /// 读取订单的线程数
        /// </summary>
        public static readonly int ORDER_THREAD_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("OrderThreadCount"));

        /// <summary>
        /// 线程文件夹名
        /// </summary>
        public static readonly string THREAD_CATEGORY_DIR = ConfigurationHelper.GetAppSetting("ThreadCategoryDirName");

        /// <summary>
        /// 备份文件夹名
        /// </summary>
        public static readonly string BACKUP_DIR = ConfigurationHelper.GetAppSetting("BackUpDirName");

        /// <summary>
        /// 异常文件夹名
        /// </summary>
        public static readonly string ABNORMAL_DIR = ConfigurationHelper.GetAppSetting("AbnormalFileDirName");
    }
}

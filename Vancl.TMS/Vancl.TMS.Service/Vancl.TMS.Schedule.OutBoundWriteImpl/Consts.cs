using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.OutBoundWriteImpl
{
    public class Consts
    {
        /// <summary>
        /// 文件名过滤模型
        /// </summary>
        public static readonly string SEARCH_PATTERN = ConfigurationHelper.GetAppSetting("SearchPattern");

        /// <summary>
        /// 批次号文件夹后缀名
        /// </summary>
        public static readonly string BatchModelDirName = ConfigurationHelper.GetAppSetting("BatchModelDirName");

        /// <summary>
        /// 正常按照箱号取模值
        /// </summary>
        public static readonly int NormalModValue = int.Parse(ConfigurationHelper.GetAppSetting("NormalModValue"));

        /// <summary>
        /// 缓冲文件存储路径
        /// </summary>
        public static readonly string FILE_DIR = ConfigurationHelper.GetAppSetting("FileDir");
   
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.OutBoundReadImpl
{
    public class Consts
    {
        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public static readonly int IntervalDay = int.Parse(ConfigurationHelper.GetAppSetting("IntervalDay"));

        /// <summary>
        /// 上线时间
        /// </summary>
        public static readonly DateTime OnlineTime = DateTime.Parse(ConfigurationHelper.GetAppSetting("OnlineTime"));

        /// <summary>
        /// 可读取文件名称头
        /// </summary>
        public static readonly string ReadFileHeader = ConfigurationHelper.GetAppSetting("ReadFileHeader");

        /// <summary>
        /// 批次号文件夹后缀名
        /// </summary>
        public static readonly string BatchModelDirName = ConfigurationHelper.GetAppSetting("BatchModelDirName");

        /// <summary>
        /// 正常按照箱号取模值
        /// </summary>
        public static readonly int NormalModValue = int.Parse(ConfigurationHelper.GetAppSetting("NormalModValue"));

          /// <summary>
        /// 按照批次号取模值
        /// </summary>
        public static readonly int BatchModValue = int.Parse(ConfigurationHelper.GetAppSetting("BatchModValue"));
        
        /// <summary>
        /// 写文件名称头
        /// </summary>
        public static readonly string WriteFileHeader = ConfigurationHelper.GetAppSetting("WriteFileHeader");

        /// <summary>
        /// 缓冲文件存储路径
        /// </summary>
        public static readonly string FILE_DIR = ConfigurationHelper.GetAppSetting("FileDir");

        /// <summary>
        /// 线程文件夹名
        /// </summary>
        public static readonly string THREAD_CATEGORY_DIR = ConfigurationHelper.GetAppSetting("ThreadCategoryDirName");

    }
}

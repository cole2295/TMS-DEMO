using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.Tms2ThridPartyImpl
{
    public static class Consts
    {
        /// <summary>
        /// 每批次最大执行条数
        /// </summary>
        public static readonly int BATCH_MAX_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("BatchCount"));

        /// <summary>
        /// 线程数
        /// </summary>
        public static readonly int THREAD_COUNT = int.Parse(ConfigurationHelper.GetAppSetting("ThreadCount"));

        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public static readonly int IntervalDay = int.Parse(ConfigurationHelper.GetAppSetting("IntervalDay"));

        /// <summary>
        /// 上线时间
        /// </summary>
        public static readonly DateTime OnlineTime = DateTime.Parse(ConfigurationHelper.GetAppSetting("OnlineTime"));
    }
}

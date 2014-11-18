using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.PreDispatchImpl
{
    public static class Consts
    {
        /// <summary>
        /// 每次调度的最大数量
        /// </summary>
        public static readonly int BATCH_COUNT = Convert.ToInt32(ConfigurationHelper.GetAppSetting("BatchCount"));

        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public static readonly int IntervalDay = int.Parse(ConfigurationHelper.GetAppSetting("IntervalDay"));


    }
}

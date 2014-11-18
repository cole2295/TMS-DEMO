using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.MonitorImpl
{
    public class Consts
    {
        /// <summary>
        /// Job配置文件夹名称
        /// </summary>
        public static readonly string JobConfigDir = ConfigurationHelper.GetAppSetting("JobConfigDir");

    }
}

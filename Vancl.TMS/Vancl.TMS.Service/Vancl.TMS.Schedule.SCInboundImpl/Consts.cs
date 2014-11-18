using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.Schedule.SCInboundImpl
{
    public class Consts
    {
        /// <summary>
        /// 取模值
        /// </summary>
        public static readonly int ModValue = int.Parse(ConfigurationHelper.GetAppSetting("ModValue"));

        /// <summary>
        /// 处理计数
        /// </summary>
        public static readonly int OpCount = int.Parse(ConfigurationHelper.GetAppSetting("OpCount"));

        /// <summary>
        /// 每次处理的运单数量
        /// </summary>
        public static readonly int PerBatchCount = int.Parse(ConfigurationHelper.GetAppSetting("PerBatchCount"));

        /// <summary>
        /// 同步间隔，用于缩小中间表检索的数据量
        /// </summary>
        public static readonly int IntervalDay = int.Parse(ConfigurationHelper.GetAppSetting("IntervalDay"));
        
        /// <summary>
        /// Vancl,V+区域短信启用停用标志
        /// </summary>
        public static readonly bool IsAreaEnabled = Convert.ToBoolean(ConfigurationHelper.GetAppSetting("IsAreaEnabled"));

        /// <summary>
        /// Vancl,V+区域短信生效时间
        /// </summary>
        public static readonly DateTime? AreaEffectiveTime = String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("AreaEffectiveTime")) ? (DateTime?)null : DateTime.Parse(ConfigurationHelper.GetAppSetting("AreaEffectiveTime"));

        /// <summary>
        /// Vancl,V+区域短信截止时间
        /// </summary>
        public static readonly DateTime? AreaDeadLine = String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("AreaDeadLine")) ? (DateTime?)null : DateTime.Parse(ConfigurationHelper.GetAppSetting("AreaDeadLine"));

        /// <summary>
        /// 商家短信启用停用标志
        /// </summary>
        public static readonly bool IsMerchantEnabled = Convert.ToBoolean(ConfigurationHelper.GetAppSetting("IsMerchantEnabled"));

        /// <summary>
        /// Vancl,V+区域短信生效时间
        /// </summary>
        public static readonly DateTime? MerchantEffectiveTime = String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("MerchantEffectiveTime")) ? (DateTime?)null : DateTime.Parse(ConfigurationHelper.GetAppSetting("MerchantEffectiveTime"));

        /// <summary>
        /// Vancl,V+区域短信截止时间
        /// </summary>
        public static readonly DateTime? MerchantDeadLine = String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("MerchantDeadLine")) ? (DateTime?)null : DateTime.Parse(ConfigurationHelper.GetAppSetting("MerchantDeadLine"));


    }
}

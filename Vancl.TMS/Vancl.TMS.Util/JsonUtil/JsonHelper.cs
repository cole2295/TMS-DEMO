using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Vancl.TMS.Util.JsonUtil
{
    public class JsonHelper
    {
        /// <summary>
        /// 使用JSON.NET 转换对象到JSON字符串
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertToJosnString(object item)
        {
            if (item != null)
            {
                return JsonConvert.SerializeObject(item);
            }
            return "";
        }

        /// <summary>
        /// 使用JSON.NET 转换JSON字符串到.NET对象
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T ConvertToObject<T>(string strJson)
        {
            if (!string.IsNullOrEmpty(strJson))
            {
                return JsonConvert.DeserializeObject<T>(strJson);
            }
            return default(T);
        }

        
    }
}

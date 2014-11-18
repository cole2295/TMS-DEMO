using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Vancl.TMS.Model;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using Vancl.TMS.Model.Common;
using System.Web;
using Vancl.TMS.Util;
using Vancl.TMS.Core.ObjectFactory;

namespace Vancl.TMS.Core.ServiceFactory
{
    /// <summary>
    /// 普通服务对象工厂
    /// </summary>
    public class ServiceFactory : ObjectFactory.ObjectFactory
    {
        #region 静态方法,获取指定服务类型的对象 +2重载

        /// <summary>
        /// 获取普通服务对象
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <param name="name">服务名</param>
        /// <returns></returns>
        public static T GetService<T>(string name)
        {
            return (T)GetNormalObject(name, ObjectCategory.Service);
        }

        /// <summary>
        /// 获取普通服务对象(无参,返回配置文件中第一个实现接口T的对象)
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            Type type = typeof(T);

            Type[] genericTypes = type.GetGenericArguments();
            if (genericTypes.Length == 0)
            {
                genericTypes = null;
            }

            string cfgName = string.Empty;
            string cfgInterfaceName = type.FullName;
            foreach (DictionaryEntry item in normalServices)
            {
                ObjectInfo oi = (ObjectInfo)item.Value;
                if (StringUtil.GetRealName(oi.InterFaceName, genericTypes) == cfgInterfaceName)
                {
                    cfgName = oi.Name;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(cfgName))
            {
                throw new Exception("未能找到实现" + cfgInterfaceName + "接口的服务!");
            }
            else
            {
                return GetService<T>(cfgName);
            }
        }

        #endregion
    }
}

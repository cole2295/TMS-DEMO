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
using Vancl.TMS.Util.IO;
using Vancl.TMS.Util;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.FormulaManager;

namespace Vancl.TMS.Core.ObjectFactory
{
    public class ObjectFactory
    {
        #region 服务和公式对象列表

        /// <summary>
        /// 普通服务配置列表
        /// </summary>
        protected static Hashtable normalServices = new Hashtable();

        /// <summary>
        /// 公式类服务配置列表
        /// </summary>
        protected static Hashtable formulaServices = new Hashtable();

        #endregion

        #region 静态构造函数,实现应用程序启动时加载配置的服务和公式

        /// <summary>
        /// 静态构造函数,在应用程序启动时执行
        /// </summary>
        static ObjectFactory()
        {
            LookUpServices();
            LookUpFormulas();
            LoadNormalServices();
            LoadFormulas();
        }

        #endregion

        #region 读取配置文件,并将相关dll加载到AppDomain

        /// <summary>
        /// 加载服务类型
        /// </summary>
        private static void LoadNormalServices()
        {
            ICollection nss = normalServices.Values;
            foreach (ObjectInfo info in nss)
            {
                string name = info.Name;
                string className = info.ClassFullName;
                //动态加载
                try
                {
                    string tmpScopName = info.LoadFrom.Split('\\').LastOrDefault();
                    if (!AppDomain.CurrentDomain.GetAssemblies().Any(p => p.ManifestModule.ScopeName.ToLower().Equals(tmpScopName.ToLower())))
                    {
                        Assembly ass = Assembly.LoadFrom(info.LoadFrom);
                        Type classType = ass.GetType(className);
                        info.ClassType = classType;
                    }
                    else
                    {
                        info.ClassType = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(p => p.ManifestModule.ScopeName.ToLower().Equals(tmpScopName.ToLower())).GetType(className);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("加载程序集失败:" + e.Message);
                }
            }
        }

        /// <summary>
        /// 从配置文件初始化服务
        /// </summary>
        private static void LookUpServices()
        {
            XmlDocument Doc = new XmlDocument();
            try
            {
                //判断配置的路径是否为绝对路径
                string path = Consts.SERVICE_CONFIG_PATH.LastIndexOf(":") == 1 ? Consts.SERVICE_CONFIG_PATH : IOHelper.GetAbsolutePath(Consts.SERVICE_CONFIG_PATH);
                string serverPath = Consts.SERVER_PATH.LastIndexOf(":") == 1 ? Consts.SERVER_PATH : IOHelper.GetAbsolutePath(Consts.SERVER_PATH);
                StringBuilder sb = new StringBuilder("");
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }

                Doc.LoadXml(sb.ToString());
                sb = null;
                XmlElement element = Doc.DocumentElement;
                string domainStr = element.GetAttribute("domain");
                string domainDescStr = element.GetAttribute("desc");
                XmlNodeList list = element.GetElementsByTagName("service");

                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        ObjectInfo serviceInfo = new ObjectInfo();

                        serviceInfo.Name = list[i].Attributes["name"] == null ? null : list[i].Attributes["name"].Value;
                        serviceInfo.ClassFullName = list[i].Attributes["className"] == null ? null : list[i].Attributes["className"].Value;
                        serviceInfo.LoadFrom = list[i].Attributes["loadFrom"] == null ? null : Path.Combine(serverPath, list[i].Attributes["loadFrom"].Value);
                        serviceInfo.InterFaceName = list[i].Attributes["serviceName"] == null ? null : list[i].Attributes["serviceName"].Value;

                        if (!normalServices.ContainsKey(serviceInfo.Name))
                        {
                            normalServices.Add(serviceInfo.Name, serviceInfo);
                        }
                        else
                        {
                            throw new Exception("配置中存在重复的服务名" + serviceInfo.Name + "请检查配置文件!");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception e)
            {
                //log.Error("服务信息载入失败，请检查服务配置文件" + fileName + ", " + e.Message);
                //System.Console.WriteLine("服务信息载入失败，请检查服务配置文件" + fileName + ", " + e.Message);
                throw e;
            }
            finally
            {
                if (Doc != null)
                {
                    Doc = null;
                }
            }
        }

        /// <summary>
        /// 加载公式类型
        /// </summary>
        private static void LoadFormulas()
        {
            ICollection nss = formulaServices.Values;
            foreach (ObjectInfo info in nss)
            {
                string name = info.Name;
                string className = info.ClassFullName;
                //动态加载
                try
                {
                    string tmpScopName = info.LoadFrom.Split('\\').LastOrDefault();
                    if (!AppDomain.CurrentDomain.GetAssemblies().Any(p => p.ManifestModule.ScopeName.ToLower().Equals(tmpScopName.ToLower())))
                    {
                        Assembly ass = Assembly.LoadFrom(info.LoadFrom);
                        Type classType = ass.GetType(className);
                        info.ClassType = classType;
                    }
                    else
                    {
                        info.ClassType = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(p => p.ManifestModule.ScopeName.ToLower().Equals(tmpScopName.ToLower())).GetType(className);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("加载程序集失败:" + e.Message);
                }
            }
        }

        /// <summary>
        /// 从配置文件初始化函数
        /// </summary>
        private static void LookUpFormulas()
        {
            XmlDocument Doc = new XmlDocument();
            try
            {
                //判断配置的路径是否为绝对路径
                string path = Consts.SERVICE_CONFIG_PATH.LastIndexOf(":") == 1 ? Consts.FORMULA_CONFIG_PATH : IOHelper.GetAbsolutePath(Consts.FORMULA_CONFIG_PATH);
                string serverPath = Consts.SERVER_PATH.LastIndexOf(":") == 1 ? Consts.SERVER_PATH : IOHelper.GetAbsolutePath(Consts.SERVER_PATH);
                StringBuilder sb = new StringBuilder("");
                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("GB2312")))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }

                Doc.LoadXml(sb.ToString());
                sb = null;
                XmlElement element = Doc.DocumentElement;
                XmlNodeList list = element.GetElementsByTagName("formula");

                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        ObjectInfo formulaInfo = new ObjectInfo();

                        formulaInfo.Name = list[i].Attributes["name"] == null ? null : list[i].Attributes["name"].Value;
                        formulaInfo.ClassFullName = list[i].Attributes["className"] == null ? null : list[i].Attributes["className"].Value;
                        formulaInfo.LoadFrom = list[i].Attributes["loadFrom"] == null ? null : Path.Combine(serverPath, list[i].Attributes["loadFrom"].Value);
                        formulaInfo.InterFaceName = list[i].Attributes["serviceName"] == null ? null : list[i].Attributes["serviceName"].Value;
                        if (!formulaServices.ContainsKey(formulaInfo.Name))
                        {
                            formulaServices.Add(formulaInfo.Name, formulaInfo);
                        }
                        else
                        {
                            throw new Exception("存在相同的函数名" + formulaInfo.Name + "请检查配置文件!");
                        }
                    }
                    catch (Exception ee)
                    {
                        throw ee;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (Doc != null)
                {
                    Doc = null;
                }
            }
        }

        #endregion

        #region 静态方法,获取指定名称服务对象

        /// <summary>
        /// 获取指定名称和类型对象
        /// </summary>
        /// <param name="name">配置文件中的name</param>
        /// <param name="category">类型(公式服务,普通服务)</param>
        /// <returns></returns>
        protected static object GetNormalObject(string name, ObjectCategory category)
        {
            try
            {
                ObjectInfo info = null;
                if (category == ObjectCategory.Service)
                {
                    if (normalServices.ContainsKey(name))
                        info = (ObjectInfo)normalServices[name];
                    else
                        throw new Exception("不存在的服务:" + name);
                }
                if (category == ObjectCategory.Formula)
                {
                    if (formulaServices.ContainsKey(name))
                        info = (ObjectInfo)formulaServices[name];
                    else
                        throw new Exception("不存在的服务:" + name);
                }
                Type classType = info.ClassType;
                if (classType == null)
                    throw new Exception("不存在的类型:" + info.ClassFullName);

                return System.Activator.CreateInstance(classType, false);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion


    }
}

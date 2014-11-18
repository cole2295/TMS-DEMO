using System;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Vancl.TMS.Util;

namespace LB_PanlService
{
    public class ServicesManager
    {
        IScheduler _sched = null;

        public void Start()
        {
            try
            {
                if (null == _sched)
                {
                    var properties = new NameValueCollection();
                    properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";
                    properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                    properties["quartz.threadPool.threadCount"] = "1";
                    properties["quartz.threadPool.threadPriority"] = "Normal";
                    properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.JobInitializationPlugin, Quartz";
                    properties["quartz.plugin.xml.fileNames"] = "~/JobConfig.xml";

                    ISchedulerFactory sf = new StdSchedulerFactory(properties);
                    _sched = sf.GetScheduler();
                }

                _sched.Start();

                MessageCollector.Instance.Collect("default", "提货计划服务启动", true);
               
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("default", ex.ToString(), true);
            }
        }

        public void Stop()
        {
            try
            {
                if (null != _sched)
                {
                    _sched.Shutdown(true);
                }

                MessageCollector.Instance.Collect("default", "提货计划服务停用", true);
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect("default", ex.ToString(), true);
            }
        }
    }
}

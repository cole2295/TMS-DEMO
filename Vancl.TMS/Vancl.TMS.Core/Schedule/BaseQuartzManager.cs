using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;

namespace Vancl.TMS.Core.Schedule
{
    public class BaseQuartzManager
    {
        protected IScheduler _sched = null;

        public virtual void Start()
        {
            if (null == _sched)
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "XmlConfiguredInstance";
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool,Quartz";
                properties["quartz.threadPool.threadCount"] = CommonConst.ThreadCount.ToString();
                properties["quartz.threadPool.threadPriority"] = "Normal";
                properties["quartz.plugin.xml.type"] = "Quartz.Plugin.Xml.JobInitializationPlugin,Quartz";
                properties["quartz.plugin.xml.fileNames"] = CommonConst.JobConfigPath.ToString();
                ISchedulerFactory sf = new StdSchedulerFactory(properties);
                _sched = sf.GetScheduler();
            }
            Init();
            _sched.Start();
        }

        /// <summary>
        /// 初始化,添加Listener等
        /// </summary>
        protected virtual void Init()
        {

        }

        public virtual void Stop()
        {
            if (null != _sched)
            {
                _sched.Shutdown(true);
            }
        }
    }
}

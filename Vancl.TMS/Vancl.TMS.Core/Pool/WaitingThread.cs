using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace Vancl.TMS.Core.Pool
{
    public class WaitingThread
    {
        private Hashtable _waitingThreads = null;
        /// <summary>
        /// 等待线程集合
        /// </summary>
        public Hashtable WaitingThreads
        {
            get
            {
                if (_waitingThreads == null)
                {
                    _waitingThreads = new Hashtable();
                }
                return _waitingThreads;
            }
            set
            {
                _waitingThreads = value;
            }
        }

        /// <summary>
        /// 通知等待线程进行唤醒
        /// </summary>
        /// <param name="key">线程key</param>
        public void Notify(object key)
        {
            if (_waitingThreads.Count == 0)
            {
                return;
            }
            if (_waitingThreads.Contains(key))
            {
                //唤醒等待线程，并移除出等待线程池
                (_waitingThreads[key] as Thread).Interrupt();
                Remove(key);
                //Console.WriteLine(key + ":唤醒等待线程");
            }
        }

        /// <summary>
        /// 添加线程
        /// </summary>
        /// <param name="key">线程key</param>
        /// <param name="value">线程对象</param>
        public void Add(object key, Thread value)
        {
            WaitingThreads.Add(key, value);
        }

        /// <summary>
        /// 移除线程
        /// </summary>
        /// <param name="key">线程key</param>
        public void Remove(object key)
        {
            WaitingThreads.Remove(key);
        }
    }
}

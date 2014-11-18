using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using Vancl.TMS.Model.Common;
using System.Timers;

namespace Vancl.TMS.Core.Pool
{
    public class SocketPool
    {
        public static readonly Hashtable context = new Hashtable();
        private static readonly Queue _waitingQueue = new Queue(Consts.SOCKET_WAITING_QUEUE_MAX_COUNT);
        private static Timer _timer = new Timer(10);
        private static object _lockObj = new object();
        private static readonly WaitingThread _waitingThread = new WaitingThread();
        static SocketPool()
        {
            Pool.GetInstance().PoolList.Add(context);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
        }

        static void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_waitingQueue.Count == 0)
            {
                return;
            }
            //把等待队列中的key去匹配可用对象
            lock (_waitingQueue)
            {
                object key = _waitingQueue.Peek();
                VanclObjectInPool<Socket> s = GetAvailableSocket(key);
                if (s != null)
                {
                    //把已匹配的key移除出队列
                    _waitingQueue.Dequeue();
                    //通知对应的等待线程
                    _waitingThread.Notify(key);
                }
            }
        }

        /// <summary>
        /// 从VanclSocket对象池中获取VanclSocket对象
        /// (默认使用线程id作为key)
        /// </summary>
        /// <returns>VanclSocket对象</returns>
        public static VanclObjectInPool<Socket> Get()
        {
            return Get(System.Threading.Thread.CurrentThread.ManagedThreadId);
        }

        /// <summary>
        /// 从VanclSocket对象池中获取VanclSocket对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>VanclSocket对象</returns>
        public static VanclObjectInPool<Socket> Get(object key)
        {
            VanclObjectInPool<Socket> s = null;
            //去池中获取对象或者新生成一个对象
            s = GetSocket(key);
            if (s != null)
            {
                return s;
            }
            //排队队列达到最大值则返回
            if (_waitingQueue.Count >= Consts.SOCKET_WAITING_QUEUE_MAX_COUNT)
            {
                //throw new Exception("等待队列已满");
                //Console.WriteLine(key + ":等待队列已满");
                return null;
            }
            //排队等待
            WaitingForAvailableSocket(key);
            s = GetSocketUseSameKey(key);
            if (s == null)
            {
                //Console.WriteLine(key + ":等待超时");
            }
            return s;
        }

        /// <summary>
        /// 排队等待可用Socket
        /// </summary>
        /// <param name="key">对象的key</param>
        private static void WaitingForAvailableSocket(object key)
        {
            //加入等待队列
            lock (_waitingQueue)
            {
                _waitingQueue.Enqueue(key);
            }
            //把该线程的引用加入到等待线程类中
            _waitingThread.Add(key, System.Threading.Thread.CurrentThread);
            //线程进行排队等待
            System.Threading.Thread.Sleep(Consts.SOCKET_WAITING_TIME_OUT);
        }

        /// <summary>
        /// 使用key直接去池中取得对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>VanclSocket对象</returns>
        private static VanclObjectInPool<Socket> GetSocketUseSameKey(object key)
        {
            if (context.Contains(key))
            {
                SocketPoolModel spm = context[key] as SocketPoolModel;
                context[key] = spm;
                //Console.WriteLine(key + ":获得同一线程的对象");
                return (spm.Value as VanclObjectInPool<Socket>);
            }
            return null;
        }

        /// <summary>
        /// 获取VanclSocket对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>VanclSocket对象</returns>
        private static VanclObjectInPool<Socket> GetSocket(object key)
        {
            VanclObjectInPool<Socket> s = null;
            //直接去池中取相同key的对象
            s = GetSocketUseSameKey(key);
            if (s != null)
            {
                return s;
            }
            if (context.Count <= Consts.SOCKET_POOL_MAX_COUNT && _waitingQueue.Count == 0)
            {
                //去池中取得可用对象
                s = GetAvailableSocket(key);
                if (s != null)
                {
                    return s;
                }
                lock (_lockObj)
                {
                    if (context.Count < Consts.SOCKET_POOL_MAX_COUNT)
                    {
                        //生成一个新的对象并加入池中
                        s = GetNewSocket();
                        SocketPoolModel spm = new SocketPoolModel();
                        spm.Value = s;
                        context.Add(key, spm);
                    }
                }
                return s;
            }
            return null;
        }

        /// <summary>
        /// 获取对象池中可用的VanclSocket对象
        /// </summary>
        /// <param name="key">对象的key</param>
        /// <returns>VanclSocket对象</returns>
        private static VanclObjectInPool<Socket> GetAvailableSocket(object key)
        {
            if (context.Count > Consts.SOCKET_POOL_MAX_COUNT)
            {
                return null;
            }
            VanclObjectInPool<Socket> s = null;
            SocketPoolModel spm = null;
            Array array = new object[context.Count];
            context.Keys.CopyTo(array, 0);
            foreach (object k in array)
            {
                spm = context[k] as SocketPoolModel;
                //找到池中第一个没有被使用的对象
                if (!spm.IsUsing)
                {
                    //把找到的对象的key更新为最新的key
                    s = spm.Value as VanclObjectInPool<Socket>;
                    context.Add(key, spm);
                    context.Remove(k);
                    //Console.WriteLine(key + ":获得可用对象");
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取新的VanclSocket对象
        /// </summary>
        /// <returns>VanclSocket对象</returns>
        private static VanclObjectInPool<Socket> GetNewSocket()
        {
            return new VanclObjectInPool<Socket>();
        }
    }
}

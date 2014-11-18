using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Timers;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.DateTimeUtil;

namespace Vancl.TMS.Core.Pool
{
    public class Pool
    {
        public readonly List<Hashtable> PoolList = new List<Hashtable>();
        private Timer _timer = null;
        private static Pool _pool = null;
        private static readonly object _lockObj = new object();
        private Pool()
        {
            _timer = new Timer(Consts.POOL_CLEAR_INTERVAL);
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true;
        }

        public static Pool GetInstance()
        {
            if (_pool == null)
            {
                lock (_lockObj)
                {
                    if (_pool == null)
                    {
                        _pool = new Pool();
                    }
                }
            }
            return _pool;
        }

        protected void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (PoolList.Count == 0)
            {
                return;
            }
            //对池中一定时间没有使用的对象进行执行清除
            DateTime now = DateTime.Now;
            //Console.WriteLine("执行清空超时未用的对象:数量" + PoolList[0].Count);
            foreach (Hashtable ht in PoolList)
            {
                Array array = new object[ht.Count];
                ht.Keys.CopyTo(array, 0);
                foreach (object k in array)
                {
                    PoolModel pm = ht[k] as PoolModel;
                    if (!pm.IsUsing && now.DateDiff(pm.LastTime).TotalMilliseconds > Consts.POOL_CLEAR_TIMESPAN)
                    {
                        ht.Remove(k);
                        //Console.WriteLine(k + ":清空超时未用的对象");
                    }
                }
            }
        }
    }
}

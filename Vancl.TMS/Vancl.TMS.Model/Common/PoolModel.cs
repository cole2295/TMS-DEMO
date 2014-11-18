using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Common
{
    public class PoolModel
    {
        private object _value;
        /// <summary>
        /// 池中的值
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private DateTime _lastTime;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastTime
        {
            set { _lastTime = value; }
            get { return _lastTime; }
        }

        /// <summary>
        /// 是否正在使用
        /// </summary>
        public virtual bool IsUsing
        {
            get { return false; }
        }
    }
}

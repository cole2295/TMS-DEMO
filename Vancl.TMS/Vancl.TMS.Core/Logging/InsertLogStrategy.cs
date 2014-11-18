using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Core.Logging
{
    /// <summary>
    /// 新增日志策略
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InsertLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable, new()
    {
        public override string GetLogNote()
        {
            var custAttr = typeof(T).GetCustomAttributes(typeof(LogNameAttribute), false);
            if (custAttr != null && custAttr.Length > 0)
            {
                _note = string.Format("新增:{0}", (custAttr[0] as LogNameAttribute).Name);
            }
            return _note;
        }

        public override bool IsDoOperation()
        {
            return true;
        }
    }
}

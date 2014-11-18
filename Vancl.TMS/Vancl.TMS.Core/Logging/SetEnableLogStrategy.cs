using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using System.Reflection;

namespace Vancl.TMS.Core.Logging
{
    /// <summary>
    /// 启用停用策略
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SetEnableLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable ,new()
    {
        public override string GetLogNote()
        {
            if (_nowModel == null)
            {
                return "";
            }
            PropertyInfo pi = _nowModel.GetType().GetProperty("IsEnabled");
            if (pi == null)
            {
                return "";
            }
            bool isEnabled = Convert.ToBoolean(pi.GetValue(_nowModel, null));
            string strPre = isEnabled ? "启用" : "停用";
            return "进行了[" + strPre + "]操作";
        }

        public override bool IsDoOperation()
        {
            if (_nowModel is ICanSetEnable)
            {
                return true;
            }
            return false;
        }

        public override string GetPrimaryKey()
        {
            if (_nowModel == null)
            {
                return "";
            }
            PropertyInfo pi = _nowModel.GetType().GetProperty("LogKeyValue");
            if (pi == null)
            {
                return "";
            }
            return Convert.ToString(pi.GetValue(_nowModel, null));
        }
    }
}

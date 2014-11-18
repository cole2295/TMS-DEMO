using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;

namespace Vancl.TMS.Core.Logging
{
    /// <summary>
    /// 自定义日志策略
    /// </summary>
    public class CustomizeLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable, new()
    {

        public override string GetLogNote()
        {
            if (_nowModel == null)
            {
                return "";
            }
            return (_nowModel as ICustomizeLogable).CustomizeLog;
        }

        public override bool IsDoOperation()
        {
            if (_nowModel is ICustomizeLogable)
            {
                return true;
            }
            return false;
        }


    }
}

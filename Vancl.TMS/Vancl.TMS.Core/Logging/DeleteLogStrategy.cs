using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;

namespace Vancl.TMS.Core.Logging
{
    public class DeleteLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable, new()
    {
        public override string GetLogNote()
        {
            return "";
        }

        public override bool IsDoOperation()
        {
            return true;
        }
    }
}

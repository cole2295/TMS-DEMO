using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.Exceptions
{
    public class CodeNotValidException : Exception
    {
        public override string Message
        {
            get
            {
                return "代码有误";
            }
        }
    }
}

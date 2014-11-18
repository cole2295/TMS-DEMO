using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Core.ACIDManager
{
    public interface IACID :  IDisposable
    {
        void Complete();
    }


}

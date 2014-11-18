using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vancl.TMS.Model.CustomizeFlow;

namespace Vancl.TMS.IBLL.CustomizeFlow
{
    public interface IChecker<T>
    {
        CheckerResult Check(T t);

        bool IsMatchChecker(string checkerType);
    }
}

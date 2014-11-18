using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IDAL.Log
{
    public interface ILogDAL<T> where T : BaseLogModel
    {
        int Write(T model);
        List<T> Read(BaseLogSearchModel model);
    }
}

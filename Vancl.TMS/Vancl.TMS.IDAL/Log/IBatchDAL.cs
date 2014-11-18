using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Log
{
    public interface IBatchDAL
    {
        List<BatchModel> GetBatch(BatchSearchModel searchModel);

        Int32 RepairTest(String txt);
    }
}

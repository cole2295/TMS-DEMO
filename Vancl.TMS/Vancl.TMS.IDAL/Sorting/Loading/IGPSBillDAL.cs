using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Loading;

namespace Vancl.TMS.IDAL.Sorting.Loading
{
    public interface IGPSBillDAL
    {
        int AddGPSBill(GPSBillModel model);

        bool IsExist(string formCode);
    }
}

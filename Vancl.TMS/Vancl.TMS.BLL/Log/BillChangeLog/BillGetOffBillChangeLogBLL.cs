using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class BillGetOffBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "{0}已经下车，车牌号{1}";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ExtendedObj.FormCode == null)
            {
                throw new ArgumentNullException("FormCode is null");
            }
            if (dynamicModel.ExtendedObj.TruckNo == null)
            {
                throw new ArgumentNullException("TruckNo is null");
            }
            return String.Format(LOG_NOTE, dynamicModel.ExtendedObj.FormCode, dynamicModel.ExtendedObj.TruckNo);
        }
    }
}

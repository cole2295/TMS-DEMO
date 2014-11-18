using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class ReturnBoxPrintBackPackingBillChangeLogBLL:BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "退货分拣称重装箱打印";
        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ReturnStatus == null)
            {
                throw new ArgumentNullException("ReturnStatus is null");
            }
            return LOG_NOTE;
        }
    }
}

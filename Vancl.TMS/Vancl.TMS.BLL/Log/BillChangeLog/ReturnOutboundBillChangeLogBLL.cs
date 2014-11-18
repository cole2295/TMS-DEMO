using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class ReturnOutboundBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "您的订单已到达{0}，正在退货出库";
        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ReturnStatus == null)
            {
                throw new ArgumentNullException("ReturnStatus is null");
            }
            return String.Format(LOG_NOTE, dynamicModel.ExtendedObj.CreateDeptName);
        }
    }
}

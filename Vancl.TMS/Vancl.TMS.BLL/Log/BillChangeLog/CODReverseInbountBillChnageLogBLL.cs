using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class MerchantBillRefundBillChnageLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "商家入库确认";
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

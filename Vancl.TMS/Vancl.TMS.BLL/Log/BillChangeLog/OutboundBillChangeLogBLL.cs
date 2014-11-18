using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class OutboundBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "您的订单已从{0}出库，正在发往{1}途中";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ExtendedObj.SortCenterMnemonicName == null)
            {
                throw new ArgumentNullException("SortCenterMnemonicName is null");
            }
            if (dynamicModel.ExtendedObj.ArrivalMnemonicName == null)
            {
                throw new ArgumentNullException("ArrivalMnemonicName is null");
            }
            return String.Format(LOG_NOTE, dynamicModel.ExtendedObj.SortCenterMnemonicName, dynamicModel.ExtendedObj.ArrivalMnemonicName);
        }
    }
}

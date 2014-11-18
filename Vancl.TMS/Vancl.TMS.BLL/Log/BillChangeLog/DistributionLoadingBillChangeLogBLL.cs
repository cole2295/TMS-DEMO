using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class DistributionLoadingBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "您的订单已从{0}发出，正在发往{1}";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ExtendedObj.OpStationMnemonicName == null)
            {
                throw new ArgumentNullException("OpStationMnemonicName is null");
            }
            if (dynamicModel.ExtendedObj.DeliverStationMnemonicName == null)
            {
                throw new ArgumentNullException("DeliverStationMnemonicName is null");
            }
            return String.Format(LOG_NOTE, dynamicModel.ExtendedObj.OpStationMnemonicName, dynamicModel.ExtendedObj.DeliverStationMnemonicName);
        }
    }
}

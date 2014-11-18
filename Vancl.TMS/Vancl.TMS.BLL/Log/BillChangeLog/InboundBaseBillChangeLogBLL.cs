using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    /// <summary>
    /// 入库通用更改日志
    /// </summary>
    internal class InboundBaseBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "您的订单已到达{0}{1}，正在进行分拣";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            //if (dynamicModel.ExtendedObj.DistributionMnemonicName == null)
            //{
            //    throw new ArgumentNullException("DistributionMnemonicName is null");
            //}
            if (dynamicModel.ExtendedObj.SortCenterMnemonicName == null)
            {
                throw new ArgumentNullException("SortCenterMnemonicName is null");
            }
            return String.Format(LOG_NOTE, "", dynamicModel.ExtendedObj.SortCenterMnemonicName);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    internal class BillValidateBillChangeLogBLL : BillChangeLogBLL
    {
        protected override string GetNote(Model.Log.BillChangeLogDynamicModel dynamicModel)
        {
            return string.Format("订单号：{0}，运单号：{1}，验证结果：{2}",
                dynamicModel.ExtendedObj.CustomerOrder,
                dynamicModel.ExtendedObj.FormCode,
                dynamicModel.ExtendedObj.Validated ? "已通过" : "未通过"
                );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    /// <summary>
    /// 面单打印变更日志
    /// </summary>
    internal class WeighPrintBillChangeLogBLL : BillChangeLogBLL
    {
        // private static readonly String LOG_NOTE = "您的订单已称重打印，等待入库中";
        private static readonly String LOG_NOTE = "{0}({1}/{2})，{3}";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel.ExtendedObj.IsPrint == null) throw new ArgumentNullException("IsPrint is null");
            if (dynamicModel.ExtendedObj.Weight == null) throw new ArgumentNullException("Weight is null");
            if (dynamicModel.ExtendedObj.PackageIndex == null) throw new ArgumentNullException("PackageIndex is null");
            if (dynamicModel.ExtendedObj.PackageCount == null) throw new ArgumentNullException("PackageCount is null");

            bool IsPrint = Convert.ToBoolean(dynamicModel.ExtendedObj.IsPrint);
            decimal Weight = Convert.ToDecimal(dynamicModel.ExtendedObj.Weight);
            int PackageIndex = Convert.ToInt32(dynamicModel.ExtendedObj.PackageIndex);
            int PackageCount = Convert.ToInt32(dynamicModel.ExtendedObj.PackageCount);
            if (PackageCount <= 0) PackageCount = 1;

            string BeWeighted = Weight > 0 ? "已记录重量" : "未记录重量";
            string BePrint = IsPrint ? "面单已打印" : "面单未打印";

            return String.Format(LOG_NOTE, BePrint, PackageIndex, PackageCount, BeWeighted);
        }
    }
}

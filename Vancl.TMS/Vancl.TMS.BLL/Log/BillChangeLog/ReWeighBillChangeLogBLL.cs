using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.BillChangeLog
{
    /// <summary>
    /// 重新称重变更日志
    /// </summary>
    internal class ReWeighBillChangeLogBLL : BillChangeLogBLL
    {
        private static readonly String LOG_NOTE = "重新称重，箱号：{0}，重量：{1}";

        protected override string GetNote(BillChangeLogDynamicModel dynamicModel)
        {
            //日志扩展属性
            //dynamicModel.ExtendedObj.IsPrint = false;
            //dynamicModel.ExtendedObj.Weight = weight;
            //dynamicModel.ExtendedObj.PackageIndex = packageIndex;
            //dynamicModel.ExtendedObj.PackageCount = BillInfoModel.PackageCount;

            return String.Format(LOG_NOTE, dynamicModel.ExtendedObj.PackageIndex, dynamicModel.ExtendedObj.Weight);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.IDAL.Sorting.BillPrint
{
    public interface IBillInfoDAL
    {
        BillInfoModel GetBillInfoByFormCode(string formCode);

        /// <summary>
        /// 更新运单称重重量
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        int UpdateBillInfoWeight(string fromCode, decimal weight);


        /// <summary>
        /// 设置面单校验状态
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        int UpdateValidateStatus(string formCode, bool p);
    }
}

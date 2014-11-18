using System.Collections.Generic;
using Vancl.TMS.Model.LadingBill;

namespace Vancl.TMS.IBLL.LadingBill
{
    public interface IMERCHANTWAREHOUSEBLL
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        IList<MERCHANTWAREHOUSE> GetModelList(Dictionary<string, object> searchParams);

        /// <summary>
        /// 根据仓库id获得仓库实体
        /// </summary>
        /// <param name="MERCHANTWAREHOUSE"></param>
        /// <returns></returns>
        MERCHANTWAREHOUSE GetModelByid(string MERCHANTWAREHOUSE);
    }
}

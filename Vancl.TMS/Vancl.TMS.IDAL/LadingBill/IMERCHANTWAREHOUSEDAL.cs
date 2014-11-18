using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LadingBill;

namespace Vancl.TMS.IDAL.LadingBill
{
    public interface IMERCHANTWAREHOUSEDAL
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        IList<MERCHANTWAREHOUSE> GetModelList(Dictionary<string, object> searchParams);

        /// <summary>
        /// 根据仓库iD
        /// </summary>
        /// <param name="MERCHANTWAREHOUSE"></param>
        /// <returns></returns>
        MERCHANTWAREHOUSE GetModelByid(string warehouseid);
    }
}

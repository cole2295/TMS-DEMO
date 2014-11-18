using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.IBLL.Common
{
    public interface ITransPointBLL
    {
        /// <summary>
        /// 获取出发地
        /// </summary>
        /// <returns></returns>
        List<ExpressCompanyModel> GetDepartures();

        /// <summary>
        /// 获取目的地
        /// </summary>
        /// <returns></returns>
        List<ExpressCompanyModel> GetArrivals(int departureId);
    }
}

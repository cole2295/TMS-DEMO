using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 分拣关系数据层
    /// </summary>
    public interface IWaybill_SortCenterDAL
    {
        /// <summary>
        /// 出入库关系记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Merge(Waybill_SortCenterEntityModel model);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 逆向流程LMS接口层
    /// </summary>
    public interface IWayBillReturnDAL
    {
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="WaybillNo"></param>
        /// <returns></returns>
        WaybillReturnEntityModel GetModel(long WaybillNo);

        bool Update(WaybillReturnEntityModel model);
    }
}

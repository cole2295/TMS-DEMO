using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 退货分拣称重数据层接口
    /// </summary>
    public interface IWaybillReturnDetailInfoDAL
    {
        /// <summary>
        /// 添加退货分拣称重运单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>退货运单主键ID</returns>
        long Add(WaybillReturnDetailInfoEntityModel model);
    }
}

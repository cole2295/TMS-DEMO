using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Transport.Dispatch
{
    public interface IDispTransitionBLL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(DispTransitionModel model);

        /// <summary>
        /// 根据提货单号删除交接单信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ResultModel Delete(string deliveryNo);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Update(DispTransitionModel model);

        /// <summary>
        /// 根据提货单号获取提货单交接信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DispTransitionModel Get(string deliveryNo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;

namespace Vancl.TMS.IDAL.Transport.Dispatch
{
    public interface IDispTransitionDAL : ISequenceDAL
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(DispTransitionModel model);

        /// <summary>
        /// 根据提货单号删除交接单信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        int Delete(string deliveryNo);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(DispTransitionModel model);

        /// <summary>
        /// 根据提货单号获取提货单交接信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DispTransitionModel Get(string deliveryNo);
    }
}

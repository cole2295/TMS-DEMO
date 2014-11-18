using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.InTransit;

namespace Vancl.TMS.IDAL.Delivery.InTransit
{
    /// <summary>
    /// 到货延误
    /// </summary>
    public interface IDelayDAL : ISequenceDAL
    {
        /// <summary>
        /// 添加到货延误信息
        /// </summary>
        /// <param name="delay">延误对象</param>
        /// <returns></returns>
        int Add(DelayModel delay);

        /// <summary>
        /// 取得到货延误Model
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DelayModel GetDelayModel(string deliveryNo);

        /// <summary>
        /// 根据延误主键id取得提货单号
        /// </summary>
        /// <param name="did">延误主键id</param>
        /// <returns></returns>
        string GetDeliveryNo(long did);
    }
}

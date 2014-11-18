using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim.Lost;

namespace Vancl.TMS.IDAL.Claim
{
    public interface ILostDetailDAL
    {
        /// <summary>
        /// 增加丢失明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(LostDetailModel model);

        /// <summary>
        /// 删除丢失明细
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        int Delete(string deliveryNo);
    }
}

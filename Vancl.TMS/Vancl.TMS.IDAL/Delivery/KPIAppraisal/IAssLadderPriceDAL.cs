using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.KPIAppraisal;

namespace Vancl.TMS.IDAL.Delivery.KPIAppraisal
{
    public interface IAssLadderPriceDAL
    {
        /// <summary>
        /// 批量新增阶梯价格
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        int Add(List<AssLadderPriceModel> lstModel);

        /// <summary>
        /// 删除阶梯价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        int Delete(string deliveryNo);

        /// <summary>
        /// 是否已经存在阶梯价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExist(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得阶梯价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        List<AssLadderPriceModel> Get(string deliveryNo);
    }
}

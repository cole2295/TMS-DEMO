using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.KPIAppraisal;

namespace Vancl.TMS.IDAL.Delivery.KPIAppraisal
{
    public interface IAssFixedPriceDAL
    {
        /// <summary>
        /// 新增固定价格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(AssFixedPriceModel model);

        /// <summary>
        /// 修改固定价格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(AssFixedPriceModel model);

        /// <summary>
        /// 是否已经存在固定价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExist(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得固定价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        AssFixedPriceModel Get(string deliveryNo);
    }
}

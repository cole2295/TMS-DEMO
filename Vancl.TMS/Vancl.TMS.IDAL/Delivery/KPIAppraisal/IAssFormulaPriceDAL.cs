using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.KPIAppraisal;

namespace Vancl.TMS.IDAL.Delivery.KPIAppraisal
{
    /// <summary>
    /// 公式价格数据层接口
    /// </summary>
    public interface IAssFormulaPriceDAL
    {
        /// <summary>
        /// 新增公式价格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(AssFormulaPriceModel model);

        /// <summary>
        /// 新增公式价格续价明细信息
        /// </summary>
        /// <param name="detailModel"></param>
        /// <returns></returns>
        int AddOverPriceDetail(List<AssFormulaPriceExModel> detailModel);

        /// <summary>
        /// 删除公式价格续价明细信息
        /// </summary>
        /// <param name="DeliveryNo"></param>
        /// <returns></returns>
        int DeleteOverPriceDetail(string DeliveryNo);

        /// <summary>
        /// 修改公式价格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(AssFormulaPriceModel model);

        /// <summary>
        /// 是否已经存在公式价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExist(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得公式价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        AssFormulaPriceModel Get(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得公式续价明细列表
        /// </summary>
        /// <param name="DeliveryNo"></param>
        /// <returns></returns>
        List<AssFormulaPriceExModel> GetOverPriceDetail(string DeliveryNo);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.Delivery.KPIAppraisal
{
    /// <summary>
    /// KPI考核
    /// </summary>
    public interface IDeliveryAssessmentBLL
    {
        /// <summary>
        /// 添加KPI考核信息
        /// </summary>
        /// <param name="model">KPI考核Model</param>
        /// <returns></returns>
        ResultModel Add(KPICalcInputModel  model);

        /// <summary>
        /// 修改KPI考核信息
        /// </summary>
        /// <param name="model">KPI考核Model</param>
        /// <returns></returns>
        ResultModel Update(KPICalcInputModel model);

        /// <summary>
        /// 是否已经存在提货单KPI考核信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExist(string deliveryNo);

        /// <summary>
        /// 检索KPI考核主列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewDeliveryAssessmentModel> Search(DeliveryAssessmentSearchModel searchModel);

        /// <summary>
        /// 查询或者计算KPI考核运费UI Model
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        ViewAssPriceModel SearchAssPrice(string DeliveryNo);

        /// <summary>
        /// 采用提货单默认设置计算KPI考核运费
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        ViewAssPriceModel KPICalculateByDeliveryDefaultSetting(String DeliveryNo);

        /// <summary>
        /// 计算KPI价格
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        KPICalcOutputModel KPICalculate(KPICalcInputModel inputModel);

        /// <summary>
        /// 根据提货单号取得提货单考核信息主表
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DeliveryAssessmentModel Get(string deliveryNo);
    }
}

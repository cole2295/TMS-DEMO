using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.Delivery.KPIAppraisal
{
    public interface IDeliveryAssessmentDAL
    {
        /// <summary>
        /// 新增提货单KPI考核信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(DeliveryAssessmentModel model);

        /// <summary>
        /// 更新提货单KPI考核信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(DeliveryAssessmentModel model);

        /// <summary>
        /// 删除提货单KPI考核信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        int Delete(string deliveryNo);

        /// <summary>
        /// 是否已经存在提货单KPI考核信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExist(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得提货单KPI考核信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DeliveryAssessmentModel Get(string deliveryNo);

        /// <summary>
        /// 检索KPI考核主列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewDeliveryAssessmentModel> Search(DeliveryAssessmentSearchModel searchModel);
    }
}

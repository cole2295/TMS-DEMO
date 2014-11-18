using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.Spot;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Delivery.Spot
{
    public interface ISiteAssBLL
    {
        /// <summary>
        /// 新增发货现场数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(SiteAssessmentModel model);

        /// <summary>
        /// 批量新增发货现场数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddBatch(SiteAssessmentBatchModel model);

        /// <summary>
        /// 查询状态为“已调度”的调度信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        IList<ViewSiteAssModel> Search(SiteAssSearchModel searchModel);
    }
}

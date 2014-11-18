using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.Spot;

namespace Vancl.TMS.IDAL.Delivery.Spot
{
    public interface ISiteAssDAL
    {
        /// <summary>
        /// 新增发货现场数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(SiteAssessmentModel model);

        /// <summary>
        /// 查询状态为“已调度”的调度信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        IList<ViewSiteAssModel> Search(SiteAssSearchModel searchModel);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.WeighReview;

namespace Vancl.TMS.IDAL.Sorting.Search
{
    public interface IWeighReviewSearchDAL
    {
        /// <summary>
        /// 称重复核查询
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<WeighReviewViewModel> WeighReviewSearch(WeighReviewSearchModel searchModel);
    }
}

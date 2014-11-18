using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.WeighReview;

namespace Vancl.TMS.IBLL.Sorting.Search
{
    public interface IWeighReviewSearchService
    {
        PagedList<WeighReviewViewModel> WeighReviewSearch(WeighReviewSearchModel searchModel);
    }
}

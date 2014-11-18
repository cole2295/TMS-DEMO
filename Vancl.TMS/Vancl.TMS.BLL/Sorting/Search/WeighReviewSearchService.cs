using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Search;
using Vancl.TMS.Model.Sorting.WeighReview;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IDAL.Sorting.Search;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Sorting.Search
{
    public class WeighReviewSearchService : IWeighReviewSearchService
    {
        IWeighReviewSearchDAL WeighReviewSearchDAL = ServiceFactory.GetService<IWeighReviewSearchDAL>();
        public PagedList<WeighReviewViewModel> WeighReviewSearch(WeighReviewSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentException("参数WeighReviewSearchModel为空");
            return WeighReviewSearchDAL.WeighReviewSearch(searchModel);
        }
    }
}

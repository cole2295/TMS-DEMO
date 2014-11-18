using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.Return;

namespace Vancl.TMS.IBLL.Sorting.Search
{
    public interface IReturnSearchBLL
    {

        /// <summary>
        /// 退货查询
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        /// <returns>分页列表</returns>
        PagedList<ReturnBillInfoModel> Search(ReturnSearchModel searchModel);

    }
}

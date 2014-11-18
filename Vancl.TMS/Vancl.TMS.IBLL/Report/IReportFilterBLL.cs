using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Report;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Report
{
    /// <summary>
    /// 报表标题列筛选接口
    /// </summary>
    public interface IReportFilterBLL
    {
        /// <summary>
        /// 检索报表筛选对象
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        List<ReportFilterModel> Search(ReportFilterSearchModel searchModel);

        /// <summary>
        /// 新增报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(ReportFilterModel model);

        /// <summary>
        /// 修改报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Update(ReportFilterModel model);

        /// <summary>
        /// 删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Delete(long RFDI);

        /// <summary>
        /// 批量新增报表筛选对象
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        ResultModel BatchAdd(List<ReportFilterModel> listModel);

        /// <summary>
        /// 批量删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel BatchDelete(List<long> listRFDI);
    }
}

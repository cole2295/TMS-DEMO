using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Report;

namespace Vancl.TMS.IDAL.Report
{
    /// <summary>
    /// 报表标题列筛选数据层接口
    /// </summary>
    public interface IReportFilterDAL
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
        int Add(ReportFilterModel model);

        /// <summary>
        /// 修改报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(ReportFilterModel model);

        /// <summary>
        /// 删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete(long RFDI);

        /// <summary>
        /// 批量新增报表筛选对象
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        int BatchAdd(List<ReportFilterModel> listModel);

        /// <summary>
        /// 批量删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int BatchDelete(List<long> listRFDI);
    }
}

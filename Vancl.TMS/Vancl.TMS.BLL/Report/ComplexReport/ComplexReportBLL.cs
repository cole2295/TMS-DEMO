using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Report.ComplexReport;
using Vancl.TMS.IDAL.Report.ComplexReport;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Report.ComplexReport;
using Vancl.TMS.Model.Report;
using Vancl.TMS.IBLL.Report;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.BLL.Report.ComplexReport
{
    /// <summary>
    /// 综合报表
    /// </summary>
    public class ComplexReportBLL : BaseBLL, IComplexReportBLL
    {
        IComplexReportDAL _reportDAL = ServiceFactory.GetService<IComplexReportDAL>("complexReportDAL");
        IReportFilterBLL _filterBLL = ServiceFactory.GetService<IReportFilterBLL>("reportFilterBLL");

        #region IComplexReportBLL 成员

        /// <summary>
        /// 取得当前用户报表筛选列表
        /// </summary>
        /// <returns></returns>
        public List<ReportFilterModel> GetCurrentUserReportFilter()
        {
            return _filterBLL.Search(new ReportFilterSearchModel()
            {
                ReportType = Model.Common.Enums.ReportCategory.ComplexReport,
                IsShow = false,
                CreateBy = UserContext.CurrentUser.ID
            });
        }

        /// <summary>
        /// 综合报表查询
        /// </summary>
        /// <param name="searchModel">检索</param>
        /// <returns></returns>
        public ViewComplexReportPageModel Search(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("ComplexReportSearchModel is null");
            ViewComplexReportPageModel viewModel = new ViewComplexReportPageModel();
            viewModel.ReportData = _reportDAL.Search(searchModel);
            viewModel.Filter = _filterBLL.Search(new ReportFilterSearchModel()
            {
                ReportType = Model.Common.Enums.ReportCategory.ComplexReport,
                IsShow = false,
                CreateBy = UserContext.CurrentUser.ID
            });
            return viewModel; 
        }

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ViewComplexReportExportModel Export(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("ComplexReportSearchModel is null");
            ViewComplexReportExportModel exportModel = new ViewComplexReportExportModel();
            exportModel.ReportData = _reportDAL.Export(searchModel);
            exportModel.Filter = _filterBLL.Search(new ReportFilterSearchModel()
            {
                ReportType = Model.Common.Enums.ReportCategory.ComplexReport,
                IsShow = false,
                CreateBy = UserContext.CurrentUser.ID
            }); ;
            return exportModel;
        }

        #endregion
    }
}

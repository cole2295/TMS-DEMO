using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Report;
using Vancl.TMS.IDAL.Report;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Report;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Report
{
    /// <summary>
    /// 报表标题列筛选
    /// </summary>
    public class ReportFilterBLL : BaseBLL, IReportFilterBLL
    {
        IReportFilterDAL _filterDAL = ServiceFactory.GetService<IReportFilterDAL>("reportFilterDAL");


        #region IReportFilterBLL 成员
        /// <summary>
        /// 检索报表筛选对象
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        public List<ReportFilterModel> Search(ReportFilterSearchModel searchModel)
        {
            return _filterDAL.Search(searchModel);
        }

        /// <summary>
        /// 新增报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Add(ReportFilterModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("没有报表筛选对象");
            }
            int i = _filterDAL.Add(model);
            return AddResult(i, "报表筛选");
        }

        /// <summary>
        /// 修改报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Update(ReportFilterModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("没有报表筛选对象");
            }
            int i = _filterDAL.Update(model);
            return UpdateResult(i, "报表筛选");
        }

        /// <summary>
        /// 删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Delete(long RFDI)
        {
            int i = _filterDAL.Delete(RFDI);
            return DeleteResult(i, "报表筛选");
        }

        /// <summary>
        /// 批量新增报表筛选对象
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public ResultModel BatchAdd(List<ReportFilterModel> listModel)
        {
            int i = _filterDAL.BatchAdd(listModel);
            return AddResult(i, "报表筛选");
        }

        /// <summary>
        /// 批量删除报表筛选
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel BatchDelete(List<long> listRFDI)
        {
            int i = _filterDAL.BatchDelete(listRFDI);
            return DeleteResult(i, "报表筛选");
        }

        #endregion
    }
}

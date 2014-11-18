using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.CityScan;
using Vancl.TMS.Model.Sorting.CityScan;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Sorting.CityScan;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.Sorting.CityScan
{
    public class CityScanBLL : ICityScanBLL
    {
        ICityScanDAL cityScanDAL = ServiceFactory.GetService<ICityScanDAL>("CityScanDAL");
        IExpressCompanyDAL expressCompanyDAL = ServiceFactory.GetService<IExpressCompanyDAL>("ExpressCompanyDAL");
        IBillDAL billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");
        public ResultModel ScanCode(CityScanModel cityScanModel)
        {
            if (cityScanModel == null)
            {
                throw new ArgumentNullException("CityScan");
            }
            if (cityScanDAL.Exists(cityScanModel.FormCode))
            {
                return new ResultModel() { IsSuccess = false, Message = cityScanModel.FormCode+"此单已经扫描" };
            }
            BillModel billModel = billDAL.GetBillByFormCode(cityScanModel.FormCode);
            if (billModel == null)
            {
                return new ResultModel() { IsSuccess = false, Message = cityScanModel.FormCode + "未找到运单信息" };
            }
            ExpressCompanyModel expressCompanyModel = expressCompanyDAL.GetModel(billModel.DeliverStationID);
            if (expressCompanyModel==null)
            {
                return new ResultModel() { IsSuccess = false, Message = cityScanModel.FormCode + "没有获取到站点信息" };
            }
            string sortCenterIds=","+expressCompanyModel.ParentSortingCenterId+",";
            if (!sortCenterIds.Contains("," + cityScanModel.ScanSortCenter + ","))
            {
                return new ResultModel() { IsSuccess = false, Message = cityScanModel.FormCode + "不属于同城运单" };
            }
            cityScanModel.BCSID = (int)cityScanDAL.GetNextSequence(cityScanModel.SequenceName);
            int n = cityScanDAL.AddCityScan(cityScanModel);
            if (n == 1)
            {
                return new ResultModel() { IsSuccess = true, Message = "" };
            }
            else
            {
                return new ResultModel() { IsSuccess = false, Message = "操作失败" };
            }
        }


        public PagedList<CityScanModel> SearchCityScanStatistics(CityScanSearchModel searchModel)
        {
            return cityScanDAL.SearchCityScanStatistics(searchModel);
        }


        public IList<CityScanExprotModel> SearchExportScan(List<string> batchnoList)
        {
            return cityScanDAL.SearchExportScan(batchnoList);
        }


        public IList<CityScanBatchDetail> SearchCityScanPrint(string batchno)
        {
            return cityScanDAL.SearchCityScanPrint(batchno);
        }
    }
}

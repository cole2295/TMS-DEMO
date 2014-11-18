using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Report;
using Vancl.TMS.Model.Report.CarrierAccountReport;
using Vancl.TMS.IDAL.Report;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Report
{
    public class CarrierAccountReportBLL : BaseBLL, ICarrierAccountReportBLL
    {
        ICarrierAccountReportDAL dal = ServiceFactory.GetService<ICarrierAccountReportDAL>("CarrierAccountReportDAL");
        IDeliveryAssessmentBLL kpiBLL = ServiceFactory.GetService<IDeliveryAssessmentBLL>("DeliveryAssessmentBLL");

        #region ICarrierAccountReportBLL 成员

        public ViewCarrierAccountReportPageModel Search(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            ViewCarrierAccountReportPageModel result = new ViewCarrierAccountReportPageModel();
            result.ReportData = dal.Search(searchModel);
            result.StatisticsData = dal.GetStatisticsInfo(searchModel);
            return result;
        }

        public ViewCarrierAccountReportExportModel Export(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            ViewCarrierAccountReportExportModel result = new ViewCarrierAccountReportExportModel();
            result.ReportData = dal.Export(searchModel);
            return result;
        }

        #endregion

        #region ICarrierAccountReportBLL 成员

        public ViewCarrierAccountReportPageModel SearchDynamicReport(CarrierAccountReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("CarrierAccountReportSearchModel is null");
            ViewCarrierAccountReportPageModel result = new ViewCarrierAccountReportPageModel();
            result.DynamicReportData = dal.SearchDynamicReport(searchModel);
            result.StatisticsData = dal.GetDynamicStatisticsInfo(searchModel);
            CreateDynamicReportData(result.DynamicReportData, result.StatisticsData);
            return result;
        }

        private void CreateDynamicReportData(List<ViewCarrierAccountReport> dynamicReportData, ViewCarrierAccountReportStatisticsModel statisticsData)
        {
            if (dynamicReportData == null || statisticsData == null)
                return;
            foreach (var item in dynamicReportData)
            {
                var kpiAssPrice = kpiBLL.KPICalculateByDeliveryDefaultSetting(item.DeliveryNo);
                if (kpiAssPrice != null)
                {
                    item.BaseAmount = kpiAssPrice.BaseAmount;
                    item.InsuranceAmount = kpiAssPrice.InsuranceAmount;
                    item.ComplementAmount = kpiAssPrice.ComplementAmount;
                    item.LongDeliveryAmount = kpiAssPrice.LongDeliveryAmount;
                    item.LongTransferAmount = kpiAssPrice.LongTransferAmount;
                    item.LongPickPrice = kpiAssPrice.LongPickPrice;
                    item.NeedAmount = kpiAssPrice.NeedAmount;
                    if (kpiAssPrice.KPIDelayType == Enums.KPIDelayType.DelayDiscount)
                    {
                        item.KPIAmount = Math.Round(kpiAssPrice.NeedAmount * (kpiAssPrice.Discount ?? 1), 2) - kpiAssPrice.NeedAmount;
                    }
                    else
                    {
                        item.KPIAmount = kpiAssPrice.DelayAmount ?? 0;
                    }
                    item.LostDeduction = kpiAssPrice.LostDeduction;
                    item.OtherAmount = kpiAssPrice.OtherAmount;
                    item.ConfirmedAmount = kpiAssPrice.ConfirmedAmount;
                }
            }

            statisticsData.InsuranceAmount = dynamicReportData.Sum(p => p.InsuranceAmount);
            statisticsData.ComplementAmount = dynamicReportData.Sum(p => p.ComplementAmount);
            statisticsData.LongDeliveryAmount = dynamicReportData.Sum(p => p.LongDeliveryAmount);
            statisticsData.LongTransferAmount = dynamicReportData.Sum(p => p.LongTransferAmount);
            statisticsData.LongPickPrice = dynamicReportData.Sum(p => p.LongPickPrice);
            statisticsData.NeedAmount = dynamicReportData.Sum(p => p.NeedAmount);
            statisticsData.LostDeduction = dynamicReportData.Sum(p => p.LostDeduction);
            statisticsData.OtherAmount = dynamicReportData.Sum(p => p.OtherAmount);
            statisticsData.ConfirmedAmount = dynamicReportData.Sum(p => p.ConfirmedAmount);
            statisticsData.BaseAmount = dynamicReportData.Sum(p => p.BaseAmount);
            statisticsData.KPIAmount = dynamicReportData.Sum(p => p.KPIAmount ?? 0);
        }

        #endregion
    }
}

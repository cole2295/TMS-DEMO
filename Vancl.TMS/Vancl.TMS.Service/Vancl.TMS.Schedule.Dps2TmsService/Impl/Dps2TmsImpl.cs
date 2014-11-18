using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Schedule.Dps2TmsService.DpsOrdersForTMS;
using Vancl.TMS.Util;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.Schedule.Dps2TmsService.Impl
{
    public class Dps2TmsImpl : IStatefulJob
    {
        IDps2Tms _bll = ServiceFactory.GetService<IDps2Tms>("Dps2Tms");

        public void DoJob()
        {
            IOrdersForTms dpsforTms = new OrdersForTmsClient();
            var dpss = dpsforTms.GetDpsDataForTMS(10);
            if (null == dpss)
            {
                return;
            }

            List<long> successNos = new List<long>();
            foreach (var lmsOrder in dpss)
            {
                var billModel = new BillModel();
                var billInfoModel = new BillInfoModel();
                BuildBillDataModel(lmsOrder, out billModel, out billInfoModel);
                if (_bll.ImportBaseDataToTms(billModel, billInfoModel))
                {
                    successNos.Add(lmsOrder.OrderForThirdPartyId);
                }
            }

            dpsforTms.UpdateGetStatus(successNos.ToArray());
        }

        private void BuildBillDataModel(OrderForThirdParty model, out BillModel billModel, out BillInfoModel billInfoModel)
        {
            billModel = new BillModel();
            billModel.BillType = (Enums.BillType)int.Parse(model.ReplaceType);
            billModel.CreateBy = model.CreateBy.HasValue ? model.CreateBy.Value : 0;
            billModel.CreateDept = model.CreateStationId.HasValue ? model.CreateStationId.Value : 0;
            billModel.CurrentDistributionCode = string.IsNullOrEmpty(model.LogisticProviderID) ?model.DistributionCode : model.LogisticProviderID;
            billModel.CustomerOrder = model.FormCode;
            //为空字符串或者null，默认空格符
            billModel.DeliverCode = String.IsNullOrWhiteSpace(model.DeliverCode) ? " " : model.DeliverCode;
            billModel.DeliverStationID = model.ExpressCompanyID.HasValue ? model.ExpressCompanyID.Value : -1;
            billModel.DistributionCode = model.DistributionCode;
            billModel.FormCode = model.WaybillNo.ToString();
            billModel.MerchantID = model.Marchartid.HasValue ? model.Marchartid.Value : 0;
            //billModel.ReturnStatus = model.;
            billModel.Source = (Enums.BillSource)(model.Source.HasValue ? model.Source : 2);
            billModel.Status = (Enums.BillStatus)model.Status;
            billModel.UpdateBy = (model.UpdateBy == null) ? 0 : model.UpdateBy.Value;
            billModel.UpdateDept = (model.UpdateStationId == null) ? 0 : model.UpdateStationId.Value;
            billModel.UpdateTime = DateTime.Now;
            //billModel.WarehouseID = model.;
            billInfoModel = new BillInfoModel();
            billInfoModel.BillGoodsType = string.IsNullOrWhiteSpace(model.GoodsProperty) ? Enums.BillGoodsType.Normal : EnumHelper.GetValue<Enums.BillGoodsType>(model.GoodsProperty);
            billInfoModel.CustomerBoxNo = model.BoxNo;
            billInfoModel.CustomerWeight = model.Weight.HasValue ? model.Weight.Value : 0;
            billInfoModel.FormCode = model.WaybillNo.ToString();
            billInfoModel.InsuredAmount = model.Protectedprice.HasValue ? model.Protectedprice.Value : 0;
            billInfoModel.PackageCount = model.BoxCount.HasValue ? model.BoxCount.Value : 0;
            billInfoModel.PackageMode = model.WaybillBoxModel;
            billInfoModel.PayType = model.CashonDeliveryTypeName == "POS机" ? Enums.PayType.POS : Enums.PayType.Cash;
            billInfoModel.ReceivableAmount = model.NeedFund.HasValue ? model.NeedFund.Value : 0;
            //billInfoModel.Tips = model.oORDERCOMMENT;
            billInfoModel.TotalAmount = model.Price.HasValue ? model.Price.Value : 0;
            billInfoModel.Weight = model.Weight.HasValue ? model.Weight.Value : 0;

        }




        public void Execute(JobExecutionContext context)
        {
            try
            {
                DoJob();
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex, true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Log
{
    public class BatchBLL:IBatchBLL
    {
        IBatchDAL dal = ServiceFactory.GetService<IBatchDAL>();
        IDeliveryDataEntranceBLL DeliveryDataEntranceBLL = ServiceFactory.GetService<IDeliveryDataEntranceBLL>("DeliveryDataEntranceBLL");

        public List<BatchModel> GetBatch(BatchSearchModel searchModel)
        {
            List<BatchModel> batchModelList= dal.GetBatch(searchModel);

            BillServiceClient CloudBillService = new BillServiceClient();
            WaybillBatch waybillBatch = CloudBillService.GetWaybillByBatchNoOrFormCode(searchModel.BatchNo,null);

            if (waybillBatch==null || waybillBatch.Detail.Count() <= 0)
                return batchModelList;
            if(waybillBatch.Detail.Count()==batchModelList.Count)
                return batchModelList;

            foreach (WaybillBatchDetails detail in waybillBatch.Detail)
            {
                if (!JudgeExists(detail.FormCode, batchModelList))
                {
                    BatchModel bm = new BatchModel();
                    bm.FormCode = detail.FormCode;
                    bm.CustomerBatchNo = waybillBatch.BatchNo;
                    bm.IsConsistency = false;
                    bm.ArrivalId = waybillBatch.Arrival;
                    bm.DepartureId = waybillBatch.Departure;
                    bm.ArrivalName = waybillBatch.ArrivalName;
                    bm.DepartureName = waybillBatch.DepartureName;
                    batchModelList.Add(bm);
                }
            }
            return batchModelList.OrderBy(row=>row.SerialNumber).ToList();
        }

        private bool JudgeExists(string formCode, List<BatchModel> batchModelList)
        {
            var list = (from t in batchModelList where t.FormCode == formCode select t).ToList();
            return list.Count > 0;
        }

        public ResultModel RepairBatchDetail(String formCode)
        {
            BillServiceClient CloudBillService = new BillServiceClient();
            WaybillBatch waybillBatch = CloudBillService.GetWaybillByBatchNoOrFormCode(null, formCode);
            if (waybillBatch == null || waybillBatch.Detail.Count() !=1)
                return new ResultModel() { IsSuccess = false, Message = "单号" + formCode + "查询查询无记录或记录大于1" };

            TMSEntranceModel tmsEntranceModel = new TMSEntranceModel();
            tmsEntranceModel.Source = (Vancl.TMS.Model.Common.Enums.TMSEntranceSource)waybillBatch.Source;
            tmsEntranceModel.BatchNo = waybillBatch.BatchNo;
            tmsEntranceModel.Departure = waybillBatch.Departure;
            tmsEntranceModel.Arrival = waybillBatch.Arrival;
            tmsEntranceModel.TotalCount = int.Parse(waybillBatch.TotalAmount.ToString());
            tmsEntranceModel.Weight = waybillBatch.Weight;
            tmsEntranceModel.ContentType = Vancl.TMS.Model.Common.Enums.GoodsType.Normal;
            tmsEntranceModel.TotalAmount = 0;
            tmsEntranceModel.Detail = new List<BillDetailModel>()
                             {
                                new BillDetailModel(){
                                    FormCode = waybillBatch.Detail[0].FormCode,
                                    BillType = (Vancl.TMS.Model.Common.Enums.BillType)int.Parse(waybillBatch.Detail[0].BillType),
                                    Price = waybillBatch.Detail[0].Price,
                                    GoodsType = (Vancl.TMS.Model.Common.Enums.GoodsType)int.Parse(waybillBatch.Detail[0].GoodsType),
                                    CustomerOrder = waybillBatch.Detail[0].CustomerOrder,
                                },
                             };
            return DeliveryDataEntranceBLL.DataEntrance(tmsEntranceModel);
        }

        public Int32 RepairTest(String txt)
        {
            return dal.RepairTest(txt);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.DeliveryImport;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Delivery.DataInteraction.Entrance
{
    /// <summary>
    /// TMS数据交互数据入口实现
    /// </summary>
    public class DeliveryDataEntranceBLL : BaseBLL, IDeliveryDataEntranceBLL
    {
        //IN参数化默认传人数组最大长度
        private static readonly int PerInArgumentCount = 200;
        IBoxDAL _boxDAL = ServiceFactory.GetService<IBoxDAL>("BoxDAL");
        IOrderDAL _orderDAL = ServiceFactory.GetService<IOrderDAL>("OrderDAL");
        IServiceLogDAL _logService = ServiceFactory.GetService<IServiceLogDAL>();
        IExpressCompanyDAL _expressCompanyDAL = ServiceFactory.GetService<IExpressCompanyDAL>("ExpressCompanyDAL");

        #region IDeliveryDataEntranceBLL 成员

        /// <summary>
        /// 验证数据，初始化
        /// </summary>
        /// <param name="entranceModel"></param>
        /// <returns></returns>
        private ResultModel Validate(TMSEntranceModel entranceModel)
        {
            ResultModel result = new ResultModel();
            if (entranceModel == null)
            {
                return result.Failed("TMSEntranceModel is null");
            }
            if (String.IsNullOrWhiteSpace(entranceModel.BatchNo))
            {
                return result.Failed("TMSEntranceModel.BatchNo is null or empty");
            }
            if (String.IsNullOrWhiteSpace(entranceModel.Departure))
            {
                return result.Failed("TMSEntranceModel.Departure is null or empty");
            }
            if (String.IsNullOrWhiteSpace(entranceModel.Arrival))
            {
                return result.Failed("TMSEntranceModel.Arrival is null or empty");
            }
            if (entranceModel.Detail == null || entranceModel.Detail.Count < 1)
            {
                return result.Failed("TMSEntranceModel.Detail is null or count = 0");
            }
            String strHeader = String.Format("批次号：{0}，来源：{1} ", entranceModel.BatchNo, EnumHelper.GetDescription(entranceModel.Source));
            var groupResult = entranceModel.Detail.GroupBy(p => p.FormCode);
            if (groupResult.Count() != entranceModel.Detail.Count)
            {
                return result.Failed(String.Format("TMSEntranceModel.Detail包含重复的运单号，{0}", strHeader));
            }
            //Vancl, V+ 的出发地目的地需要进行转换
            if (entranceModel.Source == Enums.TMSEntranceSource.VANCL
                || entranceModel.Source == Enums.TMSEntranceSource.VJIA)
            {
                int? Departure = _expressCompanyDAL.GetDepartureByWMSCode(entranceModel.Departure);
                if (!Departure.HasValue)
                {
                    return result.Failed(String.Format("TMSEntranceModel.Departure:{0} 有误，无法找到对应的出发地，{1}", entranceModel.Departure, strHeader));
                }
                entranceModel.Departure = Departure.Value.ToString();
                int? Arrival = null;
                if (entranceModel.Source == Enums.TMSEntranceSource.VANCL)
                {
                    Arrival = _expressCompanyDAL.GetArrivalByVanclSCMArrival(entranceModel.Arrival);
                }
                else
                {
                    Arrival = _expressCompanyDAL.GetArrivalByVJIASCMArrival(entranceModel.Arrival);
                }
                if (!Arrival.HasValue)
                {
                    return result.Failed(String.Format("TMSEntranceModel.Arrival:{0} 有误，无法找到对应的目的地，{1}", entranceModel.Arrival, strHeader));
                }
                entranceModel.Arrival = Arrival.Value.ToString();
            }
            else
            {
                int DepartureID, ArrivalID;
                if (!int.TryParse(entranceModel.Departure, out DepartureID))
                {
                    return result.Failed(String.Format("TMSEntranceModel.Departure:{0} 有误，数据类型不正确，{1}", entranceModel.Departure, strHeader));
                }
                if (!int.TryParse(entranceModel.Arrival, out ArrivalID))
                {
                    return result.Failed(String.Format("TMSEntranceModel.Arrival:{0} 有误，数据类型不正确，{1}", entranceModel.Arrival, strHeader));
                }
            }
            return result.Succeed("验证通过");
        }

        /// <summary>
        /// 记录服务日志
        /// </summary>
        /// <param name="entranceModel"></param>
        /// <param name="strMsg"></param>
        /// <param name="IsSucceed"></param>
        private void RecordLog(TMSEntranceModel entranceModel, String strMsg, bool IsSucceed = false)
        {
            if (entranceModel != null)
            {
                if (entranceModel.Detail != null && entranceModel.Detail.Count > 0)
                {
                    var listLog = new List<ServiceLogModel>();
                    foreach (var item in entranceModel.Detail)
                    {
                        listLog.Add(new ServiceLogModel()
                        {
                            CreateBy = 0,
                            UpdateBy = 0,
                            FormCode = String.IsNullOrWhiteSpace(item.FormCode) ? "***" : item.FormCode,
                            IsSuccessed = IsSucceed,
                            LogType = Enums.ServiceLogType.Lms2TmsSync,
                            Note = strMsg,
                            OpFunction = (int)Enums.Lms2TmsOperateType.TmsDataEntrance,
                            SyncKey = "***"
                        });
                    }
                    _logService.BatchWriteLog(listLog);
                }
            }
        }

        /// <summary>
        /// LMS写入接口
        /// </summary>
        /// <param name="entranceModel"></param>
        /// <returns></returns>
        public ResultModel DataEntrance(TMSEntranceModel entranceModel)
        {
            var result = new ResultModel();
            try
            {
                var validateResult = Validate(entranceModel);
                if (!validateResult.IsSuccess)
                {
                    RecordLog(entranceModel, validateResult.Message, false);
                    return result.Failed(validateResult.Message);
                }
                int ArrivalID = int.Parse(entranceModel.Arrival);
                String strHeader = String.Format("批次号：{0}，来源：{1} ", entranceModel.BatchNo, EnumHelper.GetDescription(entranceModel.Source));
                using (TransPointService.TmsDataServiceClient Pms4TmsProxy = new TransPointService.TmsDataServiceClient())
                {
                    //通过始发地--目的地，查询pms中是配置TMS城际运输
                    if (Pms4TmsProxy.IsNeedTmsTransfer(int.Parse(entranceModel.Departure), ref ArrivalID) <= 0)
                    {
                        String strFailedMsg = String.Format("出发地：{0}，目的地：{1} 不走TMS城际运输，{2}", entranceModel.Departure, entranceModel.Arrival, strHeader);
                        RecordLog(entranceModel, strFailedMsg, false);
                        return result.Failed(strFailedMsg);
                    }
                }
                entranceModel.Arrival = ArrivalID.ToString();
                var boxModel = new BoxModel()
                {
                    ArrivalID = int.Parse(entranceModel.Arrival),
                    BoxNo = String.Format("{0}ST{1}", (int)entranceModel.Source, entranceModel.BatchNo.Trim()),
                    CustomerBatchNo = entranceModel.BatchNo.Trim(),
                    Source = entranceModel.Source,
                    BoxType = entranceModel.Source == Enums.TMSEntranceSource.SortingPacking ? Enums.BoxType.Normal : Enums.BoxType.Other,
                    ContentType = entranceModel.ContentType,
                    DepartureID = int.Parse(entranceModel.Departure),
                    IsPreDispatch = Enums.BatchPreDispatchedStatus.NoDispatched,
                    IsDeleted = false,
                    TotalAmount = entranceModel.TotalAmount,
                    TotalCount = entranceModel.TotalCount,
                    Weight = entranceModel.Weight
                };
                var listFormCode = new List<String>(entranceModel.Detail.Count);
                var order = new List<OrderModel>(entranceModel.Detail.Count);
                var boxDetail = new List<BoxDetailModel>(entranceModel.Detail.Count);
                entranceModel.Detail.ForEach(p =>
                {
                    listFormCode.Add(p.FormCode);
                    order.Add(new OrderModel()
                    {
                        ArrivalID = int.Parse(entranceModel.Arrival),
                        BoxNo = boxModel.BoxNo,
                        CustomerOrder = p.CustomerOrder,
                        DepartureID = int.Parse(entranceModel.Departure),
                        FormCode = p.FormCode,
                        GoodsType = p.GoodsType,
                        IsDeleted = false,
                        LMSwaybillType = (int)p.BillType,
                        LMSwaybillNo = 0,
                        OrderTMSStatus = Enums.OrderTMSStatus.Normal,
                        Price = p.Price
                    });
                    boxDetail.Add(new BoxDetailModel() { BoxNo = boxModel.BoxNo, FormCode = p.FormCode });
                });
                List<String> existsFormCode = new List<string>();
                int NRemainder = listFormCode.Count % PerInArgumentCount;      //余数
                int NCount = listFormCode.Count / PerInArgumentCount + (NRemainder > 0 ? 1 : 0);   //循环次数
                List<String> tmplistFormCode = null;
                //分批查询已经存在的运单
                for (int i = 0; i < NCount; i++)
                {
                    if (i != NCount - 1)
                    {
                        tmplistFormCode = listFormCode.GetRange(i * PerInArgumentCount, PerInArgumentCount);
                    }
                    else
                    {
                        tmplistFormCode = listFormCode.GetRange(i * PerInArgumentCount, NRemainder == 0 ? PerInArgumentCount : NRemainder);
                    }
                    if (tmplistFormCode != null && tmplistFormCode.Count > 0)
                    {
                        var tmperExists = _orderDAL.GetExistsFormCode(tmplistFormCode);
                        if (tmperExists != null && tmperExists.Count > 0)
                        {
                            existsFormCode.AddRange(tmperExists);
                        }
                    }
                }
                if (existsFormCode != null && existsFormCode.Count > 0)
                {
                    OrderModel tmpModel = null;
                    foreach (var item in existsFormCode)
                    {
                        tmpModel = order.Find(p => p.FormCode.Equals(item));
                        if (tmpModel != null)
                        {
                            order.Remove(tmpModel);
                        }
                    }
                }
                String strSucceedMsg = String.Format("对接成功，{0}", strHeader);
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    if (!_boxDAL.IsBoxNoExists(boxModel.BoxNo))
                    {
                        _boxDAL.Add(boxModel);
                    }
                    _boxDAL.AddBoxDetail(boxDetail);
                    if (order.Count > 0)
                    {
                        _orderDAL.Add(order);
                    }
                    RecordLog(entranceModel, strSucceedMsg, true);
                    scope.Complete();
                }
                return result.Succeed(strSucceedMsg);
            }
            catch (Exception ex)
            {
                RecordLog(entranceModel, ex.Message, false);
                return result.Failed(ex.Message);
            }
        }

        #endregion
    }
}

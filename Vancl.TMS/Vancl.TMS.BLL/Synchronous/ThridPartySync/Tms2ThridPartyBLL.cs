using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.OutServiceProxy;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.BLL.Synchronous.ThridPartySync
{
    public class Tms2ThridPartyBLL : BaseBLL, ITms2ThridPartyBLL
    {
        IDispatchDAL _dispatchDAL = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        IEmployeeDAL _empDAL = ServiceFactory.GetService<IEmployeeDAL>();
        IProxy4Cloud _proxy4Cloud = ServiceFactory.GetService<IProxy4Cloud>("Proxy4Cloud");
        IServiceLogBLL _serviceLogBLL = ServiceFactory.GetService<IServiceLogBLL>("ServiceLogBLL");

        #region ITms2ThridPartyBLL 成员

        private void WriteLog(String formcode, bool IsSuccessed, String Key, int OpFunction, String Note)
        {
            _serviceLogBLL.WriteLog(new Model.Log.ServiceLogModel()
            {
                CreateBy = 0,
                FormCode = formcode,
                IsDeleted = false,
                IsHandled = false,
                IsSuccessed = IsSuccessed,
                LogType = Enums.ServiceLogType.Tms2LmsSync,
                Note = String.Format(@"同步时效监控日志 {0} , {1}", IsSuccessed ? "成功" : "失败", Note),
                OpFunction = OpFunction,   //item.FlowType == Model.Common.Enums.DeliverFlowType.Outbound ? (int)Enums.TmsOperateType.IntercityInTransit : (int)Enums.TmsOperateType.IntercityArrivaled,
                SyncKey = String.IsNullOrWhiteSpace(Key) ? "*" : Key,
                UpdateBy = 0
            });
        }


        public void TMS2LMS4AgingMonitoring(Model.Synchronous.OutSync.Tms2ThridPartyJobArgs args)
        {
            if (args == null) throw new ArgumentNullException("args is null");
            var listresult = _dispatchDAL.GetDeliveryInfo4TMS2ThridParty(args);
            if (listresult == null || listresult.Count <= 0)
            {
                return;
            }
            try
            {
                foreach (var item in listresult)
                {
                    var OpUser = _empDAL.GetAgingMonitoringLogEmployee(item.OperateBy);
                    OpUser.OperateTime = item.OperateTime;
                    var log = _dispatchDAL.GetTMSAgingMonitoringLogList(new DispatchModel() { DeliveryNo = item.DeliveryNO } , item.FlowType);
                    if (log == null || log.Count <= 0)
                    {
                        _dispatchDAL.UpdateDeliveryFlowSyncFlag4TMS2ThridParty(item.DFLID, Enums.SyncStatus.Already);
                        continue;
                    }
                    foreach (var itemlog in log)
                    {
                        using (CloudBillService.BillServiceClient proxy = new CloudBillService.BillServiceClient())
                        {
                            var model = proxy.GetWaybillModel(long.Parse(itemlog.FormCode));
                            if (model == null)
                            {
                                WriteLog(
                                    itemlog.FormCode
                                    , false
                                    , ""
                                    , item.FlowType == Model.Common.Enums.DeliverFlowType.Outbound ? (int)Enums.TmsOperateType.IntercityInTransit : (int)Enums.TmsOperateType.IntercityArrivaled
                                    , "木有从LMS物流主库取得运单主信息");
                                continue;
                            }
                            itemlog.CurrentDistributionCode = OpUser.CurrentDistributionCode;    //String.IsNullOrWhiteSpace(model.CurrentDistributionCode) ? model.DistributionCode : model.CurrentDistributionCode;
                            itemlog.DistributionCode = model.DistributionCode;
                            itemlog.MerchantID = model.MerchantID;
                            itemlog.OperateArea = OpUser.OperateArea;
                            itemlog.Operator = OpUser.Operator;
                            itemlog.OperateCity = OpUser.OperateCity;
                            itemlog.OperateDept = OpUser.OperateDept;
                            itemlog.OperateProvince = OpUser.OperateProvince;
                            itemlog.Status = item.FlowType == Model.Common.Enums.DeliverFlowType.Outbound ? Enums.BillStatus.IntercityInTransit : Enums.BillStatus.IntercityArrivaled;
                            itemlog.OperateTime = OpUser.OperateTime;
                        }
                        var result = _proxy4Cloud.AddAgingMonitoringLog(itemlog);
                        WriteLog(
                                    itemlog.FormCode
                                    , result == null ? false : result.IsSuccess
                                    , itemlog.DODID.ToString()
                                    , item.FlowType == Model.Common.Enums.DeliverFlowType.Outbound ? (int)Enums.TmsOperateType.IntercityInTransit : (int)Enums.TmsOperateType.IntercityArrivaled
                                    , result.Message);
                    }
                    _dispatchDAL.UpdateDeliveryFlowSyncFlag4TMS2ThridParty(item.DFLID, Enums.SyncStatus.Already);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

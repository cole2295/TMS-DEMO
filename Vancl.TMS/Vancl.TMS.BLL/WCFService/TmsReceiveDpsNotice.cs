using System;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.WCFService;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util;

namespace Vancl.TMS.BLL.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“TmsReceiveDpsNotice”。
    public class TmsReceiveDpsNotice : ITmsReceiveDpsNotice
    {
        IBillChangeLogDAL _billChangeLogDAL = ServiceFactory.GetService<IBillChangeLogDAL>("BillChangeLogDAL");


        public void DoNotify(Model.Dps.LifeCycleModel model)
        {
            try
            {
                MessageCollector.Instance.Collect(GetType(), model.WaybillNo.ToString());
                BillChangeLogModel changeLog = new BillChangeLogModel();

                changeLog.FormCode = model.WaybillNo.ToString();
                //changeLog.CurNode = model.CurNode;
                changeLog.CurrentStatus = (Enums.BillStatus)int.Parse(model.Status);
                changeLog.ReturnStatus = (Enums.ReturnStatus)model.SubStatus;
                //changeLog.CustomerOrder = model.FormCode;
                //changeLog.MerchantID = model.MerchantID;
                changeLog.CurrentDistributionCode = string.IsNullOrEmpty(model.DistributionCode)
                                                        ? "空"
                                                        : model.DistributionCode;
                changeLog.DeliverStationID = model.DeliverStationID.HasValue ? model.DeliverStationID.Value : 0;
                changeLog.SyncTime = model.CreateTime;
                changeLog.CreateBy = model.CreateBy;
                changeLog.CreateDept = model.CreateStation;
                changeLog.SyncFlag = Enums.SyncStatus.HasReceiveNotice;
                changeLog.Note = string.IsNullOrEmpty(model.Note) ? "" : model.Note;
                changeLog.ToDistributionCode = model.ToDistributionCode;
                if (model.ToExpressCompanyID != null) changeLog.ToExpressCompanyID = model.ToExpressCompanyID.Value;

                _billChangeLogDAL.Add(changeLog);
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex, true);
            }
        }

        public void DoNotifys(System.Collections.Generic.List<Model.Dps.LifeCycleModel> models)
        {
            foreach (var m in models)
            {
                DoNotify(m);
            }
        }
    }
}
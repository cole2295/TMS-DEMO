using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Delivery.InTransit;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IDAL.Transport.Dispatch;

namespace Vancl.TMS.BLL.Delivery.InTransit
{
    public class CarrierWaybillBLL : BaseBLL, ICarrierWaybillBLL
    {
        ICarrierWaybillDAL _dal = ServiceFactory.GetService<ICarrierWaybillDAL>("CarrierWaybillDAL");
        IDispatchDAL _dispatchDal = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        #region ICarrierWaybillBLL 成员

        public ResultModel Add(CarrierWaybillModel model)
        {
            if (model == null)
            {
                throw new CodeNotValidException();
            }
            ResultModel rm = new ResultModel();
            if (model.CWID == 0)
            {
                _dal.GetNextSequence(model.SequenceName);
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.Add(model);
                WriteInsertLog<CarrierWaybillModel>(model);
                scope.Complete();
            }
            if (i <= 0)
            {
                rm.IsSuccess = false;
                rm.Message = "新增承运商运单信息失败！";
            }
            else
            {
                rm.IsSuccess = true;
                rm.Message = "新增承运商运单信息成功！";
            }
            return rm;
        }

        public ResultModel Delete(List<long> lstCwid)
        {
            if (lstCwid == null || lstCwid.Count == 0)
            {
                throw new CodeNotValidException();
            }
            ResultModel rm = new ResultModel();
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                WriteBatchDeleteLog<CarrierWaybillModel>(lstCwid);
                i = _dal.Delete(lstCwid);
                scope.Complete();
            }
            rm.IsSuccess = true;
            rm.Message = string.Format("成功删除{0}条承运商运单信息！", i);
            return rm;
        }

        public ResultModel Update(CarrierWaybillModel model)
        {
            if (model == null)
            {
                throw new CodeNotValidException();
            }
            ResultModel rm = new ResultModel();
            int i = 0;
            CarrierWaybillModel pastModel = Get(model.CWID);
            if (pastModel == null)
            {
                rm.IsSuccess = false;
                rm.Message = "该承运商运单信息存在或者已经被删除";
                return rm;
            }
            DispatchModel dm = new DispatchModel();
            dm.DID = model.DID;
            dm.UpdateBy = model.UpdateBy;
            dm.BoxCount = model.Boxcount;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.Update(model);
                if (_dispatchDal.UpdateDispatchBoxCount(dm)<=0) { return new ResultModel() { IsSuccess = false,Message = "更新承运商运单信息--箱数失败！"}; }
                WriteUpdateLog<CarrierWaybillModel>(model, pastModel);
                scope.Complete();
            }
            if (i <= 0)
            {
                rm.IsSuccess = false;
                rm.Message = "更新承运商运单信息失败！";
            }
            else
            {
                rm.IsSuccess = true;
                rm.Message = "更新承运商运单信息成功！";
            }
            return rm;
        }

        public CarrierWaybillModel Get(long cwid)
        {
            return _dal.Get(cwid);
        }

        public CarrierWaybillModel GetByDispatchID(long dispatchID)
        {
            return _dal.GetByDispatchID(dispatchID);
        }

        #endregion
    }
}

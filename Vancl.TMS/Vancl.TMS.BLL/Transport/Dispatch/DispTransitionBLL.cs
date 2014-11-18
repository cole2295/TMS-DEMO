using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Transport.Dispatch
{
    public class DispTransitionBLL : BaseBLL, IDispTransitionBLL
    {
        IDispTransitionDAL _dal = ServiceFactory.GetService<IDispTransitionDAL>("DispTransitionDAL");

        #region IDispTransitionBLL 成员

        public Model.Common.ResultModel Add(Model.Transport.Dispatch.DispTransitionModel model)
        {
            return AddResult(_dal.Add(model), "调度交接信息");
        }

        public Model.Common.ResultModel Delete(string deliveryNo)
        {
            return DeleteResult(_dal.Delete(deliveryNo), "调度交接信息");
        }

        public Model.Common.ResultModel Update(Model.Transport.Dispatch.DispTransitionModel model)
        {
            var transition = Get(model.DeliveryNo);
            int i = 0;
            if (transition == null)
            {
                i = _dal.Add(model);
            }
            else
            {
                model.DTID = transition.DTID;
                i = _dal.Update(model);
            }

            if (i > 0)
            {
                return InfoResult("修改调度交接信息成功");
            }
            else
            {
                return ErrorResult("修改调度交接信息失败");
            }
        }

        public Model.Transport.Dispatch.DispTransitionModel Get(string deliveryNo)
        {
            return _dal.Get(deliveryNo);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class LineFormulaPriceBLL : BaseBLL, ILineFormulaPriceBLL
    {
        ILineFormulaPriceDAL service = ServiceFactory.GetService<ILineFormulaPriceDAL>("LineFormulaPriceDAL");

        #region ILineFormulaPriceBLL 成员

        public int Add(LineFormulaPriceModel model)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = service.Add(model);
                service.AddDetail(model.Detail);
                scope.Complete();
            }
            return i;
        }

        public LineFormulaPriceModel GetByLinePlanID(int lpid)
        {
            LineFormulaPriceModel model = service.GetByLinePlanID(lpid);
            if (model != null)
                model.Detail = service.GetLineFormulaDetails(model.LPID);

            return model;
        }

        public int Update(LineFormulaPriceModel model)
        {
            LineFormulaPriceModel pastModel = service.GetByLinePlanID(model.LPID);
            int i = 0;
            if (pastModel == null)
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    WriteInsertLog<LineFormulaPriceModel>(model);
                    i = Add(model);
                    scope.Complete();
                }
            }
            else
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    service.DeleteDetails(model.LPID);
                    foreach (var item in model.Detail)
                    {
                        item.LPID = model.LPID;
                    }
                    service.AddDetail(model.Detail);
                    WriteForcedUpdateLog<LineFormulaPriceDetailModel>(model.LPID.ToString());
                    i = service.Update(model);
                    WriteUpdateLog<LineFormulaPriceModel>(model, pastModel);
                    scope.Complete();
                }
            }
            return i;
        }

        public int Delete(List<int> lpidList)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                service.DeleteDetails(lpidList[0]);
                i = service.Delete(lpidList);
                scope.Complete();
            }

            return i;
        }

        #endregion
    }
}

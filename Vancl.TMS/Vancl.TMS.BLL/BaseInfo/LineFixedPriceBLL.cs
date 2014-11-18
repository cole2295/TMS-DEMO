using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class LineFixedPriceBLL : BaseBLL, ILineFixedPriceBLL
    {
        ILineFixedPriceDAL service = ServiceFactory.GetService<ILineFixedPriceDAL>("LineFixedPriceDAL");

        #region ILineFixedPriceBLL 成员

        public int Add(LineFixedPriceModel model)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                WriteInsertLog<LineFixedPriceModel>(model);
                i = service.Add(model);
                scope.Complete();
            }
            return i;
        }

        public LineFixedPriceModel GetByLinePlanID(int lpid)
        {
            return service.GetByLinePlanID(lpid);
        }

        public int Update(LineFixedPriceModel model)
        {
            LineFixedPriceModel pastModel = GetByLinePlanID(model.LPID);
            if (pastModel == null)
            {
                return Add(model);
            }
            else
            {
                int i = 0;
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    WriteUpdateLog<LineFixedPriceModel>(model, pastModel);
                    i = service.Update(model);
                    scope.Complete();
                }
                return i;
            }
        }

        public int Delete(List<int> lpidList)
        {
            return service.Delete(lpidList);
        }

        #endregion
    }
}

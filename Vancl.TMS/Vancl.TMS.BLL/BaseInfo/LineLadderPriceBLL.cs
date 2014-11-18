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
    public class LineLadderPriceBLL : BaseBLL, ILineLadderPriceBLL
    {
        ILineLadderPriceDAL service = ServiceFactory.GetService<ILineLadderPriceDAL>("LineLadderPriceDAL");

        #region ILineLadderPriceBLL 成员

        public int Add(List<LineLadderPriceModel> models)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                List<int> lpidList = new List<int>();
                lpidList.Add(models[0].LPID);
                int preCount = Delete(lpidList);
                foreach (LineLadderPriceModel item in models)
                {
                    i += service.Add(item);
                }
                if (preCount <= 0)
                {
                    WriteInsertLog<LineLadderPriceModel>(new LineLadderPriceModel() { LPID = models[0].LPID });
                }
                else
                {
                    WriteForcedUpdateLog<LineLadderPriceModel>(models[0].LPID.ToString());
                }
                scope.Complete();
            }
            return i;
        }

        public IList<LineLadderPriceModel> GetByLinePlanID(int lpid)
        {
            return service.GetByLinePlanID(lpid);
        }

        public int Delete(List<int> lpidList)
        {
            return service.Delete(lpidList);
        }

        public int Update(List<LineLadderPriceModel> models)
        {
            return Add(models);
        }

        #endregion
    }
}

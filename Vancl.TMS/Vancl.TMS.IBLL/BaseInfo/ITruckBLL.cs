using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Truck;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface ITruckBLL
    {
        PagedList<ViewTruckModel> GetTruckList(TruckSearchModel searchModel);

        ResultModel Add(TruckBaseInfoModel model);

        ResultModel Update(TruckBaseInfoModel model);

        ResultModel SetDisabled(List<string> tbidList);

        TruckBaseInfoModel GetTruckBaseInfo(string tbid);

        IList<TruckBaseInfoModel> GetAll();
    }
}

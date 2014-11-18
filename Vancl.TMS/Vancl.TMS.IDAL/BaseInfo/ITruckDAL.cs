using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Truck;

namespace Vancl.TMS.IDAL.BaseInfo
{
    public interface ITruckDAL :ISequenceDAL
    {
        PagedList<ViewTruckModel> GetTruckList(TruckSearchModel searchModel);

        int Add(TruckBaseInfoModel model);

        int Update(TruckBaseInfoModel model);

        int SetDisabled(List<string> tbidList);

        TruckBaseInfoModel GetTruckBaseInfo(string tbid);

        IList<TruckBaseInfoModel> GetAll();
    }
}

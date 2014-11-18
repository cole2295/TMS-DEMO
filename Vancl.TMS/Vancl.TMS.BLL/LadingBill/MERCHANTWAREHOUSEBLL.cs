using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.LadingBill;

namespace Vancl.TMS.BLL.LadingBill
{
    public class MERCHANTWAREHOUSEBLL : IMERCHANTWAREHOUSEBLL
    {
        //private IMERCHANTWAREHOUSEBLL _merchantwarehousebll = new MERCHANTWAREHOUSEBLL();

        //IMERCHANTWAREHOUSEBLL  = ServiceFactory.GetService<IMERCHANTWAREHOUSEBLL>();

        IMERCHANTWAREHOUSEDAL _dal = ServiceFactory.GetService<IMERCHANTWAREHOUSEDAL>();

        public IList<Model.LadingBill.MERCHANTWAREHOUSE> GetModelList(Dictionary<string, object> searchParams)
        {
            return _dal.GetModelList(searchParams);
        }


        public Model.LadingBill.MERCHANTWAREHOUSE GetModelByid(string  warehouseid)
        {
            return _dal.GetModelByid(warehouseid);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.BLL.Synchronous.Dps2TmsSync
{
    public class Dps2Tms : IDps2Tms
    {
        ILms2TmsSyncTMSDAL _tmsDAL = ServiceFactory.GetService<ILms2TmsSyncTMSDAL>("Lms2TmsSyncTMSDAL");
        public bool ImportBaseDataToTms(BillModel billm, BillInfoModel billInfom)
        {
            bool isBillExists = _tmsDAL.IsBillExists(billm.FormCode);
            bool isBillInfoExists = true;
            if (!isBillExists)
            {
                isBillInfoExists = _tmsDAL.IsBillInfoExists(billm.FormCode);
            }
         
            if (isBillInfoExists || isBillExists)
            {
                return true;
            }
            bool isOk = true;
            //先插入明细表
            isOk = _tmsDAL.AddBillInfo(billInfom) == 1;

            //插入主表
            if (isOk)
            {
                isOk = _tmsDAL.AddBill(billm) == 1;
            }
            return isOk;
        }

    }
}

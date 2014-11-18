using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;

namespace Vancl.TMS.BLL.BaseInfo
{
    public class MerchantBLL : BaseBLL, IMerchantBLL
    {
        IMerchantDAL MerchantDAL = ServiceFactory.GetService<IMerchantDAL>();

        public IList<Model.BaseInfo.MerchantModel> GetMerchantListByDistributionCode(string distributionCode)
        {
            if (string.IsNullOrWhiteSpace(distributionCode)) throw new ArgumentException("传入的参数为空");

            return MerchantDAL.GetMerchantListByDistributionCode(distributionCode);
        }

        public Model.BaseInfo.MerchantModel GetByID(long ID)
        {
            return MerchantDAL.GetByID(ID);
        }
    }
}

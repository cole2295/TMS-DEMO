using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;


namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface IMerchantBLL
    {
        /// <summary>
        /// 获取与配送商有对应关系的商家
        /// </summary>
        /// <param name="distributionCode">配送商编码</param>
        /// <returns>商家Model列表</returns>
        IList<MerchantModel> GetMerchantListByDistributionCode(string distributionCode);

        /// <summary>
        /// 获取商家
        /// </summary>
        /// <param name="ID">商家ID</param>
        /// <returns>商家Model</returns>
        MerchantModel GetByID(long ID);
    }
}

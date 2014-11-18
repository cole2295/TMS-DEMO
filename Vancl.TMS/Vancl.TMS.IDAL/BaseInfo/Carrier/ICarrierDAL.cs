using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Carrier;

namespace Vancl.TMS.IDAL.BaseInfo.Carrier
{
    public interface ICarrierDAL : ISequenceDAL
    {
        /// <summary>
        /// 新增承运商
        /// </summary>
        /// <param name="model">承运商</param>
        /// <returns></returns>
        int Add(CarrierModel model);
        /// <summary>
        /// 根据承运商id取得承运商
        /// </summary>
        /// <param name="id">承运商id</param>
        /// <returns></returns>
        CarrierModel Get(int id);
        /// <summary>
        /// 更新承运商
        /// </summary>
        /// <param name="model">承运商</param>
        /// <returns></returns>
        int Update(CarrierModel model);
        /// <summary>
        /// 删除承运商
        /// </summary>
        /// <param name="ids">承运商id，多个以逗号隔开</param>
        /// <returns></returns>
        int Delete(IList<int> ids);
        /// <summary>
        /// 承运商名称是否已经存在
        /// </summary>
        /// <param name="name">承运商名称</param>
        /// <param name="carrierID">承运商id</param>
        /// <returns></returns>
        bool IsExistCarrierName(string name, int carrierID);
        /// <summary>
        /// 承运商合同编号是否已经存在
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="carrierID"></param>
        /// <returns></returns>
        bool IsExistContractNumber(string contractNumber, int carrierID);
        /// <summary>
        /// 承运商全称是否已经存在
        /// </summary>
        /// <param name="name">承运商全称</param>
        /// <param name="carrierID">承运商id</param>
        /// <returns></returns>
        bool IsExistCarrierAllName(string name, int carrierID);
        /// <summary>
        /// 邮件地址是否已经存在
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="carrierID">承运商id</param>
        /// <returns></returns>
        bool IsExistEmail(string email, int carrierID);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagedList<CarrierModel> Search(CarrierSearchModel searchModel);
        /// <summary>
        /// 取得所有承运商
        /// </summary>
        /// <returns></returns>
        IList<CarrierModel> GetAll();

        /// <summary>
        /// 获取承运商ID
        /// </summary>
        /// <param name="name">承运商名称</param>
        /// <param name="distributionCode">配送商编号</param>
        /// <returns></returns>
        int GetCarrierIdByName(string name, string distributionCode);
    }
}

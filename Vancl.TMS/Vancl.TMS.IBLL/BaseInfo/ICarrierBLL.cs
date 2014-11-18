using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.BaseInfo
{
    /// <summary>
    /// 承运商业务层操作接口
    /// </summary>
    public interface ICarrierBLL
    {
        /// <summary>
        /// 新增承运商
        /// </summary>
        /// <param name="carrier">承运商</param>
        /// <param name="delayCriteriaList">延误审核标准</param>
        /// <param name="coverageList">覆盖范围</param>
        /// <param name="msg">返回消息</param>
        /// <returns>操作结果</returns>
        ResultModel Add(CarrierModel carrier, IList<DelayCriteriaModel> delayCriteriaList, IList<CoverageModel> coverageList);

        /// <summary>
        /// 根据承运商id取得承运商
        /// </summary>
        /// <param name="carrierID">承运商ID</param>
        /// <returns>承运商模型</returns>
        CarrierModel Get(int carrierID);

        /// <summary>
        /// 取得所有承运商数据
        /// </summary>
        /// <returns>承运商列表</returns>
        IList<CarrierModel> GetAll();

        /// <summary>
        /// 更新承运商
        /// </summary>
        /// <param name="carrier">承运商</param>
        /// <param name="delayCriteriaList">延误审核标准</param>
        /// <param name="coverageList">适用范围</param>
        /// <returns>操作结果</returns>
        ResultModel Update(CarrierModel carrier, IList<DelayCriteriaModel> delayCriteriaList, IList<CoverageModel> coverageList);

        /// <summary>
        /// 批量删除承运商
        /// </summary>
        /// <param name="ids">承运商id列表</param>
        /// <returns>操作结果</returns>
        ResultModel Delete(List<int> ids);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns>承运商分页列表</returns>
        PagedList<CarrierModel> Search(CarrierSearchModel searchModel);

        /// <summary>
        /// 是否已经存在承运商名称或全名
        /// </summary>
        /// <param name="name">承运商名称或全名</param>
        /// <param name="carrierID">承运商id</param>
        /// <param name="isAllName">是否是承运商全名</param>
        /// <returns>存在:true,不存在:flase</returns>
        bool IsExistCarrier(string name, int carrierID, bool isAllName = false);

        /// <summary>
        /// 是否已经存在承运商合同编号
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="carrierID"></param>
        /// <returns></returns>
        bool IsExistContractNumber(string contractNumber, int carrierID);

        /// <summary>
        /// 是否已经存在承运商的邮件地址
        /// </summary>
        /// <param name="email">承运商邮件地址</param>
        /// <param name="carrierID">承运商id</param>
        /// <returns>存在:true,不存在:flase</returns>
        bool IsExistEmail(string email, int carrierID);

        /// <summary>
        /// 根据承运商id取得延误审核标准
        /// </summary>
        /// <param name="carrierID">承运商id</param>
        /// <returns>延误审核标准列表</returns>
        IList<DelayCriteriaModel> GetDelayCriteriaByCarrierID(int carrierID);

        /// <summary>
        /// 根据承运商id取得适用范围
        /// </summary>
        /// <param name="carrierID">承运商id</param>
        /// <returns>适用范围列表</returns>
        IList<CoverageModel> GetCoverageByCarrierID(int carrierID);

        /// <summary>
        /// 获取承运商ID
        /// </summary>
        /// <param name="name">承运商名称</param>
        /// <param name="distributionCode">配送商编号</param>
        /// <returns></returns>
        int GetCarrierIdByName(string name, string distributionCode);
    }
}

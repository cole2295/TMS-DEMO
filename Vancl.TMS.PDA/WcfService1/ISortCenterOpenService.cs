using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using RFD.LMS.Model.SortingCenter;

namespace RFD.LMS.SortCenter.OpenService
{
    // 注意: 如果更改此处的接口名称 "ISortCenterOpenService"，也必须更新 Web.config 中对 "ISortCenterOpenService" 的引用。
    [ServiceContract]
    public interface ISortCenterOpenService
    {

        /// <summary>
        /// 入库前端接口
        /// </summary>
        /// <param name="argument">入库参数</param>
        /// <returns>入库结果信息</returns>
        [OperationContract]
        SortCenterInBoundViewModel Inbound(SortCenterInboundArgModel argument);

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument">SortCenterInboundArgModel</param>
        /// <returns>当前操作入库数量</returns>
        [OperationContract]
        int GetInboundCount(SortCenterInboundArgModel argument);

        /// <summary>
        /// 获取入库操作人对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [OperationContract]
        SortCenterInboundUserModel GetSortCenterInboundUser(int UserID);

        /// <summary>
        /// 获取入库前置条件
        /// </summary>
        /// <param name="DistributionCode">入库操作人所属DistributionCode</param>
        /// <returns></returns>
        [OperationContract]
        SortCenterInboundPreCondition GetSortCenterInboundPreCondition(string DistributionCode);

        /// <summary>
        /// 取得入库目的地对象
        /// </summary>
        /// <param name="ExpressCompanyID">ExpressCompanyID</param>
        /// <returns></returns>
        [OperationContract]
        SortCenterInboundToStationModel GetSortCenterToStation(int ExpressCompanyID);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RFD.LMS.Model.SortingCenter;

namespace RFD.LMS.SortCenter.API
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISortCenterWCFOpenService”。
    [ServiceContract]
    public interface ISortCenterWCFOpenService
    {
        [OperationContract]
        void DoWork();

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

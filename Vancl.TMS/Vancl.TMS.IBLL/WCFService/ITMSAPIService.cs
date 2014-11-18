using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Vancl.TMS.Model.WCFService;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.IBLL.WCFService
{
    /// <summary>
    /// TMS统一接口
    /// </summary>
    [ServiceContract]
    public interface ITMSAPIService
    {

        /// <summary>
        /// TMS数据入口
        /// </summary>
        /// <param name="entranceModel">TMS数据入口对象</param>
        /// <returns></returns>
        [OperationContract]
        ResultModel DataEntrance(TMSEntranceModel entranceModel);

        /// <summary>
        /// 逐单入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        [OperationContract]
        ViewInboundSimpleModel SimpleInbound(InboundSimpleArgModel argument);

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>当前操作入库数量</returns>
        [OperationContract]
        [ServiceKnownType(typeof(InboundSimpleArgModel))]
        int GetInboundCount(ISortCenterArgModel argument);

        /// <summary>
        /// 取得分拣用户对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        [OperationContract]
        SortCenterUserModel GetUserModel(int UserID);

        /// <summary>
        /// 取得目的地对象
        /// </summary>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        [OperationContract]
        SortCenterToStationModel GetToStationModel(int ArrivalID);

        /// <summary>
        /// 入库前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        [OperationContract]
        InboundPreConditionModel GetPreCondition(String DistributionCode);

    }
}

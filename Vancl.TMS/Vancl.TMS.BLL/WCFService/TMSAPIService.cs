using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using Vancl.TMS.IBLL.WCFService;
using Vancl.TMS.Model.WCFService;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.BLL.WCFService
{
    /// <summary>
    /// TMS统一接口
    /// </summary>
    public class TMSAPIService : BaseBLL, ITMSAPIService
    {
        /// <summary>
        /// TMS数据交互服务
        /// </summary>
        IDeliveryDataEntranceBLL DeliveryDataEntranceBLL = ServiceFactory.GetService<IDeliveryDataEntranceBLL>("DeliveryDataEntranceBLL");
        IInboundBLL _inboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");

        #region ITMSAPIService 成员

        /// <summary>
        /// 逐单入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        public ViewInboundSimpleModel SimpleInbound(InboundSimpleArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundSimpleArgModel is null.");
            return _inboundBLL.SimpleInbound(argument);
        }

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>当前操作入库数量</returns>
        public int GetInboundCount(ISortCenterArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundSimpleArgModel is null.");
            return _inboundBLL.GetInboundCount(argument);
        }

        /// <summary>
        /// 取得分拣用户对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public SortCenterUserModel GetUserModel(int UserID)
        {
            return _inboundBLL.GetUserModel(UserID);
        }

        /// <summary>
        /// 取得目的地对象
        /// </summary>
        /// <param name="ArrivalID"></param>
        /// <returns></returns>
        public SortCenterToStationModel GetToStationModel(int ArrivalID)
        {
            return _inboundBLL.GetToStationModel(ArrivalID);
        }

        /// <summary>
        /// 入库前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        public InboundPreConditionModel GetPreCondition(String DistributionCode)
        {
            if (String.IsNullOrWhiteSpace(DistributionCode)) throw new ArgumentNullException("InboundSimpleArgModel is null.");
            return _inboundBLL.GetPreCondition(DistributionCode);
        }

        /// <summary>
        /// TMS数据入口
        /// </summary>
        /// <param name="entranceModel">TMS数据入口对象</param>
        /// <returns></returns>
        public ResultModel DataEntrance(TMSEntranceModel entranceModel)
        {
            return DeliveryDataEntranceBLL.DataEntrance(entranceModel);
        }

        #endregion
    }
}
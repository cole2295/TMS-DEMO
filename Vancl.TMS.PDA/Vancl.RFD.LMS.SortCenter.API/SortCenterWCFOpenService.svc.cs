using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LMS.Util;
using RFD.LMS.Service.SortingCenter;
using RFD.LMS.Service.BasicSetting;
using LMS.Model;
using RFD.LMS.Model.SortingCenter;

namespace RFD.LMS.SortCenter.API
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SortCenterWCFOpenService”。
    public class SortCenterWCFOpenService : ISortCenterWCFOpenService
    {
        /// <summary>
        /// 入库分拣接口
        /// </summary>
        private static readonly ISortCenterInboundBLL sortingCenterService = ServiceLocator.GetService<ISortCenterInboundBLL>();
        private static readonly IInBound inboundService = ServiceLocator.GetService<IInBound>();
        private static readonly IDistributionService distributionService = ServiceLocator.GetService<IDistributionService>();


        public void DoWork()
        {
        }

        #region ISortCenterWCFOpenService 成员

        /// <summary>
        /// 入库前端接口
        /// </summary>
        /// <param name="argument">入库参数</param>
        /// <returns>入库结果信息</returns>
        public RFD.LMS.Model.SortingCenter.SortCenterInBoundViewModel Inbound(RFD.LMS.Model.SortingCenter.SortCenterInboundArgModel argument)
        {
            return sortingCenterService.Inbound(argument);
        }

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument">SortCenterInboundArgModel</param>
        /// <returns>当前操作入库数量</returns>
        public int GetInboundCount(RFD.LMS.Model.SortingCenter.SortCenterInboundArgModel argument)
        {
            return sortingCenterService.GetInboundCount(argument);
        }

        /// <summary> 
        /// 获取入库操作人对象
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public RFD.LMS.Model.SortingCenter.SortCenterInboundUserModel GetSortCenterInboundUser(int UserID)
        {
            if (UserID < 0) throw new ArgumentNullException("GetSortCenterInboundUser User ID Error");
            return inboundService.GetSortCenterInboundUser(UserID);
        }


        /// <summary>
        /// 获取入库前置条件
        /// </summary>
        /// <param name="DistributionCode">入库操作人所属DistributionCode</param>
        /// <returns></returns>
        public RFD.LMS.Model.SortingCenter.SortCenterInboundPreCondition GetSortCenterInboundPreCondition(string DistributionCode)
        {
                if (String.IsNullOrEmpty(DistributionCode)) throw new ArgumentNullException("GetSortCenterInboundPreCondition DistributionCode Error");
                EnumCommon.WayBillStatusEnum? preStatus = distributionService.GetDistributionWaybillPreStatus(DistributionCode,
                                           ((int)EnumCommon.WayBillStatusEnum.AssignedBound).ToString());
                if (preStatus.HasValue)
                {
                    return new SortCenterInboundPreCondition() { PreStatus = preStatus.Value };
                }
                return null;
        }


        /// <summary>
        /// 取得入库目的地对象
        /// </summary>
        /// <param name="ExpressCompanyID">ExpressCompanyID</param>
        /// <returns></returns>
        public Model.SortingCenter.SortCenterInboundToStationModel GetSortCenterToStation(int ExpressCompanyID)
        {
            if (ExpressCompanyID < 0) throw new ArgumentNullException("GetSortCenterToStation ExpressCompanyID Error");
            return inboundService.GetSortCenterInboundToStation(ExpressCompanyID);
        }

        #endregion
    }
}

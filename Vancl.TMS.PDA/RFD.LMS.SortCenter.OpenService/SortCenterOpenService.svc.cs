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
using System.IO;
using System.ServiceModel.Activation;

namespace RFD.LMS.SortCenter.OpenService
{
    // 注意: 如果更改此处的类名 "SortCenterOpenService"，也必须更新 Web.config 中对 "SortCenterOpenService" 的引用。
    public class SortCenterOpenService : ISortCenterOpenService
    {
        /// <summary>
        /// 入库分拣接口
        /// </summary>
        private static readonly ISortCenterInboundBLL sortingCenterService = ServiceLocator.GetService<ISortCenterInboundBLL>();
        private static readonly IInBound inboundService = ServiceLocator.GetService<IInBound>();
        private static readonly IDistributionService distributionService = ServiceLocator.GetService<IDistributionService>();


        #region ISortCenterOpenService 成员

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
            try
            {
                if (UserID < 0) throw new ArgumentNullException("GetSortCenterInboundUser User ID Error");
                return inboundService.GetSortCenterInboundUser(UserID);
            }
            catch (Exception e)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("PDA");
                log.Error(e);
                return new SortCenterInboundUserModel() { UserName = e.StackTrace, CompanyName = e.Message };
            }
        }

        /// <summary>
        /// 获取入库前置条件
        /// </summary>
        /// <param name="DistributionCode">入库操作人所属DistributionCode</param>
        /// <returns></returns>
        public RFD.LMS.Model.SortingCenter.SortCenterInboundPreCondition GetSortCenterInboundPreCondition(string DistributionCode)
        {
            try
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
            catch (Exception e)
            {
                log4net.ILog log = log4net.LogManager.GetLogger("PDA接口异常");
                log.Error(e);
                throw;
            }
        }

        #endregion

        #region ISortCenterOpenService 成员

        /// <summary>
        /// 取得入库目的地对象
        /// </summary>
        /// <param name="ExpressCompanyID">ExpressCompanyID</param>
        /// <returns></returns>
        public SortCenterInboundToStationModel GetSortCenterToStation(int ExpressCompanyID)
        {
            if (ExpressCompanyID < 0) throw new ArgumentNullException("GetSortCenterToStation ExpressCompanyID Error");
            return inboundService.GetSortCenterInboundToStation(ExpressCompanyID);
        }

        #endregion
    }
}

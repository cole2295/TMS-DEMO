using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.Proxy.TMSAPIService;

namespace Vancl.TMS.BLL.Proxy
{
    public class InBoundProxy
    {
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static ViewInboundSimpleModel SimpleInBound(InboundSimpleArgModel arg)
        {
            if (arg == null) throw new ArgumentNullException();
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                return proxy.SimpleInbound(arg);
            }

        }

        /// <summary>
        /// 获取当前已入库数量
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static int GetInboundCount(InboundSimpleArgModel arg)
        {
            if (arg == null) throw new ArgumentNullException();
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                return proxy.GetInboundCount(arg);
            }
        }

        /// <summary>
        /// 获取用户MODEL
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static SortCenterUserModel GetUserModel(int userID)
        {
            if (userID == -1) throw new ArgumentNullException();
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                return proxy.GetUserModel(userID);
            }
        }

        /// <summary>
        /// 获取配送商入库前置条件
        /// </summary>
        /// <param name="distributionCode"></param>
        /// <returns></returns>
        public static InboundPreConditionModel GetPreCondition(string distributionCode)
        {
            if (string.IsNullOrWhiteSpace(distributionCode)) throw new ArgumentNullException();
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                return proxy.GetPreCondition(distributionCode);
            }
        }

        /// <summary>
        /// 获取站点信息
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public static SortCenterToStationModel GetToStationModel(int stationID)
        {
            if (stationID == -1) throw new ArgumentNullException();
            using (TMSAPIService.TMSAPIServiceClient proxy = new TMSAPIService.TMSAPIServiceClient())
            {
                return proxy.GetToStationModel(stationID);
            }
        }
    }
}

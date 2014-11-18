using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RFD.LMS.Service.SortingCenter;
using RFD.LMS.Service.BasicSetting;
using LMS.Util;

namespace RFD.LMS.BLL.Proxy
{
    public class SortCenterContext
    {
        private static readonly ISortCenterInboundBLL sortingCenterService = ServiceLocator.GetService<ISortCenterInboundBLL>();
        private static readonly IInBound inboundService = ServiceLocator.GetService<IInBound>();
        private static readonly IDistributionService distributionService = ServiceLocator.GetService<IDistributionService>();

        /// <summary>
        /// 入库前端接口
        /// </summary>
        /// <param name="argument">入库参数</param>
        /// <returns>入库结果信息</returns>
        public static RFD.LMS.Model.SortingCenter.SortCenterInBoundViewModel Inbound(RFD.LMS.Model.SortingCenter.SortCenterInboundArgModel argument)
        {
            //RFD.LMS.
            return sortingCenterService.Inbound(argument);
        }
    }
}

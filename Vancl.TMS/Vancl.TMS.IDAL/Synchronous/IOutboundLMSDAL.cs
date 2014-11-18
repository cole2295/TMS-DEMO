using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous;
using  Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.JobMonitor;


namespace Vancl.TMS.IDAL.Synchronous
{
    /// <summary>
    /// 出库同步从LMS读取数据到文件
    /// </summary>
    public interface IOutboundLMSDAL
    {
        /// <summary>
        /// 根据条件取得一条未同步的箱对象
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        OutBoxModel GetBoxModel(OutboundReadParam argument);

        /// <summary>
        /// 根据箱号取得订单明细
        /// </summary>
        /// <param name="boxModel"></param>
        /// <returns></returns>
        List<OrderModel> GetOrderList(OutBoxModel boxModel);

        /// <summary>
        /// 根据运单取得货物明细列表
        /// </summary>
        /// <param name="listOrderNo"></param>
        /// <returns></returns>
        [Obsolete]
        List<OrderDetailModel> GetOrderDetailList(List<long> listWaybillNo);

        /// <summary>
        /// 更新箱对象的同步状态
        /// </summary>
        /// <param name="boxModel"></param>
        /// <param name="prevSyncFlag"></param>
        void UpdateBoxStatus(OutBoxModel boxModel, Enums.SC2TMSSyncFlag prevSyncFlag);

        /// <summary>
        /// 取得出库统计信息
        /// </summary>
        /// <returns></returns>
        SyncStatisticInfo GetStatisticInfo();

    }
}

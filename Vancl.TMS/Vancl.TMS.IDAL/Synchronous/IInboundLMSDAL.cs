using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Synchronous
{
    public interface IInboundLMSDAL
    {
        /// <summary>
        /// 获取入库箱
        /// </summary>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        DataSet GetInboundBox(int mod, int remainder);
        /// <summary>
        /// 获取入库订单
        /// </summary>
        /// <param name="count">每次处理最大数</param>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        DataSet GetInboundOrder(int count, int mod, int remainder);
        int UpdateBoxSyncFlag(long id, Enums.SC2TMSSyncFlag syncFlag);
        int UpdateOrderSyncFlag(string ids, Enums.SC2TMSSyncFlag syncFlag);

        /// <summary>
        /// 取得入库中间箱表的统计信息
        /// </summary>
        /// <returns></returns>
        Model.JobMonitor.SyncStatisticInfo GetBoxStatisticInfo();

        /// <summary>
        /// 取得入库中间订单表的统计信息
        /// </summary>
        /// <returns></returns>
        Model.JobMonitor.SyncStatisticInfo GetOrderStatisticInfo();

    }
}

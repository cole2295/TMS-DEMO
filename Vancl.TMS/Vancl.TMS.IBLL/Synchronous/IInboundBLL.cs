using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Synchronous
{
    public interface IInboundBLL
    {
        /// <summary>
        /// 取得入库数据（刷箱子入库）
        /// </summary>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        InboundModel GetInboundBox(int mod, int remainder);

        /// <summary>
        /// 取得入库数据（刷订单入库）
        /// </summary>
        /// <param name="count">最大处理数</param>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        List<InboundModel> GetInboundOrder(int count, int mod, int remainder);

        /// <summary>
        /// 更新入库箱号中间表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="syncFlag"></param>
        /// <returns></returns>
        int UpdateBoxSyncFlag(long id, Enums.SC2TMSSyncFlag syncFlag);

        /// <summary>
        /// 更新入库订单中间表
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="syncFlag"></param>
        /// <returns></returns>
        int UpdateOrderSyncFlag(string ids, Enums.SC2TMSSyncFlag syncFlag);

        /// <summary>
        /// 更新TMS库各种状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int UpdateTMS(List<InboundModel> list);
    }
}

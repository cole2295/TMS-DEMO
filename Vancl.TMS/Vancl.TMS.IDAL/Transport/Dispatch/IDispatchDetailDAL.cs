using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;

namespace Vancl.TMS.IDAL.Transport.Dispatch
{
    /// <summary>
    /// 提货单箱明细数据接口
    /// </summary>
    public interface IDispatchDetailDAL : ISequenceDAL
    {
        /// <summary>
        /// 添加提货单箱明细
        /// </summary>
        /// <param name="model"></param>
        void Add(DispatchDetailModel model);

        /// <summary>
        /// 确认调度修改调度明细记录
        /// </summary>
        /// <param name="DID">调度主表DID</param>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        int UpdateBy_ConfirmDispatch(long DID, String DeliveryNo);

        /// <summary>
        /// 添加提货单箱明细
        /// </summary>
        /// <param name="model"></param>
        void Add(List<DispatchDetailModel> listmodel);

        /// <summary>
        /// 撤回提货单明细
        /// </summary>
        /// <param name="DeliveryNo">提货单</param>
        /// <returns></returns>
        [Obsolete("采用新模式，此方法废弃")]
        int Delete(string DeliveryNo);

        /// <summary>
        /// 根据运输调度主键逻辑删除明细信息
        /// </summary>
        /// <param name="DID">运输调度主键</param>
        /// <returns></returns>
        int Delete(long DID);

        /// <summary>
        /// 根据DID查询出所有的调度明细
        /// </summary>
        /// <param name="DID"></param>
        /// <returns></returns>
        List<DispatchDetailModel> GetDispatchDetailByDID(long DID);
    }
}

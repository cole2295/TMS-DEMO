using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.DeliveryAbnormal;

namespace Vancl.TMS.IDAL.BaseInfo
{
    public interface IBoxDAL : ISequenceDAL
    {

        /// <summary>
        /// 更新批次总价格
        /// </summary>
        /// <param name="listAmount"></param>
        void UpdateBatchTotalAmount(Dictionary<String, decimal> listAmount);


        /// <summary>
        /// 是否客户批次号、箱号存在
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        bool IsBoxNoExists(String BatchNo, Enums.TMSEntranceSource Source);

        /// <summary>
        /// 是否系统批次号存在
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <returns></returns>
        bool IsBoxNoExists(String BatchNo);

        /// <summary>
        /// 是否存在系统批次数据对接信息不全
        /// </summary>
        /// <param name="listBatchNo">系统批次号列表</param>
        /// <returns></returns>
        bool IsExistsBatchDockingFailed(List<String> listBatchNo);

        /// <summary>
        /// 新增箱对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(BoxModel model);

        /// <summary>
        /// 新增箱订单明细对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddBoxDetail(List<BoxDetailModel> model);

        /// <summary>
        /// 根据系统批次号取得批次对象
        /// </summary>
        /// <param name="arrBatchNo">系统批次号数组</param>
        /// <returns></returns>
        List<BoxModel> GetBatchInfo(String[] arrBatchNo);

        /// <summary>
        /// 取得未丢失的订单明细
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        List<OrderModel> GetUnLostOrderList(string[] box);
        /// <summary>
        /// 更新为已经预调度
        /// </summary>
        /// <param name="boxNos"></param>
        /// <returns></returns>
        int UpdatePreDispatched(string boxNos);

        /// <summary>
        /// 更新为已经预调度
        /// </summary>
        /// <param name="listBID"></param>
        /// <returns></returns>
        int UpdatePreDispatched(List<long> listBID);

        /// <summary>
        /// 更新为预调度错误
        /// </summary>
        /// <param name="listBID"></param>
        /// <returns></returns>
        int UpdatePreDispatchedError(List<long> listBID);

        /// <summary>
        /// 把预调度错误的更新为未调度状态
        /// </summary>
        /// <param name="listBID"></param>
        /// <returns></returns>
        int UpdateNoPreDispatched_ByRePreDispatch(List<long> listBID);

        /// <summary>
        /// 查询 预调度异常
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<PreDispatchAbnormalModel> GetPreDispatchAbnormalList(PreDispatchAbnormalSearchModel searchModel);
    }
}

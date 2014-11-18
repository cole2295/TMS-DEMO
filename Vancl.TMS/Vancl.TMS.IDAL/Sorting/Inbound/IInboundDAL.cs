using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound;

namespace Vancl.TMS.IDAL.Sorting.Inbound
{
    /// <summary>
    /// 入库数据层
    /// </summary>
    public interface IInboundDAL
    {
        /// <summary>
        /// 入库记录
        /// </summary>
        /// <param name="model">入库实体对象</param>
        /// <returns></returns>
        int Add(InboundEntityModel model);

        /// <summary>
        /// 入库记录V2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddV2(InboundEntityModel model);

        /// <summary>
        /// 取得处于已入库状态数量
        /// </summary>
        /// <param name="DepartureID">当前分拣中心</param>
        /// <param name="ArrivalID">目的地分检中心</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        int GetInboundCount(int DepartureID, int ArrivalID, DateTime StartTime, DateTime EndTime);

        /// <summary>
        /// 验证运单重量
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        bool ValidateBillWeight(String FormCode);

        /// <summary>
        /// 验证站点是否属于配送商
        /// </summary>
        /// <param name="DistributionCode">配送商</param>
        /// <param name="DeliverStationID">站点信息</param>
        /// <returns></returns>
        bool ValidateDistributionDeliverStation(String DistributionCode, int DeliverStationID);

        /// <summary>
        /// 存在同最后一次入库相同出发地目的地入库记录
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="DepartureID">出发地</param>
        /// <param name="ArrivalID">目的地</param>
        /// <returns></returns>
        bool ExistsLine_EqualLastInboud(String FormCode, int DepartureID, int ArrivalID);

        /// <summary>
        /// 取得TMS入库记录需要同步到LMS物流主库的入库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        InboundEntityModel GetInboundEntityModel4TmsSync2Lms(String FormCode);

        /// <summary>
        /// 更新同步状态为已经同步成功【TMS同步会LMS主库时调用】
        /// </summary>
        /// <param name="InboundKey">入库主键key</param>
        /// <returns></returns>
        int UpdateSyncedStatus4Tms2Lms(String InboundKey);

        /// <summary>
        /// 获取是否需要验证操作入库部门与打印面单部门是否一致
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        Int32 GetPrintDept(long formCode);
        /// <summary>
        /// 取得处于已入库状态数量(新)
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        int GetInboundCountNew(int DepartureID, DateTime StartTime, DateTime EndTime);
        /// <summary>
        /// 取得FormCode已经入库的单数
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        int GetFormCodeInbound(string FormCode);

        /// <summary>
        /// 获得当前配送商入库记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>

        int GetDistributionInboundCount(string FormCode, string DistributionCode);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.Transport.Dispatch
{
    /// <summary>
    /// 预调度数据层
    /// </summary>
    public interface IPreDispatchDAL
    {
        /// <summary>
        /// 查询预调度失败的记录
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewPreDispatchLogModel> SearchPreDispatchLog(PreDispatchLogSearchModel searchModel);

        /// <summary>
        /// 查询预调度失败的统计记录
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        List<ViewPreDispatchLogStatisticModel> SearchPreDispatchStatisticLog(PreDispatchLogSearchModel searchModel);

        /// <summary>
        /// 批量新增预调度异常日志
        /// </summary>
        /// <param name="listLogModel"></param>
        /// <returns></returns>
        int BatchAddPreDispatchLog(List<PreDispatchLogEntityModel> listLogModel);

        /// <summary>
        /// 新增预调度异常日志
        /// </summary>
        /// <param name="LogModel"></param>
        /// <returns></returns>
        int AddPreDispatchLog(PreDispatchLogEntityModel LogModel);

        /// <summary>
        /// 取得需要进行预调度的批次列表
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        List<BoxModel> GetNeededPreDispatchBatchList(PreDispatchJobArgModel arguments);

        /// <summary>
        /// 根据唯一键获取异常的批次
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        BoxModel GetAbnormalPreDispatchByBID(Int64 bid);

        /// <summary>
        /// 取得预调度信息【扫描批次合并】
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> GetPreDispatchInfo(PreDispatchSearchModel searchmodel);

        /// <summary>
        /// 取得预调度信息【查询批次合并】
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> SearchPreDispatchInfo(PreDispatchSearchModel searchmodel);

        /// <summary>
        /// 批量新加预调度记录
        /// </summary>
        /// <param name="model">预调度对象列表</param>
        /// <returns></returns>
        int BatchAdd(List<PreDispatchModel> model);

        /// <summary>
        /// 批量新加预调度记录V1
        /// </summary>
        /// <param name="model">预调度对象列表</param>
        /// <returns></returns>
        int BatchAddV1(List<PreDispatchModel> model);

        /// <summary>
        /// 新增加预调度记录
        /// </summary>
        /// <param name="model">预调度对象</param>
        /// <returns></returns>
        int Add(PreDispatchModel model);

        /// <summary>
        /// 运输调度根据线路ID取得预调度信息
        /// </summary>
        /// <param name="LPID">线路计划ID</param>
        /// <returns></returns>
        [Obsolete("此方法作废")]
        List<ViewDispatchBoxModel> GetPreDispatchBoxList(int LPID);

        /// <summary>
        /// 运输调度取得箱子的当前预调度对象信息
        /// </summary>
        /// <param name="box">箱子</param>
        /// <param name="isContainDispatched">是否包含已经调度的</param>
        /// <returns></returns>
        [Obsolete("此方法作废")]
        List<PreDispatchModel> GetCurPreDispatchList(string[] box, bool isContainDispatched);

        /// <summary>
        /// 可调度状态更新为已调度状态
        /// </summary>
        /// <param name="listPDID">预调度主键ID列表</param>
        /// <returns></returns>
        [Obsolete("预调度取消该枚举,此方法作废")]
        int UpdateToDispatched(List<long> listPDID);

        /// <summary>
        /// 可调度状态更新为已作废状态
        /// </summary>
        /// <param name="listPDID">预调度主键ID列表</param>
        /// <returns></returns>
        [Obsolete("预调度取消该枚举,此方法作废")]
        int UpdateToInvalid(List<long> listPDID);

        /// <summary>
        /// 直接更新为不可调度状态
        /// </summary>
        /// <param name="listPDID">预调度主键ID列表</param>
        /// <returns></returns>
        int UpdateToDisabledDispatch(List<long> listPDID);

        /// <summary>
        /// 直接更新为可调度状态
        /// </summary>
        /// <param name="listPDID">预调度主键ID列表</param>
        /// <returns></returns>
        int UpdateToCanDispatch(List<long> listPDID);

        /// <summary>
        /// 更新城际批次为待预调度状态
        /// </summary>
        /// <param name="box">城际批次对象</param>
        /// <returns></returns>
        int UpdateBoxToWaitforDispatch(BoxModel box);

        /// <summary>
        /// 批量更新城际批次为待预调度状态
        /// </summary>
        /// <param name="listbox">城际批次对象</param>
        /// <returns></returns>
        int BatchUpdateUpdateBoxToWaitforDispatch(List<BoxModel> listbox);

        /// <summary>
        /// 查询出能够预调度的箱号和运输计划ID
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        [Obsolete("预调度采用新的处理方式,此方法作废")]
        DataTable GetValidBoxNoAndTPID(int count);

        /// <summary>
        /// 根据箱号和运输计划ID新增预调度数据
        /// </summary>
        /// <param name="BoxNos">箱号，以逗号分隔</param>
        /// <param name="TPID">运输计划ID</param>
        /// <returns></returns>
        [Obsolete("预调度采用新的处理方式,此方法作废")]
        int Add(string BoxNos, int TPID);

        /// <summary>
        /// 待调度批次查询
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> SearchPreDispatchInfoV1(PreDispatchSearchModel searchmodel);
        /// <summary>
        /// 修改单据状态
        /// </summary>
        /// <param name="listPDID"></param>
        /// <param name="UpdateStatus"></param>
        /// <returns></returns>
        int UpdateToDisabledDispatchV1(List<long> listPDID, Enums.DispatchStatus UpdateStatus);

        List<PreDispatchModel> GetModelByDispatch(PreDispatchPublicQueryModel QueryModel);

        List<PreDispatchModel> GetCurPreDispatchListByBoxNos(string[] box, Enums.DispatchStatus UpdateStatus);

        List<PreDispatchModel> GetNextPreDispatchListByBoxNos(List<string> box, int iNext);

        PreDispatchModel GetPreDispatchModelByPDID(long PDID);

        int UpdateToCanDispatchV1(List<long> listPDID);

        List<ViewDispatchBoxModel> GetPreDispatchBoxListV1(int LPID);

        List<ViewPreDispatchModel> GetPreDispatchInfoV1(PreDispatchSearchModel searchmodel);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound.SMS;

namespace Vancl.TMS.IDAL.Sorting.Outbound
{
    /// <summary>
    /// 出库数据接口
    /// </summary>
    public interface IOutboundDAL
    {
        /// <summary>
        /// 取得出发地目的地对象
        /// </summary>
        /// <param name="DepartureID">出发地ID</param>
        /// <param name="ArrivalID">目的地ID</param>
        /// <returns></returns>
        DepartureArrivalInfo GetDepartureArrivalInfo(int DepartureID, int ArrivalID);

        /// <summary>
        /// 取得需要出库的运单列表信息
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        List<ViewOutboundSearchListModel.InnerFormCodeList> GetNeededOutboundFormCodeList(OutboundSearchArgModel argument);

        /// <summary>
        /// 获取需要出库的包装箱列表
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        List<ViewOutBoundByBoxModel> GetNeededOutBoundBoxList(OutboundSearchModel argument);

        /// <summary>
        /// 根据箱号获取需要出库的运单
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        ViewOutBoundBoxDetailModel GetNeededOutBoundBillListByBoxNo(string boxNo);

        /// <summary>
        /// 取得处于入库中的运单数量
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        int GetInboundingCount(OutboundSearchArgModel argument);

        /// <summary>
        /// 新增出库记录
        /// </summary>
        /// <param name="outboundModel">出库对象</param>
        /// <returns></returns>
        int Add(OutboundEntityModel outboundModel);

        /// <summary>
        /// 批量新增出库记录
        /// </summary>
        /// <param name="listOutboundModel">出库对象列表</param>
        void BatchAdd(List<OutboundEntityModel> listOutboundModel);

        /// <summary>
        /// 取得TMS出库记录需要同步到LMS物流主库的出库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        OutboundEntityModel GetOutboundEntityModel4TmsSync2Lms(String FormCode);

        /// <summary>
        /// 更新同步标识[TMS到LMS]
        /// </summary>
        /// <param name="outboundKey">出库Key</param>
        /// <returns></returns>
        int UpdateSyncedStatus4Tms2Lms(String outboundKey);

        /// <summary>
        /// 出库打印搜索
        /// </summary>
        /// <param name="toStation"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="expressID"></param>
        /// <param name="batchNo"></param>
        /// <param name="waybillNo"></param>
        /// <param name="companyFlag"></param>
        /// <returns></returns>
        IList<OutboundPrintModel> SearchOutboundPrint(int toStation, DateTime beginTime, DateTime endTime
                                            , int expressID, string batchNo, string waybillNo, Enums.CompanyFlag companyFlag);

        /// <summary>
        /// 获取出库打印导出数据
        /// </summary>
        /// <param name="batchNoList"></param>
        /// <returns></returns>
        IList<OutboundPrintExportModel> GetOutboundPrintExportModel(IList<string> batchNoList);

        /// <summary>
        /// 获取出库打印交接单统计
        /// </summary>
        /// <param name="batchNoList"></param>
        /// <returns></returns>
        IList<OutboundOrderCountModel> GetOrderCount(IList<string> batchNoList);

        /// <summary>
        ///  获取批次打印明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo);

        /// <summary>
        /// 查询返货目的地
        /// </summary>
        /// <returns></returns>
        string GetReturnTo(string formCode, int arrivalID);

        /// <summary>
        /// 根据bachNoList获取出库实体模型列表
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        IList<OutboundEntityModel> GetOutboundEntityByBatchNoList(IList<string> batchNoList);

        /// <summary>
        /// 获取批次运单信息
        /// （供出库打印菜单发邮件使用）
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <param name="outboundType"></param>
        /// <returns></returns>
        IList<BatchBillInfoForOutBound> GetBatchBillInfoForOutBoundSendMail(IList<string> formCodeList, Enums.SortCenterOperateType outboundType);

        /// <summary>
        /// 根据箱号获取包装箱信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        ViewOutBoundByBoxModel GetBoxInfoByBoxNo(string boxNo);

        /// <summary>
        /// 设置箱表出库状态
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <param name="isOutBounded">是否已出库</param>
        /// <returns></returns>
        int SetBoxOutBoundStatus(string boxNo, bool isOutBounded);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound.SMS;

namespace Vancl.TMS.IBLL.Sorting.Outbound
{
    /// <summary>
    /// 出库业务接口
    /// </summary>
    public interface IOutboundBLL : ISortCenterBLL
    {

        /// <summary>
        /// 取得出库前置条件
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        OutboundPreConditionModel GetPreCondition(String DistributionCode);

        /// <summary>
        /// 逐单出库
        /// </summary>
        /// <param name="argument">逐单出库对象</param>
        /// <returns></returns>
        ViewOutboundSimpleModel SimpleOutbound(OutboundSimpleArgModel argument);

        /// <summary>
        /// 查询需要出库的运单信息
        /// </summary>
        /// <param name="argument">查询出库条件对象</param>
        /// <returns></returns>
        ViewOutboundSearchListModel GetNeededOutboundInfo(OutboundSearchArgModel argument);

        /// <summary>
        /// 批量出库
        /// </summary>
        /// <param name="argument">批量出库参数对象</param>
        /// <returns></returns>
        ViewOutboundBatchModel BatchOutbound(OutboundBatchArgModel argument);
        
        /// <summary>
        /// 查询出库 
        /// </summary>
        /// <param name="argument">查询出库参数对象</param>
        /// <returns></returns>
        ViewOutboundBatchModel SearchOutbound(OutboundSearchArgModel argument);

        /// <summary>
        /// 按箱出库查询
        /// </summary>
        /// <param name="argument">查询参数</param>
        /// <returns></returns>
        List<ViewOutBoundByBoxModel> SearchOutBoundByBox(OutboundSearchModel argument);

        /// <summary>
        /// 分拣按箱出库
        /// </summary>
        /// <param name="argument">分拣按箱出库参数对象</param>
        /// <returns></returns>
        ViewOutboundPackingModel PackingOutbound(OutboundPackingArgModel argument);

        /// <summary>
        /// 出库打印
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
                                            , int expressID, string batchNo, string waybillNo);

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
        /// 获取批次打印明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        IList<PrintBatchDetailModel> GetPrintBatchDetail(string batchNo);

        /// <summary>
        /// 取得出库短信配置
        /// </summary>
        /// <returns></returns>
        OutboundSMSConfigModel GetSMSConfig();

        /// <summary>
        /// 出库打印批量发送邮件
        /// </summary>
        /// <param name="expressCompanyId"></param>
        /// <param name="batchNoList"></param>
        /// <param name="emailList"></param>
        /// <returns></returns>
        ResultModel OutBoundSendEmail(int expressCompanyId,IEnumerable<string> batchNoList,IEnumerable<string> emailList);
        /// <summary>
        /// 查询返货目的地
        /// </summary>
        /// <returns></returns>
        string GetReturnTo(string formCode,int arrivalID);

        /// <summary>
        /// 根据箱号获取订单明细
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        ViewOutBoundBoxDetailModel GetBoxBillsByBoxNo(string boxNo);

        /// <summary>
        /// 根据箱号获取包装箱信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        ViewOutBoundByBoxModel GetBoxInfoByBoxNo(string boxNo);

        /// <summary>
        /// 按箱出库 
        /// </summary>
        /// <param name="argument">按箱出库参数对象</param>
        /// <returns></returns>
        ResultModel OutboundByBox(OutboundByBoxArgModel argument);

        /// <summary>
        /// 设置箱表出库状态
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <param name="isOutBounded">是否已出库</param>
        /// <returns></returns>
        int SetBoxOutBoundStatus(string boxNo, bool isOutBounded);
    }
}

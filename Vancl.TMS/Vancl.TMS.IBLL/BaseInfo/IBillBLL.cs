using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.IBLL.BaseInfo
{
    /// <summary>
    /// 表单业务接口
    /// </summary>
    public interface IBillBLL
    {

        /// <summary>
        /// 取得单号的状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns>运单不存在，返回null，否者返回本身的状态</returns>
        Enums.BillStatus? GetBillStatus(String FormCode);

        /// <summary>
        /// 取得单号的逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        Enums.ReturnStatus? GetBillReturnStatus(String FormCode);

        /// <summary>
        /// 更新单号状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        int UpdateBillStatus(String FormCode, Enums.BillStatus status);

        /// <summary>
        /// 更新单号逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        int UpdateBillReturnStatus(String FormCode, Enums.ReturnStatus status);

        /// <summary>
        /// LMS系统同步到TMS系统，入库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_InboundUpdateBill(BillModel billmodel);

        /// <summary>
        /// LMS系统同步到TMS系统，运单装车更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_BillLoadingUpdateBill(BillModel billmodel);

        /// <summary>
        /// LMS系统同步到TMS系统，出库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_OutboundUpdateBill(BillModel billmodel);

        /// <summary>
        /// LMS系统同步到TMS系统，分配站点更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_AssignStationUpdateBill(BillModel billmodel);

        /// <summary>
        /// 取得LMS同步到TMS时用于比较的运单信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        T GetBillForComparing<T>(string formCode) where T : IBillLms2TmsForComparing, new();

        /// <summary>
        /// LMS系统同步到TMS系统，转站申请更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_TurnStationApplyUpdateBill(BillModel billmodel);

        /// <summary>
        /// 取得商家单号对应关系
        /// </summary>
        /// <param name="formType">单号类型</param>
        /// <param name="code">单号</param>
        /// <returns></returns>
        List<MerchantFormCodeRelationModel> GetMerchantFormCodeRelation(Enums.SortCenterFormType formType, string code, int? merchantId=null);

        /// <summary>
        /// 根据客户订单号取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <returns></returns>
        List<String> GetFormCodeByCustomerOrder(String CustomerOrder);

        /// <summary>
        /// 根据客户订单号和商家取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <param name="MerchantID">商家ID</param>
        /// <returns></returns>
        List<String> GetFormCodeByCustomerOrder(String CustomerOrder,int MerchantID);
        /// <summary>
        /// 取得入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModel(String FormCode);

        /// <summary>
        /// 取得入库单号对象【不限制站点】
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModel_NoLimitedStation(String FormCode);

        /// <summary>
        /// 转站入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        bool UpdateBillModelByTurnInbound(InboundBillModel billModel);

        /// <summary>
        /// 入库时修改主单对象【不限制站点】
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <returns></returns>
        bool UpdateBillModelByInbound_NoLimitedStation(InboundBillModel billModel);

        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        bool UpdateBillModelByInbound(InboundBillModel billModel);

        /// <summary>
        /// 取得出库单号对象【逐单出库】
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        OutboundBillModel GetOutboundBillModel(String FormCode);

	    OutboundBillModel GetOutboundBillModelV2(string FormCode);

        /// <summary>
        /// 取得出库单号对象列表【批量出库】
        /// </summary>
        /// <param name="FormCode">系统运单号列表</param>
        /// <returns></returns>
        List<OutboundBillModel> GetOutboundBillModel_BatchOutbound(List<String> FormCode);

        /// <summary>
        /// 取得出库单号对象列表【查询出库】
        /// </summary>
        /// <param name="outboundArg">出库参数对象</param>
        /// <param name="FormCode">系统运单号列表</param>
        /// <returns></returns>
        List<OutboundBillModel> GetOutboundBillModel_SearchOutbound(IOutboundArgModel outboundArg, List<String> FormCode);

        /// <summary>
        /// 取得出库单号对象列表【装箱出库】
        /// </summary>
        /// <param name="outboundArg">出库参数对象</param>
        /// <param name="FormCode">系统运单号列表</param>
        /// <returns></returns>
        List<OutboundBillModel> GetOutboundBillModel_PackingOutbound(IOutboundArgModel outboundArg, List<String> FormCode);

	    List<OutboundBillModel> GetOutboundBillModel_PackingOutboundV2(IOutboundArgModel outboundArg, List<string> FormCode);
        /// <summary>
        /// 出库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        bool UpdateBillModelByOutbound(OutboundBillModel billModel);

	    bool UpdateBillModelByOutboundV2(OutboundBillModel billModel);

        /// <summary>
        /// 批量出库时修改主单对象
        /// </summary>
        /// <param name="listBillModel">需要修改为的对象列表</param>
        void BatchUpdateBillModelByOutbound(List<OutboundBillModel> listBillModel);

	    void BatchUpdateBillModelByOutboundV2(List<OutboundBillModel> listBillModel);

        /// <summary>
        /// 队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModel_ByQueueHandled(String FormCode);

        /// <summary>
        /// 短信队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModel_BySMSQueueHandled(String FormCode);

        /// <summary>
        /// 取得转站入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModel_TurnStation(String FormCode);


        /// <summary>
        /// 根据单号获取运单实体Model
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        BillModel GetBillByFormCode(string formCode);

        /// <summary>
        /// 运单配送查询
        /// </summary>
        /// <param name="formCode">运单号</param>
        /// <returns></returns>
        ResultModel QueryBillDeliveryModel(string formCode, out BillDeliveryModel deliveryModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModelV2(string FormCode);

        /// <summary>
        /// 获得运单明细
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillInfoModel GetBillInfoModel(string FormCode);
    }
}

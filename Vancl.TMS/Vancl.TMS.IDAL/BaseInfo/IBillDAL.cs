using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Return;

namespace Vancl.TMS.IDAL.BaseInfo
{
    /// <summary>
    /// 单号数据层接口
    /// </summary>
    public interface IBillDAL
    {

        /// <summary>
        /// 批量出库时修改主单对象
        /// </summary>
        /// <param name="listBillModel">需要修改为的对象列表</param>
        void BatchUpdateBillModelByOutbound(List<OutboundBillModel> listBillModel);

	    void BatchUpdateBillModelByOutboundV2(List<OutboundBillModel> listBillModel);

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
        /// LMS系统同步到TMS系统，出库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_OutboundUpdateBill(BillModel billmodel);

        /// <summary>
        /// LMS系统同步到TMS系统，运单装车更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_BillLoadingUpdateBill(BillModel billmodel);

        /// <summary>
        /// LMS系统同步到TMS系统，入库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        int Lms2Tms_InboundUpdateBill(BillModel billmodel);

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
        List<MerchantFormCodeRelationModel> GetMerchantFormCodeRelation(Enums.SortCenterFormType formType, string code, int? merchantId);

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
        List<String> GetFormCodeByCustomerOrder(String CustomerOrder, int MerchantID);

        /// <summary>
        /// 出库时修改主单对象
        /// </summary>
        /// <param name="billModel"></param>
        /// <returns></returns>
        int UpdateBillModelByOutbound(OutboundBillModel billModel);

	    int UpdateBillModelByOutboundV2(OutboundBillModel billModel);
        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns></returns>
        int UpdateBillModelByInbound(InboundBillModel billModel);

        /// <summary>
        /// 入库时修改主单对象【不限制站点】
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <returns></returns>
        int UpdateBillModelByInbound_NoLimitedStation(InboundBillModel billModel);

        /// <summary>
        /// 转站入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        int UpdateBillModelByTurnInbound(InboundBillModel billModel);

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
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillModel GetBillByFormCode(string FormCode);

        /// <summary>
        /// 根据运单号获取商家订单号
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns>
        /// 不存在运单号，返回null
        /// 否则返回 订单号
        /// </returns>
        string GetCustomerByFormCode(string FormCode);

        /// <summary>
        /// 取得TMS分拣同步到LMS物流主库所需要的运单信息
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        BillModel GetBillModel4TmsSync2Lms(String FormCode);

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

	    List<OutboundBillModel> GetOutboundBillModel_PackingOutboundV2(IOutboundArgModel outboundArg,List<string> FormCode);
        /// <summary>
        /// 订单装车修改内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateWaybillByTruck(BillModel model);

        /// <summary>
        /// 查询配送信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="receiveArea"></param>
        /// <returns></returns>
        BillDeliveryModel QueryDeliveryInfo(string formCode, string receiveArea);
        /// <summary>
        /// 退库单入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool BillReturnInBound(BillModel model);
        string GetFormCodeLists(MerchantReturnSearchModel searchModel);
        /// <summary>
        /// 商家入库确认时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns></returns>
        int UpdateBillModelMerchantInBound(BillModel billModel);

        /// <summary>
        /// 入库实体
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        InboundBillModel GetInboundBillModelV2(string FormCode);

        /// <summary>
        /// 修改目的配送商
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="assDistributionCode"></param>
        /// <returns></returns>
        bool UpdateAssignDistribution(string formCode, string assDistributionCode);

        /// <summary>
        /// 修改配送站点
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <param name="deliverStationId"></param>
        /// <returns></returns>
        bool UpdateDeliverStation(string waybillNo, int deliverStationId);

        /// <summary>
        /// 更新返货状态
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="returnStatus"></param>
        /// <returns></returns>
        bool UpdateReturnStatus(string formCode, int returnStatus);
    }
}

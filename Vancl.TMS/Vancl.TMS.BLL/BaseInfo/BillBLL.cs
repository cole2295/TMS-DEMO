using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.BaseInfo
{
    /// <summary>
    /// 表单业务逻辑
    /// </summary>
    public class BillBLL : BaseBLL, IBillBLL
    {
        IBillDAL billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");
        IExpressCompanyBLL expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>();
        IBillInfoDAL BillInfoDal = ServiceFactory.GetService<IBillInfoDAL>("BillInfoDAL");
        #region IBillBLL 成员


        /// <summary>
        /// 取得单号的逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public Enums.ReturnStatus? GetBillReturnStatus(String FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetBillReturnStatus(FormCode);
        }

        /// <summary>
        /// 取得单号的状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns>运单不存在，返回null，否者返回本身的状态</returns>
        public Enums.BillStatus? GetBillStatus(String FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetBillStatus(FormCode);
        }

        /// <summary>
        /// 更新单号状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        public int UpdateBillStatus(string FormCode, Enums.BillStatus status)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.UpdateBillStatus(FormCode, status);
        }

        /// <summary>
        /// 更新单号逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        public int UpdateBillReturnStatus(String FormCode, Enums.ReturnStatus status)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.UpdateBillReturnStatus(FormCode, status);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，运单装车更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_BillLoadingUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            if (String.IsNullOrWhiteSpace(billmodel.CurrentDistributionCode)) throw new ArgumentNullException("billmodel.CurrentDistributionCode is null or empty");
            return billDAL.Lms2Tms_BillLoadingUpdateBill(billmodel);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，出库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_OutboundUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            if (String.IsNullOrWhiteSpace(billmodel.CurrentDistributionCode)) throw new ArgumentNullException("billmodel.CurrentDistributionCode is null or empty");
            return billDAL.Lms2Tms_OutboundUpdateBill(billmodel);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，分配站点更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_AssignStationUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            if (billmodel.DeliverStationID <= 0) throw new ArgumentNullException("billmodel.DeliverStationID == 0");
            return billDAL.Lms2Tms_AssignStationUpdateBill(billmodel);
        }

        /// <summary>
        /// 取得LMS同步到TMS时用于比较的运单信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public T GetBillForComparing<T>(string formCode) where T : IBillLms2TmsForComparing, new()
        {
            if (String.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetBillForComparing<T>(formCode);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，转站申请更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_TurnStationApplyUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            if (string.IsNullOrWhiteSpace(billmodel.TurnstationKey)) throw new ArgumentNullException("billmodel.TurnstationKey is null or empty.");
            return billDAL.Lms2Tms_TurnStationApplyUpdateBill(billmodel);
        }

        /// <summary>
        /// 取得商家单号对应关系
        /// </summary>
        /// <param name="formType">单号类型</param>
        /// <param name="code">单号</param>
        /// <returns></returns>
        public List<MerchantFormCodeRelationModel> GetMerchantFormCodeRelation(Enums.SortCenterFormType formType, string code, int? merchantId)
        {
           // if (String.IsNullOrWhiteSpace(code)) throw new ArgumentNullException("code is null or empty.");
            var list = billDAL.GetMerchantFormCodeRelation(formType, code, merchantId);
            if (list != null && list.Count > 0)
            {
                list.ForEach(x => { x.StatusName = EnumHelper.GetDescription(x.Status); });
            }
            return list;
        }

        /// <summary>
        /// 根据客户订单号取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <returns></returns>
        public List<string> GetFormCodeByCustomerOrder(string CustomerOrder)
        {
            if (String.IsNullOrWhiteSpace(CustomerOrder)) throw new ArgumentNullException("CustomerOrder is null or empty.");
            return billDAL.GetFormCodeByCustomerOrder(CustomerOrder);
        }


        /// <summary>
        /// 根据客户订单号和商家取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <param name="MerchantID">商家ID</param>
        /// <returns></returns>
        public List<string> GetFormCodeByCustomerOrder(string CustomerOrder, int MerchantID)
        {
            if (String.IsNullOrWhiteSpace(CustomerOrder)) throw new ArgumentNullException("CustomerOrder is null or empty.");
            return billDAL.GetFormCodeByCustomerOrder(CustomerOrder);
        }
        #endregion

        #region IBillBLL 成员

        /// <summary>
        /// 取得入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetInboundBillModel(FormCode);
        }

        /// <summary>
        /// 取得入库单号对象【不限制站点】
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_NoLimitedStation(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetInboundBillModel_NoLimitedStation(FormCode);
        }

        /// <summary>
        /// 队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_ByQueueHandled(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetInboundBillModel_ByQueueHandled(FormCode);
        }

        /// <summary>
        /// 短信队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_BySMSQueueHandled(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            var inboundBillModel = billDAL.GetInboundBillModel_BySMSQueueHandled(FormCode);
            if (inboundBillModel != null)
            {
                using (BillServiceClient proxy = new BillServiceClient())
                {
                    var takesendinfo = proxy.GetWaybillTakeSendInfo(long.Parse(FormCode));
                    if (takesendinfo == null)
                    {
                        throw new Exception("调用CloudForTMSServiceClient.GetWaybillTakeSendInfo异常，返回对象为null");
                    }
                    inboundBillModel.ReceiveArea = takesendinfo.ReceiveArea;
                    inboundBillModel.ReceiveMobile = takesendinfo.ReceiveMobile;
                }
            }
            return inboundBillModel;
        }

        /// <summary>
        /// 取得转站入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_TurnStation(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetInboundBillModel_TurnStation(FormCode);
        }

        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateBillModelByInbound(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            return billDAL.UpdateBillModelByInbound(billModel) > 0;
        }


        /// <summary>
        /// 入库时修改主单对象【不限制站点】
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <returns></returns>
        public bool UpdateBillModelByInbound_NoLimitedStation(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            return billDAL.UpdateBillModelByInbound_NoLimitedStation(billModel) > 0;
        }

        #endregion


        public BillModel GetBillByFormCode(string formCode)
        {
            if (string.IsNullOrWhiteSpace(formCode)) throw new ArgumentException("单号为空");
            return billDAL.GetBillByFormCode(formCode);
        }


        #region IBillBLL 成员

        /// <summary>
        /// LMS系统同步到TMS系统，入库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_InboundUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            return billDAL.Lms2Tms_InboundUpdateBill(billmodel);
        }

        #endregion

        #region IBillBLL 成员


        public bool UpdateBillModelByOutbound(OutboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("OutboundBillModel is null");
            return billDAL.UpdateBillModelByOutbound(billModel) > 0;
        }

		public bool UpdateBillModelByOutboundV2(OutboundBillModel billModel)
		{
			if (billModel == null) throw new ArgumentNullException("OutboundBillModel is null");
			return billDAL.UpdateBillModelByOutboundV2(billModel) > 0;
		}

        /// <summary>
        /// 批量出库时修改主单对象
        /// </summary>
        /// <param name="listBillModel">需要修改为的对象列表</param>
        public void BatchUpdateBillModelByOutbound(List<OutboundBillModel> listBillModel)
        {
            if (listBillModel == null) throw new ArgumentNullException("listBillModel is null");
            billDAL.BatchUpdateBillModelByOutbound(listBillModel);
        }

		public void BatchUpdateBillModelByOutboundV2(List<OutboundBillModel> listBillModel)
		{
			if (listBillModel == null) throw new ArgumentNullException("listBillModel is null");
			billDAL.BatchUpdateBillModelByOutboundV2(listBillModel);
		}

        #endregion

        #region IBillBLL 成员


        public Model.Sorting.Outbound.OutboundBillModel GetOutboundBillModel(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("GetOutboundBillModel.FormCode IS NULL OR EMPTY");
            return billDAL.GetOutboundBillModel(FormCode);
        }

        public List<Model.Sorting.Outbound.OutboundBillModel> GetOutboundBillModel_BatchOutbound(List<string> FormCode)
        {
            throw new NotImplementedException();
        }

		public Model.Sorting.Outbound.OutboundBillModel GetOutboundBillModelV2(string FormCode)
		{
			if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("GetOutboundBillModel.FormCode IS NULL OR EMPTY");
			return billDAL.GetOutboundBillModelV2(FormCode);
		}

        /// <summary>
        /// 取得出库单号对象列表【查询出库】
        /// </summary>
        /// <param name="outboundArg">出库参数对象</param>
        /// <param name="FormCode">系统运单号列表</param>
        /// <returns></returns>
        public List<OutboundBillModel> GetOutboundBillModel_SearchOutbound(IOutboundArgModel outboundArg, List<string> FormCode)
        {
            if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
            if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
            return billDAL.GetOutboundBillModel_SearchOutbound(outboundArg, FormCode);
        }

        public List<OutboundBillModel> GetOutboundBillModel_PackingOutbound(IOutboundArgModel outboundArg, List<string> FormCode)
        {
			if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
			if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
			return billDAL.GetOutboundBillModel_PackingOutbound(outboundArg, FormCode);
        }

	    public List<OutboundBillModel> GetOutboundBillModel_PackingOutboundV2(IOutboundArgModel outboundArg, List<string> FormCode)
	    {
		    if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
		    if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
		    return billDAL.GetOutboundBillModel_PackingOutboundV2(outboundArg, FormCode);
	    }

	    #endregion

        #region IBillBLL 成员

        /// <summary>
        /// 转站入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        public bool UpdateBillModelByTurnInbound(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            return billDAL.UpdateBillModelByTurnInbound(billModel) > 0;
        }

        #endregion


        public ResultModel QueryBillDeliveryModel(string formCode, out BillDeliveryModel deliveryModel)
        {
            // if (string.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException();
            deliveryModel = new BillDeliveryModel { FormCode = formCode };
            long waybillNo;
            if (!long.TryParse(formCode, out waybillNo))
            {
                return ResultModel.Create(false, "输入的运单号格式不正确");
            }
            var billModel = billDAL.GetBillByFormCode(formCode);
            if (billModel == null)
            {
                return ResultModel.Create(false, "未查询到运单的运单信息！");
            }

            var receiveArea = String.Empty;
            using (var client = new CloudBillService.BillServiceClient())
            {
                var tacksendInfo = client.GetWaybillTakeSendInfo(waybillNo);
                if (tacksendInfo == null)
                    return ResultModel.Create(false, "未查询到运单的收发信息！");
                deliveryModel.CityName = tacksendInfo.ReceiveCity;
                receiveArea = tacksendInfo.ReceiveArea;
            }

            var deliveryInfo = billDAL.QueryDeliveryInfo(formCode, receiveArea);
            if (deliveryInfo != null)
            {
                deliveryModel.StationNum = deliveryInfo.StationNum;
                deliveryModel.StationName = deliveryInfo.StationName;
                deliveryModel.CityCode = deliveryInfo.CityCode;
                deliveryModel.CompanyFlag = deliveryInfo.CompanyFlag;
            }
            return ResultModel.Create(true);
        }


        public InboundBillModel GetInboundBillModelV2(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            return billDAL.GetInboundBillModelV2(FormCode);
        }

        public BillInfoModel GetBillInfoModel(string FormCode)
        {
           return BillInfoDal.GetBillInfoByFormCode(FormCode);
        }
    }
}

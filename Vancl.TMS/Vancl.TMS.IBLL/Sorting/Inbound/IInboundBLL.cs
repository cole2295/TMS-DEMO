using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.TurnStation;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea;
using Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant;

namespace Vancl.TMS.IBLL.Sorting.Inbound
{
    /// <summary>
    /// 入库业务接口
    /// </summary>
    public interface IInboundBLL : ISortCenterBLL
    {
        /// <summary>
        /// 入库前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        InboundPreConditionModel GetPreCondition(String DistributionCode);

        /// <summary>
        /// 逐单入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        ViewInboundSimpleModel SimpleInbound(InboundSimpleArgModel argument);

        /// <summary>
        /// 逐单入库【不限制站点】
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        ViewInboundSimpleModel SimpleInbound_NoLimitedStation(InboundSimpleArgModel argument);

        /// <summary>
        /// 批量入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>批量入库 View Model</returns>
        ViewInboundBatchModel BatchInbound(InboundBatchArgModel argument);

        /// <summary>
        /// 逐单转站入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>转站入库View Model</returns>
        ViewTurnInboundSimpleModel TurnInbound(InboundSimpleArgModel argument);

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>当前操作入库数量</returns>
        int GetInboundCount(ISortCenterArgModel argument);

        /// <summary>
        /// 入库队列处理
        /// </summary>
        /// <param name="argument"></param>
        void HandleInboundQueue(InboundQueueArgModel argument);

        /// <summary>
        /// 入库短信队列处理
        /// </summary>
        /// <param name="argument"></param>
        void HandleInboundSMSQueue(InboundSMSQueueArgModel argument);

        /// <summary>
        /// 取得时间点线路区域短信配置
        /// </summary>
        /// <returns></returns>
        LineAreaSMSConfigModel GetLineAreaSMSConfig();

        /// <summary>
        /// 取得商家短信配置
        /// </summary>
        /// <returns></returns>
        MerchantSMSConfigModel GetMerchantSMSConfig();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        ViewInboundSimpleModel SimpleInboundNew(InboundSimpleArgModel argument);
        /// <summary>
        /// 取得当前操作入库数量(新)
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        int GetInboundCountNew(ISortCenterArgModel argument);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        int GetFormCodeInboundCount(string FormCode);
        /// <summary>
        /// 获得订单详细信息
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillInfoModel GetBillInfoByFormCode(string FormCode);
        /// <summary>
        /// 入库队列处理V2
        /// </summary>
        void HandleInboundQueueV2(InboundQueueArgModel argument);
    }
}

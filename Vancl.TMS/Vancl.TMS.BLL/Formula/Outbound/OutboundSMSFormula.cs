using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Sorting.Outbound.SMS;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.Outbound;

namespace Vancl.TMS.BLL.Formula.Outbound
{
    /// <summary>
    /// 出库短信算法
    /// </summary>
    public class OutboundSMSFormula : IFormula<OutboundSMSGetContentResult, OutboundSMSContext>
    {
        /// <summary>
        /// 出库数据层
        /// </summary>
        IOutboundDAL outboundDAL = ServiceFactory.GetService<IOutboundDAL>("SC_OutboundDAL");

        #region IFormula<OutboundSMSGetContentResult,OutboundSMSContext> 成员

        public OutboundSMSGetContentResult Execute(OutboundSMSContext context)
        {
            if (context == null) throw new ArgumentNullException("OutboundSMSContext is null.");
            if (context.BillModel == null) throw new ArgumentNullException("OutboundSMSContext.BillModel is null.");
            if (context.SMSConfig == null) throw new ArgumentNullException("OutboundSMSContext.SMSConfig is null.");
            OutboundSMSGetContentResult result = new OutboundSMSGetContentResult();
            if (context.SMSConfig.Detail == null)
            {
                return result.Failed("木有配置相关信息，默认不发送短信.") as OutboundSMSGetContentResult;
            }
            var ConfigNode = context.SMSConfig.Detail.FirstOrDefault(p =>
            {
                if (p.OpType == context.BillModel.OpType
                    && p.MerchantID == context.BillModel.MerchantID
                    && p.Source == context.BillModel.Source)
                {
                    return true;
                }
                return false;
            });
            if (ConfigNode == null)
            {
                return result.Failed("未找到相关短信配置信息，默认不发送短信.") as OutboundSMSGetContentResult;
            }
            DepartureArrivalInfo dainfo = outboundDAL.GetDepartureArrivalInfo(context.BillModel.DepartureID, context.BillModel.ArrivalID);
            if (dainfo == null)
            {
                return result.Failed("找不到出发地目的地信息，默认不发送短信.") as OutboundSMSGetContentResult;
            }
            var replaceTemp = new Dictionary<Enums.SMSTemplateReplaceLabel, String>();
            replaceTemp.Add(Enums.SMSTemplateReplaceLabel.DepartureCity, dainfo.DepartureCityName);
            replaceTemp.Add(Enums.SMSTemplateReplaceLabel.ArrivalCity, dainfo.ArrivalCityName);
            replaceTemp.Add(Enums.SMSTemplateReplaceLabel.ArrivalDept, dainfo.ArrivalDeptName);
            String content = ConfigNode.Template;
            foreach (var item in replaceTemp)
            {
                content = content.Replace(EnumHelper.GetDescription<Enums.SMSTemplateReplaceLabel>(item.Key), item.Value);
            }
            result.Content = content;
            return result.Succeed() as OutboundSMSGetContentResult;
        }

        #endregion
    }
}

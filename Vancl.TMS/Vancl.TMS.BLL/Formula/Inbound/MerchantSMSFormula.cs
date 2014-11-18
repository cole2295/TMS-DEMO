using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant;

namespace Vancl.TMS.BLL.Formula.Inbound
{
    /// <summary>
    /// 商家短信算法
    /// </summary>
    public class MerchantSMSFormula : IFormula<InboundSMSGetContentResult, InboundSMSContext>
    {
        #region IFormula<InboundSMSGetContentResult,InboundSMSContext> 成员

        /// <summary>
        /// 构建短信内容
        /// </summary>
        /// <param name="template"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string BuildSMSContent(string template, InboundSMSBillModel condition)
        {
            string content = template;
            content = content.Replace("[FormCode]", condition.FormCode);
            return content;
        }

        public InboundSMSGetContentResult Execute(InboundSMSContext context)
        {
            if (context == null) throw new ArgumentNullException("context is null.");
            if (context.BillModel == null) throw new ArgumentNullException("context.BillModel  is null.");
            if (context.SMSConfig == null) throw new ArgumentNullException("context.SMSConfig is null.");
            if (!(context.SMSConfig is MerchantSMSConfigModel)) throw new ArgumentException("context.SMSConfig must the type of MerchantSMSConfigModel");
            var Config = context.SMSConfig as MerchantSMSConfigModel;
            InboundSMSGetContentResult result = new InboundSMSGetContentResult();
            if (!Config.IsEnabled)
            {
                return result.Failed("商家短信规则为停用状态，不发短信") as InboundSMSGetContentResult;
            }
            if (Config.EffectiveTime.HasValue)
            {
                if (DateTime.Now < Config.EffectiveTime.Value)
                {
                    return result.Failed("商家短信规则没有生效，不发短信") as InboundSMSGetContentResult;
                }
            }
            if (Config.DeadLine.HasValue)
            {
                if (DateTime.Now > Config.DeadLine.Value)
                {
                    return result.Failed("商家短信规则已截止，不发短信") as InboundSMSGetContentResult;
                }
            }
            if (Config.Detail == null)
            {
                return result.Failed("木有配置项，默认不发送短信") as InboundSMSGetContentResult;
            }
            var ConfigNode = Config.Detail.FirstOrDefault(p => p.MerchantID == context.BillModel.MerchantID);
            if (ConfigNode == null)
            {
                return result.Failed("当前商家不包含在配置中，不发送短信") as InboundSMSGetContentResult;
            }
            if (ConfigNode.Source != context.BillModel.Source)
            {
                return result.Failed("当前运单来源同配置的不相同，不发送短信") as InboundSMSGetContentResult;
            }
            if (ConfigNode.IsValidateFirstInbound)
            {
                if (!context.BillModel.IsFirstInbound)
                {
                    return result.Failed("当前运单不是第一次入库，不发送短信") as InboundSMSGetContentResult;
                }
            }
            foreach (var item in ConfigNode.ListTimeConfig)
            {
                DateTime StartTime = DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddHours(item.StartTime.Hour).AddMinutes(item.StartTime.Minute);
                DateTime EndTime = DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddHours(item.EndTime.Hour).AddMinutes(item.EndTime.Minute);
                if (context.BillModel.InboundTime <= EndTime)
                {
                    if (context.BillModel.InboundTime > StartTime)
                    {
                        if (item.IsSend)
                        {
                            result.SendTime = item.ImmSend ? DateTime.Now : DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddDays(item.DelayDay.Value).AddHours(item.SendTime.Hour).AddMinutes(item.SendTime.Minute);
                            result.Content = BuildSMSContent(ConfigNode.SMSTemplate, context.BillModel);
                            return result.Succeed() as InboundSMSGetContentResult;
                        }
                        return result.Failed("该时间点配置为不发送短信") as InboundSMSGetContentResult;
                    }
                }
            }

            return result.Failed("未配置商家相关配置,默认不发送短信") as InboundSMSGetContentResult;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Formula.Inbound
{
    /// <summary>
    /// 入库线路区域短信算法
    /// </summary>
    public class LineAreaSMSFormula : IFormula<InboundSMSGetContentResult, InboundSMSContext>
    {
        #region IFormula<InboundSMSGetContentResult,InboundSMSContext> 成员

        /// <summary>
        /// 构建短信内容
        /// </summary>
        /// <param name="template"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string BuildSMSContent(String template, InboundSMSBillModel condition)
        {
            string content = template;
            content = content.Replace("[ContacterPhone]", condition.DeliverStationContacterPhone ?? "");
            content = content.Replace("[SortCenterName]", condition.SortCenterName ?? "");
            if (condition.Source == Enums.BillSource.VANCL)
            {
                content = content.Replace("[Source]", "凡客");
                return content;
            }
            if (condition.Source == Enums.BillSource.VJIA)
            {
                content = content.Replace("[Source]", "V+");
                return content;
            }
            return content;
        }

        public InboundSMSGetContentResult Execute(InboundSMSContext context)
        {
            if (context == null) throw new ArgumentNullException("context is null.");
            if (context.BillModel == null) throw new ArgumentNullException("context.BillModel  is null.");
            if (context.SMSConfig == null) throw new ArgumentNullException("context.SMSConfig is null.");
            if (!(context.SMSConfig is LineAreaSMSConfigModel)) throw new ArgumentException("context.SMSConfig must the type of LineAreaSMSConfigModel");
            var Config = context.SMSConfig as LineAreaSMSConfigModel;
            InboundSMSGetContentResult result = new InboundSMSGetContentResult();
            if (!Config.IsEnabled)
            {
                return result.Failed("时间区域短信规则为停用状态，不发短信") as InboundSMSGetContentResult;
            }
            if (Config.EffectiveTime.HasValue)
            {
                if (DateTime.Now < Config.EffectiveTime.Value)
                {
                    return result.Failed("时间区域短信规则没有生效，不发短信") as InboundSMSGetContentResult;
                }
            }
            if (Config.DeadLine.HasValue)
            {
                if (DateTime.Now > Config.DeadLine.Value)
                {
                    return result.Failed("时间区域短信规则已截止，不发短信") as InboundSMSGetContentResult;
                }
            }
            if (Config.Detail == null)
            {
                return result.Failed("木有配置项，默认不发送短信") as InboundSMSGetContentResult;
            }
            var ConfigNode = Config.Detail.FirstOrDefault(p => p.DepartureID == context.BillModel.DepartureID);
            if (ConfigNode == null)
            {
                return result.Failed("运单的当前操作分拣中心未在配置项中，默认不发送短信") as InboundSMSGetContentResult;
            }
            if (!ConfigNode.ListArrivalID.Contains(context.BillModel.ArrivalID))
            {
                return result.Failed("运单的当前入库目的地未在配置项中，默认不发送短信") as InboundSMSGetContentResult;
            }
            if (!ConfigNode.ListSource.Contains(context.BillModel.Source))
            {
                return result.Failed("当前运单来源未在配置项中，默认不发送短信") as InboundSMSGetContentResult;
            }
            foreach (var item in ConfigNode.TimeRangeTemplateCfg)
            {
                DateTime StartTime = DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddHours(item.StartTime.Hour).AddMinutes(item.StartTime.Minute);
                DateTime EndTime = DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddHours(item.EndTime.Hour).AddMinutes(item.EndTime.Minute);
                if (context.BillModel.InboundTime <= EndTime)
                {
                    if (context.BillModel.InboundTime > StartTime)
                    {
                        if (item.IsSend)
                        {
                            result.SendTime = item.ImmSend ? DateTime.Now : DateTime.Parse(context.BillModel.InboundTime.ToString("yyyy-MM-dd")).AddHours(item.SendTime.Hour).AddMinutes(item.SendTime.Minute);
                            //需要验证区域，则取得区域短信模板
                            if (item.IsValidateRange)
                            {
                                foreach (var rangeitem in item.RangeCfg)
                                {
                                    if (rangeitem.ReceiveRange.Contains(context.BillModel.ReceiveArea))
                                    {
                                        result.Message = BuildSMSContent(rangeitem.SMSTemplate, context.BillModel);
                                        return result.Succeed() as InboundSMSGetContentResult;
                                    }
                                }
                                return result.Failed("未配置相关区域，找不到对应的短信模板") as InboundSMSGetContentResult;
                            }
                            result.Message = BuildSMSContent(item.SMSTemplate, context.BillModel);
                            return result.Succeed() as InboundSMSGetContentResult;
                        }

                        return result.Failed("该时间点配置为不发送短信") as InboundSMSGetContentResult;
                    }
                }
            }

            return result.Failed("未配置相关时间点或者区域,默认不发送短信") as InboundSMSGetContentResult;
        }

        #endregion
    }
}

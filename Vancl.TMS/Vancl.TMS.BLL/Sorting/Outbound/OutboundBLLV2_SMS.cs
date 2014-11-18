using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.SMS;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound.SMS;

namespace Vancl.TMS.BLL.Sorting.Outbound
{
	public partial class OutboundBLLV2
	{
		/// <summary>
		/// 出库短信算法
		/// </summary>
		IFormula<OutboundSMSGetContentResult, OutboundSMSContext> smsformula = FormulasFactory.GetFormula<IFormula<OutboundSMSGetContentResult, OutboundSMSContext>>("OutboundSMSFormula");

		/// <summary>
		/// 出库短信验证
		/// </summary>
		/// <param name="billModel">运单对象</param>
		/// <param name="opType">分拣操作类型</param>
		/// <param name="config">短信配置项</param>
		/// <returns></returns>
		private OutboundSMSGetContentResult SMSValidate(OutboundBillModel billModel, OutboundSMSConfigModel config)
		{
			var context = new OutboundSMSContext()
			{
				BillModel = new OutboundSMSBillModel()
				{
					FormCode = billModel.FormCode,
					MerchantID = billModel.MerchantID,
					OpType = billModel.InboundType,
					Source = billModel.Source,
					DepartureID = billModel.DepartureID,
					ArrivalID = billModel.ArrivalID
				},
				SMSConfig = config
			};
			return smsformula.Execute(context);
		}

		/// <summary>
		/// 出库短信发送
		/// </summary>
		/// <param name="billModel"></param>
		/// <param name="Content"></param>
		private void OutboundSMSSend(OutboundBillModel billModel, String Content)
		{
			if (billModel == null || String.IsNullOrWhiteSpace(Content))
			{
				return;
			}
			String ReceiveMobile = null;
			using (BillServiceClient proxy = new BillServiceClient())
			{
				var takesendinfo = proxy.GetWaybillTakeSendInfo(long.Parse(billModel.FormCode));
				if (takesendinfo == null)
				{
					throw new Exception("调用CloudForTMSServiceClient.GetWaybillTakeSendInfo异常，返回对象为null");
				}
				ReceiveMobile = takesendinfo.ReceiveMobile;
			}
			if (!String.IsNullOrWhiteSpace(ReceiveMobile))
			{
				var result = SMSSender.Send(new SMSMessage()
				{
					Content = Content,
					FormCode = billModel.FormCode,
					PhoneNumber = ReceiveMobile,
					Title = @"拣运系统出库短信"
				});
			}
		}

		/// <summary>
		/// 出库短信发送
		/// </summary>
		/// <param name="dicbillInfo"></param>
		private void OutboundSMSSend(Dictionary<OutboundBillModel, String> dicbillInfo)
		{
			if (dicbillInfo == null || dicbillInfo.Count <= 0)
			{
				return;
			}
			foreach (var item in dicbillInfo)
			{
				String ReceiveMobile = null;
				if (item.Key == null || String.IsNullOrWhiteSpace(item.Value))
				{
					continue;
				}
				//TODO:可以改一个批量取得电话号码的接口
				using (BillServiceClient proxy = new BillServiceClient())
				{
					var takesendinfo = proxy.GetWaybillTakeSendInfo(long.Parse(item.Key.FormCode));
					if (takesendinfo == null)
					{
						throw new Exception("调用CloudForTMSServiceClient.GetWaybillTakeSendInfo异常，返回对象为null");
					}
					ReceiveMobile = takesendinfo.ReceiveMobile;
				}
				if (!String.IsNullOrWhiteSpace(ReceiveMobile))
				{
					var result = SMSSender.Send(new SMSMessage()
					{
						Content = item.Value,
						FormCode = item.Key.FormCode,
						PhoneNumber = ReceiveMobile,
						Title = @"拣运系统出库短信"
					});
				}
			}
		}



		/// <summary>
		/// 取得出库短信配置
		/// </summary>
		/// <returns></returns>
		public OutboundSMSConfigModel GetSMSConfig()
		{
			//TODO:后续用DB方式配置
			OutboundSMSConfigModel config = new OutboundSMSConfigModel();
			config.Detail = new List<OutboundSMSConfigModel.SortCenterOuboundDetailConfig>();
			config.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig()
			{
				MerchantID = 2,             //小米科技
				OpType = Enums.SortCenterOperateType.SecondSorting,
				Source = Enums.BillSource.Others,
				Template = "您好，您的小米订单已从[当前城市]分拣中心发往[目标城市]分拣中心，详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]"
			});
			config.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig()
			{
				MerchantID = 2,
				OpType = Enums.SortCenterOperateType.SimpleSorting,
				Source = Enums.BillSource.Others,
				Template = "您好，您的小米订单已从[当前城市]分拣中心发往[目标部门]正在中转，详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]"
			});
			config.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig()
			{
				MerchantID = 2,
				OpType = Enums.SortCenterOperateType.DistributionSorting,
				Source = Enums.BillSource.Others,
				Template = "您的小米订单已从[当前城市]分拣中心发往[目标部门]正在中转，订单详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]"
			});

			return config;
		}
	}
}

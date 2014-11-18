using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.SMS.LineArea;
using Vancl.TMS.Model.Sorting.Inbound.SMS.Merchant;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.SMS;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    /// <summary>
    /// 入库短信相关业务
    /// </summary>
    public partial class InboundBLL
    {
        /// <summary>
        /// 构建区域模板配置项
        /// </summary>
        /// <param name="DepartureID"></param>
        /// <returns></returns>
        private List<LineAreaSMSConfigDetailModel.RangeTemplateConfig> GetRangeTemplateConfig(int DepartureID)
        {
            //TODO:后续通过DB方式配置
            List<LineAreaSMSConfigDetailModel.RangeTemplateConfig> listConfig = new List<LineAreaSMSConfigDetailModel.RangeTemplateConfig>();
            switch (DepartureID)
            {
                case 2:
                    #region 北京分拣配置
                        //城八区
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，下午为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "东城区", "西城区", "宣武区", "崇文区", "海淀区", "石景山区", "丰台区", "朝阳区" }
                        });
                        //近郊区
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，下午至明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "昌平区", "顺义区", "通州区", "大兴区", "房山区" }
                        });
                        //远郊
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "延庆县", "密云县", "怀柔区", "平谷区", "门头沟区" }
                        });
                    #endregion  
                    break;
                case 9:
                    #region 上海分拣配置
                        //城区
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，下午为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "宝山区", "奉贤区", "虹口区", "黄浦区", "静安区", "卢湾区", "闵行区", "浦东新区", "普陀区", "松江区", "徐汇区", "杨浦区", "闸北区", "长宁区" }
                        });
                        //远郊
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "嘉定区", "金山区", "南汇区", "青浦区"
                                        , "白下区" ,"高淳县","鼓楼区","建邺区","江宁区","溧水县","六合区","浦口区","栖霞区","秦淮区","下关区","玄武区","雨花台区"
                                        ,"沧浪区","常熟市","高新区","工业园区","虎丘区","金阊区","昆山市","平江区","太仓市","吴江市","吴中区","相城区","张家港市"
                                    }
                        });
                    #endregion
                    break;
                case 20:
                    #region 广州分拣配置
                        //城区
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，下午为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "海珠区", "黄埔区", "荔湾区", "天河区", "越秀区" }
                        });
                        //近郊区
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，下午至明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "白云区", "番禺区" }
                        });
                        //远郊
                        listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                        {
                            SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                            ReceiveRange = new String[] { "从化市", "花都区", "萝岗区", "南沙区"
                                        , "增城市" ,"宝安区","福田区","光明新区","龙岗区","罗湖区","南山区","坪山新区","盐田区"
                                    }
                        });
                    #endregion
                    break;
                case 172:
                    #region 南京分拣配置
                    //远郊
                    listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                    {
                        SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                        ReceiveRange = new String[] { "白下区", "高淳县", "鼓楼区", "建邺区", "江宁区", "溧水县", "六合区", "浦口区", "栖霞区", "秦淮区", "下关区", "玄武区", "雨花台区" }
                    });
                    #endregion
                    break;
               case 194:
                    #region 武汉分拣配置
                    //远郊
                    listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                    {
                        SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                        ReceiveRange = new String[] { "蔡甸区", "东西湖区", "汉南区", "汉阳区", "洪山区", "黄陂区", "江岸区", "江汉区", "江夏区", "硚口区", "青山区", "武昌区", "新洲区" }
                    });
                    #endregion
                    break;
               case 197:
                    #region 西安分拣配置
                    //远郊
                    listConfig.Add(new LineAreaSMSConfigDetailModel.RangeTemplateConfig()
                    {
                        SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达。详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                        ReceiveRange = new String[] { "灞桥区", "碑林区", "高陵县", "高新区", "户县", "蓝田县", "莲湖区", "临潼区", "未央区", "新城区", "阎良区", "雁塔区", "长安区", "周至县" }
                    });
                    #endregion
                    break;
                default:
                    break;
            }
            return listConfig;
        }

        /// <summary>
        /// 构建线路区域短信配置
        /// </summary>
        /// <param name="DepartureID">出发地</param>
        /// <returns></returns>
        private LineAreaSMSConfigDetailModel CreateLineAreaSMSDetail(int DepartureID)
        {
            //TODO:未来有时间会通过DB配置的方式构建Config对象
            var detail = new LineAreaSMSConfigDetailModel() 
            {
                DepartureID = DepartureID,
                ListArrivalID = new List<int>(),
                ListSource = new List<Enums.BillSource>(),
                TimeRangeTemplateCfg = new List<LineAreaSMSConfigDetailModel.TimeRangeTemplateConfig>()
            };
            detail.ListSource.AddRange(new Enums.BillSource[] { Enums.BillSource.VANCL , Enums.BillSource.VJIA});
            if (detail.DepartureID != 172)          //南京分拣特殊权限
            {
                List<int> listStation = ExpressCompanyBLL.GetSortCenterStationList(DepartureID);
                if (listStation != null)
                {
                    detail.ListArrivalID.AddRange(listStation);
                }
            }
            else
            {
                detail.ListArrivalID.AddRange(new int[] { 598, 173, 175 });
            }
            #region 6:00~8:00的配置
            var Config6to8 = new LineAreaSMSConfigDetailModel.TimeRangeTemplateConfig()           
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 06:00:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00")),
                IsSend = true,
                ImmSend = false,
                IsValidateRange = true,     //需要根据不同的区域选择不同的模板
                SMSTemplate = "",
                SendTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"))
            };
            Config6to8.RangeCfg = GetRangeTemplateConfig(DepartureID);
            detail.TimeRangeTemplateCfg.Add(Config6to8);
            #endregion
            #region 8:00~12:20的配置
            var Config8to12 = new LineAreaSMSConfigDetailModel.TimeRangeTemplateConfig()
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 12:20:00")),
                IsSend = true,
                ImmSend = true,
                IsValidateRange = true,     //需要根据不同的区域选择不同的模板
                SMSTemplate = "",
                SendTime = DateTime.Now
            };
            Config8to12.RangeCfg = GetRangeTemplateConfig(DepartureID);
            detail.TimeRangeTemplateCfg.Add(Config8to12);
            #endregion
            #region 12:20~20:00的配置
            var Config12to20 = new LineAreaSMSConfigDetailModel.TimeRangeTemplateConfig()
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 12:20:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 20:00:00")),
                IsSend = true,
                ImmSend = true,
                IsValidateRange = false,
                SMSTemplate = "您好：您在[Source]订购的商品已到达[SortCenterName]，明日为您送达，详情致电[ContacterPhone]或登录官网查询，谢谢！如风达快递",
                SendTime = DateTime.Now
            };
            detail.TimeRangeTemplateCfg.Add(Config12to20);
            #endregion
            return detail;
        }

        /// <summary>
        /// 取得时间点线路区域短信配置
        /// </summary>
        /// <returns></returns>
        public LineAreaSMSConfigModel GetLineAreaSMSConfig()
        {
            //TODO: 后续从DB中取得数据     
            LineAreaSMSConfigModel config = new LineAreaSMSConfigModel();
            config.IsEnabled = Convert.ToBoolean(ConfigurationHelper.GetAppSetting("IsAreaEnabled"));
            if (!String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("AreaEffectiveTime")))
            {
                config.EffectiveTime = DateTime.Parse(ConfigurationHelper.GetAppSetting("AreaEffectiveTime"));
            }
            if (!String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("AreaDeadLine")))
            {
                config.DeadLine = DateTime.Parse(ConfigurationHelper.GetAppSetting("AreaDeadLine"));
            }
            config.Detail = new List<LineAreaSMSConfigDetailModel>();

            config.Detail.Add(CreateLineAreaSMSDetail(2));      //北京分拣
            config.Detail.Add(CreateLineAreaSMSDetail(9));      //上海分拣
            config.Detail.Add(CreateLineAreaSMSDetail(20));    //广州分拣
            config.Detail.Add(CreateLineAreaSMSDetail(172));   //南京分拣
            config.Detail.Add(CreateLineAreaSMSDetail(194));   //武汉分拣
            config.Detail.Add(CreateLineAreaSMSDetail(197));   //西安分拣

            return config;
        }

        /// <summary>
        /// 构建商家短信配置
        /// </summary>
        /// <param name="MerchantID"></param>
        /// <returns></returns>
        private MerchantSMSConfigDetailModel CreateMerchantSMSConfigDetail(int MerchantID)
        {
            //TODO:后续有时间都从DB中取得配置
            
            var detail = new MerchantSMSConfigDetailModel() 
            {
                IsValidateFirstInbound = true,
                Source = Enums.BillSource.Others,
                MerchantID = MerchantID,
                ListTimeConfig = new List<MerchantSMSConfigDetailModel.TimeRangeConfig>(),
                SMSTemplate = "如风达快递提醒：您的招行信用卡已在寄送途中，快递号:[FormCode]，配送情况详询www.rufengda.com"
            };
            #region 0~8:00配置
            var Config0to8 = new MerchantSMSConfigDetailModel.TimeRangeConfig() 
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00")),
                IsSend = true,
                ImmSend = false,
                DelayDay = 0,
                SendTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"))                
            };
            detail.ListTimeConfig.Add(Config0to8);
            #endregion
            #region 8:00~20:00配置
            var Config8to20 = new MerchantSMSConfigDetailModel.TimeRangeConfig()
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 20:00:00")),
                IsSend = true,
                ImmSend = true,
                SendTime = DateTime.Now
            };
            detail.ListTimeConfig.Add(Config8to20);
            #endregion
            #region 20:00~24:00配置
            var Config20to24 = new MerchantSMSConfigDetailModel.TimeRangeConfig() 
            {
                StartTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 20:00:00")),
                EndTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")),
                IsSend = true,
                ImmSend = false,
                DelayDay = 1,
                SendTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 08:00:00"))
            };
            detail.ListTimeConfig.Add(Config20to24);
            #endregion
            
            return detail;
        }

        /// <summary>
        /// 取得商家短信配置
        /// </summary>
        /// <returns></returns>
        public MerchantSMSConfigModel GetMerchantSMSConfig()
        {
            //TODO: 后续从DB中取得数据     
            MerchantSMSConfigModel config = new MerchantSMSConfigModel();
            config.IsEnabled = Convert.ToBoolean(ConfigurationHelper.GetAppSetting("IsMerchantEnabled"));
            if (!String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("MerchantEffectiveTime")))
            {
                config.EffectiveTime = DateTime.Parse(ConfigurationHelper.GetAppSetting("MerchantEffectiveTime"));
            }
            if (!String.IsNullOrWhiteSpace(ConfigurationHelper.GetAppSetting("MerchantDeadLine")))
            {
                config.DeadLine = DateTime.Parse(ConfigurationHelper.GetAppSetting("MerchantDeadLine"));
            }
            config.Detail = new List<MerchantSMSConfigDetailModel>();
            //招行信用卡
            config.Detail.Add(CreateMerchantSMSConfigDetail(-1));

            return config;
        }

        /// <summary>
        /// 短信发送check
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="billModel"></param>
        /// <returns></returns>
        protected InboundSMSGetContentResult InboundSMSCheck(InboundQueueArgModel argument, InboundBillModel billModel, SortCenterUserModel CurUser)
        {
            var defaultResult = new InboundSMSGetContentResult()
            {
                IsSuccess = false,
                Message = "木有找到相关配置，默认不发送短信"
            };
            InboundSMSGetContentResult Result = null;
            var BillModel = new InboundSMSBillModel()
               {
                   FormCode = billModel.FormCode,
                   ArrivalID = argument.QueueItem.ArrivalID,
                   DeliverStationContacterPhone = ExpressCompanyBLL.GetContacterPhone(billModel.DeliverStationID),
                   DeliverStationID = billModel.DeliverStationID,
                   DepartureID = argument.QueueItem.DepartureID,
                   InboundTime = argument.QueueItem.CreateTime,
                   IsFirstInbound = billModel.IsFirstInbound,
                   MerchantID = billModel.MerchantID,
                   ReceiveArea = billModel.ReceiveArea,
                   SortCenterName = CurUser.CompanyName,
                   Source = billModel.Source
               };
            //商家算法上下文
            var Merchantcontext = new InboundSMSContext()
            {
                BillModel = BillModel,
                SMSConfig = argument.MerchantSMSConfig
            };
            //线路区域算法上下文
            var LineAreacontext = new InboundSMSContext()
            {
                BillModel = BillModel,
                SMSConfig = argument.LineAreaSMSConfig
            };
            Result = MerchantSMSFormula.Execute(Merchantcontext);
            if (Result.IsSuccess)
            {
                return Result;
            }
            Result = LineAreaSMSFormula.Execute(LineAreacontext);
            if (Result.IsSuccess)
            {
                return Result;
            }
            //未找到,返回默认值
            return defaultResult;
        }

        /// <summary>
        /// 入库短信队列处理
        /// </summary>
        /// <param name="argument"></param>
        public void HandleInboundSMSQueue(InboundSMSQueueArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundSMSQueueArgModel is null.");
            if (argument.QueueItem == null) throw new ArgumentNullException("InboundSMSQueueArgModel.QueueItem is null.");
            try
            {
                var billModel = BillBLL.GetInboundBillModel_BySMSQueueHandled(argument.QueueItem.FormCode);
                if (billModel == null)
                {
                    InboundSMSQueueDAL.UpdateToError(argument.QueueItem.QUID, "运单对象为null");
                    return;
                }
                if (String.IsNullOrWhiteSpace(billModel.ReceiveMobile))
                {
                    InboundSMSQueueDAL.UpdateToError(argument.QueueItem.QUID, "收货人移动号码为空");
                    return;
                }
                var result = SMSSender.Send(new SMSMessage()
                {
                    Content = argument.QueueItem.SendedContent,
                    FormCode = argument.QueueItem.FormCode,
                    PhoneNumber = billModel.ReceiveMobile,
                    Title = @"拣运系统入库短信"
                });
                if (result.IsSuccess)
                {
                    InboundSMSQueueDAL.UpdateToHandled(argument.QueueItem.QUID);
                    return;
                }
                InboundSMSQueueDAL.UpdateOpCount(argument.QueueItem.QUID, result.Message);
            }
            catch (Exception ex)
            {
                InboundSMSQueueDAL.UpdateOpCount(argument.QueueItem.QUID, ex.StackTrace);
            }
        }
    }
}

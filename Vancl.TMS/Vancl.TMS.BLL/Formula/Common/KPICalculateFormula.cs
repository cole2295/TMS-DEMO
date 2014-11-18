using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.IDAL.BaseInfo.Carrier;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Formula.Common
{
    /*
   * (C)Copyright 2011-2012 TMS
   * 
   * 模块名称：承运商KPI考核
   * 说明：计算承运商KPI考核的价格
   * 作者：任 钰
   * 创建日期：2012-03-07 17:34:00
   * 修改人：
   * 修改时间：
   * 修改记录：记录以便查阅
   */
    /// <summary>
    /// 承运商KPI考核
    /// </summary>
    public class KPICalculateFormula : IFormula<KPICalcOutputModel, KPICalcInputModel>
    {
        IDispatchDAL _dispatchDAL = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        ILinePlanDAL _linePlanDAL = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");
        ILostDAL _lostDAL = ServiceFactory.GetService<ILostDAL>("LostDAL");
        ICarrierWaybillDAL _carrierWaybillDAL = ServiceFactory.GetService<ICarrierWaybillDAL>("CarrierWaybillDAL");
        IDelayCriteriaDAL _delayCriteriaDAL = ServiceFactory.GetService<IDelayCriteriaDAL>("DelayCriteriaDAL");

        /// <summary>
        /// 根据原始线路构建KPI的价格
        /// </summary>
        /// <param name="ExpressionType">线路价格类型</param>
        /// <param name="DeliveryNo">提货单号</param>
        /// <param name="LPID">线路计划主键ID</param>
        /// <returns></returns>
        private List<IAssPriceModel> BuildingAssPRiceByOrigLinePrice(Vancl.TMS.Model.Common.Enums.ExpressionType ExpressionType, string DeliveryNo, int LPID)
        {
            List<IAssPriceModel> listAssPrice = new List<IAssPriceModel>();
            switch (ExpressionType)
            {
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Fixed:
                    ILineFixedPriceDAL lineFixedPriceDAL = ServiceFactory.GetService<ILineFixedPriceDAL>("LineFixedPriceDAL");
                    LineFixedPriceModel fixedModel = lineFixedPriceDAL.GetByLinePlanID(LPID);
                    listAssPrice.Add(new AssFixedPriceModel()
                    {
                        DeliveryNo = DeliveryNo,
                        Price = fixedModel.Price
                    });
                    break;
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Ladder:
                    ILineLadderPriceDAL lineLadderPriceDAL = ServiceFactory.GetService<ILineLadderPriceDAL>("LineLadderPriceDAL");
                    IList<LineLadderPriceModel> listLadderModel = lineLadderPriceDAL.GetByLinePlanID(LPID);
                    foreach (var item in listLadderModel)
                    {
                        listAssPrice.Add(new AssLadderPriceModel()
                        {
                            DeliveryNo = DeliveryNo,
                            EndWeight = item.EndWeight,
                            Note = item.Note,
                            Price = item.Price,
                            StartWeight = item.StartWeight
                        });
                    }
                    break;
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Formula:
                    ILineFormulaPriceDAL lineFornulaPriceDAL = ServiceFactory.GetService<ILineFormulaPriceDAL>("LineFormulaPriceDAL");
                    LineFormulaPriceModel formulaModel = lineFornulaPriceDAL.GetByLinePlanID(LPID);
                    List<LineFormulaPriceDetailModel> listLineFormulaEx = lineFornulaPriceDAL.GetLineFormulaDetails(LPID);
                    AssFormulaPriceModel assformulaModel = new AssFormulaPriceModel()
                    {
                        BasePrice = formulaModel.BasePrice,
                        BaseWeight = formulaModel.BaseWeight,
                        DeliveryNo = DeliveryNo,
                        Note = formulaModel.Note,
                        OverPrice = formulaModel.OverPrice
                    };
                    if (listLineFormulaEx != null)
                    {
                        assformulaModel.Detail  = new List<AssFormulaPriceExModel>();
                        foreach (var item in listLineFormulaEx)
                        {
                            assformulaModel.Detail.Add(new AssFormulaPriceExModel() 
                            { 
                                DeliveryNo = DeliveryNo,
                                StartWeight = item.StartWeight,
                                EndWeight = item.EndWeight,
                                Price = item.Price
                            });
                        }
                    }
                    listAssPrice.Add(assformulaModel);
                    break;
                default:
                    listAssPrice = null;
                    break;
            }
            return listAssPrice;
        }

        /// <summary>
        /// 根据线路、调度系统数据为准初始化输入参数(非UI输入数据)
        /// </summary>
        /// <param name="context">输入参数Model</param>
        /// <param name="linePlanModel">线路计划Model</param>
        /// <param name="dispatchModel">调度Model</param>
        private void InitOrigInputModel(KPICalcInputModel context, LinePlanModel linePlanModel, DispatchModel dispatchModel)
        {
            IDelayDAL _delayDAL = ServiceFactory.GetService<IDelayDAL>("DelayDAL");
            context.InsuranceRate = linePlanModel.InsuranceRate;
            context.ExpressionType = linePlanModel.ExpressionType;
            context.LongDeliveryAmount = linePlanModel.LongDeliveryPrice;
            context.LongTransferRate = linePlanModel.LongTransferRate;
            context.LostDeduction = _lostDAL.GetLostDeduction(context.DeliveryNo);
            DelayModel delayModel = _delayDAL.GetDelayModel(context.DeliveryNo);
            if (delayModel != null)
            {
                context.Discount = _delayCriteriaDAL.GetDisCount(linePlanModel.CarrierID, delayModel.DelayTimeSpan);
            }
            context.AssPriceList = BuildingAssPRiceByOrigLinePrice(context.ExpressionType, context.DeliveryNo, dispatchModel.LPID);
        }

        /// <summary>
        /// 计算基准运费
        /// </summary>
        /// <param name="AssPriceList">价格Model</param>
        /// <param name="carrierWaybillModel">承运商运单Model</param>
        /// <returns></returns>
        private decimal GetBasePrice(Vancl.TMS.Model.Common.Enums.ExpressionType ExpressionType, List<IAssPriceModel> AssPriceList, CarrierWaybillModel carrierWaybillModel)
        {
            decimal basePrice = 0;
            switch (ExpressionType)
            {
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Fixed:
                    basePrice = (AssPriceList[0] as AssFixedPriceModel).Price * carrierWaybillModel.Weight;
                    break;
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Ladder:
                    basePrice = CalculateLadderPrice(carrierWaybillModel.Weight, AssPriceList);
                    break;
                case Vancl.TMS.Model.Common.Enums.ExpressionType.Formula:
                    AssFormulaPriceModel formulaPrice = AssPriceList[0] as AssFormulaPriceModel;
                    carrierWaybillModel.Weight = RefreshWeight(carrierWaybillModel.Weight);
                    if (carrierWaybillModel.Weight > formulaPrice.BaseWeight)
                    {
                        basePrice = formulaPrice.BasePrice;
                        basePrice += CalculateOverPrice(
                            carrierWaybillModel.Weight
                            , formulaPrice.BaseWeight
                            , formulaPrice.Detail);
                    }
                    else
                    {
                        basePrice = formulaPrice.BasePrice;
                    }
                    break;
                default:
                    break;
            }
            return basePrice;
        }

        /// <summary>
        /// 根据四舍五入等等规则得到计算价格的重量
        /// </summary>
        /// <param name="Weight"></param>
        /// <returns></returns>
        private decimal RefreshWeight(decimal Weight)
        {
            decimal IntWeight = decimal.Truncate(Weight);
            decimal decWeight = CarryRule(decimal.Parse(Weight.ToString("N1")) - IntWeight);
            return IntWeight + decWeight;
        }

        /// <summary>
        /// 计算阶梯价格
        /// </summary>
        /// <param name="Weight">提货单总重量</param>
        /// <param name="listLadderPrice">阶梯价格列表</param>
        /// <returns></returns>
        private decimal CalculateLadderPrice(decimal Weight, List<IAssPriceModel> listLadderPrice)
        {
            if (listLadderPrice == null) throw new ArgumentNullException("AssLadderPriceModel is null");
            decimal ladderPrice = 0;
            foreach (AssLadderPriceModel item in listLadderPrice)
            {
                if (Weight > item.StartWeight)
                {
                    if (!item.EndWeight.HasValue)
                    {
                        ladderPrice = item.Price * Weight;
                        break;
                    }
                    else if (Weight <= item.EndWeight.Value)
                    {
                        ladderPrice = item.Price * Weight;
                        break;
                    }
                }
            }
            return ladderPrice;
        }

        /// <summary>
        /// 计算公式价格续价
        /// </summary>
        /// <param name="Weight">提货单总重量</param>
        /// <param name="BaseWeight">提货单计价基重量</param>
        /// <param name="listOverPrice">公式续价列表</param>
        /// <returns></returns>
        private decimal CalculateOverPrice(decimal Weight, int BaseWeight, List<AssFormulaPriceExModel> listOverPrice)
        {
            if (listOverPrice == null) throw new ArgumentNullException("list Over Price is null");
            decimal OverPrice = 0;
            foreach (var item in listOverPrice)
            {
                if (Weight > item.StartWeight)
                {
                    if (!item.EndWeight.HasValue)
                    {
                        OverPrice = item.Price * (Weight - BaseWeight);
                        break;
                    }
                    else if (Weight <= item.EndWeight.Value)
                    {
                        OverPrice = item.Price * (Weight - BaseWeight);
                        break;
                    }
                }
            }
            return OverPrice;
        }

        /// <summary>
        /// 根据小数的进位规则计算数值
        /// </summary>
        /// <param name="OrigValue">重量小数部分的值</param>
        /// <returns></returns>
        private decimal CarryRule(decimal OrigValue)
        {
            OrigValue = decimal.Round(OrigValue,1);
            if (OrigValue <= 0.2M)
            {
                return 0;
            }
            if (OrigValue >= 0.3M && OrigValue <= 0.7M)
            {
                return 0.5M;
            }
            if (OrigValue >= 0.8M)
            {
                return 1;
            }
            return 0;
        }

        #region IFormula<KPICalcInputModel,KPICalcOutputModel> 成员

        public KPICalcOutputModel Execute(KPICalcInputModel context)
        {
            if (null == context) throw new ArgumentNullException("KPICalcInputModel");
            if (string.IsNullOrWhiteSpace(context.DeliveryNo)) return null;
            DispatchModel dispatchModel = _dispatchDAL.Get(context.DeliveryNo);
            if (null == dispatchModel) return null;
            LinePlanModel linePlanModel = _linePlanDAL.GetLinePlan(dispatchModel.LPID);
            CarrierWaybillModel carrierWaybillModel = _carrierWaybillDAL.Get(dispatchModel.CarrierWaybillID);
            if (null == carrierWaybillModel || null == linePlanModel) return null;
            if (context.IsInit)
            {
                InitOrigInputModel(context, linePlanModel, dispatchModel);
            }
            if (null == context.AssPriceList || context.AssPriceList.Count < 1) return null;
            //使用系统中已经确认的保价金额为准，防止UI数据被篡改造成BUG
            context.ProtectedPrice = dispatchModel.ProtectedPrice;
            //calculate
            KPICalcOutputModel outputModel = new KPICalcOutputModel();
            //outputModel.InsuranceAmount = dispatchModel.TotalAmount * context.InsuranceRate;
            //按照最新需求，使用保价金额计算保险费率
            outputModel.InsuranceAmount = context.ProtectedPrice * context.InsuranceRate;
            outputModel.BaseAmount = GetBasePrice(context.ExpressionType, context.AssPriceList, carrierWaybillModel);
            outputModel.LongTransferAmount = carrierWaybillModel.Weight * context.LongTransferRate;

            outputModel.NeedAmount = outputModel.BaseAmount + outputModel.InsuranceAmount
                + outputModel.LongTransferAmount + context.LongDeliveryAmount + context.LongPickPrice;

            outputModel.ComplementAmount = outputModel.NeedAmount < linePlanModel.LowestPrice ? linePlanModel.LowestPrice - outputModel.NeedAmount : 0;
            outputModel.NeedAmount = outputModel.NeedAmount + outputModel.ComplementAmount;
            outputModel.ApprovedAmount = outputModel.NeedAmount;

            if (context.KPIDelayType.HasValue)
            {
                if (context.KPIDelayType == Enums.KPIDelayType.DelayDiscount)
                {
                    if (context.Discount.HasValue)
                    {
                        outputModel.ApprovedAmount = context.Discount.Value * outputModel.ApprovedAmount;
                    }
                }
                else if (context.KPIDelayType == Enums.KPIDelayType.DelayAmount)
                {
                    if (context.Discount.HasValue)
                    {
                        outputModel.ApprovedAmount = outputModel.ApprovedAmount + context.Discount.Value;
                    }
                }
            }

            outputModel.ConfirmedAmount = outputModel.ApprovedAmount + context.LostDeduction + context.OtherAmount;
            return outputModel;
        }

        #endregion
    }
}

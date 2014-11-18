using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.Delivery.KPIAppraisal
{
    public class DeliveryAssessmentBLL : BaseBLL, IDeliveryAssessmentBLL
    {
        IDeliveryAssessmentDAL _deliveryAssessmentDAL = ServiceFactory.GetService<IDeliveryAssessmentDAL>("DeliveryAssessmentDAL");
        IAssFixedPriceDAL _assFixedPriceDAL = ServiceFactory.GetService<IAssFixedPriceDAL>("AssFixedPriceDAL");
        IAssFormulaPriceDAL _assFormulaPriceDAL = ServiceFactory.GetService<IAssFormulaPriceDAL>("AssFormulaPriceDAL");
        IAssLadderPriceDAL _assLadderPriceDAL = ServiceFactory.GetService<IAssLadderPriceDAL>("AssLadderPriceDAL");
        IDispatchBLL _dispatchBLL = ServiceFactory.GetService<IDispatchBLL>("DispatchBLL");
        IFormula<KPICalcOutputModel, KPICalcInputModel> _formula = FormulasFactory.GetFormula<IFormula<KPICalcOutputModel, KPICalcInputModel>>("KPICalculateFormula");
        #region IDeliveryAssessmentBLL 成员

        public ResultModel Add(KPICalcInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new CodeNotValidException();
            }
            if (inputModel.AssPriceList == null || inputModel.AssPriceList.Count == 0)
            {
                throw new CodeNotValidException();
            }
            if (!inputModel.KPIDelayType.HasValue)
            {
                return ErrorResult("请选择KPI考核类型");
            }
            if (_deliveryAssessmentDAL.IsExist(inputModel.DeliveryNo))
            {
                return ErrorResult("该提货单KPI考核信息已经存在！");
            }
            //计算运费
            ViewAssPriceModel model = Calculate(inputModel);
            DeliveryAssessmentModel daModel = VanclConverter.ConvertModel<DeliveryAssessmentModel, ViewAssPriceModel>(model);
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _deliveryAssessmentDAL.Add(daModel);
                WriteInsertLog<DeliveryAssessmentModel>(daModel);
                //更新提货单状态
                _dispatchBLL.UpdateDeliveryStatus(model.DeliveryNo, Enums.DeliveryStatus.KPIApproved);
                switch (model.ExpressionType)
                {
                    case Enums.ExpressionType.Fixed:
                        AssFixedPriceModel fixedModel = (AssFixedPriceModel)model.AssPriceList[0];
                        _assFixedPriceDAL.Add(fixedModel);
                        WriteInsertLog<AssFixedPriceModel>(fixedModel);
                        break;
                    case Enums.ExpressionType.Formula:
                        AssFormulaPriceModel formulaModel = (AssFormulaPriceModel)model.AssPriceList[0];
                        _assFormulaPriceDAL.Add(formulaModel);
                        _assFormulaPriceDAL.AddOverPriceDetail(formulaModel.Detail);
                        WriteInsertLog<AssFormulaPriceModel>(formulaModel);
                        break;
                    case Enums.ExpressionType.Ladder:
                        _assLadderPriceDAL.Add((List<AssLadderPriceModel>)VanclConverter.ConvertModelList<AssLadderPriceModel, IAssPriceModel>(model.AssPriceList));
                        break;
                }
                scope.Complete();
            }
            return AddResult(i, "KPI考核信息");
        }

        public ResultModel Update(KPICalcInputModel inputModel)
        {
            if (inputModel == null)
            {
                throw new CodeNotValidException();
            }
            if (inputModel.AssPriceList == null || inputModel.AssPriceList.Count == 0)
            {
                throw new CodeNotValidException();
            }
            if (!inputModel.KPIDelayType.HasValue)
            {
                return ErrorResult("请选择KPI考核类型");
            }
            DeliveryAssessmentModel pastDaModel = Get(inputModel.DeliveryNo);
            if (pastDaModel == null)
            {
                return ErrorResult("该提货单KPI考核信息不存在或者已经被删除！");
            }
            //计算运费
            ViewAssPriceModel model = Calculate(inputModel);
            DeliveryAssessmentModel daModel = VanclConverter.ConvertModel<DeliveryAssessmentModel, ViewAssPriceModel>(model);
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _deliveryAssessmentDAL.Update(daModel);
                WriteUpdateLog<DeliveryAssessmentModel>(daModel, pastDaModel);
                switch (model.ExpressionType)
                {
                    case Enums.ExpressionType.Fixed:
                        AssFixedPriceModel fixedModel = (AssFixedPriceModel)model.AssPriceList[0];
                        AssFixedPriceModel pastFixedModel = _assFixedPriceDAL.Get(model.DeliveryNo);
                        if (pastFixedModel == null)
                        {
                            _assFixedPriceDAL.Add(fixedModel);
                            WriteInsertLog<AssFixedPriceModel>(fixedModel);
                        }
                        else
                        {
                            _assFixedPriceDAL.Update(fixedModel);
                            WriteUpdateLog<AssFixedPriceModel>(fixedModel, pastFixedModel);
                        }
                        break;
                    case Enums.ExpressionType.Formula:
                        AssFormulaPriceModel formulaModel = (AssFormulaPriceModel)model.AssPriceList[0];
                        AssFormulaPriceModel pastformulaModel = _assFormulaPriceDAL.Get(model.DeliveryNo);
                        if (pastformulaModel == null)
                        {
                            _assFormulaPriceDAL.Add(formulaModel);
                            _assFormulaPriceDAL.AddOverPriceDetail(formulaModel.Detail);
                            WriteInsertLog<AssFormulaPriceModel>(formulaModel);
                        }
                        else
                        {
                            _assFormulaPriceDAL.Update(formulaModel);
                            _assFormulaPriceDAL.DeleteOverPriceDetail(formulaModel.DeliveryNo);
                            _assFormulaPriceDAL.AddOverPriceDetail(formulaModel.Detail);
                            WriteUpdateLog<AssFormulaPriceModel>(formulaModel, pastformulaModel);
                            WriteForcedUpdateLog<AssFormulaPriceExModel>(formulaModel.DeliveryNo);
                        }
                        break;
                    case Enums.ExpressionType.Ladder:
                        List<AssLadderPriceModel> lstLadder = (List<AssLadderPriceModel>)VanclConverter.ConvertModelList<AssLadderPriceModel, IAssPriceModel>(model.AssPriceList);
                        _assLadderPriceDAL.Delete(lstLadder[0].DeliveryNo);
                        _assLadderPriceDAL.Add(lstLadder);
                        WriteForcedUpdateLog<AssLadderPriceModel>(lstLadder[0].DeliveryNo);
                        break;
                }
                scope.Complete();
            }
            return UpdateResult(i, "KPI考核信息");
        }

        public bool IsExist(string deliveryNo)
        {
            if (string.IsNullOrWhiteSpace(deliveryNo))
            {
                throw new ArgumentNullException();
            }

            return _deliveryAssessmentDAL.IsExist(deliveryNo);
        }

        public PagedList<ViewDeliveryAssessmentModel> Search(DeliveryAssessmentSearchModel searchModel)
        {
            return _deliveryAssessmentDAL.Search(searchModel);
        }

        /// <summary>
        /// 采用提货单默认设置计算KPI考核运费
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        public ViewAssPriceModel KPICalculateByDeliveryDefaultSetting(String DeliveryNo)
        {
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo");
            KPICalcInputModel inputModel = new KPICalcInputModel(true) { DeliveryNo = DeliveryNo };
            KPICalcOutputModel outputModel = _formula.Execute(inputModel);
            if (outputModel == null) 
            {
                return null;
            }
            ViewAssPriceModel model = new ViewAssPriceModel();
            model.AssPriceList = inputModel.AssPriceList;
            model.DeliveryNo = DeliveryNo;
            model.KPIDelayType = Enums.KPIDelayType.DelayDiscount;      //默认延误折扣
            model.OtherAmount = 0M;                                                           //其他金额默认0
            model.DelayAmount = null;                                                           //默认延误折扣, 没有延误金额
            model.Discount = inputModel.Discount;
            model.ExpressionType = inputModel.ExpressionType;
            model.LostDeduction = inputModel.LostDeduction;
            model.IsDelayAssess = inputModel.IsDelayAssess;
            model.LongTransferRate = inputModel.LongTransferRate;
            model.LongDeliveryAmount = inputModel.LongDeliveryAmount;
            model.InsuranceRate = inputModel.InsuranceRate;
            model.LongPickPrice = inputModel.LongPickPrice;
            model.ProtectedPrice = inputModel.ProtectedPrice;
            //output
            model.ApprovedAmount = outputModel.ApprovedAmount;
            model.ConfirmedAmount = outputModel.ConfirmedAmount;
            model.BaseAmount = outputModel.BaseAmount;
            model.ComplementAmount = outputModel.ComplementAmount;
            model.InsuranceAmount = outputModel.InsuranceAmount;
            model.LongTransferAmount = outputModel.LongTransferAmount;
            model.NeedAmount = outputModel.NeedAmount;
            return model;
        }

        public ViewAssPriceModel SearchAssPrice(string DeliveryNo)
        {
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo");
            DeliveryAssessmentModel assModel = Get(DeliveryNo);
            DispatchModel dispMpdel = _dispatchBLL.Get(DeliveryNo);
            if (assModel != null)                   //已经做过KPI考核
            {
                ViewAssPriceModel daModel = VanclConverter.ConvertModel<ViewAssPriceModel, DeliveryAssessmentModel>(assModel);
                daModel.AssPriceList = new List<IAssPriceModel>();
                switch (daModel.ExpressionType)
                {
                    case Enums.ExpressionType.Fixed:
                        daModel.AssPriceList.Add(_assFixedPriceDAL.Get(assModel.DeliveryNo));
                        break;
                    case Enums.ExpressionType.Ladder:
                        daModel.AssPriceList.AddRange(_assLadderPriceDAL.Get(assModel.DeliveryNo));
                        break;
                    case Enums.ExpressionType.Formula:
                        daModel.AssPriceList.Add(_assFormulaPriceDAL.Get(assModel.DeliveryNo));
                        break;
                    default:
                        break;
                }
                daModel.ProtectedPrice = dispMpdel.ProtectedPrice;
                return daModel;
            }
            //未做过KPI考核
            if (dispMpdel == null ||
                (dispMpdel.DeliveryStatus != Enums.DeliveryStatus.ArrivedOnTime
                && dispMpdel.DeliveryStatus != Enums.DeliveryStatus.ArrivedDelay
                && dispMpdel.DeliveryStatus != Enums.DeliveryStatus.AllLost)
                )
            {
                throw new CodeNotValidException();
            }
            KPICalcInputModel inputModel = new KPICalcInputModel(true) { DeliveryNo = DeliveryNo };
            KPICalcOutputModel outputModel = _formula.Execute(inputModel);
            if (outputModel == null) { throw new CodeNotValidException(); }
            ViewAssPriceModel model = new ViewAssPriceModel();
            model.AssPriceList = inputModel.AssPriceList;
            model.DeliveryNo = DeliveryNo;
            model.KPIDelayType = Enums.KPIDelayType.DelayDiscount;      //默认延误折扣
            model.OtherAmount = 0M;                                                           //其他金额默认0
            model.DelayAmount = null;                                                           //默认延误折扣, 没有延误金额
            model.Discount = inputModel.Discount;
            model.ExpressionType = inputModel.ExpressionType;
            model.LostDeduction = inputModel.LostDeduction;
            model.IsDelayAssess = inputModel.IsDelayAssess;
            model.LongTransferRate = inputModel.LongTransferRate;
            model.LongDeliveryAmount = inputModel.LongDeliveryAmount;
            model.InsuranceRate = inputModel.InsuranceRate;
            model.LongPickPrice = inputModel.LongPickPrice;
            model.ProtectedPrice = inputModel.ProtectedPrice;
            //output
            model.ApprovedAmount = outputModel.ApprovedAmount;
            model.ConfirmedAmount = outputModel.ConfirmedAmount;
            model.BaseAmount = outputModel.BaseAmount;
            model.ComplementAmount = outputModel.ComplementAmount;
            model.InsuranceAmount = outputModel.InsuranceAmount;
            model.LongTransferAmount = outputModel.LongTransferAmount;
            model.NeedAmount = outputModel.NeedAmount;
            return model;
        }

        /// <summary>
        /// 用户点击计算KPI价格
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public KPICalcOutputModel KPICalculate(KPICalcInputModel inputModel)
        {
            if (null == inputModel) throw new ArgumentNullException("KPICalcInputModel");
            if (inputModel.IsInit) throw new ArgumentNullException("KPICalcInputModel Wrong");
            return _formula.Execute(inputModel);
        }

        public DeliveryAssessmentModel Get(string deliveryNo)
        {
            if (String.IsNullOrWhiteSpace(deliveryNo)) throw new ArgumentNullException("deliveryNo");
            return _deliveryAssessmentDAL.Get(deliveryNo);
        }

        #endregion

        /// <summary>
        /// 后台计算KPI价格，避免前端数据被篡改
        /// </summary>
        /// <param name="model"></param>
        private ViewAssPriceModel Calculate(KPICalcInputModel inputModel)
        {
            if (inputModel == null) throw new ArgumentNullException("inputModel");
            KPICalcOutputModel outputModel = _formula.Execute(inputModel);
            if (outputModel == null)
            {
                throw new CodeNotValidException();
            }
            return new ViewAssPriceModel()
            {
                ApprovedAmount = outputModel.ApprovedAmount,
                AssPriceList = inputModel.AssPriceList,
                BaseAmount = outputModel.BaseAmount,
                ComplementAmount = outputModel.ComplementAmount,
                ConfirmedAmount = outputModel.ConfirmedAmount,
                DeliveryNo = inputModel.DeliveryNo,
                KPIDelayType = inputModel.KPIDelayType.Value,
                OtherAmount = inputModel.OtherAmount,
                DelayAmount = inputModel.KPIDelayType.Value == Enums.KPIDelayType.DelayAmount ? inputModel.Discount : null,
                Discount = inputModel.KPIDelayType.Value == Enums.KPIDelayType.DelayDiscount ? inputModel.Discount : null,
                ExpressionType = inputModel.ExpressionType,
                InsuranceAmount = outputModel.InsuranceAmount,
                InsuranceRate = inputModel.InsuranceRate,
                IsDelayAssess = inputModel.IsDelayAssess,
                LongDeliveryAmount = inputModel.LongDeliveryAmount,
                LongPickPrice = inputModel.LongPickPrice,
                LongTransferAmount = outputModel.LongTransferAmount,
                LongTransferRate = inputModel.LongTransferRate,
                LostDeduction = inputModel.LostDeduction,
                NeedAmount = outputModel.NeedAmount
            };
        }


    }
}

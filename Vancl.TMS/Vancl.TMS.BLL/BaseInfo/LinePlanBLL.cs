using System.Collections.Generic;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.BLL.Formula.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Util.Pager;
using System;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Util.Exceptions;
using System.Linq;
using Vancl.TMS.IBLL.Transport.Plan;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.BLL.BaseInfo
{

    public class LinePlanBLL : BaseBLL, ILinePlanBLL, IOperateLogBLL
    {
        ILinePlanDAL _dal = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");
        ILineFixedPriceBLL _fixedPriceBLL = ServiceFactory.GetService<ILineFixedPriceBLL>("LineFixedPriceBLL");
        ILineFormulaPriceBLL _formulaPriceBLL = ServiceFactory.GetService<ILineFormulaPriceBLL>("LineFormulaPriceBLL");
        ILineLadderPriceBLL _ladderPriceBLL = ServiceFactory.GetService<ILineLadderPriceBLL>("LineLadderPriceBLL");
        IFormula<string, LineCodeContextModel> lineCodeFormula = FormulasFactory.GetFormula<IFormula<string, LineCodeContextModel>>("LineCodeGenerateFormula");
        ITransportPlanBLL _transportPlanBLL = ServiceFactory.GetService<ITransportPlanBLL>();

        /// <summary>
        /// 新增线路计划
        /// </summary>
        /// <param name="model">线路计划模型</param>
        /// <returns></returns>
        public ResultModel Add(LinePlanModel model, IList<ILinePrice> linePriceModel)
        {
            if (model == null || linePriceModel == null)
            {
                throw new ArgumentNullException("lineplan");
            }
            if (model.EffectiveTime <= DateTime.Now)
            {
                return ErrorResult("请设置为今天之后的生效时间！");
            }
            //如果同时选择 普通以及禁航则自动全部选上
            if (model.LineGoodsType == (Enums.GoodsType.Normal | Enums.GoodsType.Contraband))
            {
                model.LineGoodsType = Enums.GoodsType.Normal | Enums.GoodsType.Contraband | Enums.GoodsType.Frangible;
            }
            //新增线路时需要判断是否已存在该类型线路，新增线路计划时则不用判断
            if (model.LPID == 0)
            {
                if (IsExsitLinePlan(model))
                {
                    return ErrorResult("该条线路已经存在！");
                }
            }
            int i = 0;
            model.IsEnabled = true;
            if (string.IsNullOrWhiteSpace(model.LineID))
            {
                LineCodeContextModel m = new LineCodeContextModel();
                m.Departure = model.DepartureID.ToString();
                m.Arrival = model.ArrivalID.ToString();
                m.GoodsType = model.LineGoodsType;
                m.BusinessType = model.BusinessType;
                m.TransportType = model.TransportType;
                m.CarrierID = model.CarrierID.ToString();
                model.LineID = lineCodeFormula.Execute(m);
            }
            else
            {
                //获取启用停用状态
                model.IsEnabled = _dal.GetLineIsEnabled(model.LineID);
            }
            model.LPID = (int)_dal.GetNextSequence(model.SequenceName);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.Add(model);
                WriteInsertLog<LinePlanModel>(model);
                if (linePriceModel[0] is LineFixedPriceModel)
                {
                    LineFixedPriceModel fixedPriceModel = (LineFixedPriceModel)linePriceModel[0];
                    fixedPriceModel.LPID = model.LPID;
                    _fixedPriceBLL.Add(fixedPriceModel);
                }
                else if (linePriceModel[0] is LineFormulaPriceModel)
                {
                    LineFormulaPriceModel formulaPriceModel = (LineFormulaPriceModel)linePriceModel[0];
                    formulaPriceModel.LPID = model.LPID;
                    foreach (var item in formulaPriceModel.Detail)
                    {
                        item.LPID = model.LPID;
                    }
                    _formulaPriceBLL.Add(formulaPriceModel);
                }
                else if (linePriceModel[0] is LineLadderPriceModel)
                {
                    List<LineLadderPriceModel> ladderList = new List<LineLadderPriceModel>();
                    foreach (LineLadderPriceModel ladderPriceModel in linePriceModel)
                    {
                        ladderPriceModel.LPID = model.LPID;
                        ladderList.Add(ladderPriceModel);
                    }
                    _ladderPriceBLL.Add(ladderList);
                }
                scope.Complete();
            }
            return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "新增线路成功，等待审核" : "新增线路失败" };
        }

        /// <summary>
        /// 审核线路
        /// </summary>
        /// <param name="lpid"></param>
        /// <param name="lineStatus"></param>
        /// <param name="effectiveTime"></param>
        /// <returns></returns>
        public ResultModel AuditLinePlan(int lpid, Enums.LineStatus lineStatus, DateTime? effectiveTime)
        {
            LinePlanModel pastModel = _dal.GetLinePlan(lpid);
            if (pastModel.Status != Enums.LineStatus.NotApprove)
            {
                return ErrorResult("该线路已审核，请刷新页面后重试!");
            }
            LinePlanModel nowModel = VanclConverter.ConvertModel<LinePlanModel, LinePlanModel>(pastModel);
            nowModel.Status = lineStatus;
            if (effectiveTime.HasValue)
            {
                nowModel.EffectiveTime = effectiveTime.Value;
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.AuditLinePlan(lpid, lineStatus, effectiveTime, false);
                WriteUpdateLog<LinePlanModel>(nowModel, pastModel);
                scope.Complete();
            }
            return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "审核线路计划成功" : "审核线路计划失败" };
        }

        /// <summary>
        /// 批量审核线路
        /// </summary>
        /// <param name="lstLpid"></param>
        /// <param name="lineStatus"></param>
        /// <returns></returns>
        public ResultModel BatchAuditLinePlan(List<int> lstLpid, Enums.LineStatus lineStatus)
        {
            if (lstLpid == null || lstLpid.Count == 0)
            {
                throw new CodeNotValidException();
            }
            List<LinePlanModel> pastList = new List<LinePlanModel>();
            LinePlanModel pastModel = null;
            foreach (int lpid in lstLpid)
            {
                pastModel = _dal.GetLinePlan(lpid);
                if (pastModel == null)
                {
                    return ErrorResult(string.Format("线路ID【{0}】:该线路不存在或已经被删除！", lpid));
                }
                if (pastModel.Status != Enums.LineStatus.NotApprove)
                {
                    return ErrorResult("存在已审核线路，请刷新页面后重试!");
                }
                pastList.Add(pastModel);
            }
            int i = 0;
            int iSuccess = 0;
            int iFailed = 0;
            foreach (int lpid in lstLpid)
            {
                pastModel = pastList.FirstOrDefault(l => l.LPID == lpid);
                LinePlanModel nowModel = VanclConverter.ConvertModel<LinePlanModel, LinePlanModel>(pastModel);
                nowModel.Status = lineStatus;
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    i = _dal.AuditLinePlan(lpid, lineStatus, null, true);
                    WriteUpdateLog<LinePlanModel>(nowModel, pastModel);
                    scope.Complete();
                }
                if (i > 0)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            string strPre = lineStatus == Enums.LineStatus.Approved ? "审核" : "驳回";
            string strSuccessNoticeInfo = String.Format(strPre + "成功{0}条线路计划！" + Environment.NewLine + strPre + "失败{1}条线路计划！", iSuccess, iFailed);
            return InfoResult(strSuccessNoticeInfo);
        }

        /// <summary>
        /// 获取指定线路模型
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        public LinePlanModel GetLinePlan(int lpid)
        {
            return _dal.GetLinePlan(lpid);
        }

        /// <summary>
        /// 获取指定线路模型(包含始发地、目的地名称)
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        public ViewLinePlanModel GetViewLinePlan(int lpid)
        {
            return _dal.GetViewLinePlan(lpid);
        }

        /// <summary>
        /// 查询线路对应的运费
        /// </summary>
        /// <param name="lpid"></param>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public IList<ILinePrice> GetLinePrice(int lpid, Enums.ExpressionType expressionType)
        {
            IList<ILinePrice> linePriceList = new List<ILinePrice>();
            switch (expressionType)
            {
                case Enums.ExpressionType.Fixed:
                    LineFixedPriceModel fixedPriceModel = _fixedPriceBLL.GetByLinePlanID(lpid);
                    if (fixedPriceModel != null)
                    {
                        linePriceList.Add(fixedPriceModel);
                    }
                    break;
                case Enums.ExpressionType.Ladder:
                    linePriceList = _ladderPriceBLL.GetByLinePlanID(lpid).ConvertToFatherListModel<ILinePrice, LineLadderPriceModel>();
                    break;
                case Enums.ExpressionType.Formula:
                    LineFormulaPriceModel formulaPriceModel = _formulaPriceBLL.GetByLinePlanID(lpid);
                    if (formulaPriceModel != null)
                    {
                        linePriceList.Add(formulaPriceModel);
                    }
                    break;
            }
            return linePriceList;
        }

        /// <summary>
        /// 查询线路列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public PagedList<ViewLinePlanModel> GetLinePlan(LinePlanSearchModel searchModel)
        {
            return _dal.GetLinePlan(searchModel);
        }

        /// <summary>
        /// 修改线路信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="linePriceModel"></param>
        /// <returns></returns>
        public ResultModel Update(LinePlanModel model, IList<ILinePrice> linePriceModel)
        {
            if (model == null)
            {
                throw new CodeNotValidException();
            }
            int i = 0;
            LinePlanModel pastModel = GetLinePlan(model.LPID);
            if (pastModel == null)
            {
                return ErrorResult("该条线路不存在或者已经被删除！");
            }
            if (pastModel.Status != Enums.LineStatus.NotApprove
                && pastModel.Status != Enums.LineStatus.Dismissed)
            {
                return ErrorResult("只能对[未审核]或[已驳回]状态的线路计划进行修改！！");
            }
            if (model.EffectiveTime <= DateTime.Now)
            {
                return ErrorResult("请设置为今天之后的生效时间！");
            }
            model.Status = Enums.LineStatus.NotApprove;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.Update(model);
                WriteUpdateLog<LinePlanModel>(model as LinePlanModel, pastModel);
                if (linePriceModel[0] is LineFixedPriceModel)
                {
                    LineFixedPriceModel fixedPriceModel = (LineFixedPriceModel)linePriceModel[0];
                    fixedPriceModel.LPID = model.LPID;
                    _fixedPriceBLL.Update(fixedPriceModel);
                }
                else if (linePriceModel[0] is LineFormulaPriceModel)
                {
                    LineFormulaPriceModel formulaPriceModel = (LineFormulaPriceModel)linePriceModel[0];
                    formulaPriceModel.LPID = model.LPID;
                    foreach (var item in formulaPriceModel.Detail)
                    {
                        item.LPID = model.LPID;
                    }
                    _formulaPriceBLL.Update(formulaPriceModel);
                }
                else if (linePriceModel[0] is LineLadderPriceModel)
                {
                    List<LineLadderPriceModel> ladderList = new List<LineLadderPriceModel>();
                    foreach (LineLadderPriceModel ladderPriceModel in linePriceModel)
                    {
                        ladderPriceModel.LPID = model.LPID;
                        ladderList.Add(ladderPriceModel);
                    }
                    _ladderPriceBLL.Update(ladderList);
                }
                scope.Complete();
            }
            return new ResultModel { IsSuccess = i > 0, Message = i > 0 ? "线路修改成功，等待审核" : "线路修改失败" };
        }

        /// <summary>
        /// 批量删除线路
        /// </summary>
        /// <param name="lpidList">线路ID列表</param>
        /// <returns></returns>
        public ResultModel Delete(List<int> lpidList)
        {
            if (lpidList == null)
            {
                throw new ArgumentNullException("lpidList");
            }
            if (lpidList.Count == 0)
            {
                throw new ArgumentNullException("线路计划id列表数必须大于0");
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.Delete(lpidList);
                WriteBatchDeleteLog<LinePlanModel>(lpidList);
                _fixedPriceBLL.Delete(lpidList);
                _formulaPriceBLL.Delete(lpidList);
                _ladderPriceBLL.Delete(lpidList);
                scope.Complete();
            }
            return DeleteResult(i, "线路计划");
        }

        /// <summary>
        /// 判断线路是否已经存在
        /// </summary>
        /// <param name="linePlanModel"></param>
        /// <returns></returns>
        public bool IsExsitLinePlan(LinePlanModel linePlanModel)
        {
            return _dal.IsExsitLinePlan(linePlanModel);
        }

        public int UpdateDeadLineStatus()
        {
            int? LPID = _dal.GetNeedEffectivedLinePlan();
            if (LPID.HasValue)
            {
                var needEffectiveLinePlan = _dal.GetLinePlan(LPID.Value);
                var prelistLPID = _dal.GetEffectivedLinePlan(needEffectiveLinePlan.LineID);
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    if (prelistLPID != null && prelistLPID.Count > 0)
                    {
                        foreach (var item in prelistLPID)
                        {
                            _dal.UpdateToExpired(item);
                            WriteCustomizeLog<LinePlanModel>(new LinePlanModel()
                            {
                                LPID = item,
                                CustomizeLog = String.Format("线路计划:{0}状态改为已生效，导致当前线路计划{1}状态改为已作废", needEffectiveLinePlan.LPID, item)
                            });
                        }
                    }
                    int i = _dal.UpdateToEffective(needEffectiveLinePlan.LPID);
                    WriteCustomizeLog<LinePlanModel>(new LinePlanModel()
                    {
                        LPID = needEffectiveLinePlan.LPID,
                        CustomizeLog = String.Format("线路计划:{0}状态改为已生效", needEffectiveLinePlan.LPID)
                    });
                    scope.Complete();
                    return i;
                }
            }
            return 0;
        }

        public ResultModel SetIsEnabled(List<string> lineID, bool isEnabled)
        {
            if (lineID == null || lineID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            lineID = lineID.Distinct().ToList();
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.SetIsEnabled(lineID, isEnabled);
                foreach (string id in lineID)
                {
                    WriteSetEnableLog<LinePlanModel>(id, isEnabled);
                }
                scope.Complete();
            }
            string strPre = isEnabled ? "启用" : "停用";
            if (i <= 0)
            {
                return ErrorResult(strPre + "线路失败！");
            }
            else
            {
                return InfoResult(strPre + "线路成功！");
            }
        }

        #region 线路编号修复

        public List<LinePlanLineIDRepairModel> GetAllValidLinePlan()
        {
            return _dal.GetAllValidLinePlan();
        }

        public ResultModel RepairLineID(List<LinePlanLineIDRepairModel> lstModel)
        {
            if (lstModel == null || lstModel.Count == 0)
            {
                return ErrorResult("没有需要更新的线路编号！");
            }
            LineCodeContextModel lccm = new LineCodeContextModel();
            lstModel.ForEach(m =>
            {
                lccm.Departure = m.DepartureID.ToString();
                lccm.Arrival = m.ArrivalID.ToString();
                lccm.GoodsType = m.LineGoodsType;
                lccm.BusinessType = m.BusinessType;
                lccm.TransportType = m.TransportType;
                lccm.CarrierID = m.CarrierID.ToString();
                m.LineID = lineCodeFormula.Execute(lccm);
            });
            List<TransportPlanDetailLineIDRepairModel> lstTPDModel = _transportPlanBLL.GetValidTransportPlanDetail();
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dal.RepairLineID(lstModel);
                if (i > 0)
                {
                    ResultModel rm = _transportPlanBLL.RepairTransportPlanDetailLineID(lstTPDModel, lstModel);
                    if (rm.IsSuccess)
                    {
                        scope.Complete();
                    }
                }
            }
            if (i > 0)
            {
                return InfoResult(string.Format("成功修复{0}条线路编号！", i));
            }
            else
            {
                return ErrorResult("修复线路编号失败！");
            }
        }

        public ResultModel RestoreLineID(List<LinePlanLineIDRepairModel> lstModel)
        {
            int i = _dal.RepairLineID(lstModel);
            if (i > 0)
            {
                return InfoResult(string.Format("成功恢复{0}条线路编号！", i));
            }
            else
            {
                return ErrorResult("恢复线路编号失败！");
            }
        }

        #endregion

        #region IOperateLogBLL 成员
        /// <summary>
        /// 读取操作日志
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        public List<Model.Log.OperateLogModel> Read(Model.Log.BaseOperateLogSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("Read OperateLogModel.searchmodel is null");
            searchmodel.Module = Enums.SysModule.LineModule;
            return base.ReadLog<Model.Log.OperateLogModel>(searchmodel);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using Vancl.TMS.IBLL.Transport.Plan;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IDAL.Transport.Plan;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.BaseInfo.Line;
using System.Linq;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.BLL.Transport.Plan
{
    public class TransportPlanBLL : BaseBLL, ITransportPlanBLL, IOperateLogBLL
    {
        IFormula<PointPathModel, PointPathSearchModel> formula = FormulasFactory.GetFormula<IFormula<PointPathModel, PointPathSearchModel>>("LinePathSearchFormula");
        ITransportPlanDAL _transportPlanDAL = ServiceFactory.GetService<ITransportPlanDAL>("TransportPlanDAL");
        ITransportPlanDetailDAL _transportPlanDetailDAL = ServiceFactory.GetService<ITransportPlanDetailDAL>("TransportPlanDetailDAL");
        IExpressCompanyDAL _expressCompanyDal = ServiceFactory.GetService<IExpressCompanyDAL>("ExpressCompanyDAL");
        public ResultModel Add(TransportPlanModel model, IList<TransportPlanDetailModel> planDetail)
        {
            if (model == null || planDetail == null || planDetail.Count == 0)
            {
                throw new CodeNotValidException();
            }
            if (model.EffectiveTime <= DateTime.Now)
            {
                return ErrorResult("生效时间必须大于当前时间！");
            }
            if (model.EffectiveTime >= model.Deadline)
            {
                return ErrorResult("生效时间必须小于有效期！");
            }
            int i = 0;
            model.TPID = (int)_transportPlanDAL.GetNextSequence(model.SequenceName);
            model.Deadline = DateTime.Parse(model.Deadline.ToString("yyyy-MM-dd") + " 23:59:59.999");
            model.Status = Enums.TransportStatus.NotEffective;
            //默认为启用状态
            model.IsEnabled = true;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _transportPlanDAL.Add(model);
                WriteInsertLog<TransportPlanModel>(model);
                foreach (TransportPlanDetailModel detail in planDetail)
                {
                    detail.TPID = model.TPID;
                    _transportPlanDetailDAL.Add(detail);
                }
                scope.Complete();
            }
            return AddResult(i, "运输计划");
        }

        public ResultModel Update(TransportPlanModel model, IList<TransportPlanDetailModel> planDetail)
        {
            if (model == null || planDetail == null || planDetail.Count == 0)
            {
                throw new CodeNotValidException();
            }
            if (model.EffectiveTime >= model.Deadline)
            {
                return ErrorResult("生效时间必须小于有效期！");
            }
            //取得更新前的数据
            TransportPlanModel pastModel = Get(model.TPID);
            if (pastModel == null)
            {
                return ErrorResult("该运输计划不存在或者已经被删除！");
            }
            if (pastModel.Status == Enums.TransportStatus.Effective)
            {
                if (model.EffectiveTime >= DateTime.Now)
                {
                    model.Status = Enums.TransportStatus.NotEffective;
                }
                model.Status = pastModel.Status;
            }
            else
            {
                if (model.EffectiveTime <= DateTime.Now)
                {
                    return ErrorResult("生效时间必须大于当前时间！");
                }
                model.Status = Enums.TransportStatus.NotEffective;
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                //更新主表
                i = _transportPlanDAL.Update(model);
                //写日志
                WriteUpdateLog<TransportPlanModel>(model, pastModel);
                //删除明细表
                List<int> lstTpid = new List<int>();
                lstTpid.Add(model.TPID);
                _transportPlanDetailDAL.Delete(lstTpid);
                //添加明细数据
                foreach (TransportPlanDetailModel detail in planDetail)
                {
                    detail.TPID = model.TPID;
                    _transportPlanDetailDAL.Add(detail);
                }
                //写明细日志
                WriteForcedUpdateLog<TransportPlanDetailModel>(model.TPID.ToString());
                scope.Complete();
            }
            return UpdateResult(i, "运输计划");
        }

        public bool IsExistsTransportPlan(TransportPlanModel model)
        {
            return _transportPlanDAL.IsExistsTransportPlan(model);
        }

        public ResultModel Delete(List<int> lstTpid)
        {
            if (lstTpid == null || lstTpid.Count == 0)
            {
                throw new CodeNotValidException();
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                WriteBatchDeleteLog<TransportPlanModel>(lstTpid);
                i = _transportPlanDAL.Delete(lstTpid);
                _transportPlanDetailDAL.Delete(lstTpid);
                scope.Complete();
            }
            return DeleteResult(i, "运输计划");
        }

        public PagedList<ViewTransportPlanModel> Search(TransportPlanSearchModel searchModel)
        {
            var pagelist = _transportPlanDAL.Search(searchModel);
            foreach (var viewTransportPlanModel in pagelist)
            {
                if (!String.IsNullOrEmpty(viewTransportPlanModel.TransferStationMulti))
                {
                    List<string> stationnamelist = new List<string>();
                    var transtations = viewTransportPlanModel.TransferStationMulti.Split(',');
                    foreach (var transtation in transtations)
                    {
                        stationnamelist.Add(_expressCompanyDal.Get(Convert.ToInt32(transtation)).Name);
                    }
                    if (stationnamelist.Count > 0)
                    {
                        viewTransportPlanModel.TransferStation = string.Join(",", stationnamelist);
                    }
                }


            }
            return pagelist;

        }

        public IList<TransportPlanDetailModel> SearchTransportPlanPath(TransportPlanSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 搜索两点之间所有可用的线路路径点
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public PointPathModel SearchAllPath(PointPathSearchModel searchModel)
        {
            return formula.Execute(searchModel);
        }

        /// <summary>
        /// 根据主键ID取得一条数据
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        public TransportPlanModel Get(int tpid)
        {
            return _transportPlanDAL.Get(tpid);
        }

        public ViewTansportEditorModel GetViewData(int tpid)
        {
            var model = _transportPlanDAL.GetViewData(tpid);
            var lines = _transportPlanDetailDAL.GetByTransportPlanID(model.TPID);
            if (lines.Count > 0)
            {
                model.Line1 = lines[0].LineID;
                if (lines.Count == 2)
                {
                    model.Line2 = lines[1].LineID;
                }
                model.Lines = string.Join(",", lines.OrderBy(o => o.SeqNo).Select(o => o.LineID).ToArray());
            }
            if (!String.IsNullOrEmpty(model.TransitStationMulti))
            {
                var transtations = model.TransitStationMulti.Split(',');
                var stationnamelist = transtations.Select(transtation => _expressCompanyDal.Get(Convert.ToInt32(transtation)).Name).ToList();
                if (stationnamelist.Count > 0)
                {
                    model.TransitStationName = string.Join(",", stationnamelist);
                }

            }


            return model;
        }

        public IList<ViewLinePlanModel> GetLinePlanByTpid(int tpid)
        {
            return _transportPlanDAL.GetLinePlanByTpid(tpid);
        }

        #region ITransportPlanBLL 成员

        /// <summary>
        /// 设置为停用状态
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        public ResultModel SetToDisabled(int tpid)
        {
            if (tpid <= 0) throw new ArgumentNullException("tpid");
            TransportPlanModel pastModel = Get(tpid);
            if (null == pastModel)
            {
                return ErrorResult("运输计划不存在!");
            }
            if (!pastModel.IsEnabled)
            {
                return ErrorResult("此运输计划已经为停用状态!");
            }
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                _transportPlanDAL.UpdateToDisabled(tpid);
                WriteSetEnableLog<TransportPlanModel>(tpid.ToString(), false);
                scope.Complete();
            }
            return InfoResult("停用成功!");
        }

        /// <summary>
        /// 设置为启用状态
        /// </summary>
        /// <param name="tpid">运输计划id</param>
        /// <returns></returns>
        public ResultModel SetToEnabled(int tpid)
        {
            if (tpid <= 0) throw new ArgumentNullException("tpid");
            TransportPlanModel pastModel = Get(tpid);
            if (null == pastModel)
            {
                return ErrorResult("运输计划不存在!");
            }
            if (pastModel.IsEnabled)
            {
                return ErrorResult("此运输计划已经为启用状态!");
            }
            if (pastModel.Deadline < DateTime.Now)
            {
                return ErrorResult("此运输计划已经过期，不能启用!");
            }
            List<int> listTPID = _transportPlanDAL.GetUsefullyTPIDs(pastModel.DepartureID, pastModel.ArrivalID, pastModel.LineGoodsType);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (pastModel.Status == Enums.TransportStatus.Effective)
                {
                    if (listTPID != null && listTPID.Count > 0)
                    {
                        _transportPlanDAL.BatchUpdateToDisabled(listTPID);
                    }
                }
                _transportPlanDAL.UpdateToEnabled(tpid);
                WriteSetEnableLog<TransportPlanModel>(tpid.ToString(), true);
                scope.Complete();
            }
            return InfoResult("启用成功!");
        }

        #endregion

        #region ITransportPlanBLL 成员

        /// <summary>
        /// 设置需要做生效处理的TPID
        /// </summary>
        /// <returns></returns>
        public void UpdateNeedEffectived()
        {
            TransportPlanModel model = _transportPlanDAL.GetNeedEffectivedTPID();
            if (model != null)
            {
                List<int> listPreTPIDs = null;
                if (model.IsEnabled)
                {
                    listPreTPIDs = _transportPlanDAL.GetUsefullyTPIDs(model.DepartureID, model.ArrivalID, model.LineGoodsType);
                }
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    if (model.IsEnabled && listPreTPIDs != null && listPreTPIDs.Count > 0)
                    {
                        _transportPlanDAL.BatchUpdateToDisabled(listPreTPIDs);
                        foreach (var item in listPreTPIDs)
                        {
                            WriteCustomizeLog<TransportPlanModel>(new TransportPlanModel()
                            {
                                TPID = item,
                                CustomizeLog = String.Format("运输计划:{0}状态改为已生效并且已是启用状态，导致当前运输计划{1}状态改为已停用", model.TPID, item)
                            });
                        }
                    }
                    _transportPlanDAL.UpdateToEffective(model.TPID);
                    WriteCustomizeLog<TransportPlanModel>(new TransportPlanModel()
                    {
                        TPID = model.TPID,
                        CustomizeLog = String.Format("运输计划:{0}状态改为已生效", model.TPID)
                    });
                    scope.Complete();
                }
            }
        }

        #endregion

        #region 线路ID修复

        public List<TransportPlanDetailLineIDRepairModel> GetValidTransportPlanDetail()
        {
            return _transportPlanDetailDAL.GetValidTransportPlanDetail();
        }

        public ResultModel RepairTransportPlanDetailLineID(List<TransportPlanDetailLineIDRepairModel> lstTPDModel, List<LinePlanLineIDRepairModel> lstLinePlanModel)
        {
            if (lstTPDModel == null || lstTPDModel.Count == 0)
            {
                return InfoResult("没有需要修复的运输计划！");
            }
            if (lstLinePlanModel.FindAll(m => string.IsNullOrWhiteSpace(m.LineID)).Count() > 0)
            {
                return ErrorResult("线路编号为空！");
            }
            List<TransportPlanDetailLineIDRepairModel> tpdModels = null;
            foreach (LinePlanLineIDRepairModel lpModel in lstLinePlanModel)
            {
                tpdModels = lstTPDModel.FindAll(m => m.LPID == lpModel.LPID);
                if (tpdModels != null && tpdModels.Count > 0)
                {
                    tpdModels.ForEach(m => m.NewLineID = lpModel.LineID);
                }
            }
            int i = _transportPlanDetailDAL.RepairTransportPlanDetailLineID(lstTPDModel);
            if (i > 0)
            {
                return InfoResult(string.Format("成功修复{0}条运输计划的线路编号！", i));
            }
            else
            {
                return ErrorResult("修复运输计划的线路编号失败！");
            }
        }

        public ResultModel RestoreLineID(List<TransportPlanDetailLineIDRepairModel> lstModel)
        {
            if (lstModel == null || lstModel.Count == 0)
            {
                return ErrorResult("没有需要恢复的运输计划！");
            }
            int i = _transportPlanDetailDAL.RepairTransportPlanDetailLineID(lstModel);
            if (i > 0)
            {
                return InfoResult(string.Format("成功恢复{0}条运输计划的线路编号！", i));
            }
            else
            {
                return ErrorResult("恢复运输计划的线路编号失败！");
            }
        }
        #endregion

        #region ITransportPlanBLL 成员

        /// <summary>
        /// 批量停用运输计划
        /// </summary>
        /// <param name="listTPID">运输计划ID列表</param>
        /// <returns></returns>
        public ResultModel BatchSetToDisabled(List<int> listTPID)
        {
            if (listTPID == null || listTPID.Count <= 0) throw new ArgumentNullException("BatchSetToDisabled listTPID is null");
            ResultModel tmpResult = null;
            int iSuccess = 0, iFailed = 0;
            foreach (var item in listTPID)
            {
                tmpResult = SetToDisabled(item);
                if (tmpResult.IsSuccess)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            return InfoResult(String.Format("停用成功{0}条运输计划！" + Environment.NewLine + "停用失败{1}条运输计划！", iSuccess, iFailed));
        }

        /// <summary>
        /// 批量启用运输计划
        /// </summary>
        /// <param name="listTPID">运输计划ID列表</param>
        /// <returns></returns>
        public ResultModel BatchSetToEnabled(List<int> listTPID)
        {
            if (listTPID == null || listTPID.Count <= 0) throw new ArgumentNullException("BatchSetToEnabled listTPID is null");
            ResultModel tmpResult = null;
            int iSuccess = 0, iFailed = 0;
            foreach (var item in listTPID)
            {
                tmpResult = SetToEnabled(item);
                if (tmpResult.IsSuccess)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            return InfoResult(String.Format("启用成功{0}条运输计划！" + Environment.NewLine + "启用失败{1}条运输计划！", iSuccess, iFailed));
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
            searchmodel.Module = Enums.SysModule.TransPortPlanModule;
            return base.ReadLog<Model.Log.OperateLogModel>(searchmodel);
        }

        #endregion

        #region ITransportPlanBLL 成员


        public LinePlanModel GetLinePlan(TransportPlanSearchModel condition)
        {
            return _transportPlanDAL.GetLinePlan(condition);
        }

        #endregion
    }
}

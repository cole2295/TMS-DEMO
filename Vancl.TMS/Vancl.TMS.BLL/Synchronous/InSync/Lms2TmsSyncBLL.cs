using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.Synchronous.InSync;
using System.Reflection;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceCache;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    public class Lms2TmsSyncBLL : BaseBLL, ILms2TmsSyncBLL
    {
        ILms2TmsSyncLMSDAL _lmsDAL = ServiceFactory.GetService<ILms2TmsSyncLMSDAL>("Lms2TmsSyncLMSDAL");
        IServiceLogBLL _serviceLogBLL = ServiceFactory.GetService<IServiceLogBLL>("ServiceLogBLL");
        /// <summary>
        /// 读取lms中间表数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public List<LmsWaybillStatusChangeLogModel> ReadLmsChangeLogs(Lms2TmsJobArgs args)
        {
            if (args == null) throw new ArgumentNullException("args is null.");
            return _lmsDAL.ReadLmsChangeLogs(args);
        }

        /// <summary>
        /// 执行同步操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DoSync(LmsWaybillStatusChangeLogModel model)
        {
            Lms2TmsStrategy strategy = GetStrategy(model.OperateType);
            ResultModel rm = ErrorResult("没有对应的策略");
            bool hasException = false;
            try
            {
                if (strategy == null)
                {
                    return rm;
                }
                rm = Sync(model, strategy);
                return rm;
            }
            catch (Exception ex)
            {
                hasException = true;
                //转站申请异常，不更新LMS ChangeLog,oracle to sql 临时使用
                if(model.OperateType==Enums.Lms2TmsOperateType.TurnStationApply)
                {
                    rm.NoUpdateLmsLog = true;
                }
                throw;
            }
            finally
            {
                ServiceLogModel logModel = new ServiceLogModel();
                logModel.FormCode = model.WaybillNo.ToString();
                logModel.IsSuccessed = (rm == null) ? false : rm.IsSuccess;
                logModel.LogType = Enums.ServiceLogType.Lms2TmsSync;
                logModel.OpFunction = (int)model.OperateType;
                if (hasException)
                {
                    logModel.Note = BillSyncCache.GetInstance().GetExceptionMessage(model.WaybillNo.ToString());
                }
                else
                {
                    logModel.Note = (rm == null) ? "程序严重错误,ResultModel返回为null" : rm.Message;
                }
                logModel.SyncKey = model.ID.ToString();
                BillSyncCache.GetInstance().RemoveBill(model.WaybillNo.ToString());
                //记录日志
                _serviceLogBLL.WriteLog(logModel);
                //更新lms同步状态
                if(!rm.NoUpdateLmsLog)
                {
                    _lmsDAL.UpdateSyncStatus(model.ID, ((rm == null) ? false : rm.IsSuccess) ? Enums.SyncStatus.Already : Enums.SyncStatus.Error);
                } 
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        private ResultModel Sync(LmsWaybillStatusChangeLogModel model, Lms2TmsStrategy strategy)
        {
            try
            {
                return strategy.DoSync(model);
            }
            catch (Exception ex)
            {
                //出错重试一定次数
                BillSyncCache.GetInstance().SetExceptions(model.WaybillNo.ToString(), ex);
                int times = BillSyncCache.GetInstance().GetTryTimes(model.WaybillNo.ToString());
                if (times < Consts.SYNC_SERVICE_TRY_TIMES)
                {
                    BillSyncCache.GetInstance().SetTryTimes(model.WaybillNo.ToString(), ++times);
                    return Sync(model, strategy);
                }
                else
                {
                    //抛出错误
                    throw BillSyncCache.GetInstance().GetLastException(model.WaybillNo.ToString());
                }
            }
        }

        /// <summary>
        /// 根据操作类型实例化策略
        /// </summary>
        /// <param name="OperateType">操作类型</param>
        /// <returns></returns>
        private Lms2TmsStrategy GetStrategy(Enums.Lms2TmsOperateType OperateType)
        {
            Type type = Assembly.GetExecutingAssembly().GetType("Vancl.TMS.BLL.Synchronous.InSync." + OperateType.ToString() + "Strategy");
            if (type == null)
            {
                return null;
            }
            return (Lms2TmsStrategy)Activator.CreateInstance(type);

        }
    }
}

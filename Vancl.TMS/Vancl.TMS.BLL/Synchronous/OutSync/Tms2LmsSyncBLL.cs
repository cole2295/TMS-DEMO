using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Model.Log;
using System.Reflection;
using Vancl.TMS.Core.ServiceCache;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class Tms2LmsSyncBLL : BaseBLL, ITms2LmsSyncBLL
    {
        ITms2LmsSyncTMSDAL _tmsDAL = ServiceFactory.GetService<ITms2LmsSyncTMSDAL>("Tms2LmsSyncTMSDAL");
        IServiceLogBLL _serviceLogBLL = ServiceFactory.GetService<IServiceLogBLL>("ServiceLogBLL");

        /// <summary>
        /// 读取tms中间表数据
        /// </summary>
        /// <param name="count">每次读取数量</param>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        public List<BillChangeLogModel> ReadTmsChangeLogs(Tms2LmsJobArgs argument)
        {
            if (argument == null) throw new ArgumentNullException("Tms2LmsJobArgs is null");
            var listResult = _tmsDAL.ReadTmsChangeLogs(argument);
            if (listResult != null && listResult.Count > 0)
            {
                var orderlist = listResult.OrderBy(p => p.CreateTime).ThenBy(p => p.BCID);
                return orderlist.ToList();
            }
            return null;
        }

        /// <summary>
        /// 执行同步操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel DoSync(BillChangeLogModel model)
        {
            Tms2LmsStrategy strategy = GetStrategy(model.OperateType);
            ResultModel rm = ErrorResult("没有对应策略");
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
				throw ex;
            }
            finally
            {
                ServiceLogModel logModel = new ServiceLogModel();
                logModel.FormCode = model.FormCode;
                logModel.IsSuccessed = (rm == null) ? false : rm.IsSuccess;
                logModel.LogType = Enums.ServiceLogType.Tms2LmsSync;
                logModel.OpFunction = (int)model.OperateType;
                if (hasException)
                {
                    logModel.Note = BillSyncCache.GetInstance().GetExceptionMessage(model.FormCode);
                }
                else
                {
                    logModel.Note = (rm == null) ? "程序严重错误,ResultModel返回为null" : rm.Message;
                }
                logModel.SyncKey = model.BCID.ToString();
                BillSyncCache.GetInstance().RemoveBill(model.FormCode);
                //记录日志
                _serviceLogBLL.WriteLog(logModel);
                //更新tms同步状态
                _tmsDAL.UpdateSyncStatus(model.BCID, ((rm == null) ? false : rm.IsSuccess) ? Enums.SyncStatus.Already : Enums.SyncStatus.Error);
            }
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        private ResultModel Sync(BillChangeLogModel model, Tms2LmsStrategy strategy)
        {
            try
            {
                return strategy.DoSync(model);
            }
            catch (Exception ex)
            {
                //出错重试一定次数
                BillSyncCache.GetInstance().SetExceptions(model.FormCode, ex);
                int times = BillSyncCache.GetInstance().GetTryTimes(model.FormCode);
                if (times < Consts.SYNC_SERVICE_TRY_TIMES)
                {
                    BillSyncCache.GetInstance().SetTryTimes(model.FormCode, ++times);
                    return DoSync(model);
                }
                else
                {
                    //抛出错误
                    throw BillSyncCache.GetInstance().GetLastException(model.FormCode);
                }
            }
        }

        /// <summary>
        /// 根据操作类型实例化策略
        /// </summary>
        /// <param name="OperateType">操作类型</param>
        /// <returns></returns>
        private Tms2LmsStrategy GetStrategy(Enums.TmsOperateType OperateType)
        {
            Type type = Assembly.GetExecutingAssembly().GetType("Vancl.TMS.BLL.Synchronous.OutSync." + OperateType.ToString() + "Strategy");
            if (type == null)
            {
                return null;
            }
            return (Tms2LmsStrategy)Activator.CreateInstance(type);
        }

    }
}

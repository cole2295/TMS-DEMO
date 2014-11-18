using System.Collections.Generic;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Core.Logging;
using Vancl.TMS.IDAL.Log;
using System.Collections;
using System;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.BLL.Log.BillChangeLog;

namespace Vancl.TMS.BLL
{
    public class BaseBLL
    {
        /// <summary>
        /// 写新增的日志
        /// </summary>
        /// <typeparam name="T">操作对象</typeparam>
        /// <param name="model">对象实例</param>
        /// <returns></returns>
        protected virtual int WriteInsertLog<T>(T model) where T : BaseModel, ILogable, new()
        {
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.Insert, model, null);
            return WriteLog(wlm);
        }

        /// <summary>
        /// 写更新的日志
        /// </summary>
        /// <typeparam name="T">操作对象</typeparam>
        /// <param name="nowModel">操作对象实例</param>
        /// <param name="pastModel">操作对象前一实例</param>
        /// <returns></returns>
        protected virtual int WriteUpdateLog<T>(T nowModel, T pastModel) where T : BaseModel, ILogable, new()
        {
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.Update, nowModel, pastModel);
            return WriteLog(wlm);
        }

        /// <summary>
        /// 强行写更新日志(不需要同之前对象进行比较,强行记录更新日志)
        /// </summary>
        /// <typeparam name="T">操作对象实例</typeparam>
        /// <returns></returns>
        protected virtual int WriteForcedUpdateLog<T>(string key) where T : BaseModel, IForceLog, ILogable, new()
        {
            T t = new T();
            t.PrimaryKey = key;
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.Update, t, t);
            return WriteLog(wlm);
        }

        /// <summary>
        ///写自定义日志 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual int WriteCustomizeLog<T>(T opModel) where T : BaseModel, ICustomizeLogable, ILogable, new()
        {
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.Customize, opModel, opModel);
            return WriteLog(wlm);
        }

        protected virtual int WriteDeleteLog<T>(T model) where T : BaseModel, ILogable, new()
        {
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.Delete, model, null);
            return WriteLog(wlm);
        }

        protected virtual int WriteBatchDeleteLog<T>(ICollection ids) where T : BaseModel, ILogable, new()
        {
            if (ids == null || ids.Count == 0)
            {
                return 0;
            }
            T t = new T();
            int i = 0;
            foreach (object id in ids)
            {
                t.PrimaryKey = Convert.ToString(id);
                i = i + WriteDeleteLog<T>(t);
            }
            return i;
        }

        /// <summary>
        /// 启用停用日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        protected virtual int WriteSetEnableLog<T>(string id, bool isEnabled) where T : BaseModel, ILogable, ICanSetEnable, new()
        {
            T t = new T();
            t.IsEnabled = isEnabled;
            t.LogKeyValue = id;
            WriteLogModel wlm = BuildWriteLogModel<T>(Enums.LogOperateType.SetEnable, t, null);
            return WriteLog(wlm);
        }

        private WriteLogModel BuildWriteLogModel<T>(Enums.LogOperateType operateType, T nowModel, T pastModel) where T : BaseModel, ILogable, new()
        {
            if (nowModel == null) throw new CodeNotValidException();
            if (operateType == Enums.LogOperateType.Update && pastModel == null) throw new CodeNotValidException();
            if (!(nowModel is ILogable))
            {
                return null;
            }
            if (nowModel.GetType().FullName != typeof(T).FullName)
            {
                nowModel = VanclConverter.ConvertModel<T, T>(nowModel);
            }
            WriteLogModel wlm = new WriteLogModel();
            if (nowModel is IOperateLogable)
            {
                LogOperate<T> ln = new LogOperate<T>(operateType);
                ln.SetNowModel(nowModel);
                ln.SetPastModel(pastModel);
                if (ln.IsDoOperation)
                {
                    OperateLogModel olm = new OperateLogModel();
                    olm.OperateType = operateType;
                    olm.TableName = nowModel.ModelTableName;
                    olm.KeyValue = ln.PrimaryKey;
                    olm.Note = ln.LogNote;
                    wlm.OperateLog = olm;
                }
            }
            if (nowModel is IDeliveryLogable)
            {
                if (operateType == Enums.LogOperateType.Delete)
                {
                    nowModel.IsDeleted = true;
                }
                LogOperate<T> ln = new LogOperate<T>(new DeliveryLogStrategy<T>());
                ln.SetNowModel(nowModel);
                ln.SetPastModel(pastModel);
                if (ln.IsDoOperation)
                {
                    DeliveryFlowLogModel dflm = new DeliveryFlowLogModel();
                    dflm.FlowType = ln.FlowType;
                    dflm.Note = ln.LogNote;
                    dflm.IsShow = ln.IsShow;
                    dflm.DeliveryNO = ln.DeliveryNo;
                    wlm.DeliveryFlowLog = dflm;
                }
            }
            return wlm;
        }

        private int WriteLog(WriteLogModel wlm)
        {
            if (wlm == null)
            {
                return 0;
            }
            int i = 0;
            if (wlm.DeliveryFlowLog != null)
            {
                ILogDAL<DeliveryFlowLogModel> dal = ServiceFactory.GetService<ILogDAL<DeliveryFlowLogModel>>("DeliveryFlowLogDAL");
                i = dal.Write(wlm.DeliveryFlowLog);
            }
            if (wlm.OperateLog != null)
            {
                ILogDAL<OperateLogModel> dal = ServiceFactory.GetService<ILogDAL<OperateLogModel>>("OperateLogDAL");
                i = dal.Write(wlm.OperateLog);
            }
            return i;
        }

        protected virtual List<T> ReadLog<T>(BaseLogSearchModel searchModel) where T : BaseLogModel, new()
        {
            ILogDAL<T> _dal = ServiceFactory.GetService<ILogDAL<T>>("OperateLogDAL");
            return _dal.Read(searchModel);
        }

        /// <summary>
        /// 新增操作的返回值
        /// </summary>
        /// <param name="affectCount">影响行数</param>
        /// <param name="name">内容名称</param>
        /// <returns></returns>
        protected virtual ResultModel AddResult(int affectCount, string name)
        {
            ResultModel rm = new ResultModel();
            if (affectCount > 0)
            {
                rm.IsSuccess = true;
                rm.Message = string.Format(Consts.ADD_RESULT_SUCCESS_MESSAGE, name);
            }
            else
            {
                rm.IsSuccess = false;
                rm.Message = string.Format(Consts.ADD_RESULT_FAILED_MESSAGE, name);
            }
            return rm;
        }

        /// <summary>
        /// 更新操作的返回值
        /// </summary>
        /// <param name="affectCount">影响行数</param>
        /// <param name="name">内容名称</param>
        /// <returns></returns>
        protected virtual ResultModel UpdateResult(int affectCount, string name)
        {
            ResultModel rm = new ResultModel();
            if (affectCount > 0)
            {
                rm.IsSuccess = true;
                rm.Message = string.Format(Consts.UPDATE_RESULT_SUCCESS_MESSAGE, name);
            }
            else
            {
                rm.IsSuccess = false;
                rm.Message = string.Format(Consts.UPDATE_RESULT_FAILED_MESSAGE, name);
            }
            return rm;
        }

        /// <summary>
        /// 删除操作的返回值
        /// </summary>
        /// <param name="affectCount">影响行数</param>
        /// <param name="name">内容名称</param>
        /// <returns></returns>
        protected virtual ResultModel DeleteResult(int affectCount, string name)
        {
            ResultModel rm = new ResultModel();
            if (affectCount > 0)
            {
                rm.IsSuccess = true;
                rm.Message = string.Format(Consts.DELETE_RESULT_SUCCESS_MESSAGE, affectCount, name);
            }
            else
            {
                rm.IsSuccess = false;
                rm.Message = string.Format(Consts.DELETE_RESULT_FAILED_MESSAGE, name);
            }
            return rm;
        }

        /// <summary>
        /// 出错返回值
        /// </summary>
        /// <param name="message">出错信息</param>
        /// <returns></returns>
        protected virtual ResultModel ErrorResult(string message)
        {
            return new ResultModel() { IsSuccess = false, Message = message };
        }

        /// <summary>
        /// 提示返回值
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected virtual ResultModel InfoResult(string message)
        {
            return new ResultModel() { IsSuccess = true, Message = message };
        }

        /// <summary>
        /// 写状态改变日志
        /// </summary>
        /// <param name="dynamicModel"></param>
        /// <returns></returns>
        protected virtual int WriteBillChangeLog(BillChangeLogDynamicModel dynamicModel)
        {
            if (dynamicModel == null) throw new ArgumentNullException("BillChangeLogDynamicModel is null");
            BillChangeLogBLL bll = BillChangeLogFactory.GetBillChangeLogBLL(dynamicModel.OperateType);
            return bll.AddLog(dynamicModel);
        }
        /// <summary>
        /// 批量写状态改变日志
        /// </summary>
        /// <param name="listdynamicModel"></param>
        /// <returns></returns>
        protected virtual int WriteBillChangeLog_Batch(List<BillChangeLogDynamicModel> listdynamicModel)
        {
            if (listdynamicModel == null) throw new ArgumentNullException("listdynamicModel is null");
            BillChangeLogBLL bll = BillChangeLogFactory.GetBillChangeLogBLL(listdynamicModel[0].OperateType);
            return bll.BatchAddLog(listdynamicModel);
        }
    }
}

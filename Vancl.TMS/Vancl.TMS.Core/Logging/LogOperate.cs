using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model;

namespace Vancl.TMS.Core.Logging
{
    /// <summary>
    /// 日志操作管理类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogOperate<T> where T : BaseModel, ILogable, new()
    {
        LogStrategy<T> _ols;
        public LogOperate(LogStrategy<T> strategy)
        {
            _ols = strategy;
        }

        public LogOperate(Enums.LogOperateType operateType)
        {
            switch (operateType)
            {
                case Enums.LogOperateType.Insert:
                    _ols = new InsertLogStrategy<T>();
                    break;
                case Enums.LogOperateType.Update:
                    _ols = new UpdateLogStrategy<T>();
                    break;
                case Enums.LogOperateType.Delete:
                    _ols = new DeleteLogStrategy<T>();
                    break;
                case Enums.LogOperateType.SetEnable:
                    _ols = new SetEnableLogStrategy<T>();
                    break;
                case Enums.LogOperateType.Customize:
                    _ols = new CustomizeLogStrategy<T>();
                    break;
                default:
                    break;
            }
        }

        public void SetNowModel(T nowModel)
        {
            _ols.NowModel = nowModel;
        }

        public void SetPastModel(T pastModel)
        {
            _ols.PastModel = pastModel;
        }

        public string LogNote
        {
            get
            {
                if (_ols == null)
                {
                    return string.Empty;
                }
                return _ols.GetLogNote();
            }
        }

        public bool IsDoOperation
        {
            get
            {
                if (_ols == null)
                {
                    return false;
                }
                return _ols.IsDoOperation();
            }
        }

        public Enums.DeliverFlowType FlowType
        {
            get
            {
                if (_ols == null)
                {
                    return Enums.DeliverFlowType.None;
                }
                return _ols.GetFlowType();
            }
        }

        public bool IsShow
        {
            get
            {
                if (_ols == null)
                {
                    return true;
                }
                return _ols.GetIsShow();
            }
        }

        public string DeliveryNo
        {
            get
            {
                if (_ols == null)
                {
                    return "";
                }
                return _ols.GetDeliveryNo();
            }
        }

        public string PrimaryKey
        {
            get
            {
                if (_ols == null)
                {
                    return string.Empty;
                }
                return _ols.GetPrimaryKey();
            }
        }
    }
}

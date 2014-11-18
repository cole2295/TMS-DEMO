using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Core.Logging
{
    /// <summary>
    /// 日志策略抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LogStrategy<T> where T : BaseModel, ILogable, new()
    {
        /// <summary>
        /// 当前操作对象
        /// </summary>
        protected T _nowModel = null;
        public T NowModel { set { _nowModel = value; } }

        /// <summary>
        /// 之前操作对象
        /// </summary>
        protected T _pastModel = null;
        public T PastModel { set { _pastModel = value; } }

        /// <summary>
        /// 日志描述
        /// </summary>
        protected string _note = "";

        public abstract string GetLogNote();

        /// <summary>
        /// 是否记录日志
        /// </summary>
        /// <returns></returns>
        public abstract bool IsDoOperation();

        public virtual Enums.DeliverFlowType GetFlowType()
        {
            return Enums.DeliverFlowType.None;
        }

        public virtual bool GetIsShow()
        {
            return true;
        }

        /// <summary>
        /// 取得提货单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetDeliveryNo()
        {
            return "";
        }

        /// <summary>
        /// 取得主键
        /// </summary>
        /// <returns></returns>
        public virtual string GetPrimaryKey()
        {
            return _nowModel.PrimaryKey;
        }
    }
}

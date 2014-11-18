using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Transport.Plan
{
    /// <summary>
    /// 运输计划
    /// </summary>
    [Serializable]
    [LogNameAttribute("运输计划")]
    public class TransportPlanModel : BaseModel, IOperateLogable, ISequenceable, ICanSetEnable, ICustomizeLogable
    {
        #region ILogable 成员

        [ColumnNot4Log]
        public string PrimaryKey
        {
            get { return TPID.ToString(); }
            set { TPID = int.Parse(value); }
        }

        #endregion

        #region ISequenceable 成员

        [ColumnNot4Log]
        public string SequenceName
        {
            get { return "SEQ_TMS_TransportPlan_TPID"; }
        }

        #endregion

        /// <summary>
        /// 主键标识
        /// </summary>
        public virtual int TPID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [LogName("出发地")]
        public virtual int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [LogName("目的地")]
        public virtual int ArrivalID { get; set; }

        /// <summary>
        /// 线路货物类型
        /// </summary>
        [LogName("货物类型")]
        public virtual Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 是否中转
        /// </summary>
        [LogName("货物类型")]
        public virtual bool IsTransit { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        [LogName("启用状态")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 运输计划状态
        /// </summary>
        [LogName("运输计划状态")]
        public Enums.TransportStatus Status { get; set; }

        /// <summary>
        /// 中转站点
        /// </summary>
        [LogName("中转站点")]
        public virtual int? TransferStation { get; set; }
        /// <summary>
        /// 多次中转站点
        /// </summary>
        [LogName("多次中转站点")]
        public virtual string TransferStationMulti { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [LogName("生效时间")]
        public DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        [LogName("截至日期")]
        public virtual DateTime Deadline { get; set; }

        #region ICanSetEnable 成员

        private string _logKeyValue = "";

        [ColumnNot4Log]
        public string LogKeyValue
        {
            get { return _logKeyValue; }
            set { _logKeyValue = value; }
        }

        #endregion

        #region ICustomizeLogable 成员

        [ColumnNot4Log]
        public string CustomizeLog
        {
            get;
            set;
        }

        #endregion
    }
}

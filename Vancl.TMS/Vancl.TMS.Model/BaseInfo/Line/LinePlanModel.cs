using System;
using System.Collections.Generic;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路计划
    /// </summary>
    [LogName("线路计划")]
    [Serializable]
    public class LinePlanModel : BaseModel, IOperateLogable, ISequenceable, ICanSetEnable, ICustomizeLogable
    {
        public string PrimaryKey
        {
            get { return LPID.ToString(); }
            set { LPID = int.Parse(value); }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public virtual int LPID { get; set; }

        /// <summary>
        /// 线路编号
        /// </summary>
        [LogName("线路编号")]
        public virtual string LineID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [LogName("出发地")]
        public virtual int DepartureID { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        [ColumnNot4Log]
        public virtual string DepartureName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [LogName("目的地")]
        public virtual int ArrivalID { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
         [ColumnNot4Log]
        public virtual string ArrivalName { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        [LogName("承运商ID")]
        public virtual int CarrierID { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
         [ColumnNot4Log]
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [LogName("运输方式")]
        public virtual Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 提货到库考核点
        /// </summary>
        [LogName("提货到库考核点")]
        [LogTimeCompare]
        public virtual DateTime ArrivalAssessmentTime { get; set; }

        /// <summary>
        /// 离库考核点
        /// </summary>
        [LogName("离库考核点")]
        [LogTimeCompare]
        public virtual DateTime LeaveAssessmentTime { get; set; }

        /// <summary>
        /// 到货时效
        /// </summary>
        [LogName("到货时效")]
        public virtual decimal ArrivalTiming { get; set; }

        /// <summary>
        /// 保险费率
        /// </summary>
        [LogName("保险费率")]
        public virtual decimal InsuranceRate { get; set; }

        /// <summary>
        /// 最低收费
        /// </summary>
        [LogName("最低收费")]
        public virtual decimal LowestPrice { get; set; }

        /// <summary>
        /// 超远送货费用
        /// </summary>
        [LogName("超远送货费用")]
        public virtual decimal LongDeliveryPrice { get; set; }

        /// <summary>
        /// 超远提货费用
        /// </summary>
        [LogName("超远提货费用")]
        public virtual decimal LongPickPrice { get; set; }

        /// <summary>
        /// 超远运转费率
        /// </summary>
        [LogName("超远运转费率")]
        public virtual decimal LongTransferRate { get; set; }

        /// <summary>
        /// 优先级别
        /// </summary>
        [LogName("优先级别")]
        public virtual Enums.LinePriority Priority { get; set; }

        /// <summary>
        /// 线路状态
        /// </summary>
        [LogName("线路状态")]
        public virtual Enums.LineStatus Status { get; set; }

        /// <summary>
        /// 最后审核人
        /// </summary>
        [LogName("审核人")]
        public virtual int ApproveBy { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [LogName("审核时间")]
        public virtual DateTime ApproveTime { get; set; }

        /// <summary>
        /// 线路类型
        /// </summary>
        [LogName("线路类型")]
        public virtual Enums.LineType LineType { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        [LogName("货物类型")]
        public virtual Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 运费类型
        /// </summary>
        [LogName("运费类型")]
        public virtual Enums.ExpressionType ExpressionType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [LogName("生效时间")]
        [LogDateCompare]
        public virtual DateTime EffectiveTime { get; set; }

        /// <summary>
        /// 营业类型
        /// </summary>
        [LogName("营业类型")]
        public virtual Enums.BusinessType BusinessType { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        [LogName("是否启用")]
        public virtual bool IsEnabled { get; set; }

        #region ISequenceable 成员

        [ColumnNot4Log]
        public string SequenceName
        {
            get { return "SEQ_TMS_LinePlan_LPID"; }
        }

        #endregion

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

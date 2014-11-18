using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度表
    /// </summary>
    [Serializable]
    public class PreDispatchModel : BaseModel, ISequenceable
    {
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_PreDispatch_PDID"; }
        }

        #endregion

        /// <summary>
        /// 预调度表主键
        /// </summary>
        public long PDID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 运输计划
        /// </summary>
        public int TPID { get; set; }

        /// <summary>
        /// 线路计划ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int SeqNo { get; set; }

        /// <summary>
        /// 调度状态
        /// </summary>
        public Enums.DispatchStatus DispatchStatus { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType LineGoodsType { get; set; }
    }
}

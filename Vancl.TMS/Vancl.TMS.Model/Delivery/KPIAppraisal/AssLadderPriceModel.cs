using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// 提货单考核阶梯价格信息
    /// </summary>
    [Serializable]
    [LogNameAttribute("提货单考核阶梯价格")]
    public class AssLadderPriceModel : BaseModel, ISequenceable, IAssPriceModel, IForceLog, IOperateLogable
    {
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_AssLadderPrice_ALPID"; }
        }

        #endregion

        /// <summary>
        /// 主键标识
        /// </summary>
        public long ALPID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 最小重量
        /// </summary>
        public int StartWeight { get; set; }

        /// <summary>
        /// 最大重量
        /// </summary>
        public int? EndWeight { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }

        #region IForceLogNull 成员

        public string PrimaryKey
        {
            get
            {
                return DeliveryNo;
            }
            set
            {
                DeliveryNo = value;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// 提货单考核公式价格续价
    /// </summary>
    [Serializable]
    [LogNameAttribute("提货单考核公式价格续价")]
    public class AssFormulaPriceExModel : BaseModel, ISequenceable, IForceLog, IOperateLogable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int AFEID { get; set; }

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
        /// 单价
        /// </summary>
        public decimal Price { get; set; }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_ASSFORMULAEX_AFEID"; }
        }

        #endregion

        #region ILogable 成员

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

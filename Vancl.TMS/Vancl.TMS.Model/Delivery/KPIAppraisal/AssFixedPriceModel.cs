using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// 提货单考核固定价格信息
    /// </summary>
    [Serializable]
    public class AssFixedPriceModel : BaseModel, IOperateLogable, IAssPriceModel
    {
        public string PrimaryKey
        {
            get { return DeliveryNo; }
            set { DeliveryNo = value; }
        }

        /// <summary>
        /// 提货单号(主键)
        /// </summary>
        [LogName("提货单号")]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [LogName("价格")]
        public decimal Price { get; set; }

    }
}

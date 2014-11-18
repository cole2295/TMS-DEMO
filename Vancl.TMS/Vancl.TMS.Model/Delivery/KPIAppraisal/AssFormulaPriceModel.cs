using System;
using System.Collections.Generic;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// 提货单考核公式价格信息
    /// </summary>
    [Serializable]
    public class AssFormulaPriceModel : BaseModel, IOperateLogable, IAssPriceModel
    {
        #region ILogable 成员

        public string PrimaryKey
        {
            get { return DeliveryNo; }
            set { DeliveryNo = value; }
        }

        #endregion

        /// <summary>
        /// 提货单号(主键)
        /// </summary>
        [LogName("提货单号")]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 基价
        /// </summary>
        [LogName("基价")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 基重
        /// </summary>
        [LogName("基重")]
        public int BaseWeight { get; set; }

        /// <summary>
        /// 续价(字段已作废-2012-04-11)
        /// </summary>
        [LogName("续价")]
        public decimal OverPrice { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [LogName("描述")]
        public string Note { get; set; }

        /// <summary>
        /// 续价明细列表
        /// </summary>
        [ColumnNot4LogAttribute]
        public List<AssFormulaPriceExModel> Detail { get; set; }
    }
}

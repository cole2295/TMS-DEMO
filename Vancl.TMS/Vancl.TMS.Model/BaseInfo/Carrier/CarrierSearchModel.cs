using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Carrier
{
    public class CarrierSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 承运商名称
        /// </summary>
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        public virtual string CarrierAllName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string Contacter { get; set; }

        /// <summary>
        /// 适用范围：1.全国
        /// </summary>
        public virtual bool IsAllCoverage { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual Enums.CarrierStatus? Status { get; set; }

        /// <summary>
        /// 合同有效期
        /// </summary>
        public virtual DateTime ExpiredDate { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public virtual string ContractNumber { get; set; }
    }
}

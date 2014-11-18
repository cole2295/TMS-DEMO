using System;
using System.Collections.Generic;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.BaseInfo.Carrier
{
    /// <summary>
    /// 承运商
    /// </summary>
    [LogName("承运商")]
    [Serializable]
    public class CarrierModel : BaseModel, IOperateLogable, ISequenceable, ICanSetEnable
    {
        /// <summary>
        /// 承运商主键ID
        /// </summary>
        public virtual int CarrierID { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        [LogName("承运商名称")]
        public virtual string CarrierName { get; set; }

        /// <summary>
        /// 承运商全名
        /// </summary>
        [LogName("承运商全名")]
        public virtual string CarrierAllName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [LogName("联系人")]
        public virtual string Contacter { get; set; }

        /// <summary>
        /// 适用范围：1.全国
        /// </summary>
        [LogName("是否全国适用")]
        public virtual bool IsAllCoverage { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [LogName("联系电话")]
        public virtual string Phone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [LogName("联系地址")]
        public virtual string Address { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [LogName("电子邮件")]
        public virtual string Email { get; set; }

        /// <summary>
        /// 承运商状态
        /// </summary>
        [LogName("承运商状态")]
        public virtual Enums.CarrierStatus Status { get; set; }

        /// <summary>
        /// 合同有效期
        /// </summary>
        [LogName("合同有效期")]
        public virtual DateTime ExpiredDate { get; set; }

        /// <summary>
        /// 承运商编号
        /// </summary>
        [LogName("承运商编号")]
        public virtual string CarrierNo { get; set; }

        /// <summary>
        /// 配送商编码
        /// </summary>
        [LogName("配送商编码")]
        public virtual string DistributionCode { get; set; }

        /// <summary>
        /// 合同起始时间
        /// </summary>
        [LogName("合同起始时间")]
        public virtual DateTime BeginDate { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        [LogName("合同编号")]
        public virtual string ContractNumber { get; set; }

        public string PrimaryKey
        {
            get { return CarrierID.ToString(); }
            set { CarrierID = Convert.ToInt32(value); }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_CARRIER_CARRIERID"; }
        }

        #endregion

        #region ICanSetEnable 成员

        /// <summary>
        /// 是否启用
        /// </summary>
        [LogName("是否启用")]
        public bool IsEnabled
        {
            get
            {
                return this.Status == Enums.CarrierStatus.Valid ? true : false;
            }
            set
            {
                this.Status = value ? Enums.CarrierStatus.Valid : Enums.CarrierStatus.Invalid;
            }
        }

        private string _logKeyValue = "";
        public string LogKeyValue
        {
            get { return _logKeyValue; }
            set { _logKeyValue = value; }
        }
        #endregion
    }
}

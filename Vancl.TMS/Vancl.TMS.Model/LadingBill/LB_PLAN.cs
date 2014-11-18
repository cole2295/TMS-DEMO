using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.LadingBill
{
    /// <summary>
    /// 提货计划 
    /// </summary>
    [Serializable]
    public partial class LB_PLAN : BaseModel, ISequenceable
    {
        #region model 信息
        /// <summary>
        /// 主键ID，自动增长
        /// </summary>
        [DataMember]
        [Display(Name = "主键ID，自动增长")]
        public System.Decimal ID { get; set; }
        /// <summary>
        /// 商家ID
        /// </summary>
        [DataMember]
        [Display(Name = "商家ID")]
        public System.Decimal MERCHANTID { get; set; }
        /// <summary>
        /// 库房ID
        /// </summary>
        [DataMember]
        [Display(Name = "库房")]
        [Required]
        public System.String WAREHOUSEID { get; set; }
        /// <summary>
        /// 配送商注册编码   来至
        /// </summary>
        [DataMember]
        [Display(Name = "配送商注册编码   来至")]
        public System.String FROMDISTRIBUTIONCODE { get; set; }
        /// <summary>
        /// 提货部门
        /// </summary>
        [DataMember]
        [Display(Name = "提货部门")]
        [StringLength(50)]
        public System.String DEPARTMENT { get; set; }
        /// <summary>
        /// 预计单量
        /// </summary>
        [DataMember]
        [Display(Name = "预计提货单量")]
        [DataType(DataType.Currency)]
        public Nullable<System.Decimal> ORDERQUANTITY { get; set; }
        /// <summary>
        /// 预计提货重量
        /// </summary>
        [DataMember]
        [Display(Name = "预计提货重量")]
        [DataType(DataType.Currency)]
        public Nullable<System.Decimal> PREDICTWEIGHT { get; set; }
        /// <summary>
        /// 公里数
        /// </summary>
        [DataMember]
        [Display(Name = "公里数")]
        [DataType(DataType.Currency)]
        public Nullable<System.Decimal> MILEAGE { get; set; }
        /// <summary>
        /// 提货价格类型，0次数计费，1单量计费
        /// </summary>
        [DataMember]
        [Display(Name = "提货价格类型，0次数计费，1单量计费")]
        public System.Decimal PRICETYPE { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        [Display(Name = "金额")]
        [DataType(DataType.Currency)]
        public Nullable<System.Double> AMOUNT { get; set; }

        /// <summary>
        /// 1每天、2工作日、3双休日、4星期、5不确定
        /// </summary>
        [DataMember]
        [Display(Name = "1每天、2工作日、3双休日、4星期、5不确定")]
        public System.Decimal TIMETYPE { get; set; }
        /// <summary>
        /// 周一，周二，...周六，周日
        /// </summary>
        [DataMember]
        [Display(Name = "周一，周二，...周六，周日")]
        public System.String WEEK { get; set; }
        /// <summary>
        /// 具体时间
        /// </summary>
        [DataMember]
        [Display(Name = "具体时间")]
        [DataType(DataType.Time)]
        public System.String SPECIFICTIME { get; set; }

        /// <summary>
        /// 0 启用  1禁用
        /// </summary>
        [DataMember]
        [Display(Name = "0 启用  1禁用")]
        public Nullable<System.Decimal> ISENABLED { get; set; }
        /// <summary>
        /// 接受任务邮箱
        /// </summary>
        [DataMember]
        [Display(Name = "任务接收邮箱")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public System.String RECEIVEMAIL { get; set; }

        /// <summary>
        /// 创建人所在部门
        /// </summary>
        [DataMember]
        [Display(Name = "创建人所在部门")]
        public Nullable<System.Decimal> CREATSTATION { get; set; }
        /// <summary>
        /// 指派  配送商注册编码 
        /// </summary>
        [DataMember]
        [Display(Name = "指派  配送商注册编码 ")]
        public System.String TODISTRIBUTIONCODE { get; set; }
        /// <summary>
        /// 是否生成 0 没有生成   1 生成
        /// </summary>
        [DataMember]
        [Display(Name = "是否生成 0 没有生成   1 生成")]
        public Nullable<System.Decimal> ISCREATED { get; set; }
        #endregion


        public string SequenceName
        {
            get { return "SEQ_LB_PLAN_ID"; }
        }
    }
}


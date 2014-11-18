using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.LadingBill
{
    /// <summary>
    /// 提货任务
    /// </summary>
    public class LB_TASK : ISequenceable
    {
        #region Model
        /// <summary>
        /// 主键ID
        /// </summary>
        [DataMember]
        [Display(Name = "主键ID")]
        public System.Decimal ID { get; set; }
        /// <summary>
        /// 任务编码
        /// </summary>
        [DataMember]
        [Display(Name = "任务编码")]
        public System.String TASKCODE { get; set; }
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
        [Display(Name = "库房ID")]
        public System.String WAREHOUSEID { get; set; }
        /// <summary>
        /// 配送商注册编码（来至）
        /// </summary>
        [DataMember]
        [Display(Name = "配送商注册编码（来至）")]
        public System.String FROMDISTRIBUTIONCODE { get; set; }
        /// <summary>
        /// 配送商注册编码（指定）
        /// </summary>
        [DataMember]
        [Display(Name = "配送商注册编码（指定）")]
        public System.String TODISTRIBUTIONCODE { get; set; }
        /// <summary>
        /// 提货部门
        /// </summary>
        [DataMember]
        [Display(Name = "提货部门")]
        public System.String DEPARTMENT { get; set; }


        /// <summary>
        /// 实际提货时间
        /// </summary>
        [DataMember]
        [Display(Name = "实际提货时间")]
        public Nullable<System.DateTime> PLANTIME { get; set; }

        /// <summary>
        /// 公里数
        /// </summary>
        [DataMember]
        [Display(Name = "公里数")]
        public Nullable<System.Decimal> MILEAGE { get; set; }
        /// <summary>
        /// 提货价格类型，0次数计费，1单量计费
        /// </summary>
        [DataMember]
        [Display(Name = "提货价格类型，0次数计费，1单量计费")]
        public System.Decimal PICKPRICETYPE { get; set; }
        /// <summary>
        /// 多少钱一次
        /// </summary>
        [DataMember]
        [Display(Name = "多少钱一次")]
        public decimal ONCEAMOUNT { get; set; }
        /// <summary>
        /// 多少钱一单
        /// </summary>
        [DataMember]
        [Display(Name = "多少钱一单")]
        public Nullable<System.Double> ORDERAMOUNT { get; set; }
        /// <summary>
        /// 1新任务 2已完成  3已考核 4取消 
        /// </summary>
        [DataMember]
        [Display(Name = "1新任务 2已完成  3已考核 4取消 ")]
        public Nullable<System.Decimal> TASKSTATUS { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        [Display(Name = "创建时间")]
        public Nullable<System.DateTime> CREATETIME { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataMember]
        [Display(Name = "更新时间")]
        public Nullable<System.DateTime> UPDATETIME { get; set; }
        /// <summary>
        /// 任务接受邮箱
        /// </summary>
        [DataMember]
        [Display(Name = "任务接受邮箱")]
        public System.String RECEIVEEMAIL { get; set; }
        /// <summary>
        /// 邮件发送时间
        /// </summary>
        [DataMember]
        [Display(Name = "邮件发送时间")]
        public Nullable<System.DateTime> RECEIVEEMAILTIME { get; set; }
        /// <summary>
        /// 提货费用
        /// </summary>
        [DataMember]
        [Display(Name = "提货费用")]
        public decimal PICKGOODSAMOUNT { get; set; }
        /// <summary>
        /// 提货完成时间
        /// </summary>
        [DataMember]
        [Display(Name = "提货完成时间")]
        public Nullable<System.DateTime> FINISHTIME { get; set; }
        /// <summary>
        /// 实际单量
        /// </summary>
        [DataMember]
        [Display(Name = "实际单量")]
        public Nullable<System.Decimal> ORDERQUANTITY { get; set; }
        /// <summary>
        /// 实际提货重量
        /// </summary>
        [DataMember]
        [Display(Name = "实际提货重量")]
        public Nullable<System.Decimal> WEIGHT { get; set; }
        /// <summary>
        /// 考核费用       相加，如果为负数则是需扣得金额
        /// </summary>
        [DataMember]
        [Display(Name = "考核费用       相加，如果为负数则是需扣得金额")]
        public System.String KPIAMOUNT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        [Display(Name = "备注")]
        public System.String REMARK { get; set; }
        /// <summary>
        /// 删除标志 0 正常  1已删除
        /// </summary>
        [DataMember]
        [Display(Name = "删除标志 0 正常  1已删除")]
        public Nullable<System.Decimal> ISDELETED { get; set; }
        /// <summary>
        /// 启用标志 0 启用  1禁用
        /// </summary>
        [DataMember]
        [Display(Name = "启用标志 0 启用  1禁用")]
        public Nullable<System.Decimal> ISENABLED { get; set; }
        /// <summary>
        /// 是否打印 0未打印  1已打印
        /// </summary>
        [DataMember]
        [Display(Name = "是否打印 0未打印  1已打印")]
        public Nullable<System.Decimal> ISPRINT { get; set; }
        #endregion Model

        /// <summary>
        /// 预计提货时间
        /// </summary>
        public DateTime PREDICTTIME { get; set; }

        /// <summary>
        /// 提货完成时间
        /// </summary>
        public DateTime TASKTIME { get; set; }

        /// <summary>
        /// 预计提货重量
        /// </summary>
        public Nullable<System.Decimal> PREDICTWEIGHT { get; set; }

        /// <summary>
        /// 预计单量
        /// </summary>
        public Nullable<System.Decimal> PREDICTORDERQUANTITY { get; set; }

        /// <summary>
        /// 提货人
        /// </summary>
        public string PICKMAN { get; set; }

        /// <summary>
        /// 仓库地址
        /// </summary>
        public string WAREHOUSEADDRESS { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WAREHOUSENAME { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string MERCHANTNAME { get; set; }

        /// <summary>
        /// 提货公司名称
        /// </summary>
        public string DISTRIBUTIONNAME { get; set; }

        public string SequenceName
        {
            get { return "SEQ_LB_TASK_ID"; }
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Vancl.TMS.Model.LadingBill
{
    /// <summary>
    /// 提货计划 
    /// </summary>
    [Serializable]
    public class LB_PLANDTO : BaseSearchModel
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
        /// 商家名称
        /// </summary>
        [DataMember]
        [Display(Name = "商家名称")]
        public System.String MERCHANTNAME { get; set; }

        /// <summary>
        /// 库房ID
        /// </summary>
        [DataMember]
        [Display(Name = "库房ID")]
        public System.String WAREHOUSEID { get; set; }

        /// <summary>
        /// 库房名称
        /// </summary>
        [DataMember]
        [Display(Name = "库房名称")]
        public System.String WAREHOUSENAME { get; set; }

        /// <summary>
        /// 库房地址
        /// </summary>
        [DataMember]
        [Display(Name = "库房地址")]
        public System.String WAREHOUSEADDRESS { get; set; }


        /// <summary>
        /// 配送商注册编码   来至
        /// </summary>
        [DataMember]
        [Display(Name = "配送商注册编码   来至")]
        public System.String FROMDISTRIBUTIONCODE { get; set; }

        /// <summary>
        /// 配送商注册名称  来至
        /// </summary>
        [DataMember]
        [Display(Name = "配送商注册名称   来至")]
        public System.String FROMDISTRIBUTIONNAME { get; set; }

        /// <summary>
        /// 提货部门
        /// </summary>
        [DataMember]
        [Display(Name = "提货部门")]
        public System.String DEPARTMENT { get; set; }

        /// <summary>
        /// 预计单量
        /// </summary>
        [DataMember]
        [Display(Name = "预计提货单量")]
        public Nullable<System.Decimal> ORDERQUANTITY { get; set; }

        /// <summary>
        /// 预计提货重量
        /// </summary>
        [DataMember]
        [Display(Name = "预计提货重量")]
        public Nullable<System.Decimal> PREDICTWEIGHT { get; set; }

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
        public System.Decimal PRICETYPE { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        [Display(Name = "金额")]
        public Nullable<System.Double> AMOUNT { get; set; }


        /// <summary>
        /// 提货时间描述
        /// </summary>
        [DataMember]
        [Display(Name = "提货时间描述")]
        public System.String WEEKS { get; set; }


        /// <summary>
        /// 价格描述
        /// </summary>
        [DataMember]
        [Display(Name = "多少钱一单")]
        public System.String AMOUNTDESC { get; set; }

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
        /// 具体时间，  时分秒
        /// </summary>
        [DataMember]
        [Display(Name = "具体时间，  时分秒")]
        public System.String SPECIFICTIME { get; set; }

        /// <summary>
        /// 具体时间，  时分秒
        /// </summary>
        [DataMember]
        [Display(Name = "具体时间，  时分秒")]
        public System.String ISDELETEDNAME { get; set; }

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
        /// 指派  配送商注册编码 
        /// </summary>
        [DataMember]
        [Display(Name = "指派  配送商注册编码 ")]
        public System.String TODISTRIBUTIONNAME { get; set; }

        /// <summary>
        /// 是否生成 0 没有生成   1 生成
        /// </summary>
        [DataMember]
        [Display(Name = "是否生成 0 没有生成   1 生成")]
        public Nullable<System.Decimal> ISCREATED { get; set; }

        [DataMember]
        [Display(Name = "排序 ")]
        public System.String OrderByString { get; set; }

        #endregion
    }
}

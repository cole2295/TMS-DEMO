using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Carrier;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    [Bind(Exclude = "CarrierID")]
    public class CarrierViewModel : CarrierModel
    {
        public CarrierViewModel()
        {
            this.ExpiredDate = DateTime.Now.Date;
        }
        public CarrierViewModel(CarrierModel model)
        {
            this.Address = model.Address;
            this.CarrierAllName = model.CarrierAllName;
            this.CarrierID = model.CarrierID;
            this.CarrierName = model.CarrierName;
            this.Contacter = model.Contacter;
            this.CreateBy = model.CreateBy;
            this.CreateTime = model.CreateTime;
            this.Email = model.Email;
            this.ExpiredDate = model.ExpiredDate;
            this.IsAllCoverage = model.IsAllCoverage;
            this.IsDeleted = model.IsDeleted;
            this.Phone = model.Phone;
            this.Status = model.Status;
            this.CarrierNo = model.CarrierNo;
            this.UpdateBy = model.UpdateBy;
            this.UpdateTime = model.UpdateTime;
            this.DistributionCode = model.DistributionCode;
            this.BeginDate = model.BeginDate;
            this.ContractNumber = model.ContractNumber;
        }

        [ScaffoldColumn(false)]
        [Display(Name = "承运商ID")]
        public override int CarrierID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "承运商编号")]
        public override string CarrierNo { get; set; }

        [Required]
        [Remote("CheckCarrierName", "Carrier", ErrorMessage = "已存在该承运商")]
        [StringLength(15)]
        [Display(Name = "承运商名称")]
        public override string CarrierName { get; set; }

        [Required]
        [Display(Name = "承运商全称")]
        [StringLength(20)]
        [Remote("CheckCarrierAllName", "Carrier", ErrorMessage = "已存在该承运商")]
        public override string CarrierAllName { get; set; }

        [Required]
        [Display(Name = "联系人")]
        [StringLength(10)]
        public override string Contacter { get; set; }

        [Required]
        [Display(Name = "全国适用")]
        public override bool IsAllCoverage { get; set; }

        [Required]
        [Display(Name = "联系电话")]
        [StringLength(11)]
        public override string Phone { get; set; }

        [Required]
        [Display(Name = "联系地址")]
        [StringLength(50)]
        public override string Address { get; set; }

        private string _email;
        [Required]
        [StringLength(40)]
        [DataType(DataType.EmailAddress)]
        [Remote("CheckCarrierEmail", "Carrier", ErrorMessage = "该邮件地址已经使用过")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "电子邮件格式不正确")]
        [Display(Name = "电子邮件")]
        public override string Email
        {
            get
            {
                if (_email == null)
                {
                    return null;
                }
                return _email.ToLower();
            }
            set
            {
                _email = value;
            }
        }

        [Required]
        [Display(Name = "状态")]
        public override Enums.CarrierStatus Status { get; set; }

        [Required]
        [Display(Name = "合同有效期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public override DateTime ExpiredDate { get; set; }

        [Required]
        [Display(Name = "合同起始时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public override DateTime BeginDate { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "密码")]
        //public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "确认新密码")]
        //[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        //public string ConfirmPassword { get; set; }

        [Required]
        [Remote("CheckContractNumber", "Carrier", ErrorMessage = "已存在该合同编号")]
        [Display(Name = "合同编号")]
        [StringLength(50)]
        public override string ContractNumber { get; set; }

    }


}
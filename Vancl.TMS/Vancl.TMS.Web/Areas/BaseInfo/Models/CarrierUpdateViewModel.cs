using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Carrier;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    [Bind(Exclude = "CarrierID")]
    public class CarrierUpdateViewModel : CarrierViewModel
    {
        public CarrierUpdateViewModel():base()
        {
        }
        public CarrierUpdateViewModel(CarrierModel model): base(model)
        {
        }

        [Required]
        [Remote("CheckCarrierName", "Carrier", ErrorMessage = "已存在该承运商")]
        [Display(Name = "承运商名称")]
        public override string CarrierName { get; set; }

        [Required]
        [Remote("CheckContractNumber", "Carrier", ErrorMessage = "已存在该合同编号")]
        [Display(Name = "合同编号")]
        [StringLength(50)]
        public override string ContractNumber { get; set; }

        private string _email;
        [Required]
        [DataType(DataType.EmailAddress)]
        //[Remote("CheckCarrierEmail", "Carrier", ErrorMessage = "该邮件地址已经使用过")]
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
    }


}
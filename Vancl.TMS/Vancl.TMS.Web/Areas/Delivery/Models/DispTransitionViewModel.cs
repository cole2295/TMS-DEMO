using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Transport.Dispatch;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Delivery.Models
{
    public class DispTransitionViewModel : DispTransitionModel
    {
        public DispTransitionViewModel()
        {
        }

        public DispTransitionViewModel(DispTransitionModel parentModel)
        {
            this.DeliveryNo = parentModel.DeliveryNo;
            this.PlateNo = parentModel.PlateNo;
            this.Consignor = parentModel.Consignor;
            this.Consignee = parentModel.Consignee;
            this.ConsigneePhone = parentModel.ConsigneePhone;
            this.ReceiveAddress = parentModel.ReceiveAddress;
        }

        /// <summary>
        /// 运单号
        /// </summary>
        [Display(Name = "运单号")]
        public override string DeliveryNo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        [Display(Name = "车牌号")]
        [StringLength(20)]
        public override string PlateNo { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        [Required]
        [Display(Name = "发货人")]
        [StringLength(10)]
        public override string Consignor { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [Required]
        [Display(Name = "收货人")]
        [StringLength(10)]
        public override string Consignee { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        [Required]
        [Display(Name = "收货人电话")]
        [StringLength(20)]
        public override string ConsigneePhone { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        [Required]
        [Display(Name = "收货人地址")]
        [StringLength(100)]
        public override string ReceiveAddress { get; set; }
    }
}
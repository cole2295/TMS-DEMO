using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Vancl.TMS.Model.Sorting.BillPrint
{
   public class BillScanWeightArgModel
    {
       ///// <summary>
       ///// 单号类型
       ///// </summary>
       //public Vancl.TMS.Model.Common.Enums.SortCenterFormType FromType { get; set; }
       ///// <summary>
       ///// 单号
       ///// </summary>
       //public string FormCode { get; set; }
       ///// <summary>
       ///// 运单号（必须由单号根据规则转换）
       ///// </summary>
       //public string BillCode { get; set; }
        /// <summary>
        /// 是否打印面单（根据PMS中的商家设置）
        /// </summary>
       public bool IsSkipPrintBill { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

       /// <summary>
       /// 运单称重重量
       /// </summary>
       public decimal BillWeight { get; set; }

       /// <summary>
       /// 是否需要称重复核（根据PMS中的商家设置）
       /// </summary>
       public bool IsNeedWeightReview { get; set; }

       /// <summary>
       /// 商家称重复核阈值
       /// </summary>
       public decimal MerchantCheckWeight { get; set; }

       /// <summary>
       /// 箱号
       /// </summary>
       public int PackageIndex { get; set; }
       /// <summary>
       /// 商家ID
       /// </summary>
       public long MerchantID { get; set; }

    }
}

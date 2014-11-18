using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    /// <summary>
    /// 运单扫描结果
    /// </summary>
    public enum ScanResult
    {
        [EnumMember]
        Failed = 0,
        [EnumMember]
        Scuccess = 1,
        [EnumMember]
        Warming = 2,
    }

    /// <summary>
    /// 箱子信息
    /// </summary>
    public class BillPackageInfo
    {
        /// <summary>
        /// 箱号
        /// </summary>
        public int PackageIndex { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 扫描时间
        /// （显示数据库里更新时间）
        /// </summary>
        public DateTime ScanTime { get; set; }

        public string StrScanTime
        {
            get { return ScanTime.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { }
        }
    }


    /// <summary>
    /// 面单打印时的页面显示数据
    /// </summary>
    [Serializable]
    public class BillPageViewModel
    {
        /// <summary>
        /// 扫描结果
        /// </summary>
        [DataMember]
        public ScanResult ScanResult { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 称重复核是否异常
        /// </summary>
        [DataMember]
        public bool IsReviewWeightAbnormal { get; set; }

        /// <summary>
        /// 客户重量
        /// </summary>
        [DataMember]
        public decimal MerchantWeight { get; set; }


        /// <summary>
        /// 运单状态
        /// </summary>
        [DataMember]
        public string BillStatus { get; set; }

        /// <summary>
        /// 总件数
        /// </summary>
        [DataMember]
        public int TotalPackageCount { get; set; }

        /// <summary>
        /// 当前件数
        /// </summary>
        [DataMember]
        public int CurrentPackageIndex { get; set; }

        /// <summary>
        /// 运单编号
        /// </summary>
        [DataMember]
        public string FormCode { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMember]
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 站点编号
        /// </summary>
        [DataMember]
        public int StationId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember]
        public string StationName { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [DataMember]
        public string BillSource { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [DataMember]
        public string BillType { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        [DataMember]
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 配送商编号
        /// </summary>
        [DataMember]
        public string SiteNo { get; set; }
        /// <summary>
        /// 公司类型 3：配送商
        /// </summary>
        [DataMember]
        public int CompanyFlag { get; set; }
        /// <summary>
        /// 配送商名称
        /// </summary>
        [DataMember]
        public string CompanyName { get; set; }

        /// <summary>
        /// 本单已扫描的包裹重量
        /// </summary>
        [DataMember]
        public IEnumerable<BillPackageInfo> PackageInfo { get; set; }

        /// <summary>
        /// 是否包含多个商家 
        /// </summary>
        [DataMember]
        public bool IsMultiMerchant { get; set; }

        [DataMember]
        public object DataBag { get; set; }


        public static BillPageViewModel Create(ScanResult scanResult, string message, object data = null)
        {
            return new BillPageViewModel { ScanResult = scanResult, Message = message, DataBag = data };
        }

    }
}

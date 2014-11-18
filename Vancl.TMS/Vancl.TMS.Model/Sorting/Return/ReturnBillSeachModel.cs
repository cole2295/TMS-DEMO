using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public class ReturnBillSeachModel
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public string BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public string EndTime { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        [DataMember]
        public string CityId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        [DataMember]
        public string StationId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember]
        public string ExpressCompanyName { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string TruckNo { get; set; }
        /// <summary>
        /// 运单返货状态
        /// </summary>
        [DataMember]
        public string ReturnStatus { get; set; }

        /// <summary>
        /// 司机号
        /// </summary>
        [DataMember]
        public string DriverID { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        /// <summary>
        ///  页面索引
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }
        /// <summary>
        /// 运单状态
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        [DataMember]
        public string FormCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public string CustomerOrder { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [DataMember]
        public string FormType { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        [DataMember]
        public string OrderBy { get; set; }
        /// <summary>
        /// 排序方向("DESC,ASC")
        /// </summary>
        [DataMember]
        public string OrderDirection { get; set; }
        /// <summary>
        /// 是否分页
        /// </summary>
        [DataMember]
        public bool IsPageing { get; set; }
        /// <summary>
        /// 大区
        /// </summary>
        [DataMember]
        public string DistrictId { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [DataMember]
        public string Province { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        [DataMember]
        public int Merchant { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        [DataMember]
        public string MerchantId { get; set; }

        /// <summary>
        /// 退库单标签号
        /// </summary>
        [DataMember]
        public string LabelNo { get; set; }

        /// <summary>
        /// 退库单箱号
        /// </summary>
        [DataMember]
        public string BoxNo { get; set; }
                /// <summary>
        /// 加盟商ID
        /// </summary>
        [DataMember]
        public string DistributionId { get; set; }
        /// <summary>
        /// 配送商编号
        /// </summary>
       [DataMember]
        public string DistributionCode { get; set; }

        /// <summary>
        /// 当前配送商编号
        /// </summary>
        [DataMember]
        public string CurrentDistributionCode { get; set; }

        /// <summary>
        /// 创建站点
        /// </summary>
        [DataMember]
        public int CreateStationId { get; set; }

        /// <summary>
        /// 打印状态
        /// </summary>
        [DataMember]
        public string PrintType { get; set; }

        /// <summary>
        /// 发货分拣中心
        /// </summary>
        [DataMember]
        public string SortingCenter { get; set; }

        /// <summary>
        /// 运单来源
        /// </summary>
        [DataMember]
        public string Source { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        [DataMember]
        public string DeliverCode { get; set; }
        /// <summary>
        /// 时间类型：0、接单时间；1、分单时间；2、申请转战时间
        /// </summary>
        [DataMember]
        public int DateType { get; set; }
        
        /// <summary>
        /// 批次号
        /// </summary>
        [DataMember]
        public string BatchNo { get; set; }

    }
}

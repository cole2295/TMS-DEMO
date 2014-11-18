using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣当前操作人员对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class SortCenterUserModel
    {
        /// <summary>
        /// 用户UserID
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// 用户所在分拣中心ExpressComapnyID
        /// </summary>
        [DataMember]
        public int? ExpressId { get; set; }

        /// <summary>
        /// 分拣中心名称
        /// </summary>
        [DataMember]
        public string CompanyName { get; set; }

        /// <summary>
        /// 配送商DistributionCode
        /// </summary>
        [DataMember]
        public string DistributionCode { get; set; }

        /// <summary>
        /// 配送商名称
        /// </summary>
        [DataMember]
        public string DistributionName { get; set; }

        /// <summary>
        /// 配送商助记名称
        /// </summary>
        [DataMember]
        public String DistributionMnemonicName { get; set; }

        /// <summary>
        /// 站点类型
        /// </summary>
        [DataMember]
        public Enums.CompanyFlag CompanyFlag { get; set; }

        /// <summary>
        ///  分拣中心助记编码
        /// </summary>
        [DataMember]
        public String MnemonicCode { get; set; }

        /// <summary>
        ///  分拣中心助记名称
        /// </summary>
        [DataMember]
        public String MnemonicName { get; set; }

    }
}

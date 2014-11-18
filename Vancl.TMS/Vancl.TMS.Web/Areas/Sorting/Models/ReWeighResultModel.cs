using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Common;
using System.Runtime.Serialization;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public enum ReWeighResult
    {
        Failure=0,
        Success=1,
        Warning=2,
    }
    public class ReWeighResultModel
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public ReWeighResult Result { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        [DataMember]
        public int PackageIndex { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        [DataMember]
        public decimal Weight { get; set; }
        /// <summary>
        /// 总重量
        /// </summary>
        [DataMember]
        public decimal TotalWeight { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣逐单操作参数对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class SortCenterSimpleArgModel
    {

        /// <summary>
        /// 单号
        /// </summary>
        [DataMember]
        public string FormCode { get; set; }

        /// <summary>
        /// 操作单类型
        /// </summary>
        [DataMember]
        public Enums.SortCenterFormType FormType { get; set; }

    }
}

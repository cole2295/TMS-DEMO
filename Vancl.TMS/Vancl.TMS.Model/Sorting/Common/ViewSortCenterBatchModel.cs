using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣批量View对象
    /// </summary>
    [Serializable]
    [DataContract]
    public class ViewSortCenterBatchModel : ResultModel
    {
        /// <summary>
        /// 失败单
        /// </summary>
        public List<SortCenterBatchErrorModel> ListErrorBill { get; set; }

        /// <summary>
        /// 该批次成功数量
        /// </summary>
        public int SucceedCount { get; set; }

        /// <summary>
        /// 该批次失败数量
        /// </summary>
        public int FailedCount { get; set; }
    }


}

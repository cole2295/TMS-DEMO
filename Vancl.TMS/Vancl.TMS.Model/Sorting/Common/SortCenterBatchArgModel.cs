using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Vancl.TMS.Model.Common;
using System.Text.RegularExpressions;

namespace Vancl.TMS.Model.Sorting.Common
{
    /// <summary>
    /// 分拣批量操作参数对象
    /// </summary>
    [Serializable]
    public class SortCenterBatchArgModel
    {
        /// <summary>
        /// 原始操作单号列表
        /// </summary>
        public String[] ArrFormCode { get; set; }

        /// <summary>
        /// 操作单类型
        /// </summary>
        public Enums.SortCenterFormType FormType { get; set; }

        /// <summary>
        /// 每批次执行的数量
        /// </summary>
        public int PerBatchCount { get; set; }

    }
}

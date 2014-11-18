using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.CityScan
{
    /// <summary>
    /// 同城扫描
    /// </summary>
    [Serializable]
    public class CityScanModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 唯一键
        /// </summary>
        public Int64 BCSID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public String ScanSortCenter { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public String ScanSortCenterName { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNO { get; set; }

        /// <summary>
        /// 统计数
        /// </summary>
        public Int32 CountNum { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public Int32 IsDeleted { get; set; }

        public String SequenceName
        {
            get { return "seq_sc_expresscityscan"; }
        }

        public String TableCode
        {
            get { return "017"; }
        }
    }
}

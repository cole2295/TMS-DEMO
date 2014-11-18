using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Tools.VCMRecords.Tool.DataGrid
{
    /// <summary>
    /// DataGrid显示时的列对象
    /// </summary>
    public class Column
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string name;

        /// <summary>
        /// 别名（一般是中文名）
        /// </summary>
        public string aliasName;

        /// <summary>
        /// 列宽
        /// </summary>
        public int width=120;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool  visible;

        /// <summary>
        /// 包含在sql查询中
        /// </summary>
        public bool isSql = true;

        /// <summary>
        /// 数据列的类型
        /// </summary>
        public Type dataType=null;
    }
}

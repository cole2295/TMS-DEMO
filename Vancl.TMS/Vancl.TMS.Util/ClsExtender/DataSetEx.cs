using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Vancl.TMS.Util.ClsExtender
{
    public static class DataSetEx
    {
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool IsNull(this DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return true;
            }
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

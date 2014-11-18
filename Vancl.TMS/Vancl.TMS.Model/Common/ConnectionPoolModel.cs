using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Common;
using Vancl.TMS.Util.DateTimeUtil;

namespace Vancl.TMS.Model.Common
{
    public class ConnectionPoolModel : PoolModel
    {
        public override bool IsUsing
        {
            get
            {
                Array array = new object[(base.Value as Hashtable).Values.Count];
                (base.Value as Hashtable).Values.CopyTo(array, 0);
                foreach (object o in array)
                {
                    if (Convert.ToBoolean(o.GetType().GetProperty("IsUsing").GetValue(o, null)))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override DateTime LastTime
        {
            get
            {
                DateTime dtReturn = DateTime.MinValue;
                DateTime dtTemp = DateTime.MinValue;
                foreach (object o in (base.Value as Hashtable).Values)
                {
                    dtTemp = Convert.ToDateTime(o.GetType().GetProperty("LastUseTime").GetValue(o, null));
                    if (dtTemp.DateDiff(dtReturn).TotalMilliseconds > 0)
                    {
                        dtReturn = dtTemp;
                    }
                }
                return dtReturn;
            }
        }
    }
}

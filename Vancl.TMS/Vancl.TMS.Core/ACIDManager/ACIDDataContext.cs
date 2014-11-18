using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace Vancl.TMS.Core.ACIDManager
{
    public class ACIDDataContext
    {
        /// <summary>
        /// 是否有事务
        /// </summary>
        public bool IsHasTransaction
        {
            get;
            set;
        }

        /// <summary>
        /// 事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get;
            set;
        }
    }
}

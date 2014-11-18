using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.PDA.Core.Model
{
    public class IDAndNameModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
    }
}

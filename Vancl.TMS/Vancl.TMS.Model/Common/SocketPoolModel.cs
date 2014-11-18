using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Vancl.TMS.Model.Common
{
    public class SocketPoolModel : PoolModel
    {
        public override bool IsUsing
        {
            get
            {
                return (base.Value as VanclObjectInPool<Socket>).IsUsing;
            }
        }

        public override DateTime LastTime
        {
            get
            {
                return (base.Value as VanclObjectInPool<Socket>).LastUseTime;
            }
        }
    }
}

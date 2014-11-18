using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Chat
{
    public class ChatRoomPoolModel : PoolModel
    {
        /// <summary>
        /// 是否还有人使用
        /// </summary>
        public override bool IsUsing
        {
            get
            {
                bool isUsing = false;
                foreach (ChatUserModel u in (base.Value as ChatRoomModel).Users)
                {
                    if (!u.IsOffline)
                    {
                        isUsing = true;
                        break;
                    }
                }
                return isUsing;
            }
        }
    }
}

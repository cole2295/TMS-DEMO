using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Chat
{
    public class ChatToUserModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }
    }
}

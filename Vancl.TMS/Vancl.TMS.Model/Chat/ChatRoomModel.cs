using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Chat
{
    public class ChatRoomModel
    {
        /// <summary>
        /// 聊天室当前信息
        /// </summary>
        public List<ChatMessageModel> Messages { get; set; }

        /// <summary>
        /// 在聊天室的人
        /// </summary>
        public List<ChatUserModel> Users { get; set; }

        public ChatRoomModel()
        {
            Messages = new List<ChatMessageModel>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Chat
{
    public class ChatMessageModel
    {
        /// <summary>
        /// 发送给用户
        /// </summary>
        public List<ChatToUserModel> ToUsers { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 来自于用户
        /// </summary>
        public ChatUserModel FromUser { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
    }
}

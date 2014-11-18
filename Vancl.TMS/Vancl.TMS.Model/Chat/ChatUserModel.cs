using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.DateTimeUtil;

namespace Vancl.TMS.Model.Chat
{
    /// <summary>
    /// 聊天人
    /// </summary>
    public class ChatUserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 最后一次请求时间
        /// </summary>
        public DateTime LastRequestTime { get; set; }

        /// <summary>
        /// 是否已下线
        /// </summary>
        public bool IsOffline
        {
            get
            {
                if (DateTime.Now.DateDiff(LastRequestTime).TotalMilliseconds > Consts.CHAT_OFFLINE_TIMESPAN)
                {
                    return true;
                }
                return false;
            }
        }
    }
}

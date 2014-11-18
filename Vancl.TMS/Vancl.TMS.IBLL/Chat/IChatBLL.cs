using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Chat;

namespace Vancl.TMS.IBLL.Chat
{
    public interface IChatBLL
    {
        /// <summary>
        /// 获取别人发送给自己的聊天信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<ChatMessageModel> GetMessages(int userID);

        /// <summary>
        /// 发送聊天信息
        /// </summary>
        /// <param name="chatRoomID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool SendMessage(string chatRoomID, ChatMessageModel message);

        /// <summary>
        /// 发起聊天
        /// </summary>
        /// <param name="userIDs"></param>
        /// <returns>返回聊天室ID</returns>
        string LaunchChat(List<int> userIDs);

        /// <summary>
        /// 增加聊天人
        /// </summary>
        /// <param name="chatRoomID"></param>
        /// <param name="userIDs"></param>
        /// <returns></returns>
        bool AddChatUsers(string chatRoomID, List<int> userIDs);

        /// <summary>
        /// 根据用户ID取得用户model
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        ChatUserModel GetUserModelByUserID(int userID);
    }
}

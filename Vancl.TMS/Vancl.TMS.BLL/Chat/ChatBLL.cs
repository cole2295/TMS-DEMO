using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Chat;
using Vancl.TMS.Core.Pool;
using Vancl.TMS.IBLL.Chat;
using System.Collections;

namespace Vancl.TMS.BLL.Chat
{
    public class ChatBLL : BaseBLL, IChatBLL
    {
        #region IChatBLL 成员

        public List<ChatMessageModel> GetMessages(int userID)
        {
            SetLastRequestTime(userID);
            Hashtable ht = ChatRoomPool.GetAllChatRooms();
            List<ChatRoomModel> lstChatRoom = new List<ChatRoomModel>();
            List<ChatMessageModel> lstMessage = new List<ChatMessageModel>();
            ChatToUserModel toUser;
            ChatRoomModel crm;
            foreach (ChatRoomPoolModel v in ht.Values)
            {
                crm = v.Value as ChatRoomModel;
                int intDeleted = 0;
                for (int i = 0; i < crm.Messages.Count; i++)
                {
                    toUser = crm.Messages[i - intDeleted].ToUsers.Find(u => (u.UserID == userID && !u.IsRead));
                    if (toUser != null)
                    {
                        toUser.IsRead = true;
                        lstMessage.Add(crm.Messages[i - intDeleted]);
                    }
                    if (crm.Messages[i - intDeleted].ToUsers.Find(u => !u.IsRead) == null)
                    {
                        crm.Messages.RemoveAt(i - intDeleted);
                        intDeleted++;
                    }
                }
            }
            return lstMessage;
        }

        public bool SendMessage(string chatRoomID, ChatMessageModel message)
        {
            ChatRoomModel crm = ChatRoomPool.Get(chatRoomID);
            if (crm == null)
            {
                return false;
            }
            crm.Messages.Add(message);
            return true;
        }

        public string LaunchChat(List<int> userIDs)
        {
            List<ChatUserModel> lstUser = new List<ChatUserModel>();
            ChatUserModel u;
            userIDs.ForEach(id =>
            {
                u = ChatRoomPool.AllUsers.Find(a => a.UserID == id);
                if (u != null)
                {
                    lstUser.Add(u);
                }
            });
            ChatRoomModel crm = new ChatRoomModel();
            crm.Users = lstUser;
            return ChatRoomPool.Add(crm);
        }

        public bool AddChatUsers(string chatRoomID, List<int> userIDs)
        {
            ChatRoomModel crm = ChatRoomPool.Get(chatRoomID);
            if (crm == null)
            {
                return false;
            }
            List<ChatUserModel> lstUser = new List<ChatUserModel>();
            ChatUserModel u;
            userIDs.ForEach(id =>
            {
                u = ChatRoomPool.AllUsers.Find(a => a.UserID == id);
                if (u != null)
                {
                    lstUser.Add(u);
                }
            });
            crm.Users.AddRange(lstUser);
            return true;
        }

        public ChatUserModel GetUserModelByUserID(int userID)
        {
            return ChatRoomPool.AllUsers.Find(a => a.UserID == userID);
        }
        #endregion

        private void SetLastRequestTime(int userID)
        {
            ChatUserModel cum = ChatRoomPool.AllUsers.Find(a => a.UserID == userID);
            if (cum != null)
            {
                cum.LastRequestTime = DateTime.Now;
            }
        }
    }
}

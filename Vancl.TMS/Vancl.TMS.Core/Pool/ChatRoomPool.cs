using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Chat;
using System.Collections;
using System.Timers;

namespace Vancl.TMS.Core.Pool
{
    public class ChatRoomPool
    {
        private static readonly Hashtable _chatRooms = new Hashtable();
        public static readonly List<ChatUserModel> AllUsers = new List<ChatUserModel>();
        static ChatRoomPool()
        {
            Pool.GetInstance().PoolList.Add(_chatRooms);
            InitUsers();
        }

        public static void InitUsers()
        {
            AllUsers.Clear();
            ChatUserModel cum = new ChatUserModel();
            cum.UserID = 1;
            cum.UserName = "张柯";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 2;
            cum.UserName = "魏明";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 3;
            cum.UserName = "任钰";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 4;
            cum.UserName = "张本东";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 5;
            cum.UserName = "吕萍";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 6;
            cum.UserName = "何名宇";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 7;
            cum.UserName = "位传海";
            AllUsers.Add(cum);
            cum = new ChatUserModel();
            cum.UserID = 8;
            cum.UserName = "马建";
            AllUsers.Add(cum);
        }

        public static Hashtable GetAllChatRooms()
        {
            return _chatRooms;
        }

        /// <summary>
        /// 获得一个聊天室
        /// </summary>
        /// <param name="_chatRoomID">聊天室id</param>
        /// <returns></returns>
        public static ChatRoomModel Get(string _chatRoomID)
        {
            if (_chatRooms.ContainsKey(_chatRoomID))
            {
                return (_chatRooms[_chatRoomID] as ChatRoomPoolModel).Value as ChatRoomModel;
            }
            return null;
        }

        /// <summary>
        /// 增加一个聊天室
        /// </summary>
        /// <param name="crm"></param>
        /// <returns>聊天室id</returns>
        public static string Add(ChatRoomModel crm)
        {
            string chatRoomID = Guid.NewGuid().ToString();
            ChatRoomPoolModel model = new ChatRoomPoolModel();
            model.LastTime = DateTime.Now;
            model.Value = crm;
            _chatRooms.Add(chatRoomID, model);
            return chatRoomID;
        }
    }
}

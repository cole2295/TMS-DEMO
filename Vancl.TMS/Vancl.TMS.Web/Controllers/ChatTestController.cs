using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.Chat;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Model.Chat;

namespace Vancl.TMS.Web.Controllers
{
    public class ChatTestController : Controller
    {
        IChatBLL _chatBLL = ServiceFactory.GetService<IChatBLL>("ChatBLL");
        //
        // GET: /ChatTest/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetMessages()
        {
            int userID = Convert.ToInt32(Request["UserID"]);
            return Json(_chatBLL.GetMessages(userID), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LaunchChat()
        {
            string userIDs = Request["UserIDs"];
            return Json(_chatBLL.LaunchChat(VanclConverter.ConvertArray<int>(userIDs.Split(',')).ToList()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            string chatRoomID = Request["ChatRoomID"];
            string userIDs = Request["UserIDs"];
            string userID = Request["UserID"];
            string message = Request["Message"];
            List<ChatToUserModel> lstUser = new List<ChatToUserModel>();
            ChatToUserModel tum;
            userIDs.Split(',').ToList().ForEach(u =>
            {
                tum = new ChatToUserModel();
                tum.UserID = Convert.ToInt32(u);
                lstUser.Add(tum);
            });
            ChatMessageModel cmm = new ChatMessageModel();
            cmm.Message = message;
            cmm.FromUser = _chatBLL.GetUserModelByUserID(Convert.ToInt32(userID));
            cmm.SendTime = DateTime.Now;
            cmm.ToUsers = lstUser;
            return Json(_chatBLL.SendMessage(chatRoomID, cmm), JsonRequestBehavior.AllowGet);
        }

    }
}
